# Backend Systems Lab - Deney Yol Haritası 🚀

Bu yol haritası, "Senior" seviyesinde sistem tasarımı ve problem çözme yetkinliğine ulaşman için hazırlanmıştır. Her deney bir prensibi öğretir.

## Bölüm 1: Temel Metrikler ve Darboğazlar (Basics)
- [ ] **Deney #1: Yapay Gecikme (Latency)**
  - *Kavram:* Latency vs Throughput, I/O Wait.
  - *Amaç:* Sistemin tek bir yavaş dış servise (DB, API) bağımlı olduğunda nasıl davrandığını görmek.
- [/] **Deney #2: CPU Darboğazı (CPU Bound)**
  - *Kavram:* Event Loop Blocking (Node.js/JS), Thread Starvation (.NET).
  - *Amaç:* I/O beklemesi yerine CPU yakan bir işlem olduğunda sistemin çöküşünü izlemek.
- [/] **Deney #3: Outgoing Limits & Concurrency**
  - **3.1 Handler Concurrency:** `MaxConnectionsPerServer` 10 vs 1000 farkı.
  - **3.2 Rate Limiter:** Uygulama içi (In-app) Outbound Rate Limiting (Token Bucket).
  - **3.3 Socket Exhaustion:** `new HttpClient()` antipattern ve Ephemeral port tükenmesi.
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
