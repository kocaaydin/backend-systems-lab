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

- SELECT / INSERT / UPDATE / DELETE
- WHERE / ORDER BY / GROUP BY / HAVING
- JOIN türleri (INNER / LEFT / RIGHT)
- Subquery vs JOIN farkları
- NULL davranışı (IS NULL)

🎤:

> “JOIN genelde subquery’den daha performanslıdır.”
> 

---

## **2️⃣ Indexing (EN ÇOK SORULUR)**

### **🔹 Clustered Index**

- Clustered index nedir?
- Table başına neden tek?
- PK her zaman clustered mı olmalı?

### **🔹 Non-Clustered Index**

- Seek vs Scan
- Covering index
- Include column’lar

### **🔹 Index Design**

- Over-indexing riskleri
- Index fragmentation
- Rebuild vs Reorganize

🎤:

> “Yanlış index, index olmamasından kötüdür.”
> 

---

## **3️⃣ Execution Plan & Query Optimization**

- Execution plan nasıl okunur?
- Cost nedir?
- Key lookup problemi
- Parameter sniffing
- Statistics önemi

📌 **Gerçek senior konusu**

---

## **4️⃣ Transaction Management**

- ACID nedir?
- BEGIN / COMMIT / ROLLBACK
- Nested transaction gerçek mi?
- Long-running transaction riskleri

🎤:

> “Uzun transaction lock süresini uzatır.”
> 

---

## **5️⃣ Locking & Concurrency (🔥)**

- Shared / Exclusive lock
- Deadlock nedir?
- Deadlock nasıl tespit edilir?
- Isolation levels

### **Isolation Levels**

- Read Uncommitted
- Read Committed
- Repeatable Read
- Serializable
- Snapshot Isolation

🎤:

> “Isolation level performans ve tutarlılık trade-off’udur.”
> 

---

## **6️⃣ Stored Procedure vs Ad-hoc Query**

- Execution plan reuse
- Security avantajı
- Versiyonlama zorlukları
- Ne zaman SP, ne zaman ORM?

---

## **7️⃣ Data Modeling**

- Normalization (1NF–3NF)
- Denormalization ne zaman?
- Surrogate vs Natural key
- Soft delete vs hard delete

---

## **8️⃣ Pagination & Large Data**

- OFFSET / FETCH
- Keyset pagination
- Large table scan riskleri
- Batch processing

🎤:

> “Offset pagination büyük tablolarda performans sorunu yaratır.”
> 

---

## **9️⃣ Performance & Scalability**

- Read replica (concept)
- Partitioning
- TempDB kullanımı
- Connection pooling
- IO vs CPU bottleneck

---

## **🔟 Error Handling (SQL Server)**

- TRY / CATCH
- THROW vs RAISERROR
- Transaction rollback
- Error propagation

---

## **1️⃣1️⃣ Security**

- SQL Injection
- Parameterized query
- Least privilege
- Encryption at rest / in transit

---

## **1️⃣2️⃣ Backup, Restore & Reliability (Concept)**

- Full / Differential / Log backup
- Point-in-time recovery
- Disaster recovery farkındalığı

# Mongo

## **1️⃣ MongoDB Fundamentals**

- MongoDB nedir?
- NoSQL ne demek?
- MongoDB vs Relational DB farkları
- MongoDB ne zaman tercih edilir?
- MongoDB ne zaman KULLANILMAZ?
- Schema-less kavramı (gerçek anlamı)

---

## **2️⃣ Data Modeling (EN KRİTİK)**

- Document yapısı
- Embedded vs Reference
- One-to-many modelleme
- Many-to-many yaklaşımları
- Document growth problemi
- 16MB document limiti

---

## **3️⃣ CRUD Operations**

- insertOne / insertMany
- find / findOne
- updateOne / updateMany
- deleteOne / deleteMany
- upsert mantığı
- Partial update ($set, $push)

---

## **4️⃣ Indexing**

- Single field index
- Compound index
- Multikey index
- Text index
- TTL index
- Index selectivity

---

## **5️⃣ Query Performance & Explain**

- explain() kullanımı
- COLLSCAN vs IXSCAN
- Covered query
- Slow query tespiti
- Index kullanım analizi

---

## **6️⃣ Aggregation Framework**

- Pipeline mantığı
- $match
- $group
- $project
- $lookup
- $unwind
- Aggregation vs MapReduce

---

## **7️⃣ Transactions & Consistency**

- Single-document atomicity
- Multi-document transaction
- ACID desteği
- Transaction maliyeti
- Transaction ne zaman KULLANILMAZ?

---

## **8️⃣ Concurrency & Locking**

- Document-level locking
- Write concern
- Read concern
- Isolation davranışı

---

## **9️⃣ Replication & High Availability**

- Replica set nedir?
- Primary / Secondary
- Failover süreci
- Read preference

---

## **🔟 Sharding & Scalability (Senior)**

- Sharding nedir?
- Shard key seçimi
- Hot shard problemi
- Balancer nasıl çalışır?

---

## **1️⃣1️⃣ Schema Design Patterns**

- Bucket pattern
- Attribute pattern
- Polymorphic schema
- Outlier pattern

---

## **1️⃣2️⃣ Validation & Schema Evolution**

- Schema validation
- Required fields
- Versioned schema
- Migration stratejileri

---

## **1️⃣3️⃣ MongoDB & .NET Kullanımı**

- MongoClient lifecycle
- Connection pooling
- Async API kullanımı
- BSON serialization
- Index creation (code-first)

---

## **1️⃣4️⃣ Security (Concept)**

- Authentication
- Authorization
- Network security
- Encryption (at rest / in transit)

---

## **1️⃣5️⃣ MongoDB Ne Zaman KULLANILMAZ?**

- Heavy transaction gerektiren sistemler
- Complex JOIN ihtiyacı
- Raporlama ağırlıklı sistemler
- Strong consistency zorunluluğu

# Elastic Search

## **1️⃣ Elasticsearch Fundamentals**

- Elasticsearch nedir?
- Full-text search ne demek?
- Elasticsearch vs RDBMS farkları
- Elasticsearch vs MongoDB farkları
- Elasticsearch ne zaman tercih edilir?
- Elasticsearch ne zaman KULLANILMAZ?

🎤:

> “Elasticsearch bir search engine’dir, primary data store değildir.”
> 

---

## **2️⃣ Core Concepts (ÇOK SORULUR)**

- Cluster nedir?
- Node türleri
- Index / Document / Field
- Shard & Replica
- Primary vs Replica shard

🎤:

> “Shard sayısı ölçeklenebilirliği doğrudan etkiler.”
> 

---

## **3️⃣ Index & Mapping**

- Mapping nedir?
- Dynamic vs explicit mapping
- Field data types
- Text vs Keyword farkı
- Analyzer nedir?

📌 **Yanlış mapping = kötü performans**

---

## **4️⃣ Analysis & Text Processing**

- Analyzer bileşenleri
- Tokenizer
- Filter
- Stop words
- Stemming
- Custom analyzer

🎤:

> “Search kalitesi analyzer ile belirlenir.”
> 

---

## **5️⃣ Query DSL (🔥)**

- match
- term
- bool
- must / should / filter
- range
- multi-match
- fuzzy search

📌 **Filter context vs query context farkı çok sorulur**

---

## **6️⃣ Relevance & Scoring**

- TF-IDF / BM25
- Score nasıl hesaplanır?
- Boosting
- Relevance tuning

🎤:

> “Search’te doğru sonuç, hızlı sonuçtan daha değerlidir.”
> 

---

## **7️⃣ Aggregations**

- Bucket aggregations
- Metric aggregations
- Nested aggregations
- Aggregation vs SQL GROUP BY

---

## **8️⃣ Pagination & Performance**

- from / size limitleri
- Deep pagination problemi
- search_after
- scroll API ne zaman kullanılır?

---

## **9️⃣ Index Lifecycle & Data Management**

- Index lifecycle management (ILM)
- Rollover index
- Time-based index
- Retention stratejileri

---

## **🔟 Write & Ingestion**

- Indexing süreci
- Bulk API
- Refresh interval
- Near real-time search

🎤:

> “Bulk API olmadan yüksek write throughput olmaz.”
> 

---

## **1️⃣1️⃣ Consistency & Reliability**

- Refresh vs flush
- Write consistency
- Replication
- Data loss senaryoları

---

## **1️⃣2️⃣ Scaling & Performance (Senior)**

- Shard sizing
- Hot vs warm node
- Rebalancing
- Query vs indexing trade-off

---

## **1️⃣3️⃣ Monitoring & Troubleshooting**

- Slow query log
- Cluster health
- JVM heap kullanımı
- GC problemleri

---

## **1️⃣4️⃣ Security (Concept)**

- Authentication
- Authorization
- TLS
- Role-based access

---

## **1️⃣5️⃣ Elasticsearch & .NET Kullanımı**

- NEST / Elasticsearch.Net
- Connection management
- Mapping (code-first)
- Async search

---

## **1️⃣6️⃣ Elasticsearch Ne Zaman KULLANILMAZ?**

- Transactional sistemler
- Strong consistency zorunluluğu
- Primary data store ihtiyacı
- Küçük dataset & basit arama

