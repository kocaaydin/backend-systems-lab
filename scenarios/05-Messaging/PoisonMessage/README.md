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
