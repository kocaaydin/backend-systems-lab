# **Design Patterns**

## **🧠 Genel Kavramlar**

- Design Pattern nedir: Yazılımda sık karşılaşılan problemlere verilen, tekrar kullanılabilir çözüm şablonlarıdır.
- Neden ihtiyaç duyulur: Tekerleği yeniden icat etmemek ve ortak bir dil (vocabulary) oluşturmak için.
- Pattern vs Anti-pattern: Doğru çözüm (Pattern) vs Yapılmaması gereken, zararlı çözüm (Anti-pattern).
- Over-engineering nedir: Basit bir probleme gereğinden fazla karmaşık (pattern dolu) çözüm üretme hatası.
- Pattern seçerken nelere dikkat edilir: Soruna uygunluk, esneklik ihtiyacı ve ekip yetkinliği.

---

## **🏗️ Creational Patterns (Nesne Yaratma)**

- **Singleton:** Uygulama ömrü boyunca TEK bir nesne örneğinin (instance) olması.
  ```csharp
  public sealed class Singleton {
      private static readonly Lazy<Singleton> lazy = new(() => new Singleton());
      public static Singleton Instance => lazy.Value;
  }
  ```
- **Factory Method:** Hangi sınıfın nesnesinin üretileceğine alt sınıfların karar vermesi.
  ```csharp
  public IProduct Create() => new ConcreteProduct(); // new anahtarını soyutlar
  ```
- **Abstract Factory:** Birbirleriyle ilişkili nesne ailelerini yaratma arayüzü. (Modern mobilya takımı vs Klasik mobilya takımı).
- **Builder:** Karmaşık bir nesneyi (çok parametreli) adım adım inşa etme.
  ```csharp
  new CarBuilder().SetEngine("V8").SetColor("Red").Build();
  ```
- **Prototype:** Var olan bir nesneyi kopyalayarak (Clone) yenisini üretme maliyetini azaltma.
  ```csharp
  var copy = (MyClass)original.MemberwiseClone(); // Shallow copy
  ```

---

## **🧱 Structural Patterns (Yapısal)**

- **Adapter:** Uyumsuz arayüzleri birbirine bağlama (Çevirici).
  ```csharp
  // IUsb portuna Lightning kablo takmak
  public void Connect() => _oldSystem.LegacyConnect();
  ```
- **Decorator:** Nesneye çalışma zamanında dinamik olarak yeni özellikler ekleme.
  ```csharp
  // Kahve -> SütlüKahve -> ŞurupluSütlüKahve (Stream yapısı örneği)
  var coffee = new MilkDecorator(new SimpleCoffee());
  ```
- **Facade:** Karmaşık bir alt sistemi basitleştirilmiş bir arayüzle sunma.
  ```csharp
  // OrderFacade: Stok düş, Ödeme al, Kargo çağır işlemlerini tek metodda toplar
  public void PlaceOrder() { _stock.Check(); _payment.Process(); _shipping.Ship(); }
  ```
- **Proxy:** Bir nesneye erişimi kontrol etme veya araya girme (Lazy loading, Caching, Logging).
- **Composite:** Nesneleri ağaç yapısında (Parça-Bütün) hiyerarşik olarak düzenleme (Klasör-Dosya yapısı).

---

## **🚦 Behavioral Patterns (Davranışsal)**

- **Observer:** Bir nesnede değişiklik olduğunda abonelerine haber verme (Event mantığı).
  ```csharp
  // Youtube Kanalı (Subject) -> Abone (Observer)
  subject.OnChange += () => observer.Update();
  ```
- **Strategy:** Bir algoritmayı çalışma zamanında seçilebilir ve değiştirilebilir yapma.
  ```csharp
  // Ödeme yöntemi seçimi
  context.SetStrategy(new CreditCardPayment()); context.Pay(100);
  ```
- **Command:** İsteği nesneye çevirerek parametre olarak geçme, kuyruğa atma veya geri alma (Undo).
  ```csharp
  // ICommand: Execute(), Undo()
  commandQueue.Enqueue(new SaveCommand());
  ```
- **Iterator:** Bir koleksiyonun elemanlarına (nasıl tutulduğunu bilmeden) sırayla erişme (`foreach`).
- **Template Method:** Algoritmanın iskeletini üst sınıfta kurup, bazı adımları alt sınıflara bırakma.
- **State:** Nesnenin durumuna göre davranışını değiştirmesi (Sipariş: Hazırlanıyor -> Kargoda -> Teslim).
  ```csharp
  order.State.Next(order); // State değişimi
  ```
- **Mediator:** Nesneler arasındaki karmaşık iletişimi tek bir merkezden yönetme (Havalimanı Kulesi).
- **Chain of Responsibility:** İsteği işleyebilecek nesneler zincirine gönderme (Middleware mantığı).
- Visitor: Sınıfları değiştirmeden, üzerlerinde yeni işlemler tanımlamayı sağlar.
- Behavioral pattern’ler ne zaman tercih edilir: Nesneler arası iletişim ve sorumluluk ataması karmaşıklaştığında.

---

## **🗄️ Backend & Architecture Odaklı Pattern’ler ⭐⭐⭐**

- Repository Pattern: Veri erişim mantığını iş mantığından soyutlar.
- Unit of Work: Birden fazla veritabanı işlemini tek bir transaction (atomik) olarak yönetir.
- CQRS (Command / Query Separation): Okuma (Query) ve yazma (Command) modellerini/işlemlerini ayırır.
- Mediator Pattern (MediatR): Controller ile Service arasındaki bağımlılığı azaltır, in-process messaging sağlar.
- Specification Pattern: İş kurallarını (query filter vb.) yeniden kullanılabilir nesneler olarak tanımlar.
- Factory + Strategy birlikte kullanımı: Doğru stratejiyi seçmek için Factory kullanılması (Ödeme Factory -> KrediKartı Strategy).
- Clean Architecture ile ilişkisi: Bağımlılıkların dıştan içe doğru (Core'a) olması ilkesini destekler.

---

## **🌐 Distributed / Microservice Pattern’leri**

- API Gateway Pattern: Tüm servislerin önüne tek bir giriş noktası koyar (Routing, Auth, Rate Limiting).
- Circuit Breaker: Hata alan servisi geçici olarak devre dışı bırakıp sistemin geri kalanını korur.
- Retry Pattern: Geçici hatalarda (transient faults) işlemi tekrar dener.
- Bulkhead: Kaynakları izole ederek bir servisteki çökmenin diğerlerini etkilemesini önler (Gemi bölmeleri gibi).
- Saga Pattern: Dağıtık transaction'ları yönetmek için sıralı işlemler ve telafi (compensating) adımları.
- Event-Driven Architecture: Servislerin olaylar (mesajlar) üzerinden asenkron haberleşmesi.
- Outbox Pattern: Veritabanı işlemi ile mesaj gönderme işleminin tutarlılığını garanti eder.
- Idempotency Pattern: Bir işlem birden fazla kez yapılsa bile sonucun değişmemesi (Tekrarlanan isteklerde güvenlik).

---

## **🔐 Security & Cross-Cutting Pattern’ler**

- Dependency Injection: Bağımlılıkların dışarıdan verilmesi (Test ve bakım kolaylığı).
- Decorator ile logging: Business koduna dokunmadan araya girip loglama yapma.
- Proxy ile caching: Metot sonucunu önbellekten dönen bir proxy katmanı.
- Aspect Oriented Programming (AOP): Loglama, cache, transaction gibi kesişen ilgileri (cross-cutting concerns) modüler hale getirme.
- Cross-cutting concern nedir: Uygulamanın birden fazla yerinde ihtiyaç duyulan ortak işlevler (Log, Auth, Cache).

---

## **🧪 Testing ile İlgili Pattern’ler**

- Test Double kavramı: Testte gerçek bağımlılık yerine kullanılan nesnelerin genel adı.
- Mock: Davranışı doğrulayan sahte nesne (Bu metot çağrıldı mı?).
- Stub: Durumu simüle eden sahte nesne (Şunu döndür).
- Fake: Basitleştirilmiş ama çalışan implementasyon (In-memory DB).
- Dependency Injection ile test edilebilirlik: Gerçek servis yerine Mock servisi enjekte edebilmeyi sağlar.
- Arrange-Act-Assert: Test yazım standardı (Hazırla - Çalıştır - Doğrula).

