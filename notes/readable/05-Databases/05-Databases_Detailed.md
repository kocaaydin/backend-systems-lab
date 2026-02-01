# Redis

## **1️⃣ Redis Fundamentals (Temel Şart)**

- Redis nedir? (In-memory data store)
- Redis hangi problemleri çözer?
- Redis ne zaman KULLANILMAZ?
- Redis vs DB farkları
- Redis persistence neden DB yerine geçmez?

🎤 Mülakat cümlesi:

> “Redis bir cache ya da state store’dur, primary data store değildir.”
> 

---

## **2️⃣ Redis Data Types (ÇOK SORULUR)**

### **🔹 String**

- Basic key-value
- TTL kullanımı
- Counter senaryoları

### **🔹 Hash**

- Object cache
- Partial update
- User profile cache

### **🔹 List**

- Queue mantığı
- FIFO işlemler
- Background job senaryosu

### **🔹 Set**

- Unique data
- Membership check
- Like / follow senaryosu

### **🔹 Sorted Set (🔥)**

- Score mantığı
- Leaderboard
- Rate limiting
- Time-based data

🎤:

> “Leaderboard için Sorted Set kullanırım.”
> 

---

## **3️⃣ TTL & Expiration Yönetimi**

- TTL neden kritik?
- Sliding expiration
- Absolute expiration
- Randomized TTL
- Stale data riskleri

📌 **Redis’te TTL = hayat kurtarıcı**

---

## **4️⃣ Cache Patterns (Redis ile)**

- Cache Aside (Lazy loading)
- Read Through (concept)
- Write Through / Write Behind (trade-off)
- Cache invalidation stratejileri

🎤:

> “Redis’te en güvenli yaklaşım Cache Aside’dır.”
> 

---

## **5️⃣ Cache Key Design (ÇOK KRİTİK)**

- Deterministic key
- Namespace kullanımı
- Versioned key
- Environment bazlı key

prod:user:profile:123:v2

## **6️⃣ Redis & High Traffic Problemleri**

- Cache stampede
- Hot key problemi
- Thundering herd
- TTL aynı anda bitmesi

Çözümler:

- Random TTL
- Lock
- Pre-warm cache

---

## **7️⃣ Concurrency & Atomic Operations**

- Atomic increment
- Race condition önleme
- Redis transactions (MULTI/EXEC)
- Lua script ne zaman kullanılır?

🎤:

> “Redis atomic operation’ları race condition’ı önlemek için kullanırım.”
> 

---

## **8️⃣ Distributed Lock (Senior Konu 🔥)**

- Lock neden gerekir?
- SETNX mantığı
- TTL’li lock
- Deadlock riskleri
- RedLock nedir, ne zaman kullanılır?

📌 **Mülakatta fark yaratan konu**

---

## **9️⃣ Rate Limiting (ÇOK SORULUR)**

- Token bucket
- Sliding window
- Fixed window
- Sorted set ile rate limit

🎤:

> “Rate limiting için Redis Sorted Set kullanırım.”
> 

---

## **🔟 Pub/Sub & Messaging (Concept)**

- Redis Pub/Sub nedir?
- Ne zaman kullanılır?
- Ne zaman KULLANILMAZ?
- Event loss riskleri

📌 **Event bus değildir**

---

## **1️⃣1️⃣ Persistence & Reliability**

- RDB snapshot
- AOF
- AOF rewrite
- Data loss senaryoları

🎤:

> “Redis persistence ayarları veri kaybı riskini belirler.”
> 

---

## **1️⃣2️⃣ Redis HA & Scaling (Concept)**

- Master–Replica
- Failover
- Redis Sentinel
- Redis Cluster (sharding)

📌 **Detay değil, mantık önemli**

---

## **1️⃣3️⃣ Redis & .NET Kullanımı**

- ConnectionMultiplexer lifecycle
- Singleton kullanımı
- Async Redis calls
- Serialization strategy

🎤:

> “ConnectionMultiplexer’ı singleton olarak kullanırım.”
> 

---

## **1️⃣4️⃣ Redis Ne Zaman KULLANILMAZ?**

- Çok kritik consistency
- Transaction-heavy işlemler
- Büyük binary data
- Uzun süreli kalıcı veri

# MSSQL

## **1️⃣ SQL Temelleri (Olmazsa Olmaz)**

- SELECT / INSERT / UPDATE / DELETE: Temel veri manipülasyon komutları (DML).
- WHERE / ORDER BY / GROUP BY / HAVING: Filtreleme, sıralama ve gruplama işlemleri (`Having` group sonrası filtreler).
- JOIN türleri (INNER / LEFT / RIGHT): Tabloları birleştirme yöntemleri (Kesişim, Sol öncelikli, Sağ öncelikli).
- Subquery vs JOIN farkları: Subquery iç içe sorgu, JOIN tabloları yan yana birleştirir; genelde JOIN daha performanslıdır.
- NULL davranışı (IS NULL): NULL bir değer değil, bilinmeyen durumudur; `= NULL` çalışmaz, `IS NULL` kullanılır.

🎤:

> “JOIN genelde subquery’den daha performanslıdır.”
> 

---

## **2️⃣ Indexing (EN ÇOK SORULUR)**

### **🔹 Clustered Index**

- Clustered index nedir: Verinin diskteki fiziksel sıralamasını belirleyen index (Telefon rehberi gibi).
- Table başına neden tek: Veri fiziksel olarak sadece bir şekilde sıralanabilir.
- PK her zaman clustered mı olmalı: Default öyledir ama zorunlu değildir (Identity ise iyidir, UUID ise kötüdür).

### **🔹 Non-Clustered Index**

- Seek vs Scan: Seek (Index ile direkt gitme - Hızlı) vs Scan (Tüm tabloyu/indexi tarama - Yavaş).
- Covering index: Sorgudaki tüm kolonların index içinde bulunması (Tabloya gitmeye gerek kalmaz - En hızlısı).
- Include column’lar: Index anahtarına dahil olmayan ama index yaprağında saklanan ek veriler.

### **🔹 Index Design**

- Over-indexing riskleri: Okuma hızlanır ama her yazma (Insert/Update) yavaşlar.
- Index fragmentation: Sık ekleme/silme sonucu indexin diskte dağınıklaşması.
- Rebuild vs Reorganize: Indexi tamamen yeniden oluşturma (Rebuild) vs Düzenleme (Reorganize).

🎤:

> “Yanlış index, index olmamasından kötüdür.”
> 

- Full / Differential / Log backup: Tam, Değişenler ve Log yedeği zinciri.
- Point-in-time recovery: Log yedekleri sayesinde "Saat 14:05:00" anına dönebilme.
- Disaster recovery farkındalığı: Sunucu tamamen giderse ne yapılacağı planı.

# Mongo

## **1️⃣ MongoDB Fundamentals**

- MongoDB nedir: JSON benzeri (BSON) belgeler saklayan NoSQL veritabanı.
- NoSQL ne demek: "Not Only SQL" veya ilişkisel olmayan veritabanı yaklaşımı.
- MongoDB vs Relational DB farkları: Sabit şema yok (Schema-less), tablo yok koleksiyon var, join yok embedding var.
- MongoDB ne zaman tercih edilir: Hızlı geliştirme, esnek şema, büyük veri ve yüksek yazma hızı gerektiğinde.
- MongoDB ne zaman KULLANILMAZ: Karmaşık transactionlar, çok fazla JOIN gerektiren raporlamalar için.
- Schema-less kavramı: Veritabanı şemayı zorlamaz ama uygulama kodu yine de bir şema bekler.

---

## **2️⃣ Data Modeling (EN KRİTİK)**

- Document yapısı: Key-value çiftlerinden oluşan JSON benzeri veri birimi.
- Embedded vs Reference: Veriyi içine gömmek (Hızlı okuma) vs ID ile referans vermek (Veri tutarlılığı/Normalize).
- One-to-many modelleme: Az sayıda ise Embedded (örn. Adresler), çok sayıda ise Reference (örn. Siparişler).
- Many-to-many yaklaşımları: İki tarafta da ID dizisi tutarak referanslama.
- Document growth problemi: Döküman güncellenip büyüdüğünde diskin yeniden yerleştirme yapması (Parçalanma).
- 16MB document limiti: Tek bir dökümanın boyutu 16MB'ı geçemez (Büyük veriler için GridFS).

---

## **3️⃣ CRUD Operations**

- insertOne / insertMany: Tek veya çoklu kayıt ekleme.
- find / findOne: Sorgulama komutları.
- updateOne / updateMany: Güncelleme komutları.
- deleteOne / deleteMany: Silme komutları.
- upsert mantığı: Kayıt varsa güncelle, yoksa yeni ekle (Update + Insert).
- Partial update ($set, $push): Dökümanın tamamını değil, sadece değişen alanını veya diziye eleman eklemeyi güncelleme.

---

## **4️⃣ Indexing**

- Single field index: Tek bir alana göre sıralama.
- Compound index: Birden fazla alanı içeren index (Sıralama yönü önemlidir).
- Multikey index: Dizi (Array) alanlarına index atma (Dizinin her elemanı için index giriş yapılır).
- Text index: Metin içi arama yapmak için.
- TTL index: Belirli bir süre sonra (Time To Live) dökümanın otomatik silinmesi (Log, Session vb.).
- Index selectivity: Index'in ne kadar özgün veri içerdiği (Yüksek selectivity = Hızlı sorgu).

---

## **5️⃣ Query Performance & Explain**

- explain() kullanımı: Sorgunun nasıl çalıştığını (Index kullandı mı, kaç döküman taradı) analiz etme.
- COLLSCAN vs IXSCAN: Collection Scan (Tüm tabloyu tara - Kötü) vs Index Scan (Indexle git - İyi).
- Covered query: Sorgunun sadece index verisiyle cevaplanabilmesi (Dökümana gitmeye gerek yok - Çok hızlı).
- Slow query tespiti: Belirli süreyi geçen sorguların loglanması (Profiler).
- Index kullanım analizi: Oluşturulan indexlerin gerçekten kullanılıp kullanılmadığının kontrolü.

---

## **6️⃣ Aggregation Framework**

- Pipeline mantığı: Veriyi aşama aşama (Stage) işleyerek dönüştürme (Linux pipe gibi).
- $match: Filtreleme (SQL WHERE).
- $group: Gruplama (SQL GROUP BY).
- $project: İstenen alanları seçme veya yeni alan türetme (SQL SELECT).
- $lookup: Başka koleksiyonla birleştirme (SQL LEFT OUTER JOIN).
- $unwind: Dizi elemanlarını ayrı dökümanlara ayırma (Flatten).
- Aggregation vs MapReduce: Aggregation daha hızlı ve modern, MapReduce eskidi (Legacy).

---

## **7️⃣ Transactions & Consistency**

- Single-document atomicity: Tek döküman üzerindeki işlemler her zaman atomiktir (Default).
- Multi-document transaction: Birden fazla dökümanı/koleksiyonu kapsayan ACID transaction (v4.0+).
- ACID desteği: MongoDB artık ilişkisel veritabanları gibi ACID destekler ama performans maliyeti vardır.
- Transaction maliyeti: Kilitleme ve koordinasyon yükü getirdiği için throughput düşer.
- Transaction ne zaman KULLANILMAZ: Mümkünse Data Model (Embedding) ile tek dökümanda iş bitirilmeli.

---

## **8️⃣ Concurrency & Locking**

- Document-level locking: WiredTiger motoru döküman bazlı kilitleme yapar (Eskiden Collection level idi).
- Write concern: Yazma işleminin ne kadar güvenli olacağı (1: Primary onayladı, Majority: Çoğunluk onayladı).
- Read concern: Okunan verinin tutarlılık seviyesi (Local, Available, Majority).
- Isolation davranışı: Transaction dışında dirty read yoktur, transaction içinde snapshot isolation vardır.

---

## **9️⃣ Replication & High Availability**

- Replica set nedir: Verinin kopyasını tutan sunucu grubu (Otomatik failover sağlar).
- Primary / Secondary: Yazma sadece Primary'e, okuma Secondary'den de yapılabilir.
- Failover süreci: Primary çökerse Secondary'ler oylama yapıp yeni Primary seçer.
- Read preference: Okumanın hangi node'dan yapılacağı tercihi (Primary, SecondaryPreferred vb.).

---

## **🔟 Sharding & Scalability (Senior)**

- Sharding nedir: Veriyi birden fazla sunucuya yatayda bölme (Horizontal Scaling).
- Shard key seçimi: Verinin hangi parçaya gideceğini belirleyen anahtar (Kardinalitesi yüksek olmalı).
- Hot shard problemi: Yanlış shard key seçimiyle tüm yükün tek sunucuya binmesi.
- Balancer nasıl çalışır: Arka planda veriyi shard'lar arasında dengeli dağıtan işlem.

---

## **1️⃣1️⃣ Schema Design Patterns**

- Bucket pattern: Zaman serisi verilerini (IoT) tek tek değil, zaman dilimlerine göre gruplayıp (Bucket) saklama.
- Attribute pattern: Dinamik özellikleri (Renk, Beden) key-value dizisi olarak saklayıp indexleme.
- Polymorphic schema: Aynı koleksiyonda farklı tipte dökümanlar saklama (Ürün -> Kitap, Gömlek).
- Outlier pattern: Çoğunluktan çok farklı (aşırı büyük) dökümanları ayrı yerde tutma.

---

## **1️⃣2️⃣ Validation & Schema Evolution**

- Schema validation: Koleksiyon seviyesinde kurallar tanımlayarak veri bütünlüğü sağlama (SQL constraint benzeri).
- Required fields: Zorunlu alan kontrolü.
- Versioned schema: Şema değişimlerini yönetmek için döküman içine versiyon alanı ekleme.
- Migration stratejileri: Eski veriyi okurken güncelleme (Lazy migration) veya toplu script çalıştırma.

---

## **1️⃣3️⃣ MongoDB & .NET Kullanımı**

- MongoClient lifecycle: Uygulama boyunca Singleton olmalıdır (Thread-safe ve connection pooling yönetir).
- Connection pooling: Arka planda açık bağlantıları yönetir, el ile aç/kapa yapılmaz.
- Async API kullanımı: Tüm G/Ç işlemleri async/await ile yapılmalıdır.
- BSON serialization: C# nesnelerinin BSON formatına dönüşümü ve attribute kontrolü (`[BsonElement]`).
- Index creation (code-first): Uygulama başlarken kod ile index tanımlama (Dikkatli kullanılmalı).

---

## **1️⃣4️⃣ Security (Concept)**

- Authentication: Kullanıcı kimlik doğrulama (SCRAM-SHA, LDAP).
- Authorization: Rol tabanlı yetkilendirme (RBAC).
- Network security: IP whitelist, VPC peering.
- Encryption (at rest / in transit): Disk şifreleme ve TLS kullanımı.

---

## **1️⃣5️⃣ MongoDB Ne Zaman KULLANILMAZ?**

- Heavy transaction gerektiren sistemler: Finansal defter kayıtları için zor olabilir.
- Complex JOIN ihtiyacı: Veri modeli çok ilişkiliyse SQL daha iyidir.
- Raporlama ağırlıklı sistemler: Analitik sorgularda SQL kadar güçlü değildir.
- Strong consistency zorunluluğu: Dağıtık yapıda anlık tutarlılık maliyetlidir.

# Elastic Search

## **1️⃣ Elasticsearch Fundamentals**

- Elasticsearch nedir: Dağıtık, RESTful arama ve analiz motoru (Lucene üzerine kurulu).
- Full-text search ne demek: Metin içindeki kelimeleri köklerine inerek (analiz) esnek arama.
- Elasticsearch vs RDBMS farkları: Şema esnek, transaction yok, join yok, okuma çok hızlı.
- Elasticsearch vs MongoDB farkları: ES bir arama motorudur, Mongo bir NoSQL DB'dir; ES'te veri kaybı riski daha fazladır.
- Elasticsearch ne zaman tercih edilir: Gelişmiş arama (Autosuggest, Fuzzy), log analizi ve metrik takibi için.
- Elasticsearch ne zaman KULLANILMAZ: Birincil veri kaynağı (Primary Data Store) olarak (güvenilirlik sorunu).

🎤:

> “Elasticsearch bir search engine’dir, primary data store değildir.”
> 

---

## **2️⃣ Core Concepts (ÇOK SORULUR)**

- Cluster nedir: Bir veya daha fazla Node'dan oluşan ES kümesi.
- Node türleri: Master (Yönetici), Data (Veri tutan), Ingest (Veri işleyen), Coordinator (Yönlendirici).
- Index / Document / Field: RDBMS karşılığı -> Database / Row / Column.
- Shard & Replica: Veriyi parçalara bölme (Shard) ve yedeğini alma (Replica).
- Primary vs Replica shard: Yazma Primary'e, okuma hem Primary hem Replica'ya; Replica failover sağlar.

🎤:

> “Shard sayısı ölçeklenebilirliği doğrudan etkiler.”
> 

---

## **3️⃣ Index & Mapping**

- Mapping nedir: Verinin tipini (String, Int, Date) belirleyen şema tanımı.
- Dynamic vs explicit mapping: Otomatik tip tanıma vs Elle (Strict) tanımlama (Explict önerilir).
- Field data types: `text` (aranabilir), `keyword` (filtrelenebilir), `long`, `date` vb.
- Text vs Keyword farkı: `text` analiz edilir (parçalanır), `keyword` olduğu gibi saklanır (exact match).
- Analyzer nedir: Metni token'lara ayıran ve filtreleyen bileşen.

📌 **Yanlış mapping = kötü performans**

---

## **4️⃣ Analysis & Text Processing**

- Analyzer bileşenleri: Character Filter -> Tokenizer -> Token Filter.
- Tokenizer: Metni kelimelere böler (Whitespace, Standard vb.).
- Filter: Token'ları işler (Küçük harfe çevir, gereksizleri at).
- Stop words: "ve", "ile", "the" gibi aramada önemsiz kelimelerin atılması.
- Stemming: Kelimeyi köküne indirme ("koşuyorum" -> "koş").
- Custom analyzer: İhtiyaca özel analiz zinciri kurma.

🎤:

> “Search kalitesi analyzer ile belirlenir.”
> 

---

## **5️⃣ Query DSL (🔥)**

- match: Full-text arama yapar, metni analiz eder.
- term: Exact match yapar, analiz etmez (ID, Status gibi alanlar).
- bool: Birden fazla koşulu birleştirir (SQL AND/OR).
- must / should / filter: Must (AND/Zorunlu), Should (OR/Skor artırır), Filter (Zorunlu ama skor hesaplamaz/Cached).
- range: Sayısal veya tarih aralığı sorgusu.
- multi-match: Aynı terimi birden fazla alanda arama.
- fuzzy search: Yazım hatalarını tolere eden arama (Levenshtein distance).

📌 **Filter context vs query context farkı çok sorulur**

---

## **6️⃣ Relevance & Scoring**

- TF-IDF / BM25: Kelimenin dökümanda geçme sıklığı (TF) ve geneldeki nadirliği (IDF) ile skor hesaplama algoritması.
- Score nasıl hesaplanır: Arama kriterlerine ne kadar uyduğuna göre `_score` değeri üretilir.
- Boosting: Belirli alanların (örn. Başlık) skora etkisini artırma (`title^2`).
- Relevance tuning: Kullanıcıya en doğru sonucu göstermek için skor ayarlamaları.

🎤:

> “Search’te doğru sonuç, hızlı sonuçtan daha değerlidir.”
> 

---

## **7️⃣ Aggregations**

- Bucket aggregations: Veriyi gruplama (Terms, Datetime Histogram) -> SQL `GROUP BY`.
- Metric aggregations: Hesaplama yapma (Avg, Sum, Max, Min).
- Nested aggregations: İç içe gruplamalar yapma.
- Aggregation vs SQL GROUP BY: ES aggregation çok daha hızlı ve yeteneklidir (Search sonuçları üzerinden çalışır).

---

## **8️⃣ Pagination & Performance**

- from / size limitleri: Standart sayfalama (Skip/Take), derin sayfalarda (Deep Paging) performans sorunu ve 10K limiti vardır.
- Deep pagination problemi: 10.000'den sonraki kayıtları çekmenin maliyetli olması.
- search_after: Cursor mantığıyla, bir önceki sonucun son değerinden devam etme (Hızlı).
- scroll API ne zaman kullanılır: Tüm veriyi çekmek (Dump/Backup) gerektiğinde.

---

## **9️⃣ Index Lifecycle & Data Management**

- Index lifecycle management (ILM): Indexlerin zamanla Hot -> Warm -> Cold -> Delete evrelerinden geçmesi.
- Rollover index: Belirli boyuta veya süreye ulaşan indexin yenisine geçmesi (log-001 -> log-002).
- Time-based index: Günlük/Aylık log indexleri (logs-2023.10).
- Retention stratejileri: Eski verinin otomatik silinmesi veya arşive alınması.

---

## **🔟 Write & Ingestion**

- Indexing süreci: Dökümanın analiz edilip Inverted Index'e yazılması.
- Bulk API: Tek tek yerine toplu yazma (Performans için şart).
- Refresh interval: Yazılan verinin aranabilir olma süresi (Default 1sn, artırılırsa yazma hızlanır).
- Near real-time search: Verinin yazıldıktan kısa süre sonra (Refresh süresi) aranabilir olması.

🎤:

> “Bulk API olmadan yüksek write throughput olmaz.”
> 

---

## **1️⃣1️⃣ Consistency & Reliability**

- Refresh vs flush: Refresh bellekteki segmenti açar, Flush diske (Lucene Commit) yazar.
- Write consistency: `wait_for_active_shards` ayarı ile kaç kopyaya yazılacağının garantisi.
- Replication: Veri yedekliliği.
- Data loss senaryoları: Translog (Transaction Log) diske yazılmadan sunucu kapanırsa veri kaybı olabilir.

---

## **1️⃣2️⃣ Scaling & Performance (Senior)**

- Shard sizing: İdeal shard boyutu 10GB-50GB arasıdır.
- Hot vs warm node: Hızlı SSD (Hot - Aktif yazma/okuma) vs Yavaş HDD (Warm - Arşiv).
- Rebalancing: Node eklenip çıkarıldığında shardların otomatik dengelenmesi.
- Query vs indexing trade-off: Indexleme hızı ile sorgu hızı ters orantılı olabilir (Refresh interval).

---

## **1️⃣3️⃣ Monitoring & Troubleshooting**

- Slow query log: Belirli süreyi aşan sorguların loglanması.
- Cluster health: Green (Her şey tam), Yellow (Replica eksik, veri tam), Red (Primary eksik, veri kaybı).
- JVM heap kullanımı: Belleğin %50'si Heap'e, %50'si OS cache'e verilmeli (Max 32GB kuralı).
- GC problemleri: Yetersiz bellek veya yanlış yapılandırma sonucu Stop-the-world GC duraksamaları.

---

## **1️⃣4️⃣ Security (Concept)**

- Authentication: Kullanıcı girişi (Native, LDAP, OIDC).
- Authorization: Index ve alan bazlı yetkilendirme.
- TLS: Node'lar arası ve Client-Server arası şifreli iletişim.
- Role-based access: Okuma/Yazma yetkilerinin rollere atanması.

---

## **1️⃣5️⃣ Elasticsearch & .NET Kullanımı**

- NEST / Elasticsearch.Net: Eski `NEST` (High-level) vs Yeni `Elastic.Clients.Elasticsearch` kütüphanesi.
- Connection management: Tek bir `ElasticClient` instance (Singleton) kullanılmalı.
- Mapping (code-first): C# sınıflarından AutoMap veya Fluent API ile mapping oluşturma.
- Async search: Network I/O olduğu için her zaman async metotlar kullanılmalı.

---

## **1️⃣6️⃣ Elasticsearch Ne Zaman KULLANILMAZ?**

- Transactional sistemler: Banka bakiyesi yönetimi için uygun değil.
- Strong consistency zorunluluğu: Dağıtık yapısı gereği anlık tutarlılık zordur.
- Primary data store ihtiyacı: Veri kaybı riski nedeniyle tek kaynak olmamalıdır.
- Küçük dataset & basit arama: SQL `LIKE` sorgusu yetiyorsa gereksiz maliyettir.

