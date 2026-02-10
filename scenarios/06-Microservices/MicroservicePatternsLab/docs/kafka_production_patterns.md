# Kafka Production Patterns (Senaryo Notu)

Bu lab'in aktif event akisi RabbitMQ uzerinden ilerliyor.
Asagidaki Kafka maddeleri, ayni senaryonun Kafka versiyonunda uygulanmasi gereken production ayarlaridir.

## 1. Consumer Backpressure
- `max.poll.interval.ms`: Uzun islemde consumer group'tan dusmeyi engeller.
- Poll dongusu + bounded processing queue.
- Lag arttiginda consumer sayisi/partition dengesi yeniden ayarlanir.

## 2. Retry + DLQ Topic
- Islenemeyen mesajlar dogrudan kaybolmaz:
  - `orders.retry.1`
  - `orders.retry.2`
  - `orders.dlq`

## 3. Idempotent Producer
- Producer'da `enable.idempotence=true` kullanilir.
- Gerekiyorsa transaction API ile exactly-once yaklasimi.

## 4. Commit Stratejisi
- Mesaj basariyla islendikten sonra offset commit.
- Erken commit duplicate riskini azaltmaz, veri kaybi riski dogurur.

## 5. Operasyonel Metrikler
- Consumer lag (partition bazli)
- Retry topic doluluk
- DLQ mesaj hizi
- Rebalance sikligi
