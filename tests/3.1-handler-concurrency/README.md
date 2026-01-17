# Deney 3.1: Handler Concurrency Limiti

## Amaç
`.NET` içindeki `SocketsHttpHandler.MaxConnectionsPerServer` özelliğinin darboğaz etkisini göstermek.

**MaxConnectionsPerServer Nedir?**
Bir uygulamanın, **tek bir hedef sunucuya** (örneğin `api.google.com`) aynı anda açabileceği maksimum TCP bağlantı sayısıdır. varsayılan değer genellikle sınırsızdır (Int.Max). Ancak bu limit manuel olarak düşürüldüğünde (örneğin 10 yapılırsa), 11. istek geldiğinde yeni bir bağlantı açamaz ve mevcut 10 bağlantıdan birinin boşa çıkmasını bekler (Kuyruklama/Queueing). Bu durum, hedef servis çok hızlı olsa bile bizim tarafımızda gecikmeye (Latency) neden olur.

## Senaryo
*   **Dış Servis:** Çok hızlı yanıt veren bir servis (`traefik/whoami`). Gecikme < 1ms.
*   **Yük:** Saniyede 1000 istek (1000 RPS).
*   **Değişken:**
    *   **Case A (Kısıtlı):** `MaxConnectionsPerServer = 10`
    *   **Case B (Sınırsız):** `MaxConnectionsPerServer = 1000`

## Beklenen Davranış
*   **Case A:** Sadece 10 bağlantı aynı anda açılabildiği için, geriye kalan 990 istek kuyrukta bekler. Ortalama latency artar.
*   **Case B:** Bağlantı havuzu (connection pool) ihtiyacı karşılayacak kadar genişler. Kuyruk oluşmaz, latency minimumda kalır.

## Nasıl Çalıştırılır?
```bash
./tests/3.1-handler-concurrency/run.sh
```
