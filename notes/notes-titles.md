
# [**Veri Yapıları**]

## **🧠 Temel Kavramlar**

- Big-O Notation (O(1), O(log n), O(n), O(n log n), O(n²))
- Time Complexity vs Space Complexity
- Worst / Average / Best Case
- Amortized Complexity
- Mutable vs Immutable collections

---

## **📦 Array**

- Array temel yapısı ve index erişimi
- Array insert / delete maliyeti
- Two Sum problemi
- Array rotate
- Duplicate eleman bulma
- Max / Min eleman bulma
- C# T[] kullanımı
- Span<T> nedir, ne zaman kullanılır

---

## **📋 List (Dynamic Array)**

- List vs Array farkları
- Capacity vs Count
- Amortized Add() maliyeti
- Listeyi ters çevirme
- Duplicate silme
- En sık geçen elemanı bulma
- C# List<T> metotları

---

## **🔗 Linked List**

- Singly Linked List mantığı
- Doubly Linked List mantığı
- Head / Tail kavramları
- Linked list traversal
- Ortadaki elemanı bulma
- Linked list reverse
- Cycle detection
- Array vs LinkedList karşılaştırması

---

## **📚 Stack (LIFO)**

- Stack temel mantığı
- Push / Pop / Peek
- Parantez kontrolü problemi
- String ters çevirme
- Undo / Redo senaryosu
- Call stack nasıl çalışır
- C# Stack<T> kullanımı

---

## **📥 Queue (FIFO)**

- Queue temel mantığı
- Enqueue / Dequeue
- Producer – Consumer problemi
- BFS algoritmasında queue kullanımı
- Queue vs Stack farkları
- C# Queue<T> kullanımı
- ConcurrentQueue<T> nedir

---

## **🗂️ Dictionary / Hash Table ⭐**

- Hashing mantığı
- Collision nedir
- Chaining vs Open Addressing
- Lookup neden O(1)
- Duplicate eleman bulma
- Frequency counter
- Two Sum (Dictionary ile)
- C# Dictionary<TKey, TValue>
- ConcurrentDictionary kullanımı
- HashSet<T> farkı

---

## **🧩 Set**

- Set temel mantığı
- Unique eleman garantisi
- İki listede ortak eleman bulma
- Subset kontrolü
- C# HashSet<T> kullanımı

---

## **🌳 Tree**

- Tree temel kavramları (root, leaf, height)
- Binary Tree
- Binary Search Tree (BST)
- Preorder traversal
- Inorder traversal
- Postorder traversal
- Level Order traversal (BFS)
- Tree yüksekliği bulma
- BST doğrulama
- DFS vs BFS farkı

---

## **⛰️ Heap / Priority Queue**

- Min Heap mantığı
- Max Heap mantığı
- En büyük K eleman problemi
- En küçük K eleman problemi
- Task scheduling senaryosu
- C# PriorityQueue<TElement, TPriority>

---

## **🕸️ Graph**

- Graph temel kavramları
- Directed vs Undirected graph
- Adjacency List
- Adjacency Matrix
- BFS implementasyonu
- DFS implementasyonu
- Cycle detection
- Graph vs Tree farkları

---

## **🔀 Sorting & Searching**

- Bubble Sort
- Selection Sort
- Insertion Sort
- Merge Sort
- Quick Sort
- Linear Search
- Binary Search
- Binary search şartları
- Hangi sort ne zaman kullanılır

# **Design Patterns**

## **🧠 Genel Kavramlar**

- Design Pattern nedir
- Neden ihtiyaç duyulur
- Pattern vs Anti-pattern
- Over-engineering nedir
- Pattern seçerken nelere dikkat edilir

---

## **🏗️ Creational Patterns**

- Singleton – tanım ve kullanım amacı
- Singleton thread-safe implementasyon
- Singleton dezavantajları
- Factory Method
- Abstract Factory
- Builder
- Prototype
- Dependency Injection ile ilişkisi

---

## **🧩 Structural Patterns**

- Adapter
- Facade
- Decorator
- Proxy
- Composite
- Bridge
- Flyweight
- Bu pattern’lerin gerçek proje senaryoları

---

## **🔁 Behavioral Patterns ⭐**

- Strategy
- Observer
- Command
- Mediator
- Chain of Responsibility
- State
- Template Method
- Iterator
- Visitor
- Behavioral pattern’ler ne zaman tercih edilir

---

## **🗄️ Backend & Architecture Odaklı Pattern’ler ⭐⭐⭐**

- Repository Pattern
- Unit of Work
- CQRS (Command / Query Separation)
- Mediator Pattern (MediatR)
- Specification Pattern
- Factory + Strategy birlikte kullanımı
- Clean Architecture ile ilişkisi

---

## **🌐 Distributed / Microservice Pattern’leri**

- API Gateway Pattern
- Circuit Breaker
- Retry Pattern
- Bulkhead
- Saga Pattern
- Event-Driven Architecture
- Outbox Pattern
- Idempotency Pattern

---

## **🔐 Security & Cross-Cutting Pattern’ler**

- Dependency Injection
- Decorator ile logging
- Proxy ile caching
- Aspect Oriented Programming (AOP)
- Cross-cutting concern nedir

---

## **🧪 Testing ile İlgili Pattern’ler**

- Test Double kavramı
- Mock
- Stub
- Fake
- Dependency Injection ile test edilebilirlik
- Arrange-Act-Assert

# C# ve .NET

## **🧠 .NET & Runtime Temelleri**

- .NET nedir (Framework vs Core vs .NET)
- CLR rolü
- Runtime vs SDK
- JIT compilation
- IL (Intermediate Language)
- Assembly nedir
- Stack vs Heap
- Managed vs Unmanaged code
- Garbage Collector genel mantığı

---

## **🧱 C# Tip Sistemi**

- Value Type vs Reference Type
- struct vs class
- record nedir
- Immutable object kavramı
- readonly vs const
- ref, out, in
- Boxing & Unboxing
- Nullable value types
- Nullable reference types
- Tip Sistemi & Memory / Performance

---

## **🧬 OOP (C# Özel)**

- Encapsulation
- Inheritance
- Polymorphism
- Abstraction
- Interface vs Abstract class
- Virtual / Override
- Sealed class / method
- Multiple inheritance neden yok

---

## **🧹 Memory Management**

- Garbage Collection generations (0,1,2)
- GC ne zaman çalışır
- Finalizer (~ClassName)
- IDisposable
- using statement
- Dispose pattern
- Memory leak senaryoları
- Large Object Heap (LOH)

---

## **🧵 Async & Concurrency (C# Seviyesi)**

- async / await çalışma mantığı
- Task vs Thread
- Task.Run ne zaman kullanılmalı
- Deadlock senaryosu
- lock keyword
- SemaphoreSlim
- Thread-safe collections
- ConfigureAwait(false)

---

## **🧩 Delegates, Events, Lambdas**

- Delegate nedir
- Func / Action / Predicate
- Multicast delegate
- Event mantığı
- Event vs Delegate farkı
- Custom event yazma
- Lambda expression kullanımı

---

## **🧮 LINQ**

- LINQ to Objects
- Deferred execution
- Immediate execution
- Select, Where, Any, All
- First, FirstOrDefault
- Single vs First
- GroupBy
- Join
- LINQ performans tuzakları

---

## **🧰 Collections (C# / .NET)**

- Array
- List<T>
- Dictionary<TKey, TValue>
- HashSet<T>
- Stack<T>
- Queue<T>
- ConcurrentDictionary
- Immutable collections
- Doğru collection seçimi

---

## **⚠️ Exception Handling**

- try / catch / finally
- Exception propagation
- Custom exception yazma
- Checked vs unchecked exception
- Exception performance etkisi
- Exception best practices

---

## **🧠 Advanced C#**

- Generics
- Generic constraints
- Covariance / Contravariance
- Reflection
- Attributes
- Expression Trees
- Span / Memory
- ValueTask
- Unsafe code (temel seviye)

---

## **🏗️ ASP.NET Core – Framework Temelleri**

- ASP.NET Core request lifecycle
- Middleware pipeline
- Middleware yazma
- Minimal API vs Controller
- Routing (attribute / conventional)
- Endpoint routing
- Model Binding
- Model Validation

---

## **💉 Dependency Injection (.NET)**

- Built-in DI container
- Transient lifetime
- Scoped lifetime
- Singleton lifetime
- Lifetime hataları
- Constructor injection
- Service Locator anti-pattern

---

## **🎯 Filters & Pipeline**

- Action Filters
- Authorization Filters
- Resource Filters
- Exception Filters
- Global filter kullanımı
- Filter vs Middleware farkı

---

## **⚙️ Configuration (.NET)**

- appsettings.json
- Environment bazlı config
- IConfiguration
- IOptions
- IOptionsSnapshot
- IOptionsMonitor

# Diğer Konular

## **1️⃣ SOLID Principles (Derinlik Önemli)**

### **🔹 S — Single Responsibility Principle**

- Bir class’ın **tek değişme sebebi** ne demek?
- SRP ihlali örneklerini tanıyabilme
- Service + Validator + Mapper ayrımı
- “Fat Service” problemini anlatabilme

---

### **🔹 O — Open / Closed Principle**

- Mevcut kodu değiştirmeden genişletme
- Strategy Pattern ile OCP
- Polymorphism vs if/else zinciri
- Feature eklerken neden refactor gerekmez?

---

### **🔹 L — Liskov Substitution Principle**

- Base class yerine derived class kullanıldığında bozulma
- Exception fırlatma kuralları
- Precondition / Postcondition ihlali
- Gerçek hayatta LSP örnekleri

---

### **🔹 I — Interface Segregation Principle**

- “Fat interface” problemi
- Küçük, amaç odaklı interface’ler
- Read / Write interface ayrımı
- ISP ihlalinin test yazmayı zorlaştırması

---

### **🔹 D — Dependency Inversion Principle**

- High-level module → abstraction bağımlılığı
- Constructor injection
- DIP vs DI farkı
- Mock edilebilirlik

---

## **2️⃣ Dağıtık Mimari Kalıpları (Distributed Patterns)**

### **🔹 Microservices Fundamentals**

- Monolith vs Microservice
- Service boundary nasıl çizilir?
- Database per service neden önemli?
- Distributed system trade-off’ları

---

### **🔹 API Communication Patterns**

- Synchronous vs Asynchronous
- REST vs Messaging
- API Gateway rolü
- Backward compatibility

---

### **🔹 Resiliency Patterns**

- Circuit Breaker
- Retry with backoff
- Timeout stratejileri
- Bulkhead pattern

👉 **Mülakat sorusu:**

“Bir servis çökerse sistemi nasıl ayakta tutarsın?”

---

### **🔹 Consistency & Reliability**

- CAP Theorem
- Eventual consistency
- Idempotency
- Exactly-once mümkün mü?

---

### **🔹 Saga Pattern**

- Choreography vs Orchestration
- Compensating transaction
- Saga ne zaman tercih edilir?
- Distributed transaction neden kötü?

---

## **3️⃣ Cache Stratejileri (Redis’e Girmeden)**

### **🔹 Cache Temelleri**

- Cache neden kullanılır?
- Cache ne zaman KULLANILMAZ?
- Data consistency riskleri
- Cache eviction mantığı

---

### **🔹 Cache Patterns**

- Cache Aside (Lazy loading)
- Read Through
- Write Through
- Write Behind

👉 **En sık kullanılan:** Cache Aside

---

### **🔹 Invalidation Stratejileri**

- TTL kullanımı
- Manual invalidation
- Versioned cache key
- Event-based invalidation

---

### **🔹 Cache Scope**

- In-memory cache
- Distributed cache
- Per-user vs global cache
- Cache stampede problemi

## **4️⃣ N+1 Problem**

- N+1 query problemi nedir?
- ORM kullanırken nasıl ortaya çıkar?
- Lazy loading vs Eager loading çözümü
- Batch fetch / Include / Join kullanımı

🎤

> “N+1 problemi performans düşmanı, ORM’de eager loading ile çözülür.”
> 

---

## **5️⃣ REST & API Design**

- RESTful API prensipleri
- Stateless API
- Resource vs Endpoint
- HTTP Methods (GET, POST, PUT, DELETE)
- Status code kullanımı (200, 201, 204, 400, 401, 404, 500)
- HATEOAS (concept)

🎤

> “REST API tasarımında resource-first ve stateless yaklaşım önemlidir.”
> 

---

## **6️⃣ gRPS (Global Requests Per Second / Throughput)**

- RPS / throughput nedir?
- API limit ve throttling
- Load testing senaryoları
- Capacity planning

🎤

> “Sistem tasarımında RPS’yi bilmek ölçeklenebilirlik için kritik.”
> 

---

## **7️⃣ Authentication & Authorization**

- JWT / OAuth 2.0 / OpenID Connect
- Session-based auth
- Token expiration
- Role-based access (RBAC)
- Claims-based auth

🎤

> “Authentication kullanıcıyı doğrular, authorization yetkiyi belirler.”
> 

---

## **8️⃣ Hashing Algorithms**

- MD5 (deprecated)
- SHA-1 / SHA-2
- scrypt / bcrypt / Argon2
- Password hashing best practices
- Salting

🎤

> “Password hash’leri için bcrypt veya Argon2 tercih edilir, MD5 kullanılmaz.”
> 

---

## **9️⃣ CORS (Cross-Origin Resource Sharing)**

- CORS nedir?
- Same-origin policy
- Access-Control-Allow-Origin header
- Preflight request (OPTIONS)
- Security riskleri

🎤

> “CORS frontend ve backend arasında güvenli cross-origin iletişim sağlar.”
> 

---

## **🔟 OWASP Top 10**

- Injection
- Broken Authentication
- Sensitive Data Exposure
- XML External Entities
- Broken Access Control
- Security Misconfiguration
- Cross-Site Scripting (XSS)
- Insecure Deserialization
- Using Components with Known Vulnerabilities
- Insufficient Logging & Monitoring

🎤

> “Backend geliştirmede OWASP Top 10’u bilmek kritik güvenlik farkındalığı sağlar.”
> 

---

## **1️⃣1️⃣ SSL / TLS**

- SSL vs TLS farkı
- HTTPS kullanımı
- Certificates / PKI
- TLS handshake ve encryption
- Certificate rotation & renewal

🎤

> “Tüm client-server trafiği TLS ile şifrelenmelidir.”
> 

---

## **1️⃣2️⃣ Two-Factor Authentication (2FA)**

- SMS / Email / Authenticator app
- Time-based OTP (TOTP)
- Backup codes
- Recovery flow

🎤

> “2FA ile güvenlik, password + second factor ile güçlendirilir.”
> 

---

## **1️⃣3️⃣ Git & Version Control**

- Commit / Push / Pull / Fetch / Merge / Rebase
- Branching strategy (Git Flow / trunk-based)
- Pull request workflow
- Tag / Release
- Conflict resolution

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

# Docker

## **1️⃣ Docker Fundamentals**

- Docker nedir?
- Container vs Virtual Machine
- Docker hangi problemleri çözer?
- Docker ne zaman KULLANILMAZ?
- Image vs Container farkı

🎤:

> “Docker uygulamayı environment’tan bağımsız hale getirir.”
> 

---

## **2️⃣ Docker Architecture**

- Docker Engine
- Docker Daemon
- Docker Client
- Docker Registry (Docker Hub)
- Image layers mantığı

---

## **3️⃣ Docker Image & Dockerfile (🔥)**

- Dockerfile nedir?
- FROM / RUN / COPY / ADD
- CMD vs ENTRYPOINT
- EXPOSE
- ENV / ARG farkı
- .dockerignore

🎤:

> “Dockerfile ne kadar küçükse o kadar iyidir.”
> 

---

## **4️⃣ Image Optimization & Best Practices**

- Multi-stage build
- Küçük base image seçimi (alpine)
- Layer caching mantığı
- Gereksiz file kopyalamamak
- Build context küçültme

---

## **5️⃣ Container Lifecycle**

- create / start / stop / restart
- Container state’leri
- Graceful shutdown
- Restart policies

---

## **6️⃣ Networking**

- Bridge network
- Host network
- Overlay network (concept)
- Port mapping
- Container-to-container communication

🎤:

> “Container’lar default olarak isolated network’te çalışır.”
> 

---

## **7️⃣ Volumes & Persistence (ÇOK SORULUR)**

- Volume nedir?
- Bind mount vs volume
- Data persistence mantığı
- Stateless container yaklaşımı

📌:

> “Container stateless, data dışarıda.”
> 

---

## **8️⃣ Environment & Configuration**

- ENV variables
- Secrets yönetimi
- Config injection
- 12-factor app prensibi

---

## **9️⃣ Docker Compose**

- docker-compose.yml
- Multi-container setup
- Service dependency
- Network & volume tanımı
- Local development senaryoları

---

## **🔟 Security Best Practices**

- Root user ile çalışmamak
- Image scanning
- Minimal image kullanımı
- Secrets image içine koymamak

---

## **1️⃣1️⃣ Logging & Monitoring**

- stdout / stderr logging
- Log driver’lar
- Container healthcheck
- Resource usage (CPU / memory)

---

## **1️⃣2️⃣ Docker & CI/CD**

- Docker build pipeline
- Image tagging
- Push / pull registry
- Versioning strategy

---

## **1️⃣3️⃣ Docker vs Kubernetes (Concept)**

- Docker ne yapar?
- Kubernetes ne yapar?
- Ne zaman sadece Docker yeter?
- Ne zaman K8s gerekir?

---

## **1️⃣4️⃣ Production Anti-Patterns**

- Container içinde DB
- Large image’lar
- Hardcoded config
- State tutan container

# Kubernetes

## **1️⃣ Kubernetes Fundamentals**

- Kubernetes nedir?
- Kubernetes hangi problemi çözer?
- Docker vs Kubernetes farkı
- Kubernetes ne zaman GEREKLİ DEĞİL?
- Kubernetes cluster kavramı

🎤

> “Kubernetes container orchestration platformudur.”
> 

---

## **2️⃣ Kubernetes Architecture (ÇOK SORULUR)**

- Control Plane nedir?
- Node nedir?
- Master / Worker node farkı
- kube-apiserver
- etcd
- scheduler
- controller-manager

🎤

> “etcd cluster state’in tek kaynağıdır.”
> 

---

## **3️⃣ Core Objects (OLMAZSA OLMAZ)**

### **🔹 Pod**

- Pod nedir?
- Pod neden container’dan farklı?
- Pod lifecycle
- Multi-container pod senaryosu

### **🔹 Deployment**

- Deployment nedir?
- ReplicaSet ilişkisi
- Rolling update
- Rollback

### **🔹 Service**

- ClusterIP
- NodePort
- LoadBalancer
- Service discovery

🎤

> “Pod’lar ephemeral, Service’ler stable’dır.”
> 

---

## **4️⃣ Configuration Management**

- ConfigMap
- Secret
- Environment variable injection
- Volume ile config bağlama

📌

> “Config image’te değil, Kubernetes objesinde olmalı.”
> 

---

## **5️⃣ Networking (🔥)**

- Pod-to-pod communication
- Service-to-pod routing
- DNS (CoreDNS)
- Ingress nedir?
- Ingress Controller

🎤

> “Ingress dış dünyaya açılan kapıdır.”
> 

---

## **6️⃣ Storage & Persistence**

- Volume nedir?
- PersistentVolume (PV)
- PersistentVolumeClaim (PVC)
- StorageClass
- Stateful vs Stateless app

📌

> “Pod gider, volume kalır.”
> 

---

## **7️⃣ Scaling & Availability**

- Replica count
- Horizontal Pod Autoscaler (HPA)
- CPU / Memory based scaling
- Self-healing mantığı

🎤

> “Kubernetes failed pod’ları otomatik ayağa kaldırır.”
> 

---

## **8️⃣ Resource Management**

- CPU requests / limits
- Memory requests / limits
- OOMKilled nedir?
- Resource starvation

---

## **9️⃣ Health Checks (ÇOK SORULUR)**

- Liveness probe
- Readiness probe
- Startup probe
- Traffic yönetimi

🎤

> “Readiness false ise pod alive ama traffic almaz.”
> 

---

## **🔟 Deployment Strategies (Senior Konu)**

- Rolling update
- Recreate
- Blue-Green
- Canary deployment

---

## **1️⃣1️⃣ Security (Concept)**

- RBAC
- ServiceAccount
- Namespace izolasyonu
- Pod Security Context
- Network Policy

---

## **1️⃣2️⃣ Observability**

- Logs (kubectl logs)
- Metrics (Prometheus)
- Tracing
- Alerts

📌

> “Gözlemleyemediğin sistemi yönetemezsin.”
> 

---

## **1️⃣3️⃣ Kubernetes & CI/CD**

- Image tagging strategy
- Deployment automation
- Rollback senaryosu
- GitOps (concept)

---

## **1️⃣4️⃣ Production Anti-Patterns**

- Pod içinde state tutmak
- Hardcoded config
- Limits tanımlamamak
- Tek replica critical service

---

## **1️⃣5️⃣ Kubernetes Ne Zaman KULLANILMAZ?**

- Küçük projeler
- Tek servis
- Düşük trafik
- Ops ekibi yoksa

# RabbitMQ

## **1️⃣ RabbitMQ Fundamentals**

- RabbitMQ nedir?
- Message broker kavramı
- Queue vs Topic vs Exchange
- RabbitMQ ne zaman tercih edilir?
- RabbitMQ ne zaman KULLANILMAZ?

🎤

> “RabbitMQ uygulamalar arası asenkron iletişim sağlar.”
> 

---

## **2️⃣ Core Concepts**

- Producer
- Consumer
- Queue
- Exchange (Direct / Fanout / Topic / Headers)
- Binding
- Routing key
- Virtual host (vhost)

---

## **3️⃣ Messaging Patterns**

- Work Queue (Task Queue)
- Publish / Subscribe
- Routing / Topic Exchange
- RPC over RabbitMQ
- Dead Letter Exchange

🎤:

> “Dead Letter Queue, mesaj işlenemezse başka kuyrukta toplanır.”
> 

---

## **4️⃣ Message Delivery Semantics**

- At-most-once
- At-least-once
- Exactly-once (concept)
- Acknowledgements (ACK / NACK)
- Durable queues & persistent messages

---

## **5️⃣ Queue & Exchange Management**

- Queue durability
- Auto-delete queue
- Exclusive queue
- TTL & message expiration
- Max-length / max-priority

---

## **6️⃣ Concurrency & Scaling**

- Prefetch count
- Multiple consumers per queue
- Consumer acknowledgment
- Load balancing across consumers

🎤:

> “Prefetch sayısı tüketiciye düşen mesaj yükünü kontrol eder.”
> 

---

## **7️⃣ Reliability & Fault Tolerance**

- Mirrored / quorum queues
- High availability cluster
- Network partition handling
- Publisher confirms

---

## **8️⃣ Performance Tuning**

- Connection & channel management
- Batch publish
- Consumer concurrency
- Persistent vs transient messages trade-off

---

## **9️⃣ Monitoring & Observability**

- RabbitMQ Management UI
- Metrics (queue length, publish rate, consumer count)
- Alerts (unacked messages, queue growth)
- Logs

---

## **🔟 Security**

- Authentication & authorization
- User / vhost permissions
- TLS encryption
- Policy management

---

## **1️⃣1️⃣ RabbitMQ & .NET Integration**

- RabbitMQ.Client usage
- Connection / channel lifecycle
- Async consumer
- Retry & Dead-letter handling

---

## **1️⃣2️⃣ Anti-Patterns & Pitfalls**

- Queue overload → OOM
- Long-running consumer without ACK
- Single point of failure (standalone RabbitMQ)
- Persistent messages everywhere → disk IO bottleneck

---

## **1️⃣3️⃣ RabbitMQ Ne Zaman KULLANILMAZ?**

- Çok düşük latency gereken işlem
- Small-scale, simple CRUD
- Strong consistency gerektiren transaction-heavy işlem
- Stateful communication yeterli ise

# Kafka

## **1️⃣ RabbitMQ Fundamentals**

- RabbitMQ nedir?
- Message broker kavramı
- Queue vs Topic vs Exchange
- RabbitMQ ne zaman tercih edilir?
- RabbitMQ ne zaman KULLANILMAZ?

🎤

> “RabbitMQ uygulamalar arası asenkron iletişim sağlar.”
> 

---

## **2️⃣ Core Concepts**

- Producer
- Consumer
- Queue
- Exchange (Direct / Fanout / Topic / Headers)
- Binding
- Routing key
- Virtual host (vhost)

---

## **3️⃣ Messaging Patterns**

- Work Queue (Task Queue)
- Publish / Subscribe
- Routing / Topic Exchange
- RPC over RabbitMQ
- Dead Letter Exchange

🎤:

> “Dead Letter Queue, mesaj işlenemezse başka kuyrukta toplanır.”
> 

---

## **4️⃣ Message Delivery Semantics**

- At-most-once
- At-least-once
- Exactly-once (concept)
- Acknowledgements (ACK / NACK)
- Durable queues & persistent messages

---

## **5️⃣ Queue & Exchange Management**

- Queue durability
- Auto-delete queue
- Exclusive queue
- TTL & message expiration
- Max-length / max-priority

---

## **6️⃣ Concurrency & Scaling**

- Prefetch count
- Multiple consumers per queue
- Consumer acknowledgment
- Load balancing across consumers

🎤:

> “Prefetch sayısı tüketiciye düşen mesaj yükünü kontrol eder.”
> 

---

## **7️⃣ Reliability & Fault Tolerance**

- Mirrored / quorum queues
- High availability cluster
- Network partition handling
- Publisher confirms

---

## **8️⃣ Performance Tuning**

- Connection & channel management
- Batch publish
- Consumer concurrency
- Persistent vs transient messages trade-off

---

## **9️⃣ Monitoring & Observability**

- RabbitMQ Management UI
- Metrics (queue length, publish rate, consumer count)
- Alerts (unacked messages, queue growth)
- Logs

---

## **🔟 Security**

- Authentication & authorization
- User / vhost permissions
- TLS encryption
- Policy management

---

## **1️⃣1️⃣ RabbitMQ & .NET Integration**

- RabbitMQ.Client usage
- Connection / channel lifecycle
- Async consumer
- Retry & Dead-letter handling

---

## **1️⃣2️⃣ Anti-Patterns & Pitfalls**

- Queue overload → OOM
- Long-running consumer without ACK
- Single point of failure (standalone RabbitMQ)
- Persistent messages everywhere → disk IO bottleneck

---

## **1️⃣3️⃣ RabbitMQ Ne Zaman KULLANILMAZ?**

- Çok düşük latency gereken işlem
- Small-scale, simple CRUD
- Strong consistency gerektiren transaction-heavy işlem
- Stateful communication yeterli ise