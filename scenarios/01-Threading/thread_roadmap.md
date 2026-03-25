# Thread Pool / Worker Pool Roadmap (Deney + Stres Test)

## Minimal Çerçeve
- ThreadPool starvation belirtilerini
- async/await ile blocking farkı
- Deadlock/race condition farkı
- lock vs SemaphoreSlim seçimini trade-off
- Metrik: queue wait, p95/p99 latency, active/pending worker.

## 0. Thread Türleri ve Pool Yapısı
- `Main Thread`: Uygulamanın giriş thread'i.
- `ThreadPool Thread`: Runtime tarafından yönetilen worker thread'ler. (Bizim odak noktamız)
- `Dedicated Thread`: Uygulamanın özel işi için açtığı manuel thread (`new Thread()`).
- `GC Threads`: GC işini yapan runtime thread'leri.
- `Finalizer Thread`: Finalizer kuyruğunu tüketen özel thread.

### ThreadPool Alt Türleri
ThreadPool aslında tek bir havuzdur ancak içinde iki farklı "görev tipi" için ayrılmış thread limitleri vardır:
1.  **Worker Threads:**
    -   **Görevi:** İşlemci yoğunluklu işler (`Task.Run`) ve bloklanan işlemler (`Thread.Sleep`, senkron DB çağrıları).
    -   **Önemi:** Bizim "CPU Bound" ve "Blocking IO" testlerimizde tükenen ve darboğaz yaratan threadler bunlardır.
2.  **IOCP (I/O Completion Port) Threads:**
    -   **Görevi:** Asenkron I/O işlemlerinin (`await file.ReadAsync`, `await db.ExecuteAsync`) bitişini beklemek ve callback'leri çalıştırmak.
    -   **Önemi:** Bu threadler bloklanmaz, sadece iş bittiğinde devreye girer. Bu yüzden `async/await` kullandığımızda Worker Threadler meşgul edilmez, sistem daha ölçeklenebilir olur.

**.NET Runtime Kararı:**
Hangi thread'in kullanılacağına yazdığınız kod karar verir:
- `Task.Run(...)` veya senkron kod -> **Worker Thread**
- `await stream.ReadAsync(...)` -> **IOCP Thread** (işletim sistemi seviyesinde)

### ThreadPool İç Analizi (2026-02-15)
`monitor` scripti ile yapılan testte (50 RPS, IO Bound), ThreadPool'un davranışını canlı izledik:

```text
TS       | Worker(Min/Max) | Active | Total | Pending
---------|-----------------|--------|-------|--------
02:11:15 | 1/32767         | 2      | 2     | 0      (Başlangıç)
02:11:16 | 1/32767         | 7      | 8     | 158    (Saniyede 50 istek geldi, kuyruk fırladı)
02:11:22 | 1/32767         | 10     | 10    | 238    (6 saniye sonra sadece 2 thread ekleyebildi!)
```

**Bulgular:**
1.  **MinWorker=1:** Container ortamında default Min çok düşüktür.
2.  **Hill Climbing Yavaşlığı:** Ani yük (Burst) geldiğinde, havuz saniyede sadece 1-2 thread ekledi (`Injection Rate`).
3.  **Kuyruk Birikmesi (Starvation):** Threadler yetersiz olduğu için işler kuyrukta (`Pending`) bekledi. Latency'nin 15 saniyelere çıkmasının asıl sebebi bu **kuyruk bekleme süresidir**.

**Çözüm:** `ThreadPool.SetMinThreads` ile başlangıç thread sayısını artırmak (Örn: 100) bu ilk şoku ("Warmup problemini") çözer.

Dosyalar:
- SH: `00_thread_pool_monitor.sh`
- Endpoint: `/thread-types/pool-stats`

Not:
- Starvation belirtileri çoğunlukla `ThreadPool` üzerinde görünür.
- Thread modelini seçmek (ThreadPool vs Dedicated) mimari karardır.

## 1. Tek Pool ile Başla
Ne yapıyor:
- `cpu-heavy-threadpool` yükü altında `/fast` endpoint gecikmesini gösterir.

Ne gözlemlemeliyim:
- Baseline hızlı yanıt.
- Yük altına girince `/fast` latency artışı.


Son konsol çıktısı (cpu=1.0 limit, 4 tekrar, 2026-02-12):
```text
=== Senaryo 1: Tek Pool Baseline ===
run_count=4 warmup=5s
baseline: duration=5s fast_rps=20
loaded  : duration=5s fast_rps=20 heavy_rps=50 heavy_n=2000000

=== Ortalama Sonuclar ===
baseline fast avg: ~2.19 ms | p95: ~3.30 ms
loaded   fast avg: ~4916 ms | p95: ~7894 ms
delta    fast avg: +4913 ms | p95: ~7890 ms
NOTE: Docker CPU limit (1.0) applied to force starvation.
summary_file: results/01-single-pool-average.json
```

Not:
- Starvation başarıyla simüle edildi.
- Loaded testinde ortalama gecikme ~2ms'den ~4.9 saniyeye fırladı.
- Hata oranı %0 kaldı (sistem çökmedi, sadece kitlendi).

## 3. Worker Pool Ayrımı (ThreadPool vs Dedicated)
Ne yapıyor:
- Hem **CPU Bound** (asal sayı hesab) hem de **IO Bound** (blocking/sleep) işleri için ThreadPool vs Dedicated karşılaştırması yapar.

Ne gözlemlemeliyim:
- İşlem tipine göre hangi modelin `/fast` üzerindeki etkisi nedir?

### A. Senaryo: CPU Bound (Asal Sayı Hesabı)
Yük: 50 RPS, Heavy Calculation (N=2M)

Sonuçlar (2026-02-15):
```text
=== CPU Bound Karşılaştırma Sonuçları ===
Senaryo              | Avg Latency | P95 Latency | Fail Rate
---------------------|-------------|-------------|-----------
ThreadPool (Heavy)   | ~8.56 s     | ~14.49 s    | 0.0%
Dedicated (Heavy)    | ~13.72 s    | ~21.23 s    | 0.0%
```

**Yorum (CPU):**
Beklentinin aksine "Dedicated Thread" senaryosunda sistem *daha fazla* yavaşladı.
Sebep: Sistem zaten CPU darboğazında olduğu için, her istekte yeni thread açmanın getirdiği "Context Switch" maliyeti durumu daha da kötüleştirdi. Thread Starvation yerine Thread Explosion yaşandı.

### B. Senaryo: IO Bound (Blocking / Sleep)
Yük: 50 RPS, 300ms Blocking Delay

Sonuçlar (2026-02-15):
```text
=== IO Bound Karşılaştırma Sonuçları ===
Senaryo              | Avg Latency | P95 Latency | Fail Rate
---------------------|-------------|-------------|-----------
ThreadPool (Block)   | ~16.18 s    | ~21.15 s    | 0.0%
Dedicated (Block)    | ~2.22 ms    | ~4.38 ms    | 0.0%
```

**Yorum (IO):**
İşte Dedicated Thread'in parladığı yer burasıdır!
- **ThreadPool:** 50 RPS ile gelen 300ms'lik bloklamalar havuzdaki tüm threadleri ("worker") tüketti. `/fast` istekleri bile kuyrukta beklediği için 16 saniyeye kadar gecikti (Starvation).
- **Dedicated:** Her bloklanan işlem kendi özel thread'inde beklediği için, ThreadPool'daki worker'lar meşgul edilmedi. `/fast` istekleri havuzdan hemen cevap alabildi (~2.2ms).

  
## 6. Çok Seviyeli Karşılaştırma
Ne yapıyor:
- `vus=1,4,8,12` seviyelerinde ThreadPool vs Dedicated karşılaştırması yapar.

Ne gözlemlemeliyim:
- Yük arttıkça model farkı netleşiyor mu?
- Özellikle p95/p99 trendi nasıl değişiyor?

Dosyalar:
- SH: `06_compare_threadpool_vs_dedicated.sh`
- K6: `k6/06_compare_threadpool_vs_dedicated.js`

Not:
- Bu plan roadmap'te duruyor, ilgili script/k6 dosyaları henüz repoda yok.

### 6.b SetMinThreads Karşılaştırması (Default vs Tuned)
Ne yapıyor:
- Aynı blocking yükü altında `default` (runtime varsayılan min thread) ve `tuned` (`minWorker=100`, `minIo=100`) karşılaştırması yapar.
- Her fazda `run_count=4` çalıştırır, her run öncesi warmup uygular.
- `set-min-threads` endpoint'i ile tuned ayarı verip `/fast` gecikmelerindeki farkı ölçer.
Dosyalar:
- SH: `scripts/06_min_threads_comparison.sh`
- K6: `k6/06_min_threads_comparison.js`
- Endpoint: `/thread-types/set-min-threads`
- Özet çıktı: `results/06-minthreads-summary.json`

Son konsol çıktısı (cpu=1.0 limit, 4 tekrar, 2026-02-24):
```text
=== Senaryo 6: SetMinThreads Karsilastirmasi (Default vs Tuned) ===
run_count=4 warmup=5s
duration=10s fast_rps=20 heavy_rps=50 block_ms=500
tuned_min_worker=100 tuned_min_io=100

=== Ortalama Sonuclar ===
default fast avg: 18407.489 ms   | p95: 25916.469 ms   | p99: 25916.469 ms
tuned   fast avg: 1.634 ms       | p95: 2.424 ms       | p99: 2.424 ms
delta   fast avg: -18405.855 ms  | p95: -25914.045 ms  | p99: -25914.045 ms
set-min-threads response(run1): {"success": true, "requested": {"minWorker": 100, "minIo": 100}, "current": {"minWorker": 100, "minIo": 100}, "note": "Min threads updated."}
summary_file: results/06-minthreads-summary.json
```

Not:
- `default` fazda `worker min=1`, `tuned` fazda `worker min=100` doğrulandı.
- Bu koşuda `p99` trend metrikleri bazı run dosyalarında üretilmediği için özet hesapta `p95` fallback kullanıldı.

## 7. GC Threads + Finalizer Thread
Ne yapıyor:
- `create -> stats -> clear -> collect -> stats` akışını tek bir sunucu isteğiyle yapar.

Ne gözlemlemeliyim:
- **Test 1: Standard Objects (No Finalizer):**
    - `Memory` referanslar silinince ANINDA düşmeli.
    - `Finalized Count` **0** kalmalı.
- **Test 2: Finalizable Objects (With Destructor):**
    - `Memory` hemen düşmeyebilir (Finalizer Queue işlenene kadar).
    - `Finalized Count` artmalı (Yıkıcı metodlar çalıştı).

Dosyalar:
- SH (Finalizer): `07_gc_finalizer.sh`
- SH (Standard): `07_b_gc_standard.sh`
- Endpoint: `/gc/standard` ve `/gc/finalizer`

Sonuçlar (2026-02-17):
```text
=== Test 1: /gc/standard (No Finalizer) ===
Baslangic Memory : 1091.86 KB
Create sonrasi   : 11754.36 KB
Clear+Collect    : 1213.59 KB
Alive (Std)      : 0
Finalized Count  : 0 (beklenen)

=== Test 2: /gc/finalizer (With Destructor) ===
Baslangic Memory : 1091.87 KB
Create sonrasi   : 11760.05 KB
Clear+Collect    : 1213.60 KB
Alive (Fin)      : 0
Finalized Count  : 10000 (beklenen)
```

Yorum:
- **Standard Objects:** Referanslar silinip GC tetiklenince bellek hızlıca geri alındı, finalizer calismadi.
- **Finalizable Objects:** Finalizer kuyrugu islendikten sonra `Finalized Count` 10000'e cikti; bu, Finalizer Thread'in devreye girdigini dogruladi.
- Bu kosuda her iki testte de son memory degeri ~1.2 MB seviyesine geri dondu.



## 8. GC Generations & Full GC (Gen0, Gen1, Gen2)
**Kavramlar:**
- **Gen 0 (Ephemeral):** Yeni doğan tüm objeler buradadır (Heap). Sık ve hızlı temizlenir.
- **Gen 1:** Gen 0'dan sağ çıkanlar buraya terfi eder. Tampon bölgedir.
- **Gen 2 (Long-Lived):** Gen 1'den sağ çıkanlar buraya gelir. "Full GC" burayı temizler.

**Heap vs Stack Yanılgısı:**
- **Stack:** Metod parametreleri ve local değişkenlerin referansları (pointer) buradadır.
- **Heap:** Objenin **kendisi** (`new Class()`) her zaman Heap'tedir. Gen 0'daki objeler de Heap'tedir, Stack'te değil.

**SOH vs LOH (Heap Ayrımı):**
- **SOH (Small Object Heap):** Standart objeler. Gen 0 -> 1 -> 2 yolunu izler ve GC bunları sıkıştırır (Compact).
- **LOH (Large Object Heap):** **85 KB'dan büyük** objeler direkt buraya (Gen 2'ye) gider. GC burayı sıkıştırmaz (Fragmantasyon riski).

**Otomatik GC Ne Zaman Çalışır?**
Bizim `GC.Collect()` ile zorla yaptığımız işi Runtime normalde şu durumlarda yapar:
1.  **Gen 0 Dolarsa (Allocation limit):** En sık çalışır. Sadece Gen 0'ı temizler.
    *   *Gen 1 ne zaman?* Eğer Gen 0 temizlenirken Gen 1'in de limiti dolmuşsa, temizlik **Gen 0 + Gen 1** olarak genişler. Tek başına çalışmaz.
2.  **Sistem RAM'i Azalırsa:** İşletim sistemi "Yer aç!" sinyali gönderirse.
    *   Bu acil durumdur, direkt **Full GC (Gen 0+1+2)** çalıştırır.
3.  **Gen 2 Çok Şişerse:** Uzun süredir temizlik yapılmadıysa (Full GC).

**Full GC Nedir?**
- Gen 0+1+2 dahil tüm belleği tarar.
- **Stop-The-World:** Tüm çalışan threadleri durdurur. Latency spike (donma) yaratır.
- Maliyetlidir. Gen 2'ye gereksiz obje kaçırmak performansı öldürür.

**Dikkat Edilmesi Gerekenler:**
1.  **Erken Terfi (Premature Promotion):** Kısa ömürlü olması gereken objelerin yanlışlıkla uzun süre referans tutularak Gen 2'ye taşınması. Bu durum Full GC sıklığını artırır.
2.  **Memory Leak:** Static listeler veya event handler'lar yüzünden objelerin Gen 2'de birikmesi ve asla silinmemesi.
3.  **LOH Fragmentasyonu:** Büyük string/array manipülasyonu yaparken belleğin delikli peynire dönmesi. Çözüm: `ArrayPool<T>`.

**Manuel GC Tetikleme (`GC.Collect()`):**
- **Kural:** Sunucu uygulamalarında **ASLA** kullanılmamalıdır. Runtime'ın heuristic (zamanlama) zekasını bozar.
- **Risk:** Her çağrıda tüm thread'ler durdurulur (**Stop-The-World**). Buna "GC Pause" denir. Sık çağrılırsa CPU sürekli temizlikle meşgul olur, iş yapamaz.
- **İstisna:** Çok büyük bir veri işlendikten sonra uygulama uzun süre (sistem boşta) bekleyecekse, RAM'i iade etmek için tek seferlik çağrılabilir (Örn: Masaüstü uygulamaları).


## 9. Full GC "Donma" (Freeze) Testi
**Amaç:** GC çalıştığında uygulamanın gerçekten durduğunu (Stop-The-World) ispatlamak.
**Yöntem:**
1.  **Arkaplan Threadi:** 1ms uyuyup uyanma süresini ölçerek "Donma"yı (Pause) yakalar.
2.  **Yükleme:** 10 Milyon nesne (LinkedList) yaratılır ama silinmez (Root edilir).
3.  **Tetikleme:** `GC.Collect()` çağrılır (Default Mod).

**Sonuçlar (Kritik Kanıt):**
1.  **Allocation (Sadece CPU Yükü):**
    *   **Max Gecikme:** **~18 ms** (CPU %100 olsa bile threadler arası geçiş hızlıdır).

2.  **Small Objects (10 Milyon Adet Linked List):**
    *   **GC Süresi (Main Thread):** **~193 ms** (Ana thread kilitlendi).
    *   **Donma (Background Thread):** **~193 ms** (Stop-The-World).
    *   **Bellek:** **840 MB** (Gen 0+1 temizlense bile Gen 2'deki nesneler kaldığı için bellek düşmedi).
    *   **Sebep:** Milyonlarca nesnenin GRAFİĞİNİ TARAMAK (Marking) ~200ms sürdü ve dünyayı durdurdu. (Eğer Compacting de olsaydı bu süre ~400ms olurdu).

3.  **Large Objects (85 Adet x 10MB = ~850 MB):**
    *   **Donma:** **~0 ms**
    *   **Bellek:** **851 MB** (Small Objects ile neredeyse aynı toplam bellek kullanıldı).
    *   **Sebep:** Nesne sayısı (85 adet) çok az olduğu için GC'nin taraması gereken grafik çok basit, milisaniyeler sürdü. LOH genelde compact edilmez.

**Ders:**
GC süresini belirleyen şey **toplam GB** değil, **canlı nesne sayısıdır (Graph Complexity)**.
*   **Small Objects (10 Milyon Adet, 840 MB):** ~193 ms Donma.
*   **Large Objects (85 Adet, 850 MB):** ~0 ms Donma.
*   **Sonuç:** Aynı bellek miktarı olsa bile, nesne sayısı arttıkça GC maliyeti (Stop-The-World) devasa artar.
CPU yükü sistemi "yavaşlatır" (slowdown), ama GC sistemi "durdurur" (freeze).

## 10. Request Cancellation Propagation
Ne yapıyor:
- Ayni endpoint'e iki farkli timeout ile istek gonderir:
- Test A: Cok dusuk timeout (iptal beklenir)
- Test B: Yeterli timeout (iptal olmamasi beklenir)

Ne gözlemlemeliyim:
- Client timeout olduğunda iş gerçekten kesiliyor mu?
- Cancellation zinciri taşınıyor mu?

Dosyalar:
- SH: `10_request_cancellation_propagation.sh`
- Endpoint: `/thread-types/cpu-cancellable`
- Not: Ayni `n` degeri ile iki test kosulur; sadece timeout farklidir.

Sonuçlar (2026-02-17):
```text
n=200000000, checkEvery=1
Test A (cancel): timeout=0.05s -> curl timeout (HTTP 000)
Test B (no-cancel): timeout=5s -> HTTP 200, "cancelled": false
```

Yorum:
- Dusuk timeout'ta client istegi erken kesiyor (iptal senaryosu).
- Yeterli timeout'ta ayni endpoint islemi tamamliyor (iptal olmayan senaryo).



## 10. Fire-and-Forget Risk Simülasyonu
Ne yapıyor:
- Non-cancellable timeout ve cancellable timeout davranışlarını kıyaslar.

Ne gözlemlemeliyim:
- Timeout sonrası sistem etkisi (özellikle `/fast` gecikmesi).
- Cancellable ve non-cancellable farkı.

Dosyalar:
- SH: `ThreadTypes/scripts/10_fire_and_forget_risk.sh`
- K6: `ThreadTypes/k6/10_fire_and_forget_risk.js`
- Endpoint: `/thread-types/cpu-timeout-risk` (`cancellable=false|true`)
- Probe: `/thread-types/fast`


## 11. Graceful Shutdown + Queue Drain
Ne yapıyor:
- Uzun queue isteği çalışırken API'ye `TERM` gönderir.

Ne gözlemlemeliyim:
- İstek tamamlandı mı, yoksa yarım mı kaldı?
- Kapanış davranışı öngörülebilir mi?


## 12. Birden Fazla ThreadPool Yorumu (Multi-Process)
Ne yapıyor:
- Aynı API'den iki ayrı process başlatır.
- A processine ağır yük verilir, A ve B'de `/fast` probu alınır.

Ne gözlemlemeliyim:
- A yük altındayken B izolasyonu korunuyor mu?
- Process başına ayrı threadpool davranışı pratikte nasıl görünüyor?

------------------------------------------------------

Ne yapıyor:
- Bounded queue ile backpressure davranışını test eder.
- Dar kapasite/yavaş işleme ile geniş kapasite/hızlı işleme karşılaştırılır.

Ne gözlemlemeliyim:
- `producer` bekliyor mu?
- Kapasite ve iş süresi değişince kuyruk davranışı nasıl değişiyor?

Dosyalar:
- SH: `04_queue_backpressure.sh`
- K6: `k6/04_queue_backpressure.js`
