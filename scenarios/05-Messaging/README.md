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
