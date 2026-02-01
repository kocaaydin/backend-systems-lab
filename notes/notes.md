
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

- Bir class’ın **tek değişme sebebi** ne demek: Bir sınıf sadece tek bir işten sorumlu olmalıdır, iş mantığı ve görünü/DB ayrılmalıdır.
- SRP ihlali örneklerini tanıyabilme: Bir sınıfın hem rapor oluşturup hem de mail atması.
- Service + Validator + Mapper ayrımı: İş mantığı, doğrulama ve veri dönüşümü ayrı sınıflarda olmalıdır.
- “Fat Service” problemini anlatabilme: Service'lerin her işi yapan devasa sınıflara dönüşmesi (Anti-pattern).

---

### **🔹 O — Open / Closed Principle**

- Mevcut kodu değiştirmeden genişletme: Yeni özellik eklerken var olan kodu değiştirmek yerine, yeni kod ekleyerek yapılmalıdır.
- Strategy Pattern ile OCP: Farklı algoritmaları ayrı sınıflara bölerek (Strategy) ana sınıfı değiştirmeden yeni algoritma ekleyebilme.
- Polymorphism vs if/else zinciri: `if (type == A) ... else if (type == B)` yerine A ve B'nin `Execute()` metodunu override etmesi.
- Feature eklerken neden refactor gerekmez: OCP'ye uyulduğunda, yeni class eklenir, eski kod bozulmaz.

---

### **🔹 L — Liskov Substitution Principle**

- Base class yerine derived class kullanıldığında bozulma: Alt sınıf, üst sınıfın yerine geçtiğinde programın davranışı bozulmamalıdır.
- Exception fırlatma kuralları: Alt sınıf, üst sınıfın beklemediği bir hata fırlatmamalıdır.
- Precondition / Postcondition ihlali: Alt sınıf, üst sınıfın kurallarını (girdi/çıktı şartları) gevşetmemeli veya daraltmamalıdır.
- Gerçek hayatta LSP örnekleri: `Kare` sınıfının `Dikdörtgen` sınıfından türetilmesi (Kare'de en/boy bağımsız değişemez -> LSP ihlali).

---

### **🔹 I — Interface Segregation Principle**

- “Fat interface” problemi: İçinde çok fazla ve alakasız metot barındıran interface'ler.
- Küçük, amaç odaklı interface’ler: İhtiyaca özel, bölünmüş interface'ler (`IPrinter`, `IScanner` vs `IMultiFunctionDevice`).
- Read / Write interface ayrımı: `IReadable`, `IWritable` gibi ayırarak sadece gerekli yetkileri verme.
- ISP ihlalinin test yazmayı zorlaştırması: Kullanılmayan metotları mock'lamak zorunda kalmak.

---

### **🔹 D — Dependency Inversion Principle**

- High-level module → abstraction bağımlılığı: Üst seviye modüller, alt seviye modüllere (detaylara) değil, soyutlamalara (interface) bağlı olmalıdır.
- Constructor injection: Bağımlılıkların sınıf oluşturulurken verilmesi.
- DIP vs DI farkı: DIP prensiptir (soyuta bağlı ol), DI bu prensibi uygulama tekniğidir (container kullan).
- Mock edilebilirlik: Interface bağımlılığı sayesinde testlerde sahte (mock) nesne verebilme imkanı.

---

## **2️⃣ Dağıtık Mimari Kalıpları (Distributed Patterns)**

### **🔹 Microservices Fundamentals**

- Monolith vs Microservice: Tek parça büyük uygulama vs küçük, bağımsız, ağ üzerinden konuşan servisler.
- Service boundary nasıl çizilir: Domain (Bounded Context) sınırlarına göre servisleri ayırma (DDD).
- Database per service neden önemli: Servislerin birbirinin verisine doğrudan erişmemesi (Loose coupling) için.
- Distributed system trade-off’ları: Karmaşıklık artar, consistency zorlaşır ama ölçeklenebilirlik artar.

---

### **🔹 API Communication Patterns**

- Synchronous vs Asynchronous: Anında cevap bekleyen (HTTP REST) vs Beklemeyen (Messaging/Queue).
- REST vs Messaging: REST (Request/Response) vs Event/Message (Fire & Forget).
- API Gateway rolü: Tek giriş noktası, routing, auth, rate limiting gibi cross-cutting işleri yönetir.
- Backward compatibility: API güncellemelerinde eski client'ların bozulmaması için versiyonlama.

---

### **🔹 Resiliency Patterns**

- Circuit Breaker: Sürekli hata alan servisi devreden çıkarıp sistemin geri kalanını koruma.
- Retry with backoff: Hata durumunda bekleme süresini artırarak (exponential) tekrar deneme.
- Timeout stratejileri: Cevap gelmeyen isteği belirli sürede kesip kaynağı serbest bırakma.
- Bulkhead pattern: Sistemi bölmelere ayırarak bir parçadaki çökmenin diğerlerini etkilemesini önleme.

👉 **Mülakat sorusu:**

“Bir servis çökerse sistemi nasıl ayakta tutarsın?”

---

### **🔹 Consistency & Reliability**

- CAP Theorem: Consistency (Tutarlılık), Availability (Erişilebilirlik), Partition Tolerance (Bölünme Toleransı) - Sadece ikisi seçilebilir (Genelde CP veya AP).
- Eventual consistency: Verinin hemen değil, bir süre sonra tüm sistemde tutarlı hale gelmesi.
- Idempotency: Aynı işlemin birden fazla kez yapılması durumunda sonucun değişmemesi (Güvenli retry).
- Exactly-once mümkün mü: Teorik olarak zor, genelde "at-least-once" + "idempotency" ile sağlanır.

---

### **🔹 Saga Pattern**

- Choreography vs Orchestration: Dağıtık servislere olayı kimin haber vereceği (Merkezi vs Kendi aralarında).
- Compensating transaction: Bir adım başarısız olursa, önceki başarılı adımları geri alan işlem (Rollback).
- Saga ne zaman tercih edilir: Dağıtık transaction gerektiren uzun iş süreçlerinde (Sipariş -> Ödeme -> Stok).
- Distributed transaction neden kötü: 2PC (Two-phase commit) yavaştır, kilitlenme riski yüksektir, ölçeklenemez.

---

## **3️⃣ Cache Stratejileri (Redis’e Girmeden)**

### **🔹 Cache Temelleri**

- Cache neden kullanılır: Performansı artırmak, DB yükünü azaltmak ve latency düşürmek için.
- Cache ne zaman KULLANILMAZ: Veri çok sık değişiyorsa veya tutarlılık (consistency) çok kritikse.
- Data consistency riskleri: Cache ile DB arasındaki veri farkı (Stale read).
- Cache eviction mantığı: Cache dolduğunda hangi verinin silineceği (LRU - Least Recently Used, LFU).

---

### **🔹 Cache Patterns**

- Cache Aside (Lazy loading): Uygulama önce Cache'e bakar, yoksa DB'den okur ve Cache'e yazar.
- Read Through: Cache provider DB'den okumayı kendi yapar, uygulama sadece Cache ile konuşur.
- Write Through: Uygulama Cache'e yazar, Cache senkron olarak DB'ye yazar (Garanti ama yavaş).
- Write Behind: Uygulama Cache'e yazar, Cache asenkron olarak (arkada) DB'ye yazar (Hızlı ama veri kaybı riski).

👉 **En sık kullanılan:** Cache Aside

---

### **🔹 Invalidation Stratejileri**

- TTL kullanımı: Veriye ömür biçme (Time To Live), süre bitince silinir.
- Manual invalidation: Veri güncellendiğinde kodla cache'i silme.
- Versioned cache key: Key sonuna versiyon (v1, v2) ekleyerek eski veriyi yetim bırakma.
- Event-based invalidation: Veri değiştiğinde event fırlatıp consumer ile cache temizleme.

---

### **🔹 Cache Scope**

- In-memory cache: Uygulamanın kendi RAM'inde (Hızlı ama dağıtık değil, Restartta gider).
- Distributed cache: Redis/Memcached gibi harici servis (Kalıcı, paylaşılabilir, network latency var).
- Per-user vs global cache: Kullanıcıya özel veri (Session) vs Herkesin gördüğü veri (Ürün listesi).
- Cache stampede problemi: Cache süresi bittiğinde binlerce isteğin aynı anda DB'ye saldırması.

## **4️⃣ N+1 Problem**

- N+1 query problemi nedir: Bir ana kayıt (1) ve ilişkili N kayıt için N adet ayrı SQL sorgusu atılması.
- ORM kullanırken nasıl ortaya çıkar: Lazy loading açıkken döngü içinde ilişkili tabloya erişildiğinde.
- Lazy loading vs Eager loading çözümü: Veriyi ihtiyaç anında çekmek (Lazy) vs Baştan join ile çekmek (Eager).
- Batch fetch / Include / Join kullanımı: `.Include(x => x.Orders)` diyerek tek sorguda veriyi almak.

🎤

> “N+1 problemi performans düşmanı, ORM’de eager loading ile çözülür.”
> 

---

## **5️⃣ REST & API Design**

- RESTful API prensipleri: Kaynağa yönelim, Client-Server ayrımı, Stateless yapı, Cachelenebilirlik.
- Stateless API: Sunucunun client durumunu (session) tutmaması, her isteğin kendi kendine yetmesi.
- Resource vs Endpoint: Resource "Ürün"dür, Endpoint ona ulaşım adresidir (`/products/123`).
- HTTP Methods (GET, POST, PUT, DELETE): Veri almak, yaratmak, güncellemek ve silmek için standart fiiller.
- Status code kullanımı (200, 201, 204, 400, 401, 404, 500): İşlem sonucunu evrensel kodlarla bildirme.
- HATEOAS (concept): Sunucunun client'a yapabileceği sonraki işlemleri link olarak dönmesi.

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

---

## **3️⃣ Execution Plan & Query Optimization**

- Execution plan nasıl okunur: Sorgunun izlediği yol haritası (Hangi index kullanıldı, ne kadar sürdü).
- Cost nedir: İşlemcinin ve diskin harcadığı tahmini kaynak maliyeti.
- Key lookup problemi: Non-clustered index kullandıktan sonra asıl veriye gitmek için tabloya (Clustered Index) sıçrama maliyeti.
- Parameter sniffing: İlk çalıştırılan parametreye göre plan oluşturulup, sonraki farklı parametrelerde o planın kötü çalışması.
- Statistics önemi: SQL Server'ın veri dağılımını bilmesini sağlar, optimizasyon için kritiktir.

📌 **Gerçek senior konusu**

---

## **4️⃣ Transaction Management**

- ACID nedir: Atomicity (Hepsi/Hiçbiri), Consistency (Tutarlılık), Isolation (Yalıtım), Durability (Kalıcılık).
- BEGIN / COMMIT / ROLLBACK: Transaction başlatma, onaylama ve geri alma komutları.
- Nested transaction gerçek mi: Hayır, SQL Server'da `@@TRANCOUNT` artar ama sadece en dıştaki Commit/Rollback gerçektir.
- Long-running transaction riskleri: Log dosyasını şişirir, tabloyu kilitler (Blocking).

🎤:

> “Uzun transaction lock süresini uzatır.”
> 

---

## **5️⃣ Locking & Concurrency (🔥)**

- Shared / Exclusive lock: Okuma kilidi (Shared - başkaları okuyabilir) vs Yazma kilidi (Exclusive - kimse erişemez).
- Deadlock nedir: İki işlemin birbirinin kilitlediği kaynağı beklemesi ve kilitlenmesi.
- Deadlock nasıl tespit edilir: SQL Error 1205 veya Extended Events / Profiler ile.
- Isolation levels: Transactionların birbirini ne kadar etkileyeceğini belirleyen seviyeler.

### **Isolation Levels**

- Read Uncommitted: Kirli okuma (Dirty Read) yapar, kilit koymaz (En hızlı, en güvensiz).
- Read Committed: Sadece commitlenmiş veriyi okur (Default).
- Repeatable Read: Okunan veri transaction bitene kadar değişmez (Update kilidi koyar).
- Serializable: Tam yalıtım, araya yeni kayıt (Phantom Read) bile giremez (En yavaş).
- Snapshot Isolation: Versiyonlama kullanarak kilit koymadan tutarlı okuma sağlar (TempDB kullanır).

🎤:

> “Isolation level performans ve tutarlılık trade-off’udur.”
> 

---

## **6️⃣ Stored Procedure vs Ad-hoc Query**

- Execution plan reuse: SP'lerin planı cachelenir ve tekrar kullanılır (Performans sağlar).
- Security avantajı: Kullanıcıya tabloya değil, SP'ye yetki verilir; SQL Injection riski azalır.
- Versiyonlama zorlukları: Kodu veritabanında saklamak Git ile takibi ve deployment'ı zorlaştırır.
- Ne zaman SP, ne zaman ORM: Karmaşık raporlar ve toplu işlemler için SP; CRUD için ORM.

---

## **7️⃣ Data Modeling**

- Normalization (1NF–3NF): Veri tekrarını önleme ve tutarlılığı sağlama kuralları.
- Denormalization ne zaman: Okuma performansını artırmak için bilerek veri tekrarı yapma (Reporting vb.).
- Surrogate vs Natural key: Yapay artan ID (Surrogate) vs TC Kimlik/Email (Natural).
- Soft delete vs hard delete: `IsDeleted=1` yapmak (Soft) vs Veriyi fiziksel silmek (Hard).

---

## **8️⃣ Pagination & Large Data**

- OFFSET / FETCH: Standart sayfalama (`Skip/Take`), büyük sayfalarda yavaştır.
- Keyset pagination: `WHERE Id > LastId` diyerek son kalınan yerden devam etme (Çok hızlıdır).
- Large table scan riskleri: Büyük tabloda indexsiz arama (Scan) veritabanını kilitler.
- Batch processing: Büyük işlemleri küçük parçalara (Chunk) bölerek yapma.

🎤:

> “Offset pagination büyük tablolarda performans sorunu yaratır.”
> 

---

## **9️⃣ Performance & Scalability**

- Read replica (concept): Okuma yükünü başka sunucuya dağıtma.
- Partitioning: Büyük tabloları fiziksel olarak parçalara bölme (Yıla göre vb.).
- TempDB kullanımı: Geçici tablolar ve sıralama işlemleri için kullanılan sistem veritabanı.
- Connection pooling: Veritabanı bağlantılarını kapatmayıp havuzda tutarak tekrar kullanma.
- IO vs CPU bottleneck: Sorunun diskte mi işlemcide mi olduğunu anlama.

---

## **🔟 Error Handling (SQL Server)**

- TRY / CATCH: T-SQL içinde hata yakalama blokları.
- THROW vs RAISERROR: `THROW` hatayı olduğu gibi fırlatır, `RAISERROR` özelleştirilebilir ama eskidir.
- Transaction rollback: Hata durumunda `ROLLBACK` çağırmanın önemi.
- Error propagation: Hatanın C# tarafına nasıl iletildiği.

---

## **1️⃣1️⃣ Security**

- SQL Injection: Kötü niyetli kodun SQL sorgusuna enjekte edilmesi.
- Parameterized query: Parametre kullanarak injection'ı %100 engelleme.
- Least privilege: Kullanıcıya sadece ihtiyacı olan en az yetkiyi verme prensibi.
- Encryption at rest / in transit: Diskte şifreli saklama (TDE) ve ağda şifreli taşıma (SSL/TLS).

---

## **1️⃣2️⃣ Backup, Restore & Reliability (Concept)**

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

# Docker

## **1️⃣ Docker Fundamentals**

- Docker nedir: Uygulamaları izole kaplarda (container) çalıştıran platform.
- Container vs Virtual Machine: Container işletim sistemini paylaşır (Hafif), VM donanımı sanallaştırır (Ağır).
- Docker hangi problemleri çözer: "Benim makinemde çalışıyordu" sorununu çözer, tutarlı ortam sağlar.
- Docker ne zaman KULLANILMAZ: Grafik arayüzlü (GUI) masaüstü uygulamaları veya kernel modifikasyonu gereken işler için.
- Image vs Container farkı: Image şablondur (Class), Container çalışan örnektir (Object).

🎤:

> “Docker uygulamayı environment’tan bağımsız hale getirir.”
> 

---

## **2️⃣ Docker Architecture**

- Docker Engine: Docker'ın çekirdek çalışma zamanı.
- Docker Daemon: Arka planda çalışan, container'ları yöneten servis (`dockerd`).
- Docker Client: Kullanıcının komut girdiği CLI (`docker build/run`).
- Docker Registry (Docker Hub): Image'ların saklandığı depo (GitLab Registry, ACR).
- Image layers mantığı: Her komutun (`RUN`, `COPY`) salt okunur bir katman oluşturması (Cache ve hız sağlar).

---

## **3️⃣ Docker Image & Dockerfile (🔥)**

- Dockerfile nedir: Image oluşturma tarifini içeren metin dosyası.
- FROM / RUN / COPY / ADD: Baz image seç / Komut çalıştır / Dosya kopyala / URL'den dosya çek.
- CMD vs ENTRYPOINT: `CMD` varsayılan komuttur (ezilebilir), `ENTRYPOINT` ana çalıştırıcıdır (argüman alır).
- EXPOSE: Container'ın dinlediği portu belirtme (Dokümantasyon amaçlı).
- ENV / ARG farkı: `ENV` çalışma zamanında (Runtime) var, `ARG` sadece build zamanında var.
- .dockerignore: Gereksiz dosyaların (node_modules, .git) image'a girmesini engelleme.

🎤:

> “Dockerfile ne kadar küçükse o kadar iyidir.”
> 

---

## **4️⃣ Image Optimization & Best Practices**

- Multi-stage build: Derleme (SDK) ve Çalıştırma (Runtime) aşamalarını ayırarak image boyutunu küçültme.
- Küçük base image seçimi (alpine): Gereksiz araçlar barındırmayan minik Linux dağıtımları kullanma.
- Layer caching mantığı: Değişmeyen katmanların (örn. `npm install`) tekrar build edilmemesi.
- Gereksiz file kopyalamamak: Sadece gereken kodları kopyalamak.
- Build context küçültme: Docker daemon'a gönderilen dosya boyutunu `.dockerignore` ile azaltma.

---

## **5️⃣ Container Lifecycle**

- create / start / stop / restart: Container yaşam döngüsü komutları.
- Container state’leri: Created, Running, Paused, Exited, Dead.
- Graceful shutdown: Container'a `SIGTERM` sinyali gönderip kapanmasını bekleme.
- Restart policies: Çöken container'ın otomatik yeniden başlatılması (`on-failure`, `always`).

---

## **6️⃣ Networking**

- Bridge network: Default ağ, aynı hosttaki container'lar haberleşir.
- Host network: Container host'un ağını direkt kullanır (Port izolasyonu yok).
- Overlay network (concept): Farklı sunuculardaki (Swarm/K8s) container'ları bağlar.
- Port mapping: Host portunu container portuna yönlendirme (`-p 8080:80`).
- Container-to-container communication: Docker ağı içinde container isimleriyle haberleşme (DNS).

🎤:

> “Container’lar default olarak isolated network’te çalışır.”
> 

---

## **7️⃣ Volumes & Persistence (ÇOK SORULUR)**

- Volume nedir: Veriyi container dışında host üzerinde kalıcı saklama alanı.
- Bind mount vs volume: Bind mount hosttaki belirli klasörü bağlar, Volume Docker tarafından yönetilen alanı bağlar.
- Data persistence mantığı: Container silinse bile verinin (DB dosyaları) kaybolmaması.
- Stateless container yaklaşımı: Container içinde veri tutmamak, veriyi volume veya DB servisine yıkmak.

📌:

> “Container stateless, data dışarıda.”
> 

---

## **8️⃣ Environment & Configuration**

- ENV variables: Çalışma zamanında konfigürasyon geçme (`-e DB_HOST=localhost`).
- Secrets yönetimi: Hassas verileri (şifreler) güvenli saklama (Docker Swarm/K8s özelliği).
- Config injection: Config dosyalarını volume ile içeri atma.
- 12-factor app prensibi: Konfigürasyonu koddan ayırma ve ortam değişkenleriyle yönetme.

---

## **9️⃣ Docker Compose**

- docker-compose.yml: Çoklu container uygulamasını tanımlayan YAML dosyası.
- Multi-container setup: API, DB, Cache gibi servisleri tek komutla (`up`) kaldırma.
- Service dependency: `depends_on` ile açılış sırasını belirleme (DB kalkmadan API kalkmasın).
- Network & volume tanımı: Servislerin ortak ağ ve volume'leri kullanması.
- Local development senaryoları: Geliştirme ortamını tek tuşla ayağa kaldırma kolaylığı.

---

## **🔟 Security Best Practices**

- Root user ile çalışmamak: Container içinde `USER` komutu ile yetkisiz kullanıcıya geçmek.
- Image scanning: Image içindeki güvenlik açıklarını taramak (Trivy, Snyk).
- Minimal image kullanımı: Saldırı yüzeyini (Attack surface) azaltmak için gereksiz paketleri silmek.
- Secrets image içine koymamak: Şifreleri Dockerfile içine GÖMMEMEK.

---

## **1️⃣1️⃣ Logging & Monitoring**

- stdout / stderr logging: Logları dosyaya değil konsola basmak (Docker yakalar).
- Log driver’lar: Logları dosya, Syslog, Fluentd veya AWS CloudWatch'a yönlendirme.
- Container healthcheck: Uygulamanın sağlıklı çalıştığını kontrol eden komut (`HEALTHCHECK`).
- Resource usage (CPU / memory): `docker stats` ile kaynak tüketimini izleme.

---

## **1️⃣2️⃣ Docker & CI/CD**

- Docker build pipeline: CI sunucusunda image build etme.
- Image tagging: Versiyonlama (`v1.0.0`, `latest`, `git-sha`).
- Push / pull registry: Image'ı depoya gönderme ve sunucuya çekme.
- Versioning strategy: Her build için unique tag kullanma.

---

## **1️⃣3️⃣ Docker vs Kubernetes (Concept)**

- Docker ne yapar: Tekil container'ı çalıştırır ve paketler.
- Kubernetes ne yapar: Binlerce container'ı yönetir (Orchestration).
- Ne zaman sadece Docker yeter: Tek sunuculu, basit uygulamalar veya geliştirme ortamı için.
- Ne zaman K8s gerekir: Çok sunuculu, yüksek erişilebilirlik, otomatik ölçeklenme gerektiğinde.

---

## **1️⃣4️⃣ Production Anti-Patterns**

- Container içinde DB: Veritabanını container'da çalıştırmak production için risklidir (Yönetimi zor).
- Large image’lar: Yavaş deployment ve güvenlik riski yaratır.
- Hardcoded config: Ortamlar arası taşımayı imkansız kılar.
- State tutan container: Ölçeklenmeyi engeller (Stateless olmalı).

# Kubernetes

## **1️⃣ Kubernetes Fundamentals**

- Kubernetes nedir: Container orchestration (yönetim) platformu (Google tarafından geliştirildi).
- Kubernetes hangi problemi çözer: Çok sayıda container'ın deployment, scaling ve yönetim karmaşasını çözer.
- Docker vs Kubernetes farkı: Docker uçaksa, Kubernetes havaalanı kulesidir.
- Kubernetes ne zaman GEREKLİ DEĞİL: Küçük, tekil uygulamalar için (Overkill).
- Kubernetes cluster kavramı: Master ve Worker node'lardan oluşan sunucu kümesi.

🎤

> “Kubernetes container orchestration platformudur.”
> 

---

## **2️⃣ Kubernetes Architecture (ÇOK SORULUR)**

- Control Plane nedir: Cluster'ın beyni (Karar mekanizması).
- Node nedir: Container'ların çalıştığı fiziksel veya sanal sunucu.
- Master / Worker node farkı: Master yönetir, Worker işi yapar.
- kube-apiserver: Tüm isteklerin geldiği API kapısı (Frontend).
- etcd: Cluster'ın tüm verisini (konfigürasyon, state) tutan key-value veritabanı.
- scheduler: Yeni oluşan Pod'un hangi Node'da çalışacağına karar verir.
- controller-manager: İstenen durum (Desired State) ile mevcut durumu eşitleyen döngü.

🎤

> “etcd cluster state’in tek kaynağıdır.”
> 

---

## **3️⃣ Core Objects (OLMAZSA OLMAZ)**

### **🔹 Pod**

- Pod nedir: Kubernetes'teki en küçük çalışma birimi (Bir veya daha fazla container).
- Pod neden container’dan farklı: Pod, container'a IP, Volume ve Network paylaşımı sağlar (Container kılıfı).
- Pod lifecycle: Pending, Running, Succeeded, Failed, Unknown.
- Multi-container pod senaryosu: Ana uygulama ve yanına yardımcı (Sidecar) container (örn. Log toplayıcı).

### **🔹 Deployment**

- Deployment nedir: Pod'ların güncellenmesini ve çoğaltılmasını yöneten obje.
- ReplicaSet ilişkisi: Deployment, ReplicaSet'i yönetir; ReplicaSet, Pod sayısını sabit tutar.
- Rolling update: Uygulamayı kesinti olmadan sırayla güncelleme stratejisi.
- Rollback: Hatalı güncellemede eski versiyona tek komutla dönme.

### **🔹 Service**

- ClusterIP: Sadece cluster içinden erişilebilen, Pod'lara sabit IP sağlayan servis (Default).
- NodePort: Her Node üzerinde bir port açarak dış erişim sağlar.
- LoadBalancer: Cloud provider'ın yük dengeleyicisini kullanarak dış erişim sağlar.
- Service discovery: Pod IP'leri değişse bile Service ismiyle (DNS) ulaşım imkanı.

🎤

> “Pod’lar ephemeral, Service’ler stable’dır.”
> 

---

## **4️⃣ Configuration Management**

- ConfigMap: Konfigürasyon verilerini (Key-Value) tutan obje.
- Secret: Şifre, token gibi hassas verileri (Base64 encoded) tutan obje.
- Environment variable injection: ConfigMap/Secret verisini Pod'a ortam değişkeni olarak verme.
- Volume ile config bağlama: Config dosyasını Pod içine dosya olarak mount etme.

📌

> “Config image’te değil, Kubernetes objesinde olmalı.”
> 

---

## **5️⃣ Networking (🔥)**

- Pod-to-pod communication: Her Pod'un kendi IP'si vardır ve NAT olmadan konuşabilirler.
- Service-to-pod routing: Service trafiği arkasındaki Pod'lara (Labels/Selectors ile) dağıtır.
- DNS (CoreDNS): Servis isimlerini IP'ye çeviren cluster içi DNS sunucusu.
- Ingress nedir: HTTP/HTTPS trafiğini yöneten, domain bazlı yönlendirme yapan akıllı router.
- Ingress Controller: Ingress kurallarını uygulayan sunucu (NGINX, Traefik).

🎤

> “Ingress dış dünyaya açılan kapıdır.”
> 

---

## **6️⃣ Storage & Persistence**

- Volume nedir: Pod ömrü kadar yaşayan veri alanı.
- PersistentVolume (PV): Cluster'daki fiziksel depolama kaynağı (Disk).
- PersistentVolumeClaim (PVC): Uygulamanın depolama talebi (Bana 10GB disk ver).
- StorageClass: Dinamik disk oluşturma profili (Fast SSD, Standard HDD).
- Stateful vs Stateless app: Veri tutan (DB) vs Tutmayan (API) uygulama ayrımı.

📌

> “Pod gider, volume kalır.”
> 

---

## **7️⃣ Scaling & Availability**

- Replica count: İstenen Pod kopya sayısı.
- Horizontal Pod Autoscaler (HPA): CPU/RAM yüküne göre Pod sayısını otomatik artırıp azaltma.
- CPU / Memory based scaling: Kaynak kullanım eşiklerine göre ölçekleme.
- Self-healing mantığı: Çöken Pod'un yerine yenisinin otomatik başlatılması.

🎤

> “Kubernetes failed pod’ları otomatik ayağa kaldırır.”
> 

---

## **8️⃣ Resource Management**

- CPU requests / limits: Pod'un garanti edilen (Request) ve aşamayacağı (Limit) işlemci gücü.
- Memory requests / limits: Pod'un garanti edilen ve aşamayacağı RAM miktarı.
- OOMKilled nedir: Limitinden fazla RAM tüketen Pod'un işletim sistemi tarafından öldürülmesi.
- Resource starvation: Request tanımlanmazsa bazı Pod'ların kaynak bulamaması.

---

## **9️⃣ Health Checks (ÇOK SORULUR)**

- Liveness probe: "Uygulama çöktü mü?" kontrolü. Başarısızsa Pod restart edilir.
- Readiness probe: "Uygulama trafik almaya hazır mı?" kontrolü. Başarısızsa trafik almaz.
- Startup probe: "Uygulama ayağa kalktı mı?" kontrolü (Yavaş başlayan uygulamalar için).
- Traffic yönetimi: Readiness probe sayesinde bozuk Pod'a kullanıcı isteği gitmez.

🎤

> “Readiness false ise pod alive ama traffic almaz.”
> 

---

## **🔟 Deployment Strategies (Senior Konu)**

- Rolling update: Yavaş yavaş eskiyi indirip yeniyi açma (Default, Zero Downtime).
- Recreate: Hepsini kapatıp yenilerini açma (Downtime olur).
- Blue-Green: Yeni versiyonu ayrı ortamda test edip trafiği birden kaydırma.
- Canary deployment: Trafiğin küçük bir kısmını (%5) yeni versiyona yönlendirme.

---

## **1️⃣1️⃣ Security (Concept)**

- RBAC: Role Based Access Control - Kimin ne yapabileceğini (Pod silebilsin vb.) belirleme.
- ServiceAccount: Pod'ların (uygulamaların) API server ile konuşurken kullandığı kimlik.
- Namespace izolasyonu: Kaynakları mantıksal gruplara ayırma (Dev, Test, Prod).
- Pod Security Context: Pod'un hangi kullanıcı ID ile çalışacağını belirleme (Root olmasın).
- Network Policy: Podlar arası trafiği kısıtlama (Firewall kuralları).

---

## **1️⃣2️⃣ Observability**

- Logs (kubectl logs): Pod loglarını okuma.
- Metrics (Prometheus): CPU, RAM ve uygulama metriklerini toplama.
- Tracing: Servisler arası isteğin izini sürme (Jaeger).
- Alerts: Sorun anında bildirim gönderme (Alertmanager).

📌

> “Gözlemleyemediğin sistemi yönetemezsin.”
> 

---

## **1️⃣3️⃣ Kubernetes & CI/CD**

- Image tagging strategy: Her commit/build için benzersiz tag kullanımı.
- Deployment automation: Git push ile cluster'ın güncellenmesi.
- Rollback senaryosu: Hatalı deployment'ın otomatik geri alınması.
- GitOps (concept): Cluster durumunun Git reposunda tutulması (ArgoCD).

---

## **1️⃣4️⃣ Production Anti-Patterns**

- Pod içinde state tutmak: Pod ölünce veri kaybolur.
- Hardcoded config: ConfigMap kullanılmalı.
- Limits tanımlamamak: Bir Pod tüm cluster kaynağını tüketebilir.
- Tek replica critical service: Node çökerse hizmet durur (En az 2 replica olmalı).

---

## **1️⃣5️⃣ Kubernetes Ne Zaman KULLANILMAZ?**

- Küçük projeler: Yönetim maliyeti faydasından fazladır.
- Tek servis: Basit Docker veya PaaS yeterlidir.
- Düşük trafik: Statik hosting veya Lambdalar daha ucuzdur.
- Ops ekibi yoksa: Yönetimi zordur, yönetilen servisler (K8s Service) veya App Runner tercih edilmeli.

# RabbitMQ

## **1️⃣ RabbitMQ Fundamentals**

- RabbitMQ nedir: AMQP protokolünü kullanan açık kaynak mesaj kuyruk sistemi.
- Message broker kavramı: Mesajları göndericiden alıp alıcıya ileten aracı yazılım.
- Queue vs Topic vs Exchange: Kuyruk (Depo), Konu (Kategori), Santral (Yönlendirici).
- RabbitMQ ne zaman tercih edilir: Karmaşık yönlendirme, güvenilir teslimat ve önceliklendirme gerektiğinde.
- RabbitMQ ne zaman KULLANILMAZ: Çok yüksek throughput (milyonlarca mesaj/sn) ve log saklama (Kafka işi) için.

🎤

> “RabbitMQ uygulamalar arası asenkron iletişim sağlar.”
> 

---

## **2️⃣ Core Concepts**

- Producer: Mesajı üreten ve RabbitMQ'ya gönderen uygulama.
- Consumer: Kuyruktan mesajı alıp işleyen uygulama.
- Queue: Mesajların beklediği tampon bölge.
- Exchange (Direct / Fanout / Topic / Headers): Mesajı kuyruklara dağıtan yönlendirici (Postane).
- Binding: Exchange ile Queue arasındaki bağlantı kuralı.
- Routing key: Mesajın hangi yoldan gideceğini belirleyen etiket.
- Virtual host (vhost): RabbitMQ içinde mantıksal izolasyon (Namespace gibi).

---

## **3️⃣ Messaging Patterns**

- Work Queue (Task Queue): İş yükünü birden fazla işçiye (Consumer) dağıtma.
- Publish / Subscribe: Bir mesajı ilgilenen tüm abonelere (Queue) iletme (Fanout).
- Routing / Topic Exchange: Mesajı konusuna göre (`log.error`, `log.info`) ilgili kuyruklara gönderme.
- RPC over RabbitMQ: Request/Response yapısını kuyruk üzerinden simüle etme.
- Dead Letter Exchange: İşlenemeyen veya hatalı mesajların gönderildiği "Ölü Mektup" kuyruğu.

🎤:

> “Dead Letter Queue, mesaj işlenemezse başka kuyrukta toplanır.”
> 

---

## **4️⃣ Message Delivery Semantics**

- At-most-once: Mesaj gönderilir, kaybolursa tekrar gönderilmez (En hızlı).
- At-least-once: Mesajın en az bir kere ulaştığı garanti edilir (Consumer idempotent olmalı).
- Exactly-once (concept): Mesajın tam olarak bir kere işlenmesi (RabbitMQ'da zordur).
- Acknowledgements (ACK / NACK): Consumer'ın "Mesajı aldım, silebilirsin" onayı.
- Durable queues & persistent messages: Sunucu kapansa bile mesajın diskte saklanması.

---

## **5️⃣ Queue & Exchange Management**

- Queue durability: RabbitMQ restart olduğunda kuyruğun silinip silinmeyeceği.
- Auto-delete queue: Son consumer ayrıldığında kuyruğun otomatik silinmesi.
- Exclusive queue: Sadece oluşturan bağlantı (Connection) tarafından kullanılan özel kuyruk.
- TTL & message expiration: Mesajın belirli sürede işlenmezse silinmesi.
- Max-length / max-priority: Kuyruk boyutu ve mesaj önceliği sınırları.

---

## **6️⃣ Concurrency & Scaling**

- Prefetch count: Consumer'ın aynı anda işleyebileceği maksimum mesaj sayısı (Yük dengeleme).
- Multiple consumers per queue: Aynı kuyruğu dinleyen birden fazla işçi ile paralel işleme (Competing Consumers).
- Consumer acknowledgment: İşlem bitince ACK göndererek kuyruktan düşme.
- Load balancing across consumers: Round-robin mantığıyla işlerin dağıtılması.

🎤:

> “Prefetch sayısı tüketiciye düşen mesaj yükünü kontrol eder.”
> 

---

## **7️⃣ Reliability & Fault Tolerance**

- Mirrored / quorum queues: Kuyruğun kopyalarının farklı node'larda tutulması (HA).
- High availability cluster: Birden fazla RabbitMQ sunucusu ile kesintisiz hizmet.
- Network partition handling: Ağ kopması durumunda sunucuların nasıl davranacağı (Pause_minority vb.).
- Publisher confirms: Mesajın broker'a ulaştığının teyidi.

---

## **8️⃣ Performance Tuning**

- Connection & channel management: TCP bağlantısını sürekli açıp kapatmak yerine Channel kullanma.
- Batch publish: Mesajları toplu gönderme.
- Consumer concurrency: İşçi sayısını artırma.
- Persistent vs transient messages trade-off: Diske yazma maliyeti vs Hız.

---

## **9️⃣ Monitoring & Observability**

- RabbitMQ Management UI: Web arayüzü ile kuyrukları izleme.
- Metrics (queue length, publish rate, consumer count): Prometheus ile izlenecek kritik metrikler.
- Alerts (unacked messages, queue growth): Mesaj birikmesi durumunda alarm üretme.
- Logs: Hata ve uyarı logları.

---

## **🔟 Security**

- Authentication & authorization: Kullanıcı adı/şifre ve izin yönetimi.
- User / vhost permissions: Hangi kullanıcının hangi vhost'a erişebileceği.
- TLS encryption: Mesaj trafiğinin şifrelenmesi.
- Policy management: Kuralların merkezi yönetimi.

---

## **1️⃣1️⃣ RabbitMQ & .NET Integration**

- RabbitMQ.Client usage: Resmi .NET kütüphanesi.
- Connection / channel lifecycle: Connection singleton, Channel thread-safe değildir.
- Async consumer: `EventingBasicConsumer` ile asenkron mesaj işleme.
- Retry & Dead-letter handling: Polly ve DLX ile hata yönetimi.

---

## **1️⃣2️⃣ Anti-Patterns & Pitfalls**

- Queue overload → OOM: Kuyruk çok şişerse RAM biter (Out Of Memory).
- Long-running consumer without ACK: Mesaj uzun süre ACK beklemezse tekrar kuyruğa dönebilir.
- Single point of failure (standalone RabbitMQ): Cluster kurulmazsa sunucu gidince her şey durur.
- Persistent messages everywhere → disk IO bottleneck: Gereksiz yere her mesajı diske yazmak sistemi yavaşlatır.

---

## **1️⃣3️⃣ RabbitMQ Ne Zaman KULLANILMAZ?**

- Çok düşük latency gereken işlem: Doğrudan TCP/gRPC daha hızlıdır.
- Small-scale, simple CRUD: Basit projeler için overkill olabilir.
- Strong consistency gerektiren transaction-heavy işlem: Veritabanı daha uygundur.
- Stateful communication yeterli ise: Redis pub/sub veya HTTP yeterli olabilir.

# Kafka

## **1️⃣ Kafka Fundamentals**

- Kafka nedir: Dağıtık streaming platformu (Log tabanlı).
- Kafka vs RabbitMQ farkları: Kafka mesajı saklar (Retention), RabbitMQ siler (Queue). Kafka pull, RabbitMQ push modelidir.
- Kafka ne zaman tercih edilir: Büyük veri akışı (Streaming), Log toplama, Event Sourcing.
- Kafka ne zaman KULLANILMAZ: Karmaşık routing, anlık request/response işleri için.
- Streaming platform kavramı: Veriyi sürekli akan bir nehir gibi işleme.

🎤

> “Kafka bir message broker değil, event streaming platformudur.”
> 

---

## **2️⃣ Kafka Architecture**

- Broker: Kafka sunucusu.
- Zookeeper / KRaft: Cluster yönetimi ve metadata saklama (KRaft ile Zookeeper kalkıyor).
- Topic vs Queue farkı: Topic log dosyasıdır, silinmez; Queue geçici depodur.
- Partitioning mantığı: Topic'in parçalara bölünerek paralel işlenmesi (Scaling).
- Replication factor: Verinin kaç kopyasının tutulacağı (Fault tolerance).
- Leader / Follower replica: Yazma/Okuma Leader'dan, Follower sadece takip eder.

---

## **3️⃣ Core Concepts**

- Producer: Mesajı üreten.
- Consumer: Mesajı okuyan.
- Consumer Group: Aynı işi yapan consumer grubu (Her partition sadece bir üyeye atanır).
- Offset kavramı (En önemli): Consumer'ın nerede kaldığını belirten işaretçi (Bookmark).
- Log retention policy: Mesajların ne kadar süre (7 gün) veya boyut (1GB) saklanacağı.

🎤

> “Consumer offset’i kendisi yönetir.”
> 

---

## **4️⃣ Data Modeling & Partitions**

- Key-based partitioning: Aynı Key'e sahip mesajların hep aynı partition'a gitmesi (Sıralama garantisi).
- Ordering guarantee: Kafka sadece partition içinde sıralama garanti eder (Tüm topic'te değil).
- Partition sayısı nasıl belirlenir: Hedeflenen throughput ve consumer sayısına göre.
- Topic compaction: Aynı key için sadece en son değeri saklama (Kütüphane mantığı).

---

## **5️⃣ Writing Data (Producer)**

- acks=0, 1, all: Yazma onayı seviyeleri (Hiç bekleme, Leader bekle, Herkesi bekle).
- Batch processing: Mesajları toplu gönderme (Network optimizasyonu).
- Compression (gzip, snappy): Veriyi sıkıştırarak gönderme (Disk/Network tasarrufu).
- Idempotent producer: Tekrar gönderilen mesajların (Retry) çiftlenmesini engelleme.

---

## **6️⃣ Reading Data (Consumer)**

- Polling mechanism: Consumer veriyi kendisi çeker (Pull).
- Consumer rebalancing: Yeni consumer gelince partitionların yeniden dağıtılması (Stop-the-world).
- Auto-commit vs Manual commit: Offset'i otomatik kaydetme vs Elle güvenli kaydetme.
- Lag monitoring: Consumer'ın ne kadar geriden geldiğinin takibi.

🎤

> “Rebalancing sırasında consumer’lar durur.”
> 

---

## **7️⃣ Kafka Ecosystem**

- Kafka Connect: Veritabanı, S3 vb. kaynaklardan Kafka'ya veri aktarımı (Source/Sink).
- Kafka Streams: Veriyi Kafka içinde işleme/dönüştürme kütüphanesi (Java/Scala).
- KSQL (ksqlDB): Kafka üzerindeki veriye SQL sorgusu atma.
- Schema Registry: Mesaj formatını (Avro, Protobuf) doğrulama ve versiyonlama.

---

## **8️⃣ Performance & Reliability**

- Zero-copy mechanism: Veriyi kernel seviyesinde kopyalayarak yüksek hız (Disk -> Network).
- Sequential I/O: Diske sıralı yazma (Rastgele yazmadan çok daha hızlıdır).
- Disk throughput: Kafka bellekten çok diski kullanır, hızlı disk önemlidir.
- High Availability (HA): Broker çökse bile veri kaybını önleme.

---

## **9️⃣ Kafka & .NET**

- Confluent.Kafka kütüphanesi: .NET için resmi client.
- Producer/Consumer implementasyonu: `ProducerBuilder` ve `ConsumerBuilder` kullanımı.
- Serialization (JSON, Avro, Protobuf): Veriyi byte dizisine çevirme yöntemleri.
- Background service ile consumption: `BackgroundService` içinde sonsuz döngüde `Consume()`.

---

## **🔟 Kafka Ne Zaman Gerekli?**

- Yüksek throughput (100k+ msg/sec): Devasa veri trafiği.
- Event Sourcing: Tüm olay tarihçesini saklama.
- Log aggregation: Tüm sistemin loglarını toplama.
- Stream processing: Anlık veri analizi ve ETL işlemleri.
