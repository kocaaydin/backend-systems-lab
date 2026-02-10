# k6 Basics Lab

Bu klasor, k6'yi sifirdan ogrenmek icin en kisa ama aciklayici ornekleri icerir.

## Ne Ogrenirim?
- `check` ile dogrulama
- `threshold` ile test basarisiz/basarili kurali
- `stages` ile yuk simulasyonu
- Sonuc yorumlama: `http_req_duration`, `checks`, `http_req_failed`

## Kavramlar (Nedir?)
- `check`: Her istekte bir beklenti kontroludur. Ornek: `status 200 olmali`.
- `threshold`: Testin gecme-kalma kuralidir. Ornek: `p(95)<800` saglanmazsa test fail olur.
- `stages`: Yukun zamanla nasil artip azalacagini tanimlar. Ornek: 10 sn 10 kullanici, sonra 20 sn 30 kullanici.
- `http_req_duration`: Istek suresi metriği (genelde ms). `p(95)` degeri "isteklerin %95'i bu sureden kisa" demektir.
- `http_req_failed`: Hata oranidir. 0.02 ise isteklerin %2'si hata donmus demektir.

## Neden Onemli?
- `check` fonksiyonel dogrulama verir: Endpoint teknik olarak donse bile icerik yanlis olabilir.
- `threshold` operasyonel SLA benzeri sinir koyar: "Calisiyor" yerine "istenen kalitede calisiyor mu?" sorusunu cevaplar.
- `stages` gercek hayata yakindir: Trafik sabit degil, pik ve dusus vardir.

## Dosya Haritasi
- `scenarios/00-k6-Basics/k6/01_smoke.js`: Tek endpoint smoke testi
- `scenarios/00-k6-Basics/k6/02_stages.js`: Kademeli yuk testi
- `scenarios/00-k6-Basics/k6/03_threshold_fail.js`: Bilerek threshold fail ornegi
- `scenarios/00-k6-Basics/scripts/run.sh`: Tum scriptleri sirayla calistirir
- `scenarios/00-k6-Basics/scripts/run_dotnet_api.sh`: k6'nin vuracagi .NET API'yi kaldirir
- `scenarios/00-k6-Basics/dotnet/K6TargetApi/Program.cs`: `/health` ve `/work` endpointleri

## Calistirma
```bash
cd scenarios/00-k6-Basics
./scripts/run_dotnet_api.sh
# ayri terminal:
./scripts/run.sh
```

## Hizli Yorumlama
- `checks < 100%`: Davranis bozulmus olabilir (status/body kontrolu gecmemis).
- `http_req_duration p(95)`: Kullanici deneyimi icin kritik gecikme gostergesi.
- `http_req_failed > 0`: Uygulama, ag veya upstream kaynakli hata arastirilir.
