# Backend Systems Lab - Deney Yol Haritası 🚀

Bu yol haritası, "Senior" seviyesinde sistem tasarımı ve problem çözme yetkinliğine ulaşman için hazırlanmıştır. Her deney bir prensibi öğretir.

## Bölüm 1: Temel Metrikler ve Darboğazlar (Basics)
- [ ] **Deney #1: Yapay Gecikme (Latency)**
  - *Kavram:* Latency vs Throughput, I/O Wait.
  - *Amaç:* Sistemin tek bir yavaş dış servise (DB, API) bağımlı olduğunda nasıl davrandığını görmek.
- [/] **Deney #2: CPU Darboğazı (CPU Bound)**
  - *Kavram:* Event Loop Blocking (Node.js/JS), Thread Starvation (.NET).
  - *Amaç:* I/O beklemesi yerine CPU yakan bir işlem olduğunda sistemin çöküşünü izlemek.
- [x] **Deney #2.1: Thread Starvation Senaryosu**
  - *Kavram:* ThreadPool üzerinde çalışan worker'ların, içeride Thread + Task.Run + .Wait() kombinasyonu ve SemaphoreSlim kısıtı nedeniyle, yine ThreadPool'dan çalışacak task'ları bekleyerek kendi kendini kilitlemesi (Thread Starvation).
  - *Amaç:* Aynı anda 50 worker tetiklendiğinde, ThreadPool thread'lerinin bloklanmasıyla task'ların ilerleyemediği, sistemin fiilen durduğu anı ayrıntılı log'larla gözle görünür hale getirmek.
  - ✅ **Uygulama:** Thread starvation deneyi senaryo klasörlerinde çalıştırılabilir hale getirildi
  - 📍 **Endpoint:** `POST /experiments/thread-starvation`
- [ ] **Deney #2.2: Thread Türleri ve Model Seçimi (ThreadPool vs Dedicated)**
  - *Kavram:* Main Thread, ThreadPool Thread, Dedicated Thread, GC/Finalizer Thread.
  - *Amaç:* CPU-heavy işlerde ThreadPool ve Dedicated yaklaşımının latency/starvation/maliyet farklarını görmek.
  - 📍 **Roadmap:** `scenarios/01-Threading/thread_roadmap.md`
- [/] **Deney #3: Outgoing Limits (Network) + Concurrency**
  - **Concurrency Odaklı (01-Threading):**
    - **3.1 Handler Concurrency:** `MaxConnectionsPerServer` 10 vs 1000 farkı.
    - **3.2 Rate Limiter:** Uygulama içi (In-app) Outbound Rate Limiting (Token Bucket).
  - **Network Odaklı (03-Network):**
    - **3.3 Socket Exhaustion:** `new HttpClient()` antipattern ve Ephemeral port tükenmesi *(Concurrency ilişkisi: kontrolsüz paralel istek açılış hızı port tüketimini dramatik artırır).* 
    - **3.4 OS Limits:** Container `ulimit -n` kısıtlaması ve "Too many open files" hataları.
    - **3.5 Proxy/Gateway Limits:** Araya Nginx koyarak reverse proxy darboğazı.

## Bölüm 2: Dayanıklılık Desenleri (Resiliency Patterns)
- [ ] **Deney #4: Retry Storm**
  - *Kavram:* Exponential Backoff, Jitter.
  - *Amaç:* Bir servis düzelmeye çalışırken, aptalca yapılan "tekrar dene" (retry) mekanizmasının onu nasıl tekrar öldürdüğünü görmek.
- [ ] **Deney #5: Circuit Breaker**
  - *Kavram:* Fail Fast.
  - *Amaç:* Hata alan servisi devre dışı bırakıp sistemin geri kalanını kurtarmak.
- [ ] **Deney #6: Bulkhead**
  - *Kavram:* Kaynak İzolasyonu.
  - *Amaç:* Bir modül (örn: Resim işleme) patladığında, alakasız modülün (örn: Login) çalışmaya devam etmesini sağlamak.

## Bölüm 3: Asenkron Mimariler (Async & Queues)
- [ ] **Deney #7: Backpressure (RabbitMQ)**
  - *Kavram:* Producer-Consumer Hız Farkı.
  - *Amaç:* Kuyruğa yazma hızı, okuma hızından fazla olunca ne olur? Sistem hafızası nasıl tükenir?
- [ ] **Deney #8: Ölçeklenme (Horizontal Scaling)**
  - *Kavram:* Competing Consumers.
  - *Amaç:* 1 Worker yetmeyince 5 Worker'a çıkmak sorunu çözer mi?

## Bölüm 4: Veri Tutarlılığı (Data Consistency)
- [ ] **Deney #9: Race Conditions**
  - *Kavram:* Locking, Optimistic Concurrency.
  - *Amaç:* Aynı anda iki kişi aynı koltuğu rezerve etmeye çalışırsa ne olur?
- [ ] **Deney #10: Distributed Tracing**
  - *Kavram:* Observability.
  - *Amaç:* Mikroservisler arasında kaybolan bir isteği bulmak.

## Bölüm 5: notes.md'den Eklenen Yeni Başlıklar
- `scenarios/10-Redis/redis_roadmap.md`
- `scenarios/11-Mongo/mongo_roadmap.md`
- `scenarios/12-Elasticsearch/elasticsearch_roadmap.md`
- `scenarios/13-Platform/docker_k8s_roadmap.md`
- `scenarios/14-Security/security_api_roadmap.md`

## Bölüm 6: k6 Temel Öğrenme Lab'i
- `scenarios/00-k6-Basics/README.md`
- Çalıştırma: `cd scenarios/00-k6-Basics && ./scripts/run.sh`
