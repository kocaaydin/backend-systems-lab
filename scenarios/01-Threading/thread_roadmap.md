# Thread Pool / Worker Pool Roadmap (Deney + Stres Test)

## Amaç

- "Teoride biliyorum"dan çıkıp,
- "Yanlış tasarımın sistemi nasıl boğduğunu gözümle gördüm" seviyesine gelmek.

## 1. Tek Pool ile Başla

API içinde:

- `/fast`
- `/cpu-heavy` (loop, hash, vs.)

Her şey aynı thread pool'da çalışsın.

Test:

- 100 paralel istek at.

Gözle:

- `/fast` neden yavaşlıyor?
- Response süreleri nasıl patlıyor?

## 2. Starvation'ı Bilinçli Üret

- Pool size: örn. 10 thread
- 10 tane `/cpu-heavy` isteği at
- Ardından `/fast` çağır

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

- Queue'yu bilinçli şişir.
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

## Bu Roadmap'i Bitirdiğinde

- Log senin için "debug output" değil, sistemin röntgeni olur.
- Thread pool / worker pool farkı "teori" değil, kas hafızası olur.
