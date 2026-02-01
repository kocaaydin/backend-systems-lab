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

