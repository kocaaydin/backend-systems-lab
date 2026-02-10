# Elasticsearch Roadmap

## 🎯 Amaç
Elasticsearch'te `mapping`, `analyzer`, `query DSL` kararlarının arama doğruluğu (relevance) ve performans üzerindeki etkisini ölçmek.

Hedef:
- "Neden sonuçlar alakasız geliyor?" sorusuna mapping/analyzer üzerinden cevap verebilmek,
- "Neden cluster yavaşladı?" sorusunda index lifecycle ve query maliyetini ayırabilmek.

## 🧩 Kavramlar (Nedir?)
- **Mapping:** Alan tiplerinin (`text`, `keyword`, `date` vb.) tanımı.
- **Analyzer:** Metni token'lara bölme + normalize etme kural seti.
- **Relevance:** Sonuçların sorguyla alakalı sıralanma kalitesi.
- **ILM (Index Lifecycle Management):** Index'in hot-warm-cold-delete yaşam döngüsü politikası.
- **Segment merge:** ES'in arka planda segment birleştirme işi; yoğunken IO/CPU baskısı yaratabilir.
- **search_after:** Derin pagination için `from/size` yerine önerilen yaklaşım.

## 🧪 Senaryolar

### 1. Mapping Hatası -> Relevance Sorunu
- **Case A (Bad):** `text/keyword` yanlış kullanımı.
- **Case B (Good):** alan amacına göre doğru mapping.
- **Beklenen fark:** Arama sonuç sıralaması anlamlı hale gelir.

### 2. Analyzer Etkisi
- **Case A (Bad):** Varsayılan analyzer ile domain'e uygun olmayan tokenization.
- **Case B (Good):** custom analyzer, normalizer, synonym.
- **Beklenen fark:** Fuzzy/search kalitesi artar, false-positive azalır.

### 3. Pagination ve Query Maliyeti
- **Case A (Bad):** Derin pagination (`from/size`) ile yüksek maliyet.
- **Case B (Good):** `search_after` / uygun strateji.
- **Beklenen fark:** Gecikme ve heap baskısı düşer.
- **Yorumlama ipucu:** Sayfa numarası büyüdükçe süre lineer artıyorsa pagination stratejisi sorunlu olabilir.

### 4. Index Lifecycle (ILM)
- **Case A (Bad):** Her index aynı politikada, sıcak shard baskısı.
- **Case B (Good):** hot-warm-cold ve retention politikası.
- **Beklenen fark:** Depolama ve sorgu maliyeti dengelenir.

## 📊 Ölçülecek Metrikler
- Query latency (p95/p99)
- Heap usage / GC
- Segment merge maliyeti
- Relevance quality (örnek query seti)

## 🧪 Kod Karşılaştırması
**Kötü Kullanım (sku = text):**
```json
"sku": { "type": "text" }
```

**İyi Kullanım (sku = keyword):**
```json
"sku": { "type": "keyword" }
```

## 🛠️ Nasıl Çalıştırılır?
```bash
cd scenarios/12-Elasticsearch
./run.sh
```

Çalışan dosyalar:
- `scenarios/12-Elasticsearch/run.sh`
- `scenarios/12-Elasticsearch/scripts/01_bad_mapping.sh`
- `scenarios/12-Elasticsearch/scripts/02_good_mapping.sh`
- `scenarios/12-Elasticsearch/k6/ElasticLab/search_load.js`

## 🧭 Notlar
- Elasticsearch primary data store değildir.
- Bulk API olmadan yüksek yazma trafiği zorlanır.
- Mapping baştan doğru tasarlanmalı, sonradan düzeltme maliyetlidir.
