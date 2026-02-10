# Microservice Roadmap

## 🎯 Amaç
Mikroservislerde sadece endpoint degil, **pattern secimlerinin** sistem davranisini nasil degistirdigini gostermek.

## 🧩 Kavramlar (Nedir?)
- `API Gateway`: Client'in tum backend'e tek kapidan erismesi.
- `Database per Service`: Her servisin kendi veritabani var, paylasilan tablo yok.
- `Idempotency`: Ayni istek tekrar geldiginde ayni sonuc donup duplicate is olusmamasini saglar.
- `Outbox`: Islem transaction'i icinde event'i DB'ye yazar, sonra broker'a publish eder.
- `Consumer`: Event dinleyip kendi bounded context'inde isleyen servis.
- `Saga/Orchestration`: Dagitik islemlerde adim adim telafi veya surec yonetimi.

## 🧪 Senaryo Seti (MicroservicePatternsLab)

### 1. API Gateway + Sync HTTP Chain
- **Case A (Bad):** Client her servisi dogrudan cagiriyor.
- **Case B (Good):** Tum trafik gateway uzerinden geciyor.
- **Fark:** Guvenlik/policy/limit ve gozlem tek noktadan yonetilir.

### 2. Database Per Service
- **Case A (Bad):** Servisler ayni DB tablosunu paylasiyor.
- **Case B (Good):** Order/Payment/Inventory ayri DB kullaniyor.
- **Fark:** Bagimsiz deploy ve hata izolasyonu artar.

### 3. Idempotency Key
- **Case A (Bad):** Retry oldugunda duplicate order.
- **Case B (Good):** `Idempotency-Key` ile dedup.
- **Fark:** Ag timeout/retry durumlarinda veri tutarliligi korunur.

### 4. Outbox Pattern
- **Case A (Bad):** DB commit oldu ama event publish olmadi.
- **Case B (Good):** Event outbox tablosuna transaction icinde yazilir.
- **Fark:** En kritik kayip penceresi daralir.

### 5. Async Eventing (RabbitMQ)
- **Case A (Bad):** Sync zincir uzuyor ve bagimlilik artiyor.
- **Case B (Good):** `order.created` event'i ile gevsek bagli akis.
- **Fark:** Servisler kendi hizinda ilerler, backpressure daha yonetilebilir olur.

### 6. Consumer Pattern (Payment/Inventory)
- **Case A (Bad):** Tek consumer butun isi yapiyor.
- **Case B (Good):** Her bounded-context kendi queue/tuketici akisini yonetiyor.
- **Fark:** Is kurallari ayrisir, domain karisikligi azalir.

## 🛠️ Çalıştırma
```bash
cd scenarios/06-Microservices/MicroservicePatternsLab
./scripts/run.sh
```

## Dosya Referanslari
- Compose: `scenarios/06-Microservices/MicroservicePatternsLab/docker-compose.yml`
- Gateway: `scenarios/06-Microservices/MicroservicePatternsLab/services/GatewayApi/Program.cs`
- Order (Idempotency + Outbox): `scenarios/06-Microservices/MicroservicePatternsLab/services/OrderApi/Program.cs`
- Payment Consumer: `scenarios/06-Microservices/MicroservicePatternsLab/services/PaymentApi/Program.cs`
- Inventory Consumer: `scenarios/06-Microservices/MicroservicePatternsLab/services/InventoryApi/Program.cs`
- k6: `scenarios/06-Microservices/MicroservicePatternsLab/k6/MicroservicePatternsLab/create-orders.js`
- Kafka production notu: `scenarios/06-Microservices/MicroservicePatternsLab/docs/kafka_production_patterns.md`

## 📊 Ölçülecek Metrikler
- Order create p95/p99
- Duplicate order oranı (idempotency test)
- Outbox publish gecikmesi
- Payment consume gecikmesi
- Inventory stock reserve başarım oranı
