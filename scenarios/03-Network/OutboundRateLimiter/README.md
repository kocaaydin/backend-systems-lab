# Deney 3.2: Outbound Rate Limiter

## Amaç
Uygulama içinde gerçekleştirilen (In-App) bir **Outbound Rate Limiter** mekanizmasının, dış servise giden istekleri belirlenen eşiğin üzerinde nasıl **drop** ettiğini (reddedip 429 döndüğünü) gözlemlemek.

**Outbound Rate Limiter Nedir?**
Uygulamamızdan dış dünyaya (örneğin 3. parti API'lara) giden trafiğin hızını kontrol eden bir mekanizmadır. Amacı, hedef sistemi boğmamak (overwhelm) veya anlaşmalı olduğumuz API limitlerini (kota) aşmamaktır. Bu deneyde **Token Bucket** (Jeton Kovası) algoritması simüle edilmiştir: Her saniye kovaya belli sayıda jeton (limit) eklenir, isteği göndermek için kovadan jeton almak gerekir. Jeton yoksa istek reddedilir (`429 Too Many Requests`).

## Senaryo
*   **Algoritma:** Basit Fixed Window / Token Bucket simülasyonu.
*   **Limit:** Saniyede 100 İstek (100 RPS).
*   **Yük:** Saniyede 500 İstek (500 RPS).

## Beklenen Davranış
*   Gelen isteklerin yaklaşık **%20'si** (100/500) başarılı olmalı (`200 OK`).
*   Geri kalan **%80'i** anında reddedilmeli (`429 Too Many Requests`).
*   Sistem kuyruklama yapmadığı (Backlog yerine Drop) için **Latency düşük kalmalı**. Sadece `429` hatası alanların sayısı artmalı.

## Nasıl Çalıştırılır?
```bash
./tests/3.2-outbound-rate-limiter/run.sh
```
