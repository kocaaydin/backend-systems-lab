# Security/API Senaryo Lab

## Amaç
AuthN, AuthZ, CORS ve TLS konularini "kotu kullanim vs iyi kullanim" seklinde pratik gostermek.

## Kavramlar (Nedir?)
- `AuthN (Authentication)`: Kullanici kimligini dogrulama (401).
- `AuthZ (Authorization)`: Dogrulanmis kullanicinin yetkisini kontrol etme (403).
- `CORS`: Tarayicida farkli origin'den gelen isteklerin kurali.
- `TLS`: Trafik sifreleme ve sertifika dogrulama katmani.
- `OWASP`: Yaygin web guvenlik riskleri referansi.

## Neden Onemli?
- AuthN/AuthZ ayrimi yapilmazsa yetkisiz erisim acigi dogar.
- CORS yanlis kurulursa tarayici tabanli saldiri yuzeyi buyur.
- TLS zayifsa MITM ve veri sizma riski artar.

## Dosya Haritasi
- `scenarios/14-Security/scripts/01_authn_authz_examples.sh`: 401/403 farki
- `scenarios/14-Security/scripts/02_cors_examples.sh`: CORS kotu/iyi config ornegi
- `scenarios/14-Security/scripts/03_tls_check.sh`: TLS sertifika kontrolu
- `scenarios/14-Security/k6/SecurityLab/auth_load.js`: Tokenli endpoint yuk testi
- `scenarios/14-Security/run.sh`: Tum kontrol akisi

## Calistirma
```bash
cd scenarios/14-Security
./run.sh
```

## Ciktiyi Nasil Yorumlarim?
- AuthN/AuthZ script'i 401 ve 403 farkini netlestirir.
- CORS script'i kotu ve iyi policy farkini kod seviyesinde gosterir.
- TLS script'i sertifika bilgisi okuyabiliyorsa temel zincir gorunurlugu saglar.

## .NET Uygulama Gerekli mi?
- Guvenlik prensiplerini teorik/kod ornegiyle gostermek icin zorunlu degil.
- Gercek dogrulama icin `.NET API` uzerinde policy, middleware ve endpoint testleriyle tamamlanmalidir.
