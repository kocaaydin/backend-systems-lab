# Mongo Senaryo Lab

## Amaç
Mongo'da `index yok` ve `index var` farkini `explain` ile dogrudan gormek.

## Kavramlar (Nedir?)
- `COLLSCAN`: Mongo tum koleksiyonu tarar. Veri buyudukce maliyet hizla artar.
- `IXSCAN`: Mongo index uzerinden arar. Genelde daha az kayit incelenir.
- `docsExamined`: Query'nin inceledigi belge sayisi.
- `nReturned`: Gercekten donen belge sayisi.
- `compound index`: Birden fazla alani ayni index'te sirali tutan index.

## Neden Onemli?
- `nReturned` dusuk ama `docsExamined` cok yuksekse sorgu gereksiz tarama yapiyor demektir.
- Dogru index, ayni sonucu daha az CPU/IO ile uretir.

## Dosya Haritasi
- `scenarios/11-Mongo/docker-compose.yml`: Mongo container
- `scenarios/11-Mongo/scripts/01_seed.js`: Ornek veri uretir
- `scenarios/11-Mongo/scripts/02_bad_no_index.js`: COLLSCAN senaryosu
- `scenarios/11-Mongo/scripts/03_good_with_index.js`: IXSCAN senaryosu
- `scenarios/11-Mongo/k6/MongoLab/api_template.js`: Mongo kullanan API icin k6 sablonu
- `scenarios/11-Mongo/run.sh`: Seed + karsilastirma akisi

## Calistirma
```bash
cd scenarios/11-Mongo
./run.sh
```

## Ciktiyi Nasil Yorumlarim?
- `02_bad_no_index` icinde `stage: COLLSCAN` gorursen sorgu pahali davranir.
- `03_good_with_index` sonrasi `IXSCAN` ve daha dusuk `docsExamined` beklenir.

## .NET Uygulama Gerekli mi?
- Bu klasor DB davranisini dogrudan Mongo shell ile gosterir.
- Uygulama etkisini gormek icin `.NET API` tarafinda ayni sorguyu endpoint'e baglayip k6 ile olcmek daha dogrudur.
