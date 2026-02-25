### 1) System Design + Trade-off
- Gereksinim netleştirme (trafik, SLA, tutarlılık, güvenlik)
- Bileşenleri ayırma (API, domain, data, cache, queue)
- Trade-off konuşma (latency vs consistency, cost vs reliability)
- Failure mode ve rollback düşüncesi

### 2) Database (Index, Transaction, Isolation, Query Plan)
- Index seçimi ve over-indexing riski
- Transaction scope ve lock etkisi
- Isolation level trade-off
- Execution plan okuma refleksi

### 3) Concurrency / Threading + Async
- ThreadPool davranışı, starvation sinyalleri
- async/await doğru kullanım, blocking etkisi
- Deadlock/race condition farkı
- Senkronizasyon primitive'leri (lock/SemaphoreSlim)

### 4) Network / HTTP (Latency, Timeout, Retry, Keep-Alive)
- TCP/TLS handshake maliyeti
- Keep-Alive ve connection reuse
- Timeout budget ve retry etkisi
- P95/P99 latency yorumlama

### 5) Resilience (Circuit Breaker, Idempotency, Backpressure)
- Retry + exponential backoff
- Circuit breaker ile fail-fast
- Idempotency ile güvenli tekrar deneme
- Backpressure ile sistem koruma

### 6) Messaging (Queue Semantics, DLQ, Retry)
- At-least-once / exactly-once farkı
- DLQ ve poison message izolasyonu
- Retry politikası ve retry storm riski
- Consumer lag/backpressure yorumu

### 7) Observability (RED/USE, Log-Metric-Trace Korelasyonu)
- RED: rate/errors/duration
- USE: utilization/saturation/errors
- Structured log + correlation ID
- Trace/metric/log ile kök neden doğrulama

### 8) Security Basics (AuthN/AuthZ, OWASP, Secrets, TLS)
- AuthN vs AuthZ ayrımı
- JWT/OAuth2 temel akış
- Secrets management + least privilege
- TLS, OWASP Top 10 farkındalığı

## Faz 2 - Destekleyici Konular (Faz 1 sonrası)

### Tooling
- Git (rebase, stash, branching strategy)
- CI/CD basics
- Docker fundamentals
- Debugging & profiling

### Connection Management (Derinleşme)
- Socket lifecycle, ephemeral ports, TIME_WAIT
- Port exhaustion
- HttpClient reuse ve DNS caching
- Connection pooling behavior

### gRPC
- HTTP/2 multiplexing
- Protobuf serialization
- Unary vs streaming
- Connection reuse behavior

### Caching
- In-memory caching
- Redis fundamentals
- Cache invalidation
- TTL strategies, cache stampede

### Search
- Elasticsearch basics
- Index mapping
- Query DSL
- Full-text search vs filter

### Platform & Runtime
- JIT basics
- GC basics
- Memory allocation
- CPU saturation
- Thread starvation

### Storage
- File storage strategies
- Object storage basics
- CDN concepts