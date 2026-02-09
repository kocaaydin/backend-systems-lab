# Microservice Lab 🔗

Bu klasör, **Mikroservis Mimarisi** senaryolarının (Distributed Tracing, Async Communication) tam implementasyonunu içerir.

## 🛠️ Projeler

### MicroserviceLab.GatewayApi (Port: 8086)
Mikroservis zincirinin **giriş noktası**. Üç farklı iletişim desenini destekler:

#### 1. HTTP Chain (Senkron)
*   **Endpoint:** `GET /experiments/microservice/chain`
*   **Akış:** Gateway → Storage Order API (Health Check)
*   **Odak:** Distributed Tracing, Context Propagation

#### 2. RabbitMQ (Asenkron)
*   **Endpoint:** `POST /experiments/microservice/queue/publish?message=Hello`
*   **Akış:** Gateway → RabbitMQ → Worker (Consumer)
*   **Odak:** Async Messaging, Trace Context Injection

#### 3. Kafka (Event Streaming)
*   **Endpoint:** `POST /experiments/microservice/kafka/produce?message=Hello`
*   **Akış:** Gateway → Kafka Topic → Worker (Consumer)
*   **Odak:** High-throughput messaging, Partitioning

## 🧪 Test Senaryoları

```bash
# HTTP Chain
curl http://localhost:8086/experiments/microservice/chain

# RabbitMQ
curl "http://localhost:8086/experiments/microservice/queue/publish?message=HelloRabbit"

# Kafka
curl "http://localhost:8086/experiments/microservice/kafka/produce?message=HelloKafka"
```

## 📊 Monitoring Tools

### Conduktor Platform
*   **URL:** http://localhost:8091
*   **Kullanım:** Kafka Topics, Messages, Consumer Groups, Schema Registry, Cluster Health
*   **Özellikler:** Professional Kafka monitoring, RabbitMQ support (via connectors)

### RabbitMQ Management
*   **URL:** http://localhost:15672
*   **Credentials:** guest/guest
*   **Kullanım:** Queues, Exchanges, Connections, Message Rates

### Jaeger (Distributed Tracing)
*   **URL:** http://localhost:16686
*   **Kullanım:** Trace görselleştirme, Latency analizi

## 📊 Sonuçlar
*   **Gateway Logs:** `scenarios/06-Microservices/results/MicroserviceLab/`
*   **Worker Logs:** `scenarios/01-Threading/k6/BasicsLab/experiments/` (Consumer tarafı)

## 🗺️ Detaylı Roadmap
Kök dizindeki [microservice_roadmap.md](../../microservice_roadmap.md) dosyasına bakın.
