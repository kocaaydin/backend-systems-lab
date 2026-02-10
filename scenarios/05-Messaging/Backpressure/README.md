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
