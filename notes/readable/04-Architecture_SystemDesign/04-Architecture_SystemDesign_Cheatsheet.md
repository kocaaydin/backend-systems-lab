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

