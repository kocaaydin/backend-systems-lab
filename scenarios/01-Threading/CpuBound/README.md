# CPU Bound Lab

**I/O beklemesi yerine işlemci (CPU) darboğazına** girdiğinde nasıl tepki verdiğini simüle etmek ve gözlemlemek 

## Amaç
Ayni endpoint uzerinde `n` degerini buyutup CPU maliyeti arttiginda latency'nin nasil degistigini gosterir.

Kisa fikir:
- `n` kucukken islem hafif -> cevaplar daha hizli
- `n` buyukken CPU daha uzun mesgul -> cevaplar daha yavas

Bu sayede "CPU-bound is yukunde neden latency artar?" sorusunu net goruruz.

## 🛠 Neler Yapıldı?

1.  **"İşlemciyi Yoracak" Kod Eklendi (`CpuBoundApi/Program.cs`)**
    *   API'ye `/experiments/cpu` adında endpoint eklendi.
    *   Bu endpoint **yoğun matematiksel işlem** yapar.
    *   **Yöntem:** Belirli bir sayıya kadar (varsayılan: 20,000) olan asal sayıları "brute-force" (kaba kuvvet) yöntemiyle hesaplar. Bu yöntem bilerek verimsiz seçilmiştir; böylece her istek geldiğinde sunucu işlemcisi %100 yük altına girer ve ilgili thread kilitlenir.

2.  **Yük Testi Senaryosu (`k6/cpu-bound.js`)**
    *   **Executor:** `constant-arrival-rate` (Sabit Geliş Hızı)
    *   **Hedef:** Sistem yanıt verse de vermese de saniyede sabit sayıda istek (RPS) göndermeye çalışır.
    *   **Parametreler:**
        *   `rate`: 20 RPS (Varsayılan)
        *   `duration`: 30s
        *   `preAllocatedVUs`: 20 (Başlangıç sanal kullanıcı)
        *   `maxVUs`: 300 (Gerekirse çıkılabilecek maksimum kullanıcı)
    *   **Amaç:** "Open Model" yük testi yaparak, sistem yavaşlasa bile trafiği kesmemek ve kuyruk oluşumunu/gecikmeyi net gözlemlemektir.

## Neyi Test Ediyoruz?

- Endpoint: `/experiments/cpu?n=<N>`
- Islem: brute-force prime counting
- Karsilastirma: `N=20000` vs `N=200000`
- K6 yuk profili: sabit `RPS=20`, `duration=30s`

Neden sabit RPS?
- Cunku ayni trafik seviyesinde sadece is maliyetini (`n`) degistirmek istiyoruz.
- Boylece farkin kaynagi daha net olur.

## Calistirma

Varsayilan karsilastirma:

```bash
bash scenarios/01-Threading/CpuBound/scripts/run.sh
```

Tek bir `N` degeri:

```bash
bash scenarios/01-Threading/CpuBound/scripts/run.sh 20000
```

Birden fazla `N` degeri:

```bash
bash scenarios/01-Threading/CpuBound/scripts/run.sh 20000 200000
```

## Sonuçlar ve Yorumlama

**Test Tarihi:** 2026-02-15

| Senaryo (N) | RPS | Avg Latency (ms) | P95 Latency (ms) | Açıklama |
| :--- | :--- | :--- | :--- | :--- |
| **N=20,000** | 20 | ~2.41 ms | ~3.67 ms | İşlemci talebi rahatça karşılar. |
| **N=200,000** | 20 | ~7.73 ms | ~11.08 ms | İşlem maliyeti arttığı için süre uzadı. |

### Gözlemler

1.  **Doğrusal Artış:** `N` değeri 10 katına çıktığında (20k -> 200k), latency de yaklaşık 3 katına çıkmıştır (2.4ms -> 7.7ms). Bu, işlemin O(N) veya daha yüksek karmaşıklıkta olduğunu ve CPU'nun darboğaz yarattığını gösterir.
2.  **Kararlılık:** `dropped_iterations` 0 olduğu için sistem henüz tam kapasite sınırına (saturation point) ulaşmamıştır, yani cevap verebilmektedir ancak daha yavaştır.

Eğer RPS'i çok daha fazla artırırsak (örn: 50 RPS), CPU %100 olduğunda "Thread Starvation" başlayacak ve süreler milisaniyelerden saniyelere fırlayacaktır.

## Scriptler

- `scenarios/01-Threading/CpuBound/scripts/run.sh`: Ana çalıştırıcı script.
- Diğer scriptler (`run_n.sh` vb.) bu ana scriptin yardımcılarıdır.

Script davranisi:
- Her `N` senaryosu `REPEAT_COUNT` kadar tekrar edilir (varsayilan 3).
- Her tekrar oncesi API yeniden baslatilir.
- API `/health` hazir olana kadar beklenir.
- Sonra k6 kosar ve summary dosyasi yazar.

## Sonuc Formati

Her `N` icin:
- 3 tekrar summary json dosyaları results klasörüne kaydedilir.
- 1 ortalama dosyasi (`average.json`) oluşturulur.

`average.json` alanlari:
- `avg_of_avg_ms`: 3 kosunun ortalama latency ortalamasi
- `avg_of_p95_ms`: 3 kosunun p95 latency ortalamasi
- `dropped_iterations`: hedef yukun yetismeyen istek sayilari
