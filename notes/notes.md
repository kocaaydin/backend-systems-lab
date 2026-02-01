
# [**Veri Yapıları**]

## **🧠 Temel Kavramlar**

- Big-O Notation (O(1), O(log n), O(n), O(n log n), O(n²)): Algoritma performansının veri girdisine (n) göre büyüme hızı.
  - **O(1) - Constant**: Girdi ne olursa olsun süre aynıdır (örn. Array index erişimi `arr[5]`).
  - **O(log n) - Logarithmic**: Girdi arttıkça süre çok az artar, genelde bölerek ilerlenir (örn. Binary Search).
  - **O(n) - Linear**: Girdi ile süre doğru orantılıdır (örn. For döngüsü ile tüm listeyi gezmek).
  - **O(n log n) - Linearithmic**: Verimli sıralama algoritmaları bu hızdadır (örn. Merge Sort, Quick Sort).
  - **O(n²) - Quadratic**: İç içe döngüler. Girdi karesi kadar yavaşlar (örn. Bubble Sort).
- Time Complexity vs Space Complexity: İşlem süresi (Time) ve bellek kullanımı (Space) analizi.
- Worst / Average / Best Case: Algoritmanın en kötü, ortalama ve en iyi durum performansı.
- Amortized Complexity: Nadiren gerçekleşen pahalı işlemlerin (örn. array resize) ortalamaya yayılmış maliyeti.
- Mutable vs Immutable collections: Değiştirilebilir (Mutable) ve değiştirilemez (Immutable) veri yapıları (Thread-safe için önemlidir).

---

## **📦 Array**

- Array temel yapısı ve index erişimi: Sabit boyutlu, bellekte ardışık tutulan, index ile O(1) erişilen yapı.
- Array insert / delete maliyeti: Araya ekleme/silme kaydırma gerektirdiğinden O(n) maliyetlidir.
- Two Sum problemi: Toplamı hedef sayıya eşit iki elemanı bulma (Genelde HashMap ile O(n)).
- Array rotate: Diziyi sağa/sola kaydırma işlemi (Modülo aritmetiği veya ters çevirme yöntemi).
- Duplicate eleman bulma: Tekrar eden sayıları tespit etme (HashSet veya Sorting ile).
- Max / Min eleman bulma: Tüm diziyi gezerek en küçük/büyük değeri bulma O(n).
- C# T[] kullanımı: `int[] numbers = new int[5];` şeklinde tanımlanan sabit dizi.
- Span<T> nedir, ne zaman kullanılır: Bellek tahsisi yapmadan (heap allocation'sız) array dilimleme (slicing) için kullanılır.

---

## **📋 List (Dynamic Array)**

- List vs Array farkları: List dinamik büyür, Array sabittir. List arkada Array kullanır.
- Capacity vs Count: Capacity ayrılan yer, Count dolu olan eleman sayısıdır.
- Amortized Add() maliyeti: Kapasite dolunca yeni dizi açıp kopyalamak O(n), diğer eklemeler O(1)'dir; ortalama O(1).
- Listeyi ters çevirme: Elemanların sırasını `Reverse()` ile döndürme O(n).
- Duplicate silme: Tekrarlı elemanları `Distinct()` ile temizleme.
- En sık geçen elemanı bulma: Frekans sayımı (Dictionary ile).
- C# List<T> metotları: `Add`, `Remove`, `Contains`, `Find` gibi yardımcı metotlar.

---

## **🔗 Linked List**

- Singly Linked List mantığı: Her düğüm veriyi ve bir sonraki düğümün adresini tutar.
- Doubly Linked List mantığı: Düğümler hem önceki hem sonraki düğüm adresini tutar.
- Head / Tail kavramları: Listenin başı (Head) ve sonu (Tail).
- Linked list traversal: Baştan sona düğümleri gezme O(n).
- Ortadaki elemanı bulma: Fast & Slow pointer tekniği ile (Biri 1, biri 2 gider).
- Linked list reverse: Pointer'ların yönünü ters çevirerek listeyi döndürme.
- Cycle detection: Listede döngü var mı? (Floyd’s Cycle Finding - Fast/Slow pointer).
- Array vs LinkedList karşılaştırması: Array erişimde O(1), eklemede O(n); LinkedList erişimde O(n), (konum belliyse) eklemede O(1).

---

## **📚 Stack (LIFO)**

- Stack temel mantığı: Last In First Out (Son giren ilk çıkar).
- Push / Pop / Peek: Ekle (Push), Çıkar (Pop), En üsttekine bak (Peek) - Hepsi O(1).
- Parantez kontrolü problemi: Açılan parantez kapananla eşleşiyor mu? (Valid Parentheses).
- String ters çevirme: Karakterleri Stack'e atıp geri çekerek ters çevirme.
- Undo / Redo senaryosu: Yapılan işlemleri Stack'te tutup geri alma.
- Call stack nasıl çalışır: Fonksiyon çağrılarının bellekte tutulduğu yer (Recursive fonksiyonlar).
- C# Stack<T> kullanımı: `Stack<int> s = new Stack<int>();`

---

## **📥 Queue (FIFO)**

- Queue temel mantığı: First In First Out (İlk giren ilk çıkar).
- Enqueue / Dequeue: Ekle (Enqueue), Çıkar (Dequeue) - Hepsi O(1).
- Producer – Consumer problemi: Bir tarafın üretip diğer tarafın tükettiği asenkron yapı.
- BFS algoritmasında queue kullanımı: Graf/Ağaç gezintisinde katman katman ilerlemek için kullanılır.
- Queue vs Stack farkları: Sıralama farkı (Biri kuyruk, biri yığın).
- C# Queue<T> kullanımı: `Queue<string> q = new Queue<string>();`
- ConcurrentQueue<T> nedir: Thread-safe kuyruk yapısı (Lock-free mekanizmalar içerir).

---

## **🗂️ Dictionary / Hash Table ⭐**

- Hashing mantığı: Key'i (anahtar) sayısal bir index'e dönüştürme fonksiyonu.
- Collision nedir: Farklı key'lerin aynı hash değerini üretmesi (Çakışma).
- Chaining vs Open Addressing: Çakışma çözme yöntemleri (Bağlı liste kullanma vs boş yer arama).
- Lookup neden O(1): Hash fonksiyonu direkt adresi verdiği için (Çakışma yoksa).
- Duplicate eleman bulma: Elemanları Dictionary key'i yaparak sayma.
- Frequency counter: Bir dizide hangi elemandan kaç tane olduğunu bulma.
- Two Sum (Dictionary ile): Aranan farkı (`Target - Current`) Dictionary'de arayarak O(n) çözüm.
- C# Dictionary<TKey, TValue>: Key-Value çiftleri tutan hash tablosu.
- ConcurrentDictionary kullanımı: Thread-safe dictionary (Çoklu okuma/yazma için).
- HashSet<T> farkı: Sadece Key tutar, Value yoktur. Unique eleman listesi.

---

## **🧩 Set**

- Set temel mantığı: Benzersiz (Unique) elemanlar kümesi.
- Unique eleman garantisi: Aynı elemandan birden fazla bulunamaz.
- İki listede ortak eleman bulma: Intersection (Kesişim) işlemi.
- Subset kontrolü: Bir küme diğerinin alt kümesi mi?
- C# HashSet<T> kullanımı: Benzersiz elemanları performanslı saklamak için.

---

## **🌳 Tree**

- Tree temel kavramları (root, leaf, height): Kök, yaprak (çocuğu olmayan), yükseklik.
- Binary Tree: Her düğümün en fazla 2 çocuğu olduğu ağaç.
- Binary Search Tree (BST): Sol çocuk küçük, sağ çocuk büyük kuralına uyan ağaç.
- Preorder traversal: Root -> Sol -> Sağ.
- Inorder traversal: Sol -> Root -> Sağ (BST'de sıralı verir).
- Postorder traversal: Sol -> Sağ -> Root.
- Level Order traversal (BFS): Katman katman gezme (Queue kullanılır).
- Tree yüksekliği bulma: Recursive olarak `1 + max(left, right)`.
- BST doğrulama: Ağacın BST kuralına uyup uymadığını kontrol etme.
- DFS vs BFS farkı: Derinlemesine (Stack/Recursion) vs Genişlemesine (Queue).

---

## **⛰️ Heap / Priority Queue**

- Min Heap mantığı: Root en küçük değerdir, ebeveyn çocuktan küçüktür.
- Max Heap mantığı: Root en büyük değerdir, ebeveyn çocuktan büyüktür.
- En büyük K eleman problemi: Min Heap kullanarak akıştaki en büyük K sayıyı tutma.
- En küçük K eleman problemi: Max Heap kullanarak en küçük K sayıyı tutma.
- Task scheduling senaryosu: Öncelik sırasına göre işleri yürütme (Priority Queue).
- C# PriorityQueue<TElement, TPriority>: .NET 6 ile gelen öncelikli kuyruk.

---

## **🕸️ Graph**

- Graph temel kavramları: Düğümler (Vertex) ve kenarlar (Edge).
- Directed vs Undirected graph: Yönlü (oklar var) vs Yönsüz bağlantılar.
- Adjacency List: Her düğümün komşularını listede tutması (Sparse graph için iyi).
- Adjacency Matrix: Komşuluk matrisi (Dense graph için iyi).
- BFS implementasyonu: En kısa yol bulmada kullanılır (Ağırlıksız graf).
- DFS implementasyonu: Tüm yolları keşfetme, cycle bulma.
- Cycle detection: Graf içinde döngü var mı?
- Graph vs Tree farkları: Tree'de cycle olamaz ve tek kök vardır; Graf daha geneldir.

---

## **🔀 Sorting & Searching**

- Bubble Sort: Yan yana elemanları takas ederek sıralama (O(n²), pratik değil).
- Selection Sort: En küçüğü bulup başa koyma (O(n²)).
- Insertion Sort: Kart dizer gibi araya ekleyerek sıralama (Küçük veride hızlı).
- Merge Sort: Böl ve yönet mantığı, O(n log n), recursive.
- Quick Sort: Pivot seçip bölme, ortalamada O(n log n) ama worst case O(n²).
- Linear Search: Tek tek bakma O(n).
- Binary Search: Sıralı dizide ortadan bölerek arama O(log n).
- Binary search şartları: Veri mutlaka sıralı olmalıdır.
- Hangi sort ne zaman kullanılır: Genel amaçlı Quick/Merge; neredeyse sıralı ise Insertion.

# **Design Patterns**

## **🧠 Genel Kavramlar**

- Design Pattern nedir: Yazılımda sık karşılaşılan problemlere verilen, tekrar kullanılabilir çözüm şablonlarıdır.
- Neden ihtiyaç duyulur: Tekerleği yeniden icat etmemek ve ortak bir dil (vocabulary) oluşturmak için.
- Pattern vs Anti-pattern: Doğru çözüm (Pattern) vs Yapılmaması gereken, zararlı çözüm (Anti-pattern).
- Over-engineering nedir: Basit bir probleme gereğinden fazla karmaşık (pattern dolu) çözüm üretme hatası.
- Pattern seçerken nelere dikkat edilir: Soruna uygunluk, esneklik ihtiyacı ve ekip yetkinliği.

---

## **🏗️ Creational Patterns**

- Singleton – tanım ve kullanım amacı: Bir sınıftan sadece tek bir nesne üretilmesini garanti eder (örn. Logger, Config).
- Singleton thread-safe implementasyon: `Lazy<T>` veya `lock` mekanizması ile çoklu thread kontrolü.
- Singleton dezavantajları: Global state yaratır, test etmesi zordur (bağımlılık gizler).
- Factory Method: Nesne yaratma işini alt sınıflara bırakan interface (Loose coupling sağlar).
- Abstract Factory: Birbiriyle ilişkili nesne ailelerini yaratmak için kullanılır.
- Builder: Karmaşık nesneleri adım adım oluşturmayı sağlar (Fluent interface).
- Prototype: Mevcut bir nesneyi kopyalayarak (clone) yeni nesne üretir (Maliyetli üretimden kaçınmak için).
- Dependency Injection ile ilişkisi: DI konteynerleri genelde nesne yaşam döngüsünü (Singleton/Transient) yöneterek bu pattern'leri soyutlar.

---

## **🧩 Structural Patterns**

- Adapter: Uyumsuz iki interface'i birbirine bağlar (Çevirici).
- Facade: Karmaşık bir alt sistemi basitleştirilmiş bir arayüzle sunar.
- Decorator: Nesneye dinamik olarak yeni özellik/davranış ekler (Inheritance yerine composition).
- Proxy: Bir nesneye erişimi kontrol eden veya araya giren vekil nesne (Lazy loading, security).
- Composite: Nesneleri ağaç yapısında (parça-bütün) hiyerarşik olarak tutar.
- Bridge: Soyutlama (Abstraction) ile gerçekleştirmeyi (Implementation) birbirinden ayırır.
- Flyweight: Çok sayıda benzer nesne için bellek optimizasyonu sağlar (Ortak veriyi paylaşarak).
- Bu pattern’lerin gerçek proje senaryoları: Adapter (3. parti API), Proxy (Cache), Decorator (Middleware/Logging).

---

## **🔁 Behavioral Patterns ⭐**

- Strategy: Bir işlemin algoritmasını çalışma zamanında değiştirmeyi sağlar (örn. Ödeme yöntemi seçimi).
- Observer: Bir nesnedeki değişikliği, ona abone olan diğer nesnelere duyurur (Event-driven).
- Command: İsteği bir nesneye (komut) dönüştürerek parametre olarak geçmeyi, sıraya koymayı sağlar.
- Mediator: Nesneler arası kaotik iletişimi merkezi bir aracı üzerinden yönetir (MediatR).
- Chain of Responsibility: Bir isteği, işleyebilecek nesneler zinciri boyunca iletir (Middleware mantığı).
- State: Nesnenin iç durumuna göre davranışını değiştirmesini sağlar (State Machine).
- Template Method: Algoritma iskeletini bir üst sınıfta tanımlayıp, detayları alt sınıflara bırakır.
- Iterator: Bir koleksiyonun elemanlarına, iç yapısını bilmeden sırayla erişmeyi sağlar.
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

## **🧵 Async & Concurrency (C# Seviyesi)**

- async / await çalışma mantığı: I/O işlemlerinde thread'i bloklamadan iş bitince devam etmesini sağlar (State Machine).
- Task vs Thread: Task iş birimidir (Promise), Thread onu çalıştıran işçidir.
- Task.Run ne zaman kullanılmalı: CPU-bound (işlemci yoran) işleri arka planda yapmak için.
- Deadlock senaryosu: UI/Context thread'ini bekleyen async metodun, o thread tarafından bloklanması.
- lock keyword: Kritik bölgeye (Critical Section) aynı anda tek thread girmesini sağlar.
- SemaphoreSlim: Eşzamanlı giriş sayısını sınırlayan (Async uyumlu) kilit mekanizması.
- Thread-safe collections: `ConcurrentDictionary`, `ConcurrentQueue` gibi kilit gerektirmeyen yapılar.
- ConfigureAwait(false): Bağlamı (Context) koruma zorunluluğunu kaldırarak deadlock riskini azaltır (Kütüphane kodlarında).

---

## **🧩 Delegates, Events, Lambdas**

- Delegate nedir: Metodu işaret eden (pointer) tip güvenli nesne.
- Func / Action / Predicate: Hazır delegate türleri (Dönüşlü / Dönüşsüz / Boolean dönüşlü).
- Multicast delegate: Birden fazla metodu sırayla çağıran delegate.
- Event mantığı: Delegate üzerine kurulu, publish-subscribe mekanizması (Sadece sahibi tetikleyebilir).
- Event vs Delegate farkı: Event, delegate'in kapsüllenmiş (encapsulated) halidir.
- Custom event yazma: `EventHandler<T>` kullanarak olay tanımlama.
- Lambda expression kullanımı: `x => x > 5` gibi isimsiz, kısa fonksiyon yazımı.

---

## **🧮 LINQ**

- LINQ to Objects: Bellekteki koleksiyonları sorgulama.
- Deferred execution: Sorgunun tanımlandığı an değil, sonucun istendiği an (foreach, ToList) çalışması.
- Immediate execution: `ToList()`, `Count()` gibi metotlarla sorgunun hemen çalışması.
- Select, Where, Any, All: Dönüştürme, Filtreleme, Var mı, Hepsi mi kontrolü.
- First, FirstOrDefault: İlk elemanı getir, yoksa hata ver / default dön.
- Single vs First: `Single` tek bir eleman bekler, birden fazla varsa hata verir.
- GroupBy: Veriyi belirli bir alana göre gruplama.
- Join: İlişkisel verileri birleştirme.
- LINQ performans tuzakları: Gereksiz `ToList()`, N+1 sorguları, veritabanına gidemeyen sorgular.

---

## **🧰 Collections (C# / .NET)**

- Array: Sabit boyut, en hızlı.
- List<T>: Dinamik boyut, genel kullanım.
- Dictionary<TKey, TValue>: Key ile hızlı erişim (Hash map).
- HashSet<T>: Benzersiz eleman kümesi, hızlı `Contains`.
- Stack<T>: LIFO yapısı.
- Queue<T>: FIFO yapısı.
- ConcurrentDictionary: Thread-safe dictionary.
- Immutable collections: Değiştirilemez koleksiyonlar (Builder pattern ile üretilir).
- Doğru collection seçimi: Okuma/yazma sıklığı, thread-safety ve sıralama ihtiyacına göre.

---

## **⚠️ Exception Handling**

- try / catch / finally: Hata yakalama blokları. `finally` her durumda çalışır.
- Exception propagation: Hatanın call stack boyunca yukarı fırlatılması.
- Custom exception yazma: `Exception` sınıfından türeterek özel hata tipleri oluşturma.
- Checked vs unchecked exception: C#'ta Java gibi zorunlu (checked) exception yoktur, hepsi uncheck.
- Exception performance etkisi: `throw` işlemi maliyetlidir (Stack trace oluşturur), akış kontrolü için kullanılmamalıdır.
- Exception best practices: Sadece beklenmedik durumlarda kullan, `catch (Exception)`'dan kaçın.

---

## **🧠 Advanced C#**

- Generics: Tip bağımsız kod yazma (`List<T>`), tip güvenliği ve performans (boxing yok) sağlar.
- Generic constraints: `where T : class` gibi kısıtlamalar.
- Covariance / Contravariance: Tipler arası uyumluluk (`IEnumerable<Derived>` -> `IEnumerable<Base>`).
- Reflection: Çalışma zamanında tip bilgilerini inceleme ve dinamik kod çalıştırma (Yavaştır).
- Attributes: Metadata (veri hakkında bilgi) ekleme (`[Obsolete]`, `[Serializable]`).
- Expression Trees: Kodu veri ağacı olarak temsil etme (LINQ provider'ları kullanır).
- Span / Memory: Bellek yönetimi optimizasyonu, kopyalamasız dilimleme.
- ValueTask: Tahsis (allocation) gerektirmeyen Task (Değer döndüren async hot-path metodlar için).
- Unsafe code (temel seviye): Pointer kullanımı, GC kontrolü dışına çıkma (`unsafe` blokları).

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
