# Microservice Resilience Lab

## Mülakat Odaklı Minimal Çerçeve
- Retry her zaman çözüm değildir; backoff şart.
- Timeout budget ve fail-fast yaklaşımını açıkla.
- Circuit breaker ile sistem koruma mantığını anlat.
- Idempotency olmadan retry'nin veri hatasına yol açacağını göster.
- Metrik: error rate, retry count, timeout ratio, breaker state.

## 🎯 Amaç
Bu laboratuvarın amacı, dağıtık sistemlerde **Dayanıklılık (Resilience)** desenlerini uygulamalı olarak test etmek ve yanlış yapılandırılmış retry stratejilerinin nasıl felaketlere yol açabileceğini gözlemlemektir.

Odak noktası, sadece "Sunucu hata verdi, tekrar dene" demek değil; **DB Retry** ile **HTTP Retry** arasındaki farkları, **Idempotency** kavramını ve **Circuit Breaker**'ın önemini anlamaktır.

## 🧪 Senaryolar

### 1. Database Retry & Transient Failures
*   **Senaryo:** Veritabanı bağlantısı anlık olarak kopuyor veya timeout veriyor.
*   **Deney A (No Retry):** Hata direkt kullanıcıya yansır.
*   **Deney B (Aggressive Retry):** Sonsuz döngüde veya beklemeden tekrar deneme (Retry Storm).
*   **Deney C (Exponential Backoff):** Artan sürelerle bekleme stratejisi.

### 2. HTTP Retry & Circuit Breaker
*   **Senaryo:** Downstream servis (örneğin Payment API) cevap veremiyor.
*   **Deney A (Naive Retry):** Her hatada tekrar dene -> Hedef sistem tamamen kilitlenir.
*   **Deney B (Circuit Breaker):** Hata eşiği aşıldığında devre kesici açılır ve istekler geçici olarak reddedilir (Fail Fast).

### 3. Idempotency Problemi
*   **Senaryo:** Bir ödeme isteği timeout alıyor ancak işlem arka planda gerçekleşiyor. Client tekrar denediğinde (Retry) mükerrer ödeme (Double Spending) oluşuyor.
*   **Çözüm:** Idempotency Key kullanımı ile güvenli retry.

## 🛠️ Kurulum
Bu lab için `Polly` kütüphanesi kullanılacaktır.
