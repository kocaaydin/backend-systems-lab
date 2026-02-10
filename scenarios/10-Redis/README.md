# Redis Senaryo Lab

## Amaç
Redis tarafinda kotu ve iyi tasarim farkini hizli gorebilmek: `incele -> duzelt -> tekrar incele`.

## Kavramlar (Nedir?)
- `cache hit`: Veri Redis'te bulundu, ana veritabani cagrilmadi.
- `cache miss`: Veri Redis'te yok, uygulama DB/servise gitti.
- `stampede`: Ayni key dusunce bir anda cok fazla istegin ayni kaynaga yuklenmesi.
- `SET NX EX`: Kilit benzeri kullanim. `NX` sadece yoksa yaz, `EX` suresi dolunca otomatik sil.
- `rate limit`: Belirli zaman araliginda belirli sayidan fazla istegi engelleme.

## Neden Onemli?
- Hit orani dustukce DB yuklenir, latency artar.
- Stampede kontrol edilmezse ani trafiklerde sistem dengesizlesir.
- Merkezi rate limit olmazsa coklu instance ortaminda tutarsiz davranis olur.

## Dosya Haritasi
- `scenarios/10-Redis/docker-compose.yml`: Redis container
- `scenarios/10-Redis/scripts/01_bad_cache_stampede.sh`: Kotu senaryo
- `scenarios/10-Redis/scripts/02_good_cache_stampede.sh`: Duzeltilmis senaryo
- `scenarios/10-Redis/scripts/03_rate_limit_demo.sh`: Redis ile merkezi rate limit
- `scenarios/10-Redis/k6/RedisLab/cache_endpoint_load.js`: API varsa yuk testi sablonu
- `scenarios/10-Redis/run.sh`: Hepsini sirayla calistirir

## Senaryo Ozeti
- **Case A (Bad):** Key expire olunca tum istekler ayni anda kaynaga gider.
- **Case B (Good):** `SET NX EX` lock ile tek uretici veriyi doldurur, digerleri bekler.
- **Case C:** In-memory yerine Redis tabanli merkezi rate limit.

## Ciktiyi Nasil Yorumlarim?
- `bad` case'te miss sayisi yuksek goruluyorsa stampede riski vardir.
- `good` case'te cache tek seferde doluyor ve tekrarli miss azalmalidir.
- Rate limit sonucunda `blocked` artiyorsa limit politikasi aktif calisiyordur.

## .NET Uygulama Gerekli mi?
- Bu klasordeki shell senaryolari `.NET` olmadan Redis davranisini gosterir.
- Gercek uygulama etkisini olcmek icin `.NET API + Redis` birlikte kullanilmalidir.

## Calistirma
```bash
cd scenarios/10-Redis
./run.sh
```
