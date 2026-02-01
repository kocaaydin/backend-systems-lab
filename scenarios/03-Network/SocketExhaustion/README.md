# Deney 3.3: Socket Exhaustion (Ephemeral Port Tükenmesi)

## Amaç
Yanlış `HttpClient` kullanımının (her istekte `new HttpClient()` oluşturma), sistemin dışarıya istek atamaz hale geldiğini göstermek.

**Socket Exhaustion (Port Tükenmesi) Nedir?**
TCP/IP protokolünde her giden bağlantı, işlemci üzerinde geçici bir port (Ephemeral Port) işgal eder. Bağlantı kapansa bile, işletim sistemi bu portu güvenlik nedeniyle bir süre (TIME_WAIT, genelde 60sn) "kullanımda" tutar. Eğer çok kısa sürede binlerce yeni bağlantı açıp kapatırsak, boşta port kalmaz (Port Exhaustion). Bu durumda işletim sistemi yeni giden isteklere (outgoing) izin vermez ve `SocketException` hatası alırız. `new HttpClient()` antipattern'i bunun en yaygın sebebidir.

## Senaryo
*   **Hatalı Kod:** Her istekte `using var client = new HttpClient();`
*   **Sorun:** `HttpClient` dispose edilse bile, kullandığı TCP soketi işletim sistemi tarafından `TIME_WAIT` durumunda belirli bir süre (varsayılan 60sn) tutulur.
*   **Yük:** Saniyede 200 İstek. 60 saniye boyunca toplam 12,000 soket açmaya çalışırız. Ephemeral port limitine (genelde 10k-30k arası) yaklaşıldığında hatalar başlar.

## Beklenen Davranış
*   Testin başlarında her şey normal çalışır.
*   Bir süre sonra (20-30. saniyelerde) sistem **"Address already in use"** veya **"SocketException"** fırlatmaya başlar.
*   Latency anlamsız şekilde artar veya istekler tamamen başarısız olur.

## Nasıl Çalıştırılır?
```bash
./tests/3.3-socket-exhaustion/run.sh
```
