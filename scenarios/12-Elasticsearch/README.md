# Elasticsearch Senaryo Lab

## Amaç
Yanlis mapping/analyzer kararlarinin arama kalitesi ve performans etkisini gormek.

## Kavramlar (Nedir?)
- `mapping`: Alanin tipi. Ornek: `text` analiz edilir, `keyword` exact match icindir.
- `analyzer`: Metni tokenlara boler/normalize eder (kucuk harf, kok bulma vb.).
- `term query`: Exact eslesme arar (genelde `keyword` alanlarda dogru calisir).
- `match query`: Metin aramada analyzer kullanir.
- `relevance`: Sonuclarin alakali siralanma kalitesi.

## Neden Onemli?
- `sku` gibi exact alanlar `text` olursa beklenmedik sorgu sonucu gelebilir.
- Dogru mapping dogrulugu, query maliyetini ve arama kalitesini dogrudan etkiler.

## Dosya Haritasi
- `scenarios/12-Elasticsearch/docker-compose.yml`: Elasticsearch container
- `scenarios/12-Elasticsearch/scripts/01_bad_mapping.sh`: Kotu mapping
- `scenarios/12-Elasticsearch/scripts/02_good_mapping.sh`: Dogru mapping + query
- `scenarios/12-Elasticsearch/k6/ElasticLab/search_load.js`: Search endpoint k6 testi
- `scenarios/12-Elasticsearch/run.sh`: A/B akisini calistirir

## Calistirma
```bash
cd scenarios/12-Elasticsearch
./run.sh
```

## Ciktiyi Nasil Yorumlarim?
- `bad` mapping sonrasinda exact filtre beklendigi gibi davranmayabilir.
- `good` mapping ile ayni sorgu daha net ve tutarli sonuc verir.

## .NET Uygulama Gerekli mi?
- Mapping/query farkini gostermek icin `.NET` zorunlu degil.
- Ama son-kullanici gecikmesini gormek icin `.NET search endpoint` uzerinden k6 yapmak gerekir.
