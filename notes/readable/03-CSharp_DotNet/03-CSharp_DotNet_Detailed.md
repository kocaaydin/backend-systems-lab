# C# ve .NET

## **🧠 .NET & Runtime Temelleri**

- .NET nedir (Framework vs Core vs .NET): Birleştirilmiş, cross-platform geliştirme platformu (.NET 5+).
- CLR rolü: Common Language Runtime - Kodun çalıştırılmasından, bellekten ve thread yönetiminden sorumludur.
- Runtime vs SDK: SDK geliştirmek için, Runtime çalıştırmak için gerekir.
- JIT compilation: Just-In-Time - IL kodunu çalışacağı makinenin Native koduna çevirir.
- IL (Intermediate Language): C# kodunun derlendiği ara dil (.dll/.exe içinde).
- Assembly nedir: Derlenmiş .NET kod birimi (DLL veya EXE).
- Stack vs Heap: Stack (Value types, metod çağrıları - Hızlı, küçük), Heap (Reference types - Yavaş, büyük, GC yönetir).
- Managed vs Unmanaged code: CLR tarafından yönetilen (.NET) vs işletim sistemi/doğrudan bellek erişimli (C++) kod.
- Garbage Collector genel mantığı: Heap'te kullanılmayan nesneleri temizleyerek bellek yönetimi yapar.

---

## **🧱 C# Tip Sistemi**

- Value Type vs Reference Type: Değeri direkt tutan (Stack) vs Adresi tutan (Heap) tipler.
- struct vs class: Struct (Value type, inheritance yok, hafif), Class (Reference type, inheritance var).
- record nedir: Immutable data taşımak için özelleşmiş class veya struct (Value equality).
- Immutable object kavramı: Durumu (state) oluşturulduktan sonra değiştirilemeyen nesne.
- readonly vs const: `const` derleme zamanı sabiti, `readonly` çalışma zamanı (ctor) sabiti.
- ref, out, in: Parametre geçiş yöntemleri (Referans ile, çıktı olarak, sadece okunabilir referans).
- Boxing & Unboxing: Value type -> Reference type dönüşümü (Boxing - Maliyetli) ve tersi (Unboxing).
- Nullable value types: `int?` gibi değer tiplerinin null alabilmesi.
- Nullable reference types: `string?` gibi referans tiplerinin null durumunu derleyicide kontrol etmesi.
- Tip Sistemi & Memory / Performance: Yanlış tip seçimi (gereksiz boxing, struct kopyalama) performansı düşürür.

---

## **🧬 OOP (C# Özel)**

- Encapsulation: Veriyi gizleme ve kontrollü erişim (Properties).
- Inheritance: Kalıtım yoluyla özellik ve davranış aktarımı.
- Polymorphism: Farklı nesnelerin aynı arayüzle farklı davranabilmesi.
- Abstraction: Gereksiz detayları gizleyip sadece önemli özellikleri sunma.
- Interface vs Abstract class: Sözleşme (Interface) vs Ortak temel sınıf (Abstract - kod içerebilir).
- Virtual / Override: Metodun ezilebilir (virtual) olması ve ezilmesi (override).
- Sealed class / method: Kalıtımı veya ezilmeyi engelleme (Performans için de iyidir).
- Multiple inheritance neden yok: "Diamond Problem" karmaşasını önlemek için (Interface ile çözülür).

---

## **🧹 Memory Management**

- Garbage Collection generations (0,1,2): Nesne ömrüne göre kuşaklar; Gen 0 sık, Gen 2 nadir temizlenir.
- GC ne zaman çalışır: Bellek (Allocation limit) dolduğunda veya `GC.Collect()` ile tetiklendiğinde.
- Finalizer (~ClassName): Nesne yok edilmeden hemen önce çalışan metot (Artık önerilmez).
- IDisposable: Unmanaged kaynakları (Dosya, SQL) manuel serbest bırakmak için interface.
- using statement: `IDisposable` nesnelerin scope sonunda otomatik `Dispose()` edilmesini sağlar.
- Dispose pattern: Hem `Dispose()` hem Finalizer içeren güvenli temizlik şablonu.
- Memory leak senaryoları: Event aboneliklerinin unutulması, statik listeler, kapatılmayan stream'ler.
- Large Object Heap (LOH): 85KB'dan büyük nesneler buraya gider, burası nadiren ve parçalı (fragmented) temizlenir.

---
## **1️⃣ Runtime & Type System**

- **Managed vs Unmanaged Code:** Managed CLR kontrolündedir (GC var), Unmanaged işletim sistemi kontrolündedir (C++ gibi, `unsafe`).
- **CLR, JIT, IL:** Kod -> IL (Intermediate Language) -> JIT (Just-In-Time) -> Makine Kodu. JIT çalışma zamanında derler.
- **Value Type vs Reference Type:** Stack (Hızlı, kopyalanır) vs Heap (GC yönetir, referans taşır).
  ```csharp
  int a = 5; // Value
  class U { } // Reference
  ```
- **Stack vs Heap:** Stack: LIFO, hızlı, yerel değişkenler. Heap: Global, dağınık, nesneler.
- **Boxing / Unboxing:** Value -> Ref (Boxing/Maliyetli), Ref -> Value (Unboxing/Riskli).
  ```csharp
  object o = 10; // Boxing
  int i = (int)o; // Unboxing
  ```
- **struct vs class:** Struct value type'tır (inheritance yok, stackte), Class reference type'tır.
- **record vs class:** Record immutable veri taşıyıcıdır, `Equals` değer tabanlıdır.
  ```csharp
  public record Person(string Name, int Age); // Value equality
  ```

---

## **2️⃣ OOP & Interfaces**

- **Abstract class vs Interface:** Abstract kod/field içerebilir, Interface sadece imza (C# 8.0+ default impl hariç). Çoklu kalıtım sadece Interface ile.
- **Explicit interface implementation:** Aynı isimli iki metodu ayırt etmek için.
  ```csharp
  interface IA { void Log(); }
  interface IB { void Log(); }
  class MyClass : IA, IB {
      void IA.Log() { /* IA'ya özel */ }
      void IB.Log() { /* IB'ye özel */ }
  }
  ```
- **Extension methods:** Var olan sınıfa (kaynağı kapalı olsa bile) yeni metot ekleme (`this` keyword'ü ile).
  ```csharp
  public static class IntExtensions {
      public static bool IsEven(this int i) => i % 2 == 0;
  }
  // Kullanım: int num = 4; num.IsEven();
  ```
- **Virtual vs Abstract vs Sealed:** Virtual ezilebilir, Abstract ezilmek ZORUNDA, Sealed ezilemez/türetilemez.
- **Polymorphism:** Referansın (Base) tuttuğu nesneye (Derived) göre farklı davranması (`virtual`/`override`).

---

## **3️⃣ Memory Management (GC)**

- **Garbage Collector Generations (0, 1, 2):**
  - **Gen 0:** Yeni nesneler, sık temizlenir.
  - **Gen 1:** Tampon bölge.
  - **Gen 2:** Uzun yaşayanlar, nadir temizlenir (Maliyetli).
- **Large Object Heap (LOH):** 85KB+ nesneler buraya gider, buradaki GC maliyetlidir ve fragmantasyon olabilir.
- **IDisposable & using:** Unmanaged kaynakları (Dosya, SQL) temizlemek için. `Finalize` GC çağırır, `Dispose` biz çağırırız.
  ```csharp
  using (var fs = new FileStream("path", FileMode.Open)) {
      // Dosya işlemleri
  } // Otomatik Dispose çağrılır
  ```
- **Finalizer (~Method):** GC nesneyi silerken çalışır, kaçış noktasıdır, performans düşürür.
- **Memory Leak neden olur:** Static listelere sürekli ekleme, event'lere abone olup çıkmama.

---

## **4️⃣ Async / Concurrency**

- **Thread vs Task:** Thread OS kaynağıdır (Ağır), Task CLR tarafından yönetilen iş birimidir (Hafif).
- **ThreadPool mantığı:** Hazırda bekleyen thread havuzu, sürekli thread açıp kapatma maliyetini önler.
- **async / await:** Asenkron programlama, thread'i bloklamadan I/O beklemeyi sağlar.
  ```csharp
  public async Task DoSomethingAsync() {
      await Task.Delay(1000); // Thread havuza döner, 1sn sonra geri gelir
      Console.WriteLine("1 saniye geçti.");
  }
  ```
- **Sync over Async (Deadlock):** `.Result` veya `.Wait()` ile asenkron kodu senkron beklemek deadlock yaratabilir.
- **Task.WhenAll vs WaitAll:** `WhenAll` non-blocking (awaitable), `WaitAll` blocking.
- **CancellationToken:** İşlemi iptal etmek için kullanılan yapı.
  ```csharp
  CancellationTokenSource cts = new CancellationTokenSource();
  // ...
  if (cts.Token.IsCancellationRequested) {
      cts.Token.ThrowIfCancellationRequested();
  }
  ```

---

## **5️⃣ Delegates, Events & Lambdas**

- **Delegate:** Metot adresini tutan referans (Function pointer).
- **Action, Func, Predicate:**
  - `Action`: Dönüş yok (`void`).
  - `Func<T, Result>`: Parametre alır, değer döner.
  - `Predicate<T>`: `bool` döner (Filtreleme).
  ```csharp
  Func<int, int, int> add = (a, b) => a + b;
  Action<string> log = message => Console.WriteLine(message);
  Predicate<int> isEven = num => num % 2 == 0;
  ```
- **Event:** Delegate'in sarmalanmış, sadece `+=` ve `-=` yapılabilen hali (Observer pattern).
- **Lambda Expressions:** Anonim metot yazma sözdizimi (`x => x > 5`).

---

## **6️⃣ LINQ & Collections**

- **IEnumerable vs IQueryable:**
  - `IEnumerable`: Veriyi belleğe çeker (Memory-bound), filter client'ta yapılır.
  - `IQueryable`: Sorguyu veritabanına atar (SQL üretir), filter sunucuda yapılır.
- **Yield return:** Elemanları tek tek, istendikçe (lazy) üretir.
  ```csharp
  IEnumerable<int> GetNumbers() {
      yield return 1;
      yield return 2;
      yield return 3;
  }
  ```
- **Lazy evaluation (Deferred Execution):** Sorgu tanımlandığında değil, `ToList()` veya `foreach` dendiğinde çalışır.
- **Dictionary internals:** Bucket array ve Linked list (chaining) yapısı.

---

## **7️⃣ Exception Handling**

- **try-catch-finally:** Hata yakalama bloğu. `finally` her zaman çalışır (Kaynak temizliği).
- **throw vs throw ex:** `throw` stack trace'i korur, `throw ex` stack trace'i siler/sıfırlar (KÖTÜ).
- **Custom Exception:** `Exception` sınıfından türeterek özel hata tipleri oluşturma.
- **Global Exception Handling:** Middleware ile tüm hataları tek bir yerden yakalama ve loglama.

---

## **8️⃣ Advanced Topics**

- **Reflection:** Çalışma zamanında tip (Type) bilgilerini okuma ve dinamik kod çalıştırma.
  ```csharp
  Type type = typeof(MyClass);
  object obj = Activator.CreateInstance(type);
  var method = type.GetMethod("Log");
  method.Invoke(obj, null);
  ```
- **Attributes:** Metadata ekleme yöntemi (`[Obsolete]`, `[Serializable]`).
- **Middleware (ASP.NET):** Request/Response hattına giren ara yazılımlar (Auth, Log, Error).
  ```csharp
  app.Use(async (context, next) => {
      // İstek öncesi
      await next(); // Sonraki middleware'i çağır
      // İstek sonrası
  });
  ```
- **Dependency Injection (DI):** Bağımlılıkları dışarıdan verme (Loose coupling). .NET Core'da built-in gelir (`AddTransient`, `AddScoped`, `AddSingleton`).
- **Expression Trees:** Kodu veri ağacı olarak temsil etme (LINQ provider'ları kullanır).
- **Span / Memory:** Bellek yönetimi optimizasyonu, kopyalamasız dilimleme.
- **ValueTask:** Tahsis (allocation) gerektirmeyen Task (Değer döndüren async hot-path metodlar için).
- **Unsafe code (temel seviye):** Pointer kullanımı, GC kontrolü dışına çıkma (`unsafe` blokları).

## **9️⃣ ASP.NET Core Fundamentals**

- **Program.cs & Startup:** Uygulamanın giriş noktası ve konfigürasyon merkezi.
- **Dependency Injection Service Lifetimes:**
  - **Transient:** Her istendiğinde yeni instance (Hafif servisler).
  - **Scoped:** Request başına bir instance (Veritabanı contexti).
  - **Singleton:** Uygulama boyunca tek instance (Cache servisi).
- **Middleware Pipeline:** Sıra önemlidir! (Exception -> HSTS -> StaticFiles -> Routing -> Auth -> Endpoints).
- **Filters:** Action çalışmadan önce/sonra araya girme (Validation, Exception, Resource filters).
- **Configuration (appsettings.json):** `IConfiguration` ile okunan ayarlar. `IOptions<T>` ile strongly-typed erişim.

---

## **🏗️ ASP.NET Core – Framework Temelleri**

- ASP.NET Core request lifecycle: İstek -> Middleware Pipeline -> Routing -> Controller -> Result.
- Middleware pipeline: İsteği işleyen zincirleme bileşenler (Auth, Logging, StaticFiles).
- Middleware yazma: `Use` veya `IMiddleware` ile özel pipeline adımı ekleme.
- Minimal API vs Controller: Daha az boilerplate, daha hızlı başlangıç (Minimal) vs Yapısal, özellikli (Controller).
- Routing (attribute / conventional): `[Route]` ile veya `MapControllerRoute` ile yol tanımlama.
- Endpoint routing: Routing'in middleware pipeline içine entegre edilmesi.
- Model Binding: HTTP isteğindeki verilerin (Body, Query, Route) C# nesnelerine dönüştürülmesi.
- Model Validation: `[Required]`, `FluentValidation` gibi yöntemlerle veri doğrulama.

---

## **💉 Dependency Injection (.NET)**

- Built-in DI container: `IServiceCollection` ile servis kaydı.
- Transient lifetime: Her istekte yeni bir instance (Hafif, stateless servisler).
- Scoped lifetime: HTTP isteği (Request) başına bir instance (DbContext, UserContext).
- Singleton lifetime: Uygulama ömrü boyunca tek instance (Cache, Config).
- Lifetime hataları: Singleton içine Scoped servis enjekte etmek (Captive Dependency).
- Constructor injection: Bağımlılıkları kurucu metotta istemek (En temiz yöntem).
- Service Locator anti-pattern: Servisi parametreyle değil, container'dan `GetService` ile istemek (Bağımlılığı gizler).

---

## **🎯 Filters & Pipeline**

- Action Filters: Action öncesi/sonrası çalışır.
- Authorization Filters: En başta çalışır, yetki kontrolü yapar.
- Resource Filters: Model binding'den önce çalışır (Cache vb.).
- Exception Filters: Hata durumunda çalışır.
- Global filter kullanımı: Tüm uygulama için geçerli kurallar.
- Filter vs Middleware farkı: Filter MVC context'ine (Action, Controller) hakimdir, Middleware daha düşük seviyedir (HTTP).

---

## **⚙️ Configuration (.NET)**

- appsettings.json: Yapılandırma dosyası (JSON formatında).
- Environment bazlı config: `appsettings.Development.json` gibi ortam bazlı ayarlar.
- IConfiguration: Ayarları okumak için kullanılan arayüz (Key-value).
- IOptions: Ayarları tip güvenli (Strongly typed) sınıflara bağlama (Options Pattern).
- IOptionsSnapshot: Scoped lifetime, istek başına config değişimi yakalar.
- IOptionsMonitor: Singleton lifetime, config değişimini anlık yakalar (Restart gerekmez).

