# Queueing & Asynchronous Messaging Roadmap

Bu roadmap, asenkron sistemlerde karşılaşılan temel zorlukları ve çözüm desenlerini inceleyen deneyleri kapsar.

## Senaryo İndeksi
- `scenarios/05-Messaging/README.md`
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
- Örnek: `scenarios/05-Messaging/Scenarios/01-backpressure.md`
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
