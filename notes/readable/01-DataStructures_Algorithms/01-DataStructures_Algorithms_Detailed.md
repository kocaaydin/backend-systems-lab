
# [**Veri Yapıları**]

## **🧠 Temel Kavramlar**

- Big-O Notation : Algoritma performansının veri girdisine (n) göre büyüme hızı.
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

