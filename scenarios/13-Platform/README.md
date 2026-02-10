# Platform (Docker/K8s) Senaryo Lab

## Amaç
Pod `Running` olsa bile neden `Ready` olmadigini ve probe/resource kararlarinin etkisini ogrenmek.

## Kavramlar (Nedir?)
- `Running`: Container process calisiyor.
- `Ready`: Pod trafige hazir. `Running` olsa da `Ready` olmayabilir.
- `liveness probe`: Uygulama kill/restart edilmeli mi kontrolu.
- `readiness probe`: Bu pod'a trafik verilsin mi kontrolu.
- `startup probe`: Uygulama gec aciliyorsa erken restart'lari engeller.
- `requests/limits`: Pod'un minimum ve maksimum kaynak sinirlari.

## Neden Onemli?
- Yanlis probe, saglikli uygulamayi bile restart dongusune sokabilir.
- Limitsiz kaynak komsu pod'lari etkileyip node dengesini bozabilir.

## Dosya Haritasi
- `scenarios/13-Platform/manifests/bad-deployment.yaml`: Probe/resource kotu ornek
- `scenarios/13-Platform/manifests/good-deployment.yaml`: Probe/resource iyi ornek
- `scenarios/13-Platform/scripts/compare_manifests.sh`: Farki metin olarak ozetler
- `scenarios/13-Platform/k6/PlatformLab/health_check_load.js`: Health endpoint yuk sablonu
- `scenarios/13-Platform/run.sh`: Manifest kontrol akisi

## Calistirma
```bash
cd scenarios/13-Platform
./run.sh
```

## Ciktiyi Nasil Yorumlarim?
- `bad` manifest'te probe endpoint'i hatali oldugu icin kararsizlik riski vardir.
- `good` manifest'te readiness/liveness/startup ayrimi rollout kalitesini artirir.

## .NET Uygulama Gerekli mi?
- Manifest farkini anlamak icin gerekli degil.
- Probe davranisini dogru gormek icin gercek bir `.NET` health endpoint'i ile test etmek daha dogru olur.
