# Backend Systems Lab 🧪

Bu repo, dağıtık sistemlerde karşılaşılan zorlu senaryoları (Distributed Systems Challenges) simüle etmek, gözlemlemek ve çözüm üretmek için tasarlanmış modüler bir laboratuvar ortamıdır.

Her bir laboratuvar (`Lab`), belirli bir konsepte odaklanır ve kendi içinde izole test senaryoları, kodları ve dokümantasyonu barındırır.

## 📂 Laboratuvarlar (Labs)

| Lab | Klasör | Port | Odak Noktası |
|---|---|---|---|
| **data-consistency** | `src/StorageLab` | **8082** | Deadlock, Slow Queries, Database Locking, Isolation Levels |
| **observability** | `src/ObservabilityLab` | **8083** | Distributed Tracing, Trace Propagation, Vendor Lock-in Simülasyonu |
| **resilience** | `src/ResilienceLab` | **8084** | Retry Pattern, Circuit Breaker, DB Connection Failures, Idempotency |
| **network** | `src/NetworkLab` | **8085** | Connection Pooling, SNAT Exhaustion, HTTP/1.1 vs HTTP/2 |
| **microservice** | `src/MicroserviceLab` | - | Asenkron iletişim, Gateway pattern, Message Queues (RabbitMQ) |

## 🚀 Kurulum ve Çalıştırma

Tüm laboratuvar ortamını tek komutla ayağa kaldırabilirsiniz:

```bash
docker-compose up -d --build
```

## 🧪 Nasıl Test Edilir?

Her laboratuvarın içinde `README.md` dosyasında detaylı senaryolar bulunmaktadır. Kısaca:

### 1. Storage Lab (Veri Tutarlılığı)
*   **Deadlock Testi:** `curl http://localhost:8082/experiments/storage/deadlock/bad`
*   **Slow Query:** `curl http://localhost:8082/experiments/storage/slow-query/bad`
*   **Çıktılar:** `scenarios/09-Storage/results/StorageLab/` klasörüne yazılır.

### 2. Resilience Lab (Dayanıklılık)
*   **Akıllı Retry (Polly):** `curl http://localhost:8084/experiments/resilience/retry/smart`
*   **DB Bağlantı Kopması:** `curl "http://localhost:8084/experiments/resilience/db/connect?useRetry=true&host=invalid"`
*   **Çıktılar:** `scenarios/04-Resilience/results/ResilienceLab/` klasörüne yazılır.

### 3. Observability Lab (Gözlemlenebilirlik)
*   **Zincirleme İstek:** `curl http://localhost:8083/experiments/microservice/chain`
*   **Jaeger UI:** [http://localhost:16686](http://localhost:16686) adresinden trace'leri izleyin.

### 5. Microservice Lab (Asenkron & Zincir)
*   **Gateway Port:** **8086** (Yeni!)
*   **Zincirleme İstek (HTTP Chain):** `curl http://localhost:8086/experiments/microservice/chain`
    *   *Akış:* Gateway -> Storage Order API (Health).
*   **RabbitMQ Testi:** `curl "http://localhost:8086/experiments/microservice/queue/publish?message=HelloRabbit"`
*   **Kafka Testi:** `curl "http://localhost:8086/experiments/microservice/kafka/produce?message=HelloKafka"`
    *   *Akış:* Gateway -> Kafka (Topic) -> Worker (Consumer).
*   **Çıktılar:** `scenarios/08-Observability/results/ObservabilityLab/` (Gateway), `scenarios/01-Threading/CpuBound/results/` ve `scenarios/01-Threading/LatencyBaseline/results/` altında.

## 📁 Proje Yapısı

```
.
├── src/
│   ├── StorageLab/         # Database deneyleri
│   ├── ResilienceLab/      # Retry, Circuit Breaker deneyleri
│   ├── NetworkLab/         # Ağ katmanı deneyleri
│   ├── ObservabilityLab/   # Trace ve Log deneyleri
│   ├── MicroserviceLab/    # Mimarisi ve dokümanları
│   └── BackendLab.Api/     # (Legacy) Temel deneyler
├── outbound-request-limit-check/ # Özel araç (Root'ta tutuldu)
├── docker-compose.yml      # Tüm servislerin orkestrasyonu
└── *_roadmap.md            # Her onunun detaylı yol haritası
```

## 📊 Sonuçlar

Her deneyin sonucu, ilgili lab klasörünün altındaki `results/` dizininde JSON formatında loglanır. Bu sayede test çıktılarını izole bir şekilde analiz edebilirsiniz.
