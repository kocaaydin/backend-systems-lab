# MongoDB Roadmap

## 🎯 Amaç
MongoDB'de veri modelleme kararlarının (`embedding` vs `referencing`) sorgu maliyetine, tutarlılığa ve ölçeklenmeye etkisini gözlemlemek.

Hedef:
- "Hızlı geliştirme" tercihinin hangi noktada performans borcuna dönüştüğünü görmek,
- `index` ve `explain` çıktısını okuyarak root-cause bulabilmek.

## 🧩 Kavramlar (Nedir?)
- **Embedding:** İlişkili veriyi aynı document içine gömmek.
- **Referencing:** İlişkili veriyi ayrı collection'da tutup id ile bağlamak.
- **COLLSCAN:** Sorgunun tüm koleksiyonu taraması (genelde pahalı).
- **IXSCAN:** Index üzerinden tarama (genelde daha verimli).
- **Read preference:** Okumanın primary/secondary'den yapılma tercihi.
- **Replication lag:** Secondary node'un primary'yi ne kadar gecikmeyle takip ettiği.

## 🧪 Senaryolar

### 1. Embedding vs Referencing
- **Case A (Bad):** Aşırı büyüyen embed dökümanlar (document bloat).
- **Case B (Good):** Doğru yerde referans, doğru yerde embed.
- **Beklenen fark:** Case A'da update/read maliyeti artar, Case B'de daha dengeli performans.

### 2. Index Eksikliği ve Explain Analizi
- **Case A (Bad):** Sık filtrelenen alanda index yok.
- **Case B (Good):** Uygun single/compound index.
- **Beklenen fark:** Case A'da COLLSCAN, Case B'de IXSCAN eğilimi.
- **Yorumlama ipucu:** `nReturned` düşük ama `docsExamined` çok yüksekse index adayı vardır.

### 3. Aggregation Pipeline Maliyeti
- **Case A (Bad):** `$match` geç konumda, erken stage'lerde veri şişmesi.
- **Case B (Good):** `$match` erkende, projection ile daraltma.
- **Beklenen fark:** Bellek ve işlem süresi düşer.

### 4. Replica / Read Preference
- **Case A (Bad):** Her şeyi primary'den okumak.
- **Case B (Good):** Uygun read preference ile yük dağıtımı.
- **Beklenen fark:** Primary baskısı azalır, fakat stale read riski gözlenir.

## 📊 Ölçülecek Metrikler
- Query duration
- scanned docs vs returned docs
- CPU/memory pressure
- replication lag

## 🧪 Kod Karşılaştırması
**Kötü Kullanım (Index yok):**
```javascript
db.orders.find({ userId: 777, status: "SUCCESS" }).explain("executionStats")
// COLLSCAN ve docsExamined yüksek olabilir
```

**İyi Kullanım (Compound Index):**
```javascript
db.orders.createIndex({ userId: 1, status: 1 })
db.orders.find({ userId: 777, status: "SUCCESS" }).explain("executionStats")
// IXSCAN ve docsExamined düşüşü beklenir
```

## 🛠️ Nasıl Çalıştırılır?
```bash
cd scenarios/11-Mongo
./run.sh
```

Çalışan dosyalar:
- `scenarios/11-Mongo/run.sh`
- `scenarios/11-Mongo/scripts/01_seed.js`
- `scenarios/11-Mongo/scripts/02_bad_no_index.js`
- `scenarios/11-Mongo/scripts/03_good_with_index.js`
- `scenarios/11-Mongo/k6/MongoLab/api_template.js`

## 🧭 Notlar
- MongoDB transaction destekler, fakat maliyeti vardır.
- `MongoClient` tekil (singleton) kullanılmalıdır.
- Modelleme kararı, index kararından önce gelir.
