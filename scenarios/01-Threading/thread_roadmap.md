# Thread Pool / Worker Pool Roadmap (Deney + Stres Test)

## Amaç

- "Teoride biliyorum"dan çıkıp,
- "Yanlış tasarımın sistemi nasıl boğduğunu gözümle gördüm" seviyesine gelmek.

## 0. Thread Türleri (Temel Harita)

Bu roadmap'e girmeden önce hangi thread'in ne yaptığını ayırt et:

- `Main Thread`: Uygulamanın başlangıç thread'i.
- `ThreadPool Thread`: Runtime tarafından yönetilen, tekrar kullanılan worker thread'leri.
- `Dedicated Thread` (`new Thread(...)`): Uygulamaya özel, manuel yaşam döngülü thread.
- `GC Threads`: Garbage Collector'ın kullandığı thread'ler.
- `Finalizer Thread`: Finalizer kuyruğunu işleyen özel thread.

Kısa not:
- En kritik mimari karar çoğunlukla `ThreadPool` vs `Dedicated Thread` ayrımıdır.
- Thread starvation problemleri çoğunlukla `ThreadPool` üzerinde görünür.

## 1. Tek Pool ile Başla

API içinde:

- `/thread-types/fast`
- `/thread-types/cpu-heavy-threadpool` (loop, hash, vs.)

Her şey aynı thread pool'da çalışsın.

Test:

- 100 paralel istek at.

Gözle:

- `/fast` neden yavaşlıyor?
- Response süreleri nasıl patlıyor?

## 2. Starvation'ı Bilinçli Üret

- Pool size: örn. 10 thread
- 10+ tane `/thread-types/starvation/blocking` isteği at
- Ardından `/thread-types/fast` çağır

Beklenen:

- `/fast` bekler.
- Sistem "ayakta ama cevap vermiyor" haline gelir.

## 3. Worker Thread Pool Ayır

- Heavy işleri ayrı bir pool'a taşı.
- Request handler sadece:
  - işi worker pool'a submit etsin
  - hemen dönsün

Aynı testi tekrar yap.

Gözle:

- `/fast` artık neden etkilenmiyor?
- Latency grafiği nasıl değişti?

## 4. Queue + Worker Simülasyonu

- In-memory queue kur.
- Worker thread'leri bu queue'dan iş çeksin.

Test:

- `/thread-types/queue/enqueue` ile queue'yu bilinçli şişir.
- Worker sayısını azalt / artır.

Şunu net gör:

- Backpressure nasıl oluşuyor?
- Worker sayısı artınca neresi tıkanıyor?

## 5. Failure Senaryosu

- Worker'ı ortada öldür.
- İş yarım kalsın.
- Retry mekanizması ekle.

Amaç:

- "Asılı kalan task" nasıl oluşur?
- Bunu mimari olarak nasıl engellersin?

## 6. Thread Türleri Deneyi (ThreadPool vs Dedicated)

Amaç:
- Aynı işi farklı thread modeliyle koşturup davranış farkını görmek.

Kurulum:
- Aynı CPU-heavy işi iki endpoint ile sun:
  - `/thread-types/cpu-heavy-threadpool`
  - `/thread-types/cpu-heavy-dedicated`
- Önerilen proje: `scenarios/01-Threading/ThreadTypes/ThreadTypesApi`

Test:
- Aynı yük profili ile iki endpoint'i ayrı ayrı çalıştır.
- Paralelde `/fast` endpoint'ini de gözlemle.

Gözle:
- ThreadPool modelinde starvation belirtileri ne zaman başlıyor?
- Dedicated modelinde latency, context switch ve kaynak maliyeti nasıl değişiyor?
- Hangi noktada dedicated thread avantajdan çok maliyet üretmeye başlıyor?

Beklenen çıktı:
- "Thread sayısı arttı = her zaman hızlı" yanılgısını kırmak.
- Hangi iş tipi için hangi thread modelinin uygun olduğunu netleştirmek.

## 7. Mevcut Test Eşleşmesi (Güncel)

Kullandığımız script ve endpoint eşleşmesi:

- Hızlı genel tur:
  - `scenarios/01-Threading/ThreadTypes/scripts/quick_overview.sh`
  - Endpointler: `info`, `cpu-heavy-threadpool`, `cpu-heavy-dedicated`, `queue/enqueue`, `queue/status`
- Starvation gözlemi:
  - `scenarios/01-Threading/ThreadTypes/scripts/starvation_observation.sh`
  - Endpointler: `starvation/blocking` + `fast`
- Cancellation gözlemi:
  - `scenarios/01-Threading/ThreadTypes/scripts/cancellation_observation.sh`
  - Endpoint: `cpu-cancellable`
- Finalizer/GC gözlemi:
  - `scenarios/01-Threading/ThreadTypes/scripts/finalizer_observation.sh`
  - Endpointler: `finalizer/create`, `finalizer/stats`, `finalizer/collect`

Not:
- Bu aşamadaki hedef kavramı görmek olduğu için k6 zorunlu değil.
- Sayısal benchmark kıyası gerektiğinde k6 ayrıca eklenebilir.

## Bu Roadmap'i Bitirdiğinde

- Log senin için "debug output" değil, sistemin röntgeni olur.
- Thread pool / worker pool farkı "teori" değil, kas hafızası olur.
