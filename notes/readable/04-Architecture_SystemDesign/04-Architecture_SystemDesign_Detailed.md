# Diğer Konular

## **1️⃣ SOLID Principles (Derinlik Önemli)**

### **🔹 S — Single Responsibility Principle**

- Bir class’ın **tek değişme sebebi** ne demek: Bir sınıf sadece tek bir işten sorumlu olmalıdır.
  ```csharp
  // Kötü: Hem rapor üret, hem mail at
  class ReportService { void Generate(); void SendEmail(); }
  // İyi: Ayrılmış sorumluluklar
  class ReportGenerator { void Generate(); }
  class EmailSender { void Send(); }
  ```
- SRP ihlali örneklerini tanıyabilme: Bir Controller'ın hem validation, hem database erişimi, hem de mapping yapması.
- Service + Validator + Mapper ayrımı: İş mantığı, doğrulama ve veri dönüşümü ayrı sınıflarda olmalıdır.

### **🔹 O — Open / Closed Principle**

- Mevcut kodu değiştirmeden genişletme: Yeni özellik eklerken var olan kodu değiştirmek yerine, yeni kod ekleyerek yapılmalıdır.
- Strategy Pattern ile OCP: Farklı algoritmaları ayrı sınıflara bölerek (Strategy) ana sınıfı değiştirmeden yeni algoritma ekleyebilme.
  ```csharp
  // Yeni indirim türü eklemek için Discount sınıfını değiştirmek yerine:
  public class BlackFridayDiscount : IDiscountStrategy { ... }
  ```

### **🔹 L — Liskov Substitution Principle**

- Base class yerine derived class kullanıldığında bozulma: Alt sınıf, üst sınıfın yerine geçtiğinde programın davranışı bozulmamalıdır.
  ```csharp
  // Kare bir Dikdörtgen değildir (Matematiksel evet, OOP hayır)
  rect.SetWidth(5); // Kare ise height de değişir, beklenmedik davranış!
  ```
- Exception fırlatma kuralları: Alt sınıf, üst sınıfın beklemediği bir hata fırlatmamalıdır.

### **🔹 I — Interface Segregation Principle**

- “Fat interface” problemi: İçinde çok fazla ve alakasız metot barındıran interface'ler.
- Küçük, amaç odaklı interface’ler: İhtiyaca özel, bölünmüş interface'ler.
  ```csharp
  // Kötü: IWorker { Work(); Eat(); } (Robot yemek yemez)
  // İyi: IWorkable { Work(); }, IFeedable { Eat(); }
  ```

### **🔹 D — Dependency Inversion Principle**

- High-level module → abstraction bağımlılığı: Üst seviye modüller, alt seviye modüllere (detaylara) değil, soyutlamalara (interface) bağlı olmalıdır.
- Constructor injection: Bağımlılıkların sınıf oluşturulurken verilmesi.
  ```csharp
  public class OrderService {
      private readonly IRepository _repo;
      public OrderService(IRepository repo) => _repo = repo; // DI
  }
  ```

---

## **2️⃣ Dağıtık Mimari Kalıpları (Distributed Patterns)**

### **🔹 Microservices Fundamentals**

- Monolith vs Microservice: Tek parça büyük uygulama vs küçük, bağımsız, ağ üzerinden konuşan servisler.
- Service boundary nasıl çizilir: Domain (Bounded Context) sınırlarına göre (Order, Payment, Shipping).
- Database per service neden önemli: Servislerin birbirinin verisine doğrudan erişmemesi (Loose coupling).

### **🔹 API Communication Patterns**

- Synchronous vs Asynchronous:
  - **Sync:** HTTP/gRPC (Cevabı bekle).
  - **Async:** RabbitMQ/Kafka (Mesajı at, unut).
- API Gateway rolü: Tek giriş noktası (Ocelot, YARP).

### **🔹 Resiliency Patterns**

- Circuit Breaker: Sürekli hata alan servisi devreden çıkarıp sistemin geri kalanını koruma.
  ```csharp
  // Polly:
  Policy.Handle<Exception>().CircuitBreaker(3, TimeSpan.FromSeconds(10));
  ```
- Retry with backoff: Hata durumunda bekleme süresini artırarak (exponential) tekrar deneme.
  ```csharp
  Policy.Handle<Exception>().WaitAndRetry(3, retry => TimeSpan.FromSeconds(Math.Pow(2, retry)));
  ```

### **🔹 Consistency & Reliability**

- CAP Theorem: Consistency (Tutarlılık), Availability (Erişilebilirlik), Partition Tolerance. Sadece 2'si seçilebilir.
- Eventual consistency: Verinin hemen değil, bir süre sonra tüm sistemde tutarlı hale gelmesi.

### **🔹 Saga Pattern**

- Choreography vs Orchestration:
  - **Choreography:** Servisler event fırlatır, birbirini tetikler (Merkezi olmayan).
  - **Orchestration:** Bir yönetici (Orchestrator) sırayla servisleri çağırır (MassTransit Saga State Machine).

---

## **3️⃣ Cache Stratejileri (Redis’e Girmeden)**

### **🔹 Cache Patterns**

- Cache Aside (Lazy loading): Önce Cache'e bak, yoksa DB'den al ve Cache'e yaz.
  ```csharp
  var val = cache.Get(key);
  if (val == null) { val = db.Get(id); cache.Set(key, val); }
  ```
- Read Through: Cache provider DB'den okumayı kendi yapar.
- Write Through: Uygulama Cache'e yazar, Cache DB'ye yazar (Senkron).
- Write Behind: Uygulama Cache'e yazar, Cache DB'ye yazar (Asenkron).

### **🔹 Invalidation Stratejileri**

- TTL kullanımı: Veriye ömür biçme (`AbsoluteExpiration`).
- Cache stampede problemi: Cache süresi bittiğinde binlerce isteğin aynı anda DB'ye saldırması (Locking veya Jitter ile çözülür).

---

## **4️⃣ N+1 Problem**

- N+1 query problemi nedir: Bir ana kayıt (1) ve ilişkili N kayıt için N adet ayrı SQL sorgusu atılması.
  ```csharp
  var users = db.Users.ToList(); // 1 Sorgu
  foreach(var u in users) { var orders = u.Orders.ToList(); } // N Sorgu
  ```
- Lazy loading vs Eager loading çözümü: Veriyi `.Include()` ile baştan çekmek (Eager).
  ```csharp
  var users = db.Users.Include(u => u.Orders).ToList(); // Tek JOIN sorgusu
  ```

---

## **5️⃣ REST & API Design**

- HTTP Methods:
  - `GET`: Okuma (Idempotent).
  - `POST`: Yaratma.
  - `PUT`: Güncelleme (Idempotent, tüm kaynak).
  - `PATCH`: Kısmi güncelleme.
  - `DELETE`: Silme.
- Status Codes:
  - `200 OK`, `201 Created`, `204 No Content`.
  - `400 Bad Request`, `401 Unauthorized`, `403 Forbidden`, `404 Not Found`.
  - `500 Internal Server Error`.
- Stateless API: Her istek, sunucunun onu işlemesi için gereken tüm bilgiyi taşımalıdır (Token vb.).


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

