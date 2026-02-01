# gRPC & HTTP/2 Internals Roadmap

Bu roadmap, gRPC ve HTTP/2 protokollerinin derinliklerine inerek performans, hata yönetimi ve bağlantı döngülerini inceleyen deneyleri kapsar.

## 1. Unary RPC Saturation
> “Bir servis saniyede 5.000 gRPC unary çağrı alıyor. Downstream servis yavaşladığında latency p95/p99 hızla yükseliyor. HTTP/2 bağlantısı açık kalıyor ama thread pool doluyor. Bu noktada backpressure nasıl oluşur? gRPC hangi katmanda tıkanır: socket mi, thread pool mu, uygulama mı?”

## 2. Streaming Backpressure
> “Bir gRPC server-streaming hattında producer sürekli veri gönderiyor, consumer yavaş. HTTP/2 flow-control devreye girdiğinde ne olur? Gönderici gerçekten durur mu, yoksa uygulama belleğinde mi birikir? Bu mekanizma kuyruk sistemlerindeki backpressure ile nasıl benzer, nasıl farklıdır?”

## 3. Retry Storm
> “Bir gRPC servisi geçici hatalar vermeye başlıyor. Client tarafında agresif retry politikası var. Aynı anda yüzlerce client retry’a giriyor. Bu durum downstream servisi nasıl ‘retry storm’ ile boğar? Circuit breaker olmadan sistem neden kendi kendini öldürür?”

## 4. Head-of-line Blocking (HTTP/2)
> “Aynı HTTP/2 bağlantısı üzerinde hem hızlı hem yavaş gRPC çağrıları gidiyor. Yavaş bir stream, aynı connection’daki hızlı unary çağrıları nasıl etkiler? Tek bağlantı üzerinden çok iş taşımak neden yapay gecikme yaratır?”

## 5. Connection Churn
> “Kubernetes ortamında gRPC client’ları sık sık restart oluyor. TCP + HTTP/2 bağlantıları sürekli kapanıp açılıyor. Bu handshake ve warm-up maliyeti throughput’u nasıl düşürür? ‘Connection pooling’ neden gRPC’de kritik hale gelir?”

## 6. gRPC vs Queue Boundary
> “Bir sistemde bazı akışlar doğrudan gRPC ile, bazıları Kafka üzerinden ilerliyor. Hangi işlerin gRPC’de kalması, hangilerinin kuyrukta olması gerekir? Dayanıklılık, replay ve bulk yük açısından gRPC neden kuyruğun yerini alamaz?”

## 7. Flow-Control vs Broker Backlog
> “gRPC’de flow-control üreticiyi yavaşlatır ama veri kaybolur. Kuyruk sisteminde ise veri broker’da birikir ve kalıcıdır. Aynı ‘yavaşlama’ davranışı bu iki dünyada neden tamamen farklı sonuçlar üretir?”