# Docker

## **1️⃣ Docker Fundamentals**

- Docker nedir?
- Container vs Virtual Machine
- Docker hangi problemleri çözer?
- Docker ne zaman KULLANILMAZ?
- Image vs Container farkı

🎤:

> “Docker uygulamayı environment’tan bağımsız hale getirir.”
> 

---

## **2️⃣ Docker Architecture**

- Docker Engine
- Docker Daemon
- Docker Client
- Docker Registry (Docker Hub)
- Image layers mantığı

---

## **3️⃣ Docker Image & Dockerfile (🔥)**

- Dockerfile nedir?
- FROM / RUN / COPY / ADD
- CMD vs ENTRYPOINT
- EXPOSE
- ENV / ARG farkı
- .dockerignore

🎤:

> “Dockerfile ne kadar küçükse o kadar iyidir.”
> 

---

## **4️⃣ Image Optimization & Best Practices**

- Multi-stage build
- Küçük base image seçimi (alpine)
- Layer caching mantığı
- Gereksiz file kopyalamamak
- Build context küçültme

---

## **5️⃣ Container Lifecycle**

- create / start / stop / restart
- Container state’leri
- Graceful shutdown
- Restart policies

---

## **6️⃣ Networking**

- Bridge network
- Host network
- Overlay network (concept)
- Port mapping
- Container-to-container communication

🎤:

> “Container’lar default olarak isolated network’te çalışır.”
> 

---

## **7️⃣ Volumes & Persistence (ÇOK SORULUR)**

- Volume nedir?
- Bind mount vs volume
- Data persistence mantığı
- Stateless container yaklaşımı

📌:

> “Container stateless, data dışarıda.”
> 

---

## **8️⃣ Environment & Configuration**

- ENV variables
- Secrets yönetimi
- Config injection
- 12-factor app prensibi

---

## **9️⃣ Docker Compose**

- docker-compose.yml
- Multi-container setup
- Service dependency
- Network & volume tanımı
- Local development senaryoları

---

## **🔟 Security Best Practices**

- Root user ile çalışmamak
- Image scanning
- Minimal image kullanımı
- Secrets image içine koymamak

---

## **1️⃣1️⃣ Logging & Monitoring**

- stdout / stderr logging
- Log driver’lar
- Container healthcheck
- Resource usage (CPU / memory)

---

## **1️⃣2️⃣ Docker & CI/CD**

- Docker build pipeline
- Image tagging
- Push / pull registry
- Versioning strategy

---

## **1️⃣3️⃣ Docker vs Kubernetes (Concept)**

- Docker ne yapar?
- Kubernetes ne yapar?
- Ne zaman sadece Docker yeter?
- Ne zaman K8s gerekir?

---

## **1️⃣4️⃣ Production Anti-Patterns**

- Container içinde DB
- Large image’lar
- Hardcoded config
- State tutan container

# Kubernetes

## **1️⃣ Kubernetes Fundamentals**

- Kubernetes nedir?
- Kubernetes hangi problemi çözer?
- Docker vs Kubernetes farkı
- Kubernetes ne zaman GEREKLİ DEĞİL?
- Kubernetes cluster kavramı

🎤

> “Kubernetes container orchestration platformudur.”
> 

---

## **2️⃣ Kubernetes Architecture (ÇOK SORULUR)**

- Control Plane nedir?
- Node nedir?
- Master / Worker node farkı
- kube-apiserver
- etcd
- scheduler
- controller-manager

🎤

> “etcd cluster state’in tek kaynağıdır.”
> 

---

## **3️⃣ Core Objects (OLMAZSA OLMAZ)**

### **🔹 Pod**

- Pod nedir?
- Pod neden container’dan farklı?
- Pod lifecycle
- Multi-container pod senaryosu

### **🔹 Deployment**

- Deployment nedir?
- ReplicaSet ilişkisi
- Rolling update
- Rollback

### **🔹 Service**

- ClusterIP
- NodePort
- LoadBalancer
- Service discovery

🎤

> “Pod’lar ephemeral, Service’ler stable’dır.”
> 

---

## **4️⃣ Configuration Management**

- ConfigMap
- Secret
- Environment variable injection
- Volume ile config bağlama

📌

> “Config image’te değil, Kubernetes objesinde olmalı.”
> 

---

## **5️⃣ Networking (🔥)**

- Pod-to-pod communication
- Service-to-pod routing
- DNS (CoreDNS)
- Ingress nedir?
- Ingress Controller

🎤

> “Ingress dış dünyaya açılan kapıdır.”
> 

---

## **6️⃣ Storage & Persistence**

- Volume nedir?
- PersistentVolume (PV)
- PersistentVolumeClaim (PVC)
- StorageClass
- Stateful vs Stateless app

📌

> “Pod gider, volume kalır.”
> 

---

## **7️⃣ Scaling & Availability**

- Replica count
- Horizontal Pod Autoscaler (HPA)
- CPU / Memory based scaling
- Self-healing mantığı

🎤

> “Kubernetes failed pod’ları otomatik ayağa kaldırır.”
> 

---

## **8️⃣ Resource Management**

- CPU requests / limits
- Memory requests / limits
- OOMKilled nedir?
- Resource starvation

---

## **9️⃣ Health Checks (ÇOK SORULUR)**

- Liveness probe
- Readiness probe
- Startup probe
- Traffic yönetimi

🎤

> “Readiness false ise pod alive ama traffic almaz.”
> 

---

## **🔟 Deployment Strategies (Senior Konu)**

- Rolling update
- Recreate
- Blue-Green
- Canary deployment

---

## **1️⃣1️⃣ Security (Concept)**

- RBAC
- ServiceAccount
- Namespace izolasyonu
- Pod Security Context
- Network Policy

---

## **1️⃣2️⃣ Observability**

- Logs (kubectl logs)
- Metrics (Prometheus)
- Tracing
- Alerts

📌

> “Gözlemleyemediğin sistemi yönetemezsin.”
> 

---

## **1️⃣3️⃣ Kubernetes & CI/CD**

- Image tagging strategy
- Deployment automation
- Rollback senaryosu
- GitOps (concept)

---

## **1️⃣4️⃣ Production Anti-Patterns**

- Pod içinde state tutmak
- Hardcoded config
- Limits tanımlamamak
- Tek replica critical service

---

## **1️⃣5️⃣ Kubernetes Ne Zaman KULLANILMAZ?**

- Küçük projeler
- Tek servis
- Düşük trafik
- Ops ekibi yoksa

# RabbitMQ

## **1️⃣ RabbitMQ Fundamentals**

- RabbitMQ nedir?
- Message broker kavramı
- Queue vs Topic vs Exchange
- RabbitMQ ne zaman tercih edilir?
- RabbitMQ ne zaman KULLANILMAZ?

🎤

> “RabbitMQ uygulamalar arası asenkron iletişim sağlar.”
> 

---

## **2️⃣ Core Concepts**

- Producer
- Consumer
- Queue
- Exchange (Direct / Fanout / Topic / Headers)
- Binding
- Routing key
- Virtual host (vhost)

---

## **3️⃣ Messaging Patterns**

- Work Queue (Task Queue)
- Publish / Subscribe
- Routing / Topic Exchange
- RPC over RabbitMQ
- Dead Letter Exchange

🎤:

> “Dead Letter Queue, mesaj işlenemezse başka kuyrukta toplanır.”
> 

---

## **4️⃣ Message Delivery Semantics**

- At-most-once
- At-least-once
- Exactly-once (concept)
- Acknowledgements (ACK / NACK)
- Durable queues & persistent messages

---

## **5️⃣ Queue & Exchange Management**

- Queue durability
- Auto-delete queue
- Exclusive queue
- TTL & message expiration
- Max-length / max-priority

---

## **6️⃣ Concurrency & Scaling**

- Prefetch count
- Multiple consumers per queue
- Consumer acknowledgment
- Load balancing across consumers

🎤:

> “Prefetch sayısı tüketiciye düşen mesaj yükünü kontrol eder.”
> 

---

## **7️⃣ Reliability & Fault Tolerance**

- Mirrored / quorum queues
- High availability cluster
- Network partition handling
- Publisher confirms

---

## **8️⃣ Performance Tuning**

- Connection & channel management
- Batch publish
- Consumer concurrency
- Persistent vs transient messages trade-off

---

## **9️⃣ Monitoring & Observability**

- RabbitMQ Management UI
- Metrics (queue length, publish rate, consumer count)
- Alerts (unacked messages, queue growth)
- Logs

---

## **🔟 Security**

- Authentication & authorization
- User / vhost permissions
- TLS encryption
- Policy management

---

## **1️⃣1️⃣ RabbitMQ & .NET Integration**

- RabbitMQ.Client usage
- Connection / channel lifecycle
- Async consumer
- Retry & Dead-letter handling

---

## **1️⃣2️⃣ Anti-Patterns & Pitfalls**

- Queue overload → OOM
- Long-running consumer without ACK
- Single point of failure (standalone RabbitMQ)
- Persistent messages everywhere → disk IO bottleneck

---

## **1️⃣3️⃣ RabbitMQ Ne Zaman KULLANILMAZ?**

- Çok düşük latency gereken işlem
- Small-scale, simple CRUD
- Strong consistency gerektiren transaction-heavy işlem
- Stateful communication yeterli ise

# Kafka

## **1️⃣ RabbitMQ Fundamentals**

- RabbitMQ nedir?
- Message broker kavramı
- Queue vs Topic vs Exchange
- RabbitMQ ne zaman tercih edilir?
- RabbitMQ ne zaman KULLANILMAZ?

🎤

> “RabbitMQ uygulamalar arası asenkron iletişim sağlar.”
> 

---

## **2️⃣ Core Concepts**

- Producer
- Consumer
- Queue
- Exchange (Direct / Fanout / Topic / Headers)
- Binding
- Routing key
- Virtual host (vhost)

---

## **3️⃣ Messaging Patterns**

- Work Queue (Task Queue)
- Publish / Subscribe
- Routing / Topic Exchange
- RPC over RabbitMQ
- Dead Letter Exchange

🎤:

> “Dead Letter Queue, mesaj işlenemezse başka kuyrukta toplanır.”
> 

---

## **4️⃣ Message Delivery Semantics**

- At-most-once
- At-least-once
- Exactly-once (concept)
- Acknowledgements (ACK / NACK)
- Durable queues & persistent messages

---

## **5️⃣ Queue & Exchange Management**

- Queue durability
- Auto-delete queue
- Exclusive queue
- TTL & message expiration
- Max-length / max-priority

---

## **6️⃣ Concurrency & Scaling**

- Prefetch count
- Multiple consumers per queue
- Consumer acknowledgment
- Load balancing across consumers

🎤:

> “Prefetch sayısı tüketiciye düşen mesaj yükünü kontrol eder.”
> 

---

## **7️⃣ Reliability & Fault Tolerance**

- Mirrored / quorum queues
- High availability cluster
- Network partition handling
- Publisher confirms

---

## **8️⃣ Performance Tuning**

- Connection & channel management
- Batch publish
- Consumer concurrency
- Persistent vs transient messages trade-off

---

## **9️⃣ Monitoring & Observability**

- RabbitMQ Management UI
- Metrics (queue length, publish rate, consumer count)
- Alerts (unacked messages, queue growth)
- Logs

---

## **🔟 Security**

- Authentication & authorization
- User / vhost permissions
- TLS encryption
- Policy management

---

## **1️⃣1️⃣ RabbitMQ & .NET Integration**

- RabbitMQ.Client usage
- Connection / channel lifecycle
- Async consumer
- Retry & Dead-letter handling

---

## **1️⃣2️⃣ Anti-Patterns & Pitfalls**

- Queue overload → OOM
- Long-running consumer without ACK
- Single point of failure (standalone RabbitMQ)
- Persistent messages everywhere → disk IO bottleneck

---

## **1️⃣3️⃣ RabbitMQ Ne Zaman KULLANILMAZ?**

- Çok düşük latency gereken işlem
- Small-scale, simple CRUD
- Strong consistency gerektiren transaction-heavy işlem
- Stateful communication yeterli ise