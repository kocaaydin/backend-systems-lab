# Network Behavior Lab

## Mülakat Odaklı Minimal Çerçeve
- Latency nerede oluşur: connect, TLS, app processing, transfer.
- Timeout + retry yanlış ayarı nasıl zincirleme sorun üretir.
- Keep-Alive/connection reuse neden kritik.
- HTTP/1.1 vs HTTP/2 farkını davranış üzerinden anlat.
- Metrik: handshake sayısı, retransmission, p95/p99 latency.

## 🎯 Amaç
Bu laboratuvarın amacı, **Ağ Katmanı (Network Layer)** davranışlarını, özellikle TCP bağlantıları, Connection Pooling ve Protokol farklarını (HTTP/1.1 vs HTTP/2) deneyimlemektir.

"Neden 502 Bad Gateway alıyoruz?", "Connection Pool tükendi ne demek?", "Keep-Alive gerçekten çalışıyor mu?" gibi sorulara yanıt arayacağız.

## 🧪 Senaryolar

### 1. Connection Pooling & TCP Reuse
*   **Amaç:** `HttpClient`'ın doğru ve yanlış kullanımının etkilerini görmek.
*   **Deney A (Bad Usage):** Her istek için `new HttpClient()` -> Port Exhaustion (TIME_WAIT yığılması) *(Concurrency ilişkisi: paralel istek sayısı arttıkça port tüketimi hızlanır).*
*   **Deney B (Good Usage):** `IHttpClientFactory` veya Singleton HttpClient -> TCP Reuse.

### 2. HTTP/1.1 vs HTTP/2 Multiplexing
*   **Amaç:** Aynı anda atılan 100 isteğin TCP üzerindeki davranışını kıyaslamak.
*   **Deney A (HTTP/1.1):** Head-of-Line Blocking ve Connection limitleri (Browser/Client başına 6-10).
*   **Deney B (HTTP/2):** Tek bir TCP bağlantısı üzerinde çoklu akış (Multiplexing).

### 3. Client-Side Timeouts vs Server Processing
*   **Senaryo:** Sunucu işlemi 10sn sürüyor, Client timeout 5sn.
*   **Gözlem:** Bağlantı Client tarafında kapansa bile Sunucu işlemi gerçekten iptal ediliyor mu? (Cancellation Token önemi).

## 🛠️ Kurulum
Bu lab için `NetworkLab.Api` oluşturulacak ve yüksek sayıda istek üreten `k6` senaryoları kullanılacaktır.
