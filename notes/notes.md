
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
  ```csharp
  int[] arr = new int[5];
  arr[0] = 10; // O(1)
  ```
- Array insert / delete maliyeti: Araya ekleme/silme kaydırma gerektirdiğinden O(n) maliyetlidir.
  ```csharp
  // 2. indekse eleman eklemek için sonrakileri kaydır
  for (int i = n; i > 2; i--) arr[i] = arr[i-1];
  ```
- Two Sum problemi: Bir dizide toplamları hedef sayıya (`Target`) eşit olan iki sayının indeksini bulma.
  ```csharp
  // O(n) Çözüm:
  Dictionary<int, int> seen = new();
  for (int i = 0; i < nums.Length; i++) {
      int diff = target - nums[i];
      if (seen.ContainsKey(diff)) return new int[] { seen[diff], i };
      seen[nums[i]] = i;
  }
  ```
- Array rotate: Diziyi sağa/sola kaydırma işlemi (Modülo aritmetiği veya ters çevirme yöntemi).
  ```csharp
  // [1,2,3,4,5] -> Sağ (2) -> [4,5,1,2,3]
  Reverse(nums, 0, n-1); Reverse(nums, 0, k-1); Reverse(nums, k, n-1);
  ```
- Duplicate eleman bulma: Tekrar eden sayıları tespit etme (HashSet veya Sorting ile).
  ```csharp
  HashSet<int> set = new();
  if (!set.Add(num)) return true; // Zaten varsa duplicate
  ```
- Max / Min eleman bulma: Tüm diziyi gezerek en küçük/büyük değeri bulma O(n).
  ```csharp
  int max = nums[0];
  foreach (var n in nums) if (n > max) max = n;
  ```
- C# T[] kullanımı: `int[] numbers = new int[5];` şeklinde tanımlanan sabit dizi.
- Span<T> nedir, ne zaman kullanılır: Bellek tahsisi yapmadan (heap allocation'sız) array dilimleme (slicing) için kullanılır.
  ```csharp
  Span<int> slice = nums.AsSpan().Slice(1, 3); // Copy yok, sadece pointer
  ```

---

## **📋 List (Dynamic Array)**

- List vs Array farkları: List dinamik büyür, Array sabittir. List arkada Array kullanır.
- Capacity vs Count: Capacity ayrılan yer, Count dolu olan eleman sayısıdır.
  ```csharp
  List<int> list = new(capacity: 100); // Resize maliyetini önlemek için
  ```
- Amortized Add() maliyeti: Kapasite dolunca yeni dizi açıp kopyalamak O(n), diğer eklemeler O(1)'dir; ortalama O(1).
- Listeyi ters çevirme: Elemanların sırasını `Reverse()` ile döndürme O(n).
- Duplicate silme: Tekrarlı elemanları `Distinct()` ile temizleme.
  ```csharp
  var uniqueList = list.Distinct().ToList();
  ```
- En sık geçen elemanı bulma: Frekans sayımı (Dictionary ile).
  ```csharp
  var mostFrequent = list.GroupBy(x => x).OrderByDescending(g => g.Count()).First().Key;
  ```
- C# List<T> metotları: `Add`, `Remove`, `Contains`, `Find` gibi yardımcı metotlar.

---

## **🔗 Linked List**

- Singly Linked List mantığı: Her düğüm veriyi ve bir sonraki düğümün adresini tutar.
  ```csharp
  class Node { public int Data; public Node Next; }
  ```
- Doubly Linked List mantığı: Düğümler hem önceki hem sonraki düğüm adresini tutar.
- Head / Tail kavramları: Listenin başı (Head) ve sonu (Tail).
- Linked list traversal: Baştan sona düğümleri gezme O(n).
  ```csharp
  var current = head;
  while (current != null) { Print(current.Data); current = current.Next; }
  ```
- Ortadaki elemanı bulma: Fast & Slow pointer tekniği ile (Biri 1, biri 2 gider).
  ```csharp
  // Slow ortadayken Fast sonda olur
  while (fast != null && fast.Next != null) { slow = slow.Next; fast = fast.Next.Next; }
  ```
- Linked list reverse: Pointer'ların yönünü ters çevirerek listeyi döndürme.
  ```csharp
  // prev -> current -> next  ==>  prev <- current <- next
  while (curr != null) { next = curr.Next; curr.Next = prev; prev = curr; curr = next; }
  ```
- Cycle detection: Listede döngü var mı? (Floyd’s Cycle Finding - Fast/Slow pointer).
  ```csharp
  if (slow == fast) return true; // Döngü var
  ```
- Array vs LinkedList karşılaştırması: Array erişimde O(1), eklemede O(n); LinkedList erişimde O(n), (konum belliyse) eklemede O(1).

---

## **📚 Stack (LIFO)**

- Stack temel mantığı: Last In First Out (Son giren ilk çıkar).
- Push / Pop / Peek: Ekle (Push), Çıkar (Pop), En üsttekine bak (Peek) - Hepsi O(1).
  ```csharp
  stack.Push(1); int val = stack.Pop(); int top = stack.Peek();
  ```
- Parantez kontrolü problemi: Açılan parantez kapananla eşleşiyor mu? (Valid Parentheses).
  ```csharp
  if (c == '(') stack.Push(')');
  else if (stack.Count == 0 || stack.Pop() != c) return false;
  ```
- String ters çevirme: Karakterleri Stack'e atıp geri çekerek ters çevirme.
- Undo / Redo senaryosu: Yapılan işlemleri Stack'te tutup geri alma.
- Call stack nasıl çalışır: Fonksiyon çağrılarının bellekte tutulduğu yer (Recursive fonksiyonlar).
- C# Stack<T> kullanımı: `Stack<int> s = new Stack<int>();`

---

## **📥 Queue (FIFO)**

- Queue temel mantığı: First In First Out (İlk giren ilk çıkar).
- Enqueue / Dequeue: Ekle (Enqueue), Çıkar (Dequeue) - Hepsi O(1).
  ```csharp
  queue.Enqueue(1); int val = queue.Dequeue();
  ```
- Producer – Consumer problemi: Bir tarafın üretip diğer tarafın tükettiği asenkron yapı.
- BFS algoritmasında queue kullanımı: Graf/Ağaç gezintisinde katman katman ilerlemek için kullanılır.
- Queue vs Stack farkları: Sıralama farkı (Biri kuyruk, biri yığın).
- C# Queue<T> kullanımı: `Queue<string> q = new Queue<string>();`
- ConcurrentQueue<T> nedir: Thread-safe kuyruk yapısı (Lock-free mekanizmalar içerir).


---

## **🗂️ Dictionary / Hash Table ⭐**

- Hashing mantığı: Key'i (anahtar) sayısal bir index'e dönüştürme fonksiyonu.
  ```csharp
  int index = GetHashCode(key) % arraySize;
  ```
- Collision nedir: Farklı key'lerin aynı hash değerini üretmesi (Çakışma).
- Chaining vs Open Addressing: Çakışma çözme yöntemleri (Bağlı liste kullanma vs boş yer arama).
- Lookup neden O(1): Hash fonksiyonu direkt adresi verdiği için (Çakışma yoksa).
- Duplicate eleman bulma: Elemanları Dictionary key'i yaparak sayma.
- Frequency counter: Bir dizide hangi elemandan kaç tane olduğunu bulma.
  ```csharp
  if (!counts.ContainsKey(num)) counts[num] = 0;
  counts[num]++;
  ```
- Two Sum (Dictionary ile): Aranan farkı (`Target - Current`) Dictionary'de arayarak O(n) çözüm.
- C# Dictionary<TKey, TValue>: Key-Value çiftleri tutan hash tablosu.
  ```csharp
  Dictionary<string, int> ages = new() { {"Ali", 25} };
  if (ages.TryGetValue("Ali", out int age)) Console.WriteLine(age);
  ```
- ConcurrentDictionary kullanımı: Thread-safe dictionary (Çoklu okuma/yazma için).
  ```csharp
  concurrentDict.TryAdd("key", 1); // Locklamadan güvenli ekleme
  ```
- HashSet<T> farkı: Sadece Key tutar, Value yoktur. Unique eleman listesi.

---

## **🧩 Set**

- Set temel mantığı: Benzersiz (Unique) elemanlar kümesi.
- Unique eleman garantisi: Aynı elemandan birden fazla bulunamaz.
- İki listede ortak eleman bulma: Intersection (Kesişim) işlemi.
  ```csharp
  set1.IntersectWith(set2); // Ortakları set1'de bırakır
  ```
- Set difference / union: Fark (ExceptWith) ve Birleşim (UnionWith) işlemleri.
- HashSet vs SortedSet: HashSet sırasız O(1), SortedSet sıralı O(log n) (Red-Black tree kullanır).
- C# HashSet<T> kullanımı: Benzersiz elemanları performanslı saklamak için.

---

## **🌳 Trees (BST, AVL, Red-Black)**

- Binary Tree vs Binary Search Tree (BST): BST'de sol küçüktür, sağ büyüktür.
- BST Insert / Search / Delete maliyeti: Dengeli ise O(log n), dengesiz (çizgi gibi) ise O(n).
  ```csharp
  // Search
  if (val < root.val) Search(root.left, val); else Search(root.right, val);
  ```
- Balanced Tree ihtiyacı: Ağacın tek tarafa uzamasını (Skewed) engellemek için.
- AVL Tree mantığı: Her eklemede dengeyi (Height difference <= 1) koruyan ağaç (Strictly balanced).
- Red-Black Tree mantığı: AVL'den daha az katı kurallı, ekleme/silme daha hızlı (C# SortedDictionary bunu kullanır).
- Tree Traversal (Preorder, Inorder, Postorder):
  - **Inorder (Sol-Kök-Sağ):** BST'yi sıralı yazdırır.
  - **Preorder (Kök-Sol-Sağ):** Ağacı kopyalamak için.
  - **Postorder (Sol-Sağ-Kök):** Ağacı silmek için (Önce çocukları sil).
  ```csharp
  void InOrder(Node n) { if(n==null) return; InOrder(n.left); Print(n.val); InOrder(n.right); }
  ```
- Tree height hesabı: `1 + max(Height(left), Height(right))`.
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

## **HEAP (Binary Heap)**

- Min-Heap vs Max-Heap: Min-Heap'te kök en küçük, Max-Heap'te kök en büyük.
- Priority Queue mantığı: Öncelikli elemanın (Max/Min) hep sırada olduğu kuyruk.
- Insert (Heapify Up) maliyeti: Sona ekleyip yukarı taşıma O(log n).
- Extract Min/Max (Heapify Down) maliyeti: Kökü alıp sonuncuyu başa koyma ve aşağı itme O(log n).
- Array ile Heap gösterimi: `Parent(i) = (i-1)/2`, `Left(i) = 2*i + 1`, `Right(i) = 2*i + 2`.
- C# PriorityQueue<TElement, TPriority>: .NET 6 ile gelen yerleşik Heap yapısı.
  ```csharp
  var pq = new PriorityQueue<string, int>();
  pq.Enqueue("Task1", 2); // Öncelik 2
  pq.Enqueue("Task2", 1); // Öncelik 1 (Önce bu çıkar)
  var item = pq.Dequeue(); // "Task2"
  ```
- Min Heap mantığı: Root en küçük değerdir, ebeveyn çocuktan küçüktür.
- Max Heap mantığı: Root en büyük değerdir, ebeveyn çocuktan büyüktür.
- En büyük K eleman problemi: Min Heap kullanarak akıştaki en büyük K sayıyı tutma.
- En küçük K eleman problemi: Max Heap kullanarak en küçük K sayıyı tutma.
- Task scheduling senaryosu: Öncelik sırasına göre işleri yürütme (Priority Queue).
- C# PriorityQueue<TElement, TPriority>: .NET 6 ile gelen öncelikli kuyruk.

---

## **🕸️ Graphs**

- Graph türleri: Directed (Yönlü), Undirected (Yönsüz), Weighted (Ağırlıklı).
- Adjacency Matrix vs Adjacency List:
  - **Matrix:** `bool[V,V]` (Hızlı erişim, çok yer kaplar).
  - **List:** `List<List<int>>` (Az yer kaplar, traverse kolay). C# genelde Dictionary kullanır: `Dictionary<int, List<int>>`.
- BFS (Breadth-First Search): En kısa yol (Shortest Path) bulmada kullanılır. Queue ile yapılır.
  ```csharp
  // Katman katman gez
  queue.Enqueue(start); visited.Add(start);
  while(queue.Count > 0) { var node = queue.Dequeue(); ... }
  ```
- DFS (Depth-First Search): Labirent çözme, tüm yolları deneme. Stack veya Recursion ile yapılır.
  ```csharp
  // Dibine kadar git
  visited.Add(node); foreach(var neighbor in adj[node]) if(!visited.Contains(neighbor)) DFS(neighbor);
  ```
- Shortest Path (Dijkstra): Ağırlıklı grafta en kısa yol (Priority Queue ile O(E log V)).
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

---

## **2️⃣ Core Concepts**

- Producer: Mesajı üreten ve RabbitMQ'ya gönderen uygulama.
  ```csharp
  channel.BasicPublish(exchange: "", routingKey: "task_queue", body: body);
  ```
- Consumer: Kuyruktan mesajı alıp işleyen uygulama.
  ```csharp
  var consumer = new EventingBasicConsumer(channel);
  consumer.Received += (model, ea) => { ... };
  ```
- Queue: Mesajların beklediği tampon bölge.
- Exchange (Direct / Fanout / Topic / Headers): Mesajı kuyruklara dağıtan yönlendirici (Postane).
  ```csharp
  channel.ExchangeDeclare("logs", ExchangeType.Fanout);
  ```
- Binding: Exchange ile Queue arasındaki bağlantı kuralı.
  ```csharp
  channel.QueueBind(queue: "my_queue", exchange: "logs", routingKey: "");
  ```
- Routing key: Mesajın hangi yoldan gideceğini belirleyen etiket.
- Virtual host (vhost): RabbitMQ içinde mantıksal izolasyon (Namespace gibi).

---

## **3️⃣ Messaging Patterns**

- Work Queue (Task Queue): İş yükünü birden fazla işçiye (Consumer) dağıtma.
- Publish / Subscribe: Bir mesajı ilgilenen tüm abonelere (Queue) iletme (Fanout).
- Routing / Topic Exchange: Mesajı konusuna göre (`log.error`, `log.info`) ilgili kuyruklara gönderme.
- RPC over RabbitMQ: Request/Response yapısını kuyruk üzerinden simüle etme. (`ReplyTo` ve `CorrelationId` propertyleri ile).
- Dead Letter Exchange (DLX): İşlenemeyen mesajların yönlendirildiği hata kuyruğu.
  ```csharp
  var args = new Dictionary<string, object> { { "x-dead-letter-exchange", "dlx_exchange" } };
  channel.QueueDeclare("main_queue", arguments: args);
  ```

---

## **4️⃣ Message Delivery Semantics**

- At-most-once: Mesaj kaybolabilir, asla çift gitmez (AutoAck = true).
- At-least-once: Mesaj kaybolmaz, çift gidebilir (AutoAck = false, manuel Ack).
- Acknowledgements (ACK / NACK):
  ```csharp
  channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
  ```
- Durable queues & persistent messages: Sunucu restart olsa bile veriyi koruma.
  ```csharp
  // Durable Queue ve Persistent Message (prop.Persistent = true)
  channel.QueueDeclare("task_queue", durable: true, ...);
  ```

---

## **5️⃣ Queue & Exchange Management**

- Queue durability: RabbitMQ restart olduğunda kuyruğun silinip silinmeyeceği.
- Auto-delete queue: Son consumer ayrıldığında kuyruğun otomatik silinmesi.
- Exclusive queue: Sadece oluşturan bağlantı (Connection) tarafından kullanılan özel kuyruk.
- TTL & message expiration: Mesajın ömrü.
  ```csharp
  // 60 saniye ömürlü mesaj
  props.Expiration = "60000";
  ```
- Max-length / max-priority: Kuyruk boyutu ve öncelik sınırları.

---

## **6️⃣ Concurrency & Scaling**

- Prefetch count: Consumer'ın aynı anda işleyebileceği maksimum mesaj sayısı (Yük dengeleme).
  ```csharp
  channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false); // Tek tek al
  ```
- Multiple consumers per queue: Aynı kuyruğu dinleyen birden fazla işçi ile paralel işleme (Competing Consumers).
- Consumer acknowledgment: İşlem bitince ACK göndererek kuyruktan düşme.

---

## **7️⃣ Reliability & Fault Tolerance**

- Mirrored / quorum queues: Kuyruğun kopyalarının farklı node'larda tutulması (HA).
- High availability cluster: Birden fazla RabbitMQ sunucusu ile kesintisiz hizmet.
- Publisher confirms: Mesajın broker'a ulaştığının teyidi.
  ```csharp
  channel.ConfirmSelect(); // Confirm modunu aç
  channel.WaitForConfirmsOrDie();
  ```

---

## **8️⃣ Performance Tuning**

- Connection & channel management: Connection pahalıdır (TCP handshake), Channel ucuzdur. Connection tek (Singleton), Channel çoklu kullanılmalı.
- Batch publish: Mesajları toplu gönderme.
- Persistent vs transient messages trade-off: Diske yazma maliyeti vs Hız.

---

## **9️⃣ RabbitMQ & .NET Integration**

- RabbitMQ.Client usage: Resmi düşük seviye driver.
- Connection / channel lifecycle: Connection ömürlük, Channel işlem bazlı (ama tekrar kullan).
- Async consumer: `EventingBasicConsumer` veya `AsyncEventingBasicConsumer` kullanımı.
- Retry & Dead-letter handling: Polly ile retry mekanizması kurup, başarısız olanı DLX'e atma.

---

# Kafka

## **1️⃣ Kafka Fundamentals**

- Kafka nedir: Dağıtık streaming platformu (Log tabanlı).
- Kafka vs RabbitMQ: Kafka mesajı saklar (Retention), RabbitMQ siler. Kafka Pull, RabbitMQ Push.
- Kafka ne zaman tercih edilir: Büyük veri (Big Data), Log toplama, Event Sourcing, Stream Processing.
- Streaming platform kavramı: Veriyi sürekli akan bir nehir gibi işleme.

---

## **2️⃣ Kafka Architecture**

- Broker: Kafka sunucusu.
- Zookeeper / KRaft: Küme durumunu yöneten koordinatör.
- Topic vs Queue: Topic log dosyasıdır (silinmez), Queue geçicidir.
- Partitioning mantığı: Topic'i parçalara bölme (Paralellik birimi).
- Replication factor: Verinin kopyalanma sayısı (Yedeklilik).
- Leader / Follower replica: Okuma/Yazma Leader'dan yapılır, Follower yedektir.

---

## **3️⃣ Core Concepts**

- Producer: Mesajı anahtarlı/anahtarsız gönderen.
- Consumer: Mesajı offset ile okuyan.
- Consumer Group: Ölçeklenme birimi. Bir grupta her partition sadece 1 consumer tarafından okunur.
- Offset kavramı: Consumer'ın kitap ayracı. Nerede kaldığını bilir.
- Log retention policy: Mesaj saklama süresi (Varsayılan 7 gün).

---

## **4️⃣ Data Modeling & Partitions**

- Key-based partitioning: Aynı Key'e sahip mesajlar aynı partition'a gider (Sıralama garantisi).
  ```csharp
  // Message Key = "Order-123" -> Hep Partition 0'a gider
  producer.ProduceAsync("orders", new Message<string, string> { Key = "123", Value = "..." });
  ```
- Ordering guarantee: Kafka SADECE partition bazında sıra garantisi verir, topic genelinde vermez.
- Partition sayısı: Tüketim hızını belirler (Partition sayısı = Maksimum paralel consumer sayısı).

---

## **5️⃣ Writing Data (Producer)**

- acks=0, 1, all:
  - `0`: Gönder ve unut (En hızlı, kayıp riski).
  - `1`: Leader kaydetti (Orta).
  - `all`: Tüm replikalar kaydetti (En güvenli, yavaş).
  ```csharp
  var config = new ProducerConfig { Acks = Acks.All };
  ```
- Compression (gzip, snappy): Veriyi sıkıştırarak gönderme (Network tasarrufu).

---

## **6️⃣ Reading Data (Consumer)**

- Polling mechanism: Consumer actively veriyi çeker (Pull).
  ```csharp
  while (true) {
      var cr = consumer.Consume(cts.Token);
      Process(cr.Message.Value);
  }
  ```
- Consumer rebalancing: Gruba üye girip çıktığında partitionların yeniden dağıtılması.
- Auto-commit vs Manual commit:
  - `EnableAutoCommit = true`: Kolay ama riskli (İşlenmeden kaybolabilir).
  - `Manual Commit`: İşledikten sonra `Commit()` çağırma (Güvenli).
  ```csharp
  consumer.Commit(consumeResult);
  ```

---

## **7️⃣ Kafka Ecosystem**

- Kafka Connect: DB -> Kafka (Source) ve Kafka -> Elastic (Sink) gibi entegrasyonlar.
- Kafka Streams: Java kütüphanesi ile stream işleme (`map`, `filter`, `join`).
- KSQL (ksqlDB): Kafka üzerinde SQL sorgusu çalıştırma.
  ```sql
  SELECT userId, COUNT(*) FROM clicks WINDOW TUMBLING (SIZE 5 MINUTES) GROUP BY userId;
  ```
- Schema Registry: Mesaj formatını (Avro, Protobuf) doğrulama ve versiyonlama.

---

## **8️⃣ Kafka & .NET**

- Confluent.Kafka kütüphanesi: Resmi .NET istemcisi.
- Producer/Consumer implementasyonu: `ProducerBuilder` ve `ConsumerBuilder`.
- Serialization (JSON, Avro): Veriyi byte dizisine çevirme (`ISerializer`).
- Background Service kullanımı: `IHostedService` içinde sonsuz döngüde `Consume()`.

---------------------------------

	•	GC tuning
	•	Async/await iç mekanizması
	•	ThreadPool heuristics
	•	Memory model / lock-free yapı
	•	Span / memory yönetimi
	•	Kestrel pipeline
	•	Distributed consistency