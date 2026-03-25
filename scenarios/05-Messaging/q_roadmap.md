# Queueing & Asynchronous Messaging Roadmap

## Minimal Çerçeve
- Queue semantics: at-least-once vs exactly-once farkı.
- Poison message için DLQ şartı.
- Retry policy'nin retry storm'a dönme riski.
- Consumer lag/backpressure ile sistem doygunluğu.
- Metrik: queue depth, consumer lag, DLQ rate, processing latency.

Bu roadmap, asenkron sistemlerde karşılaşılan temel zorlukları ve çözüm desenlerini inceleyen deneyleri kapsar.

## Senaryo İndeksi
- QueueLab API (.NET): `scenarios/05-Messaging/QueueLabApi/Program.cs`
- Çalıştırma: `scenarios/05-Messaging/run_queue_lab_api.sh`

## Production Notları (RabbitMQ / Kafka)

- **RabbitMQ Backpressure:**
  - `BasicQos(prefetch)` ile consumer hizina gore mesaj alma limiti.
  - `autoAck=false` + `ack/nack` ile gercek isleme tamamlanmadan basari vermeme.
  - `DLQ` ile poison mesaj izolasyonu.

- **Kafka Backpressure:**
  - Consumer lag metrikleri ile horizontal scaling.
  - Poll/batch ayarlari ve commit stratejisi.
  - Retry topic + DLQ topic desenleri.

## 1. Backpressure
> “Bir sistemde üreticiler saniyede 50.000 iş üretirken tüketiciler en fazla 10.000 iş işleyebiliyor. Kuyruklar dolmaya başladıkça bellek ve disk kullanımı artıyor. Sistem üreticiyi durdurmazsa ne olur? Backpressure uygulanmazsa hangi noktada sistem çöker ve bunu mimari olarak nerede, nasıl uygulamak gerekir?”
- k6: `scenarios/05-Messaging/k6/QueueLab/backpressure.js`

## 2. Poison Message
> “Bir akışta her 100.000 mesajdan biri hatalı ve her işlendiğinde crash’e sebep oluyor. Sistem bu mesajı tekrar tekrar deniyor ve aynı noktada takılıyor. Diğer tüm işler arkada bekliyor. Dead-letter hattı yoksa sistem nasıl kilitlenir? Bu mesajı izole etmek neden tüm akışı kurtarır?”
- Örnek: `scenarios/05-Messaging/Scenarios/02-poison-message.md`
- k6: `scenarios/05-Messaging/k6/QueueLab/poison.js`

## 3. Rebalance Storm
> “Bir Kafka sisteminde consumer’lar autoscale ediliyor. Trafik arttıkça yeni consumer’lar ekleniyor, azaldıkça çıkarılıyor. Her değişimde rebalance tetikleniyor ve akış saniyelerce duruyor. Bu dalgalanma neden toplam throughput’u düşürür ve ‘daha çok worker = daha hızlı’ varsayımı neden burada çöker?”
- Örnek: `scenarios/05-Messaging/Scenarios/03-rebalance-storm.md`
- k6: `scenarios/05-Messaging/k6/QueueLab/rebalance.js`

## 4. Head-of-line Blocking
> “Aynı hatta hem 5 ms süren işler hem de 5 saniye süren işler var. Tüm işler tek sıraya giriyor. Yavaş işler öne denk geldiğinde hızlı işler de beklemek zorunda kalıyor. Bu durum sistemde nasıl yapay gecikme yaratır ve işi türüne göre ayırmak bunu nasıl ortadan kaldırır?”
- Örnek: `scenarios/05-Messaging/Scenarios/04-head-of-line-blocking.md`
- k6: `scenarios/05-Messaging/k6/QueueLab/hol.js`

## 5. Burst Traffic
> “Normalde saniyede 1.000 iş alan bir sistem, bir anda 1 dakika boyunca saniyede 100.000 iş alıyor. Ortalama yük düşük olmasına rağmen sistem bu patlamada çökmeye başlıyor. Neden ‘ortalama kapasite’ tasarımı gerçek hayatta yetersizdir ve mimari neden her zaman peak’e göre şekillenmelidir?”
- Örnek: `scenarios/05-Messaging/Scenarios/05-burst-traffic.md`
- k6: `scenarios/05-Messaging/k6/QueueLab/burst.js`

## 6. TCP Buffer Saturation
> “Bir worker bilinçsiz şekilde okumaya devam ediyor ama işleyemiyor. Broker’dan gelen mesajlar TCP üzerinden akıyor. Worker yavaşladıkça uygulama belleği, client buffer ve TCP buffer dolmaya başlıyor. Hangi katman önce patlar? Backpressure yoksa sistem neden broker’da değil, uygulama tarafında ölür?”
- Örnek: `scenarios/05-Messaging/Scenarios/06-tcp-buffer-saturation.md`
- k6: `scenarios/05-Messaging/k6/QueueLab/tcp_saturation.js`

## 7. Broker Backlog vs Socket Backlog
> “Worker poll() etmeyi yavaşlatıyor. Bu durumda mesajlar socket’te mi birikir, yoksa broker tarafında mı kalır? Kafka ve Rabbit’te bu davranış nasıl farklılaşır? Sağlıklı tasarımda yük neden TCP hattında değil, broker disk/RAM’inde birikmelidir?”
- Örnek: `scenarios/05-Messaging/Scenarios/07-broker-vs-socket-backlog.md`
- k6: `scenarios/05-Messaging/k6/QueueLab/backlog.js`

## 8. Consumer Slowdown Propagation
> “Bir consumer yavaşladığında zincirleme olarak upstream sistemler ne hisseder? TCP bağlantısı açık kalırken veri akışı nasıl durur? Üretici tarafında gecikme, timeout veya backpressure sinyali nasıl oluşur? Yavaşlık ağ seviyesinde nasıl yayılır?”
- Örnek: `scenarios/05-Messaging/Scenarios/08-consumer-slowdown-propagation.md`
- k6: `scenarios/05-Messaging/k6/QueueLab/backpressure.js`

## 9. Connection Churn (Kafka Rebalance)
> “Kafka’da rebalance sırasında consumer’lar TCP bağlantılarını kapatıp yeniden açar. Bu sırada socket’ler düşer, yeniden handshake yapılır. Bu kopup bağlanma döngüsü akışı neden saniyelerce durdurur? Sık rebalance, ağ seviyesinde nasıl bir ‘mikro kesinti fırtınası’ yaratır?”
- Örnek: `scenarios/05-Messaging/Scenarios/09-connection-churn.md`
- k6: `scenarios/05-Messaging/k6/QueueLab/churn.js`

## 10. Application Memory vs Network Boundary
> “Bir sistemde backpressure yokken worker mesajları hızla alır ama yavaş işler. Mesajlar broker’da kalmak yerine uygulama belleğine taşınır. Bu noktada artık ağ problemi değil, process içi bellek problemi oluşur. Neden doğru mimaride ‘yük broker’da kalmalı, uygulamaya girmemelidir’ kuralı hayati önemdedir?”
- Örnek: `scenarios/05-Messaging/Scenarios/10-memory-vs-network-boundary.md`
- k6: `scenarios/05-Messaging/k6/QueueLab/tcp_saturation.js`


# Backpressure Scenario

## Amaç
Producer hizli, consumer yavas oldugunda kuyruk birikimini ve bunun sistem davranisina etkisini gormek.

## Production'da Kullanilan Yontemler

### RabbitMQ
- `prefetch` (`BasicQos`): Consumer'a ayni anda kac mesaj verilecegini sinirlar.
- `manual ack/nack`: Mesaj islenmeden ack atilmaz; hata olursa `nack` ile DLQ'ya yonlendirilir.
- `DLQ`: Islenemeyen mesajlar ana kuyrugu tikamasin diye dead-letter queue'ya alinir.
- `queue length / TTL`: Sinirsiz backlog yerine kontrollu birikim uygulanir.

### Kafka
- Consumer tarafi batch/poll kontrolu (`max.poll.interval.ms`, poll suresi, batch buyuklugu).
- Consumer lag takibi (group lag) ile autoscaling/alert.
- Retry topic + DLQ topic modeli.
- Producer tarafinda `linger.ms`, `batch.size`, `compression` ile kontrollu throughput.

## Bu Repoda Nerede?
- RabbitMQ production yaklasimi kod ornegi:
  - `scenarios/06-Microservices/MicroservicePatternsLab/services/PaymentApi/Program.cs`
  - `scenarios/06-Microservices/MicroservicePatternsLab/services/InventoryApi/Program.cs`
- Basit backpressure simulasyonu:
  - `scenarios/05-Messaging/Backpressure/BackpressureSim/Program.cs`

## Calistirma
```bash
cd scenarios/05-Messaging/Backpressure
./run.sh
```


# Poison Message Scenario

## Amaç
Tek bir hatali mesajin tum akisi kilitlemesini engellemek.

## Production'da Kullanilan Yontemler

### RabbitMQ
- `manual ack/nack` kullanilir.
- Islenemeyen mesaj `nack(requeue:false)` ile DLQ'ya dusurulur.
- DLQ mesajlari sonradan analiz/replay icin ayri tutulur.
- Hata orani artarsa consumer pause/scale karari verilir.

### Kafka
- Hata veren mesajlar icin retry topic zinciri (`topic.retry.1`, `topic.retry.2`, ...).
- Son adimda DLQ topic'e yazim (`topic.dlq`).
- Consumer offset commit stratejisi (erken commit/no commit) bilincli secilir.

## Bu Repoda Nerede?
- RabbitMQ DLQ ornegi:
  - `scenarios/06-Microservices/MicroservicePatternsLab/services/PaymentApi/Program.cs`
  - `scenarios/06-Microservices/MicroservicePatternsLab/services/InventoryApi/Program.cs`



# 01 - Backpressure

- k6: `scenarios/05-Messaging/k6/QueueLab/backpressure.js`
- Amaç: Producer > Consumer olduğunda backlog ve gecikme artışını görmek.
- Prod yöntemleri: RabbitMQ `prefetch`, `manual ack`, Kafka lag bazlı autoscale.


# 02 - Poison Message

- k6: `scenarios/05-Messaging/k6/QueueLab/poison.js`
- Amaç: Hatalı mesajın akışı kilitlemesini göstermek.
- Prod yöntemleri: DLQ, retry topic zinciri, nack(requeue:false).

# 03 - Rebalance Storm

- k6: `scenarios/05-Messaging/k6/QueueLab/rebalance.js`
- Amaç: Consumer ekle/çıkar dalgalanmasının throughput etkisini gözlemek.
- Prod yöntemleri: Stabil consumer group, kontrollü autoscale, rebalance minimizasyonu.


# 04 - Head-of-line Blocking

- k6: `scenarios/05-Messaging/k6/QueueLab/hol.js`
- Amaç: Yavaş işlerin hızlı işleri bekletmesini gözlemek.
- Prod yöntemleri: Queue ayrıştırma, priority routing, worker partitioning.


# 05 - Burst Traffic

- k6: `scenarios/05-Messaging/k6/QueueLab/burst.js`
- Amaç: Ani trafik patlamasında kuyruğun ve gecikmenin davranışı.
- Prod yöntemleri: Rate limit, burst buffer, peak kapasiteye göre ölçekleme.

# 06 - TCP Buffer Saturation

- k6: `scenarios/05-Messaging/k6/QueueLab/tcp_saturation.js`
- Amaç: Consumer okuyup işleyemediğinde bellek/tcp buffer etkisini görmek.
- Prod yöntemleri: Pull hız kontrolü, bounded in-memory queue, backpressure sinyali.

# 07 - Broker Backlog vs Socket Backlog

- k6: `scenarios/05-Messaging/k6/QueueLab/backlog.js`
- Amaç: Birikimin broker tarafında mı uygulama/socket tarafında mı kaldığını ayırt etmek.
- Prod yöntemleri: Broker-centric backlog, consumer poll disiplini, prefetched mesaj limitleri.

# 08 - Consumer Slowdown Propagation

- k6: `scenarios/05-Messaging/k6/QueueLab/backpressure.js`
- Amaç: Consumer yavaşlığının upstream gecikme/hata sinyallerine etkisi.
- Prod yöntemleri: Circuit breaker, timeout, retry budget, lag alarmı.

# 09 - Connection Churn (Kafka Rebalance)

- k6: `scenarios/05-Messaging/k6/QueueLab/churn.js`
- Amaç: Sık reconnect/rebalance ile mikro kesinti etkisini görmek.
- Prod yöntemleri: Connection pooling, grup stabilizasyonu, dalga yerine kademeli ölçekleme.

# 10 - Application Memory vs Network Boundary

- k6: `scenarios/05-Messaging/k6/QueueLab/tcp_saturation.js`
- Amaç: Yük broker yerine uygulama belleğine taşınınca oluşan risk.
- Prod yöntemleri: Max in-flight limit, queue-depth guardrail, broker retention politikaları.



# Messaging Senaryo İndeksi

Bu klasörde roadmap'teki 10 başlığın script ve senaryo eşleşmeleri bulunur.

## Hızlı Harita
1. Backpressure -> `scenarios/05-Messaging/k6/QueueLab/backpressure.js`
2. Poison Message -> `scenarios/05-Messaging/k6/QueueLab/poison.js`
3. Rebalance Storm -> `scenarios/05-Messaging/k6/QueueLab/rebalance.js`
4. Head-of-line Blocking -> `scenarios/05-Messaging/k6/QueueLab/hol.js`
5. Burst Traffic -> `scenarios/05-Messaging/k6/QueueLab/burst.js`
6. TCP Buffer Saturation -> `scenarios/05-Messaging/k6/QueueLab/tcp_saturation.js`
7. Broker Backlog vs Socket Backlog -> `scenarios/05-Messaging/k6/QueueLab/backlog.js`
8. Consumer Slowdown Propagation -> `scenarios/05-Messaging/k6/QueueLab/backpressure.js`
9. Connection Churn (Kafka Rebalance) -> `scenarios/05-Messaging/k6/QueueLab/churn.js`
10. Application Memory vs Network Boundary -> `scenarios/05-Messaging/k6/QueueLab/tcp_saturation.js`

## Senaryo Dokümanları
- `scenarios/05-Messaging/Scenarios/01-backpressure.md`
- `scenarios/05-Messaging/Scenarios/02-poison-message.md`
- `scenarios/05-Messaging/Scenarios/03-rebalance-storm.md`
- `scenarios/05-Messaging/Scenarios/04-head-of-line-blocking.md`
- `scenarios/05-Messaging/Scenarios/05-burst-traffic.md`
- `scenarios/05-Messaging/Scenarios/06-tcp-buffer-saturation.md`
- `scenarios/05-Messaging/Scenarios/07-broker-vs-socket-backlog.md`
- `scenarios/05-Messaging/Scenarios/08-consumer-slowdown-propagation.md`
- `scenarios/05-Messaging/Scenarios/09-connection-churn.md`
- `scenarios/05-Messaging/Scenarios/10-memory-vs-network-boundary.md`

## Not
Mevcut k6 scriptleri `http://localhost:8090` üzerindeki QueueLab endpointlerini hedefler.
Bu endpointler için .NET API eklendi:
- Proje: `scenarios/05-Messaging/QueueLabApi/QueueLabApi.csproj`
- Startup: `scenarios/05-Messaging/QueueLabApi/Program.cs`
- Çalıştırma: `scenarios/05-Messaging/run_queue_lab_api.sh`

## Controller Kategorileri
- `scenarios/05-Messaging/QueueLabApi/Controllers/BackpressureController.cs`
- `scenarios/05-Messaging/QueueLabApi/Controllers/PoisonController.cs`
- `scenarios/05-Messaging/QueueLabApi/Controllers/RebalanceController.cs`
- `scenarios/05-Messaging/QueueLabApi/Controllers/HolController.cs`
- `scenarios/05-Messaging/QueueLabApi/Controllers/BurstController.cs`
- `scenarios/05-Messaging/QueueLabApi/Controllers/TcpLabController.cs`
- `scenarios/05-Messaging/QueueLabApi/Controllers/MetricsController.cs`
- `scenarios/05-Messaging/QueueLabApi/Controllers/HealthController.cs`

## Endpoint -> .NET Haritası
- `POST /api/backpressure/produce` -> `QueueLabApi/Controllers/BackpressureController.cs`
- `POST /api/poison/publish` -> `QueueLabApi/Controllers/PoisonController.cs`
- `POST /api/rebalance/produce` -> `QueueLabApi/Controllers/RebalanceController.cs`
- `POST /api/hol/job` -> `QueueLabApi/Controllers/HolController.cs`
- `GET /api/burst/work` -> `QueueLabApi/Controllers/BurstController.cs`
- `POST /api/tcplab/flood-saturation` -> `QueueLabApi/Controllers/TcpLabController.cs`
- `POST /api/tcplab/fill-kafka-slow` -> `QueueLabApi/Controllers/TcpLabController.cs`
- `POST /api/tcplab/fill-rabbit-pressure` -> `QueueLabApi/Controllers/TcpLabController.cs`
- `POST /api/tcplab/churn-load` -> `QueueLabApi/Controllers/TcpLabController.cs`
