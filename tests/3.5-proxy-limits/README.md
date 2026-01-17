# Deney 3.5: Proxy / Gateway Limits

## Amaç
Uygulamamızdan çıkan isteklerin, hedefe ulaşmadan önce aradaki bir **Proxy** tarafından nasıl kesildiğini gözlemlemek.

**Proxy / Gateway Limit Nedir?**
Modern mikroservis mimarilerinde servisler birbirine doğrudan değil, genellikle bir ara katman (Nginx, HAProxy, AWS API Gateway vb.) üzerinden erişir. Bu ara katmanlar, trafiği yönetmek için kendi kurallarını uygular. Eğer hedef servis saniyede 500 isteği kaldırabilse bile, önündeki Nginx'e "saniyede en fazla 50 istek geçir" (Throttling) kuralı konulmuşsa, bizim uygulamamız 51. isteği gönderdiğinde **503 Service Unavailable** hatası alır. Bu limit, uygulama kodundan bağımsızdır ve altyapı seviyesindedir.

## Senaryo
*   **Mimari:** API -> Nginx (Proxy) -> External Service
*   **Kısıtlama:** Nginx üzerinde `limit_req zone=mylimit burst=10 nodelay rate=50r/s;` konfigürasyonu var.
*   **Yük:** Saniyede 200 istek gönderiyoruz.

## Beklenen Davranış
*   Nginx saniyede sadece **50 isteği** (artı burst kadarını) geçirir.
*   Geri kalan istekler için Nginx **503 Service Unavailable** döner.
*   Bizim API'miz, `SocketsHttpHandler` kullandığı için bu 503 hatasını alır. Uygulamada hata yönetimi olmadığı için bu durum istemciye **500 Internal Server Error** olarak yansıyabilir (veya kodda ele alırsak 503).
*   Sonuç olarak: **Requests Failed** oranı yüksek olur.

## Nasıl Çalıştırılır?
```bash
./tests/3.5-proxy-limits/run.sh
```
