# Microservice Patterns Lab

Bu lab, mikroservis tarafinda pattern kullanimlarini tek yerde gormek icin hazirlandi.
Amaç: "Sadece endpoint var" seviyesinden cikip, uretim benzeri kararlarin etkisini gormek.

## Dahil Pattern'ler
- API Gateway: Disaridan tek giris noktasi (`gateway-api`).
- Database per Service: Her servisin ayri PostgreSQL veritabani var.
- Idempotency Key: Ayni istegin tekrarinda duplicate order olusmasini engeller.
- Outbox Pattern: Order transaction'i icinde event outbox'a yazilir, arkaplanda publish edilir.
- Async Eventing: RabbitMQ topic exchange ile `order.created` eventi fanout edilir.
- Consumer Pattern: Payment ve Inventory kendi queue'larindan eventi tuketir.
- Basic Resilience: Consumer tarafinda reconnect/retry dongusu var.
- RabbitMQ production yaklasimi: `prefetch + manual ack/nack + DLQ`.

## Servisler
- `gateway-api` (8090): Order endpointlerini disariya acar.
- `order-api`: Siparis olusturur, idempotency + outbox uygular.
- `payment-api` (8092): `order.created` eventi ile odeme kaydi olusturur.
- `inventory-api` (8093): `order.created` eventi ile stok rezervasyonu yapar.
- `rabbitmq` (15682/5674): Event broker.
- `order-db`, `payment-db`, `inventory-db`: Servis-basi ayri DB.

## Senaryo Akisi
1. Client `gateway-api` uzerinden `POST /api/orders` cagirir.
2. `order-api` transaction acip:
   - order kaydi,
   - idempotency key kaydi,
   - outbox event kaydi olusturur.
3. `OutboxPublisher` arkaplanda event'i RabbitMQ exchange'e basar.
4. `payment-api` ve `inventory-api` kendi queue'larindan ayni eventi tuketir.
5. Her servis kendi DB'sine yazar (paylasilan DB yok).

## Calistirma
```bash
cd scenarios/06-Microservices/MicroservicePatternsLab
./scripts/run.sh
```

## Sonuclar
`scenarios/06-Microservices/MicroservicePatternsLab/results` altina yazilir:
- `order_create.json`
- `payments.json`
- `reservations.json`
- `stock.json`
- `create-orders-summary.json`
- `idempotency-summary.json`

## K6 Scriptleri
- `k6/MicroservicePatternsLab/create-orders.js`
- `k6/MicroservicePatternsLab/idempotency.js`

## RabbitMQ Production Yontemleri (Bu Lab'te Uygulanan)
- `prefetch=20`: Tek consumer'a sinirsiz mesaj akmasini engeller.
- `autoAck=false`: Mesaj sadece islenince ack edilir.
- `nack(requeue:false)`: Hata durumunda mesaj tekrar tekrar main queue'da donmez.
- `dead-letter-exchange`: Hata mesajlari `*.dlq` kuyruklarina ayrilir.

## Notlar (Gercek Uretim Icin)
- Outbox publish adiminda delivery guarantee guclendirilmeli (publisher confirms, DLQ).
- Circuit Breaker / Timeout policy HTTP zincirde merkezi hale getirilmeli.
- Trace context (OpenTelemetry) ve merkezi loglama eklenmeli.
- Secret/config yonetimi (`vault`, `external secret`) compose disina alinmali.
