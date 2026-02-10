# k6 Basics Lab

Bu klasor, k6'yi temel seviyeden ogrenmek icin hazirlanmis kucuk bir laboratuvardir.

## Bu Lab'de Ne Var?
- k6 scriptleri: `scenarios/00-Tooling/K6-Basics/k6/`
- Hedef .NET API: `scenarios/00-Tooling/K6-Basics/K6TargetApi/`
- Calistirma scriptleri: `scenarios/00-Tooling/K6-Basics/scripts/`
- Compose: `scenarios/00-Tooling/K6-Basics/docker-compose.yml`

## Kavramlar (Kisa)
- `check`: Her istekte beklenen sonucu dogrular. Ornek: status 200.
- `threshold`: Testin gecme/kalma kuralidir. Ornek: `p(95)<800`.
- `stages`: Yukun zamanla artip azalmasini tanimlar.

## Dosya Haritasi
- `scenarios/00-Tooling/K6-Basics/k6/01_smoke.js`: Basit smoke testi (`check` ornegi burada)
- `scenarios/00-Tooling/K6-Basics/k6/02_stages.js`: Kademeli yuk testi (`stages` + `check` burada)
- `scenarios/00-Tooling/K6-Basics/k6/03_threshold_fail.js`: Bilerek fail olan `threshold` ornegi
- `scenarios/00-Tooling/K6-Basics/scripts/run_dotnet_api.sh`: Hedef API'yi calistirir
- `scenarios/00-Tooling/K6-Basics/scripts/run.sh`: k6 testlerini docker compose ile calistirir
- `scenarios/00-Tooling/K6-Basics/K6TargetApi/Controllers/K6TargetController.cs`: `/health` ve `/work` endpointleri

## Adim Adim Calistirma
```bash
cd scenarios/00-Tooling/K6-Basics
./scripts/run_dotnet_api.sh
```

Yeni terminal:
```bash
cd scenarios/00-Tooling/K6-Basics
./scripts/run.sh
```

API ve k6 containerlarini durdurmak icin:
```bash
cd scenarios/00-Tooling/K6-Basics
docker compose down
```

## Ciktiyi Nasil Okurum?
- `checks < 100%`: Beklenen davranislardan biri gecmedi.
- `http_req_duration p(95)`: Isteklerin %95'inin gecikme ust siniri.
- `http_req_failed > 0`: Hata orani vardir, endpoint/ag tarafi incelenir.

## Bu Lab Sonucu Nasil Yorumlanir?
- `01_smoke` (ornek): `checks=100%`, `http_req_failed=0%`, `p95<1ms` gorursen servis ayakta ve temel davranis dogru demektir.
- `02_stages` (ornek): VU arttikca `p95` biraz yukselir ama hata oranı `0%` kalıyorsa servis yuk altinda stabil demektir.
- `03_threshold_fail` (ornek): `p95=56ms` iken threshold `p95<10ms` ise test fail olur; bu normaldir ve threshold ihlali gostermek icin yapilir.
