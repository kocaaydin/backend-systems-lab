# Deney 15: Bağlantı Yönetimi (Connection Management) ve Protokol Analizi

## Hedef
Bu laboratuvar, **HTTP/1.1 vs HTTP/2 vs gRPC** arasındaki farkları salt teorik değil, **"Network Level"** analizle anlamayı hedefler. Sadece "Hangisi daha hızlı?" değil, **"Neden daha hızlı/yavaş?"** sorusuna Wireshark ve düşük seviyeli metriklerle cevap arayacağız.

---

## 🔬 Deney Planı

### Aşama 1: Ortam İzolasyonu (Variable Control)
Sonuçların tutarlı olması için **"Noisy Neighbor"** etkisini minimuma indireceğiz.
- **Container Limits:** `cpus: "0.5"`, `memory: "256m"`. (Saturasyon erken başlasın diye).
- **Runtime Config:** `MinThreads=50` (Thread ramp-up etkisini silmek için).
- **TLS:** **Kapalı (Plaintext)**. (Şifreleme maliyetini denklemden çıkarmak için).
- **Network:** Docker bridge network.

---

### Aşama 2: Senaryolar

#### Senaryo A: Head-of-Line (HOL) Blocking & Multiplexing
*Aynı bağlantı üzerinden, yanıt süresi farklı isteklerin yönetimi.*
- **Kurgu:** İstemci 6 paralel istek atar. İlk istek 2sn sürer, diğerleri 10ms.
- **HTTP/1.1 (Browser Mode):** Tarayıcı limiti (6 connections) dolarsa diğerleri bekler.
- **HTTP/1.1 (Single Conn):** 1. istek bitmeden 2. gitmez (Serial).
- **HTTP/2:** Multiplexing sayesinde 2sn süren istek diğerlerini bloklamaz.

#### Senaryo B: Connection & Resource Overhead
*Yüksek bağlantı sayısının sunucuya etkisi.*
- **Kurgu:** 1000 VU, kısa keep-alive süresi.
- **Ölçüm:** `Active Connections` vs `Memory Usage`.
- **Hipotez:** HTTP/1.1 "Connection Storm" yaratırken, HTTP/2 stabil kalacak.

#### Senaryo C: Payload & Serialization (gRPC vs JSON)
*Büyük veri transferinde binary vs text farkı.*
- **Kurgu:** 50KB'lık kompleks nesne listesi.
- **Ölçüm:** `Network Throughput (MB/s)` vs `CPU Usage`.
- **Hipotez:** gRPC Protobuf, JSON parse maliyetinden %X daha az CPU tüketecek.

---

## 🛠️ Kurulum

### Server (.NET 8/9 Minimal API + gRPC)
Tek proje üzerinde 3 port dinleyecek:
- **Port 5001:** HTTP/1.1 Only
- **Port 5002:** HTTP/2 Only (H2C)
- **Port 5003:** gRPC

### Client (k6 & Wireshark)
- **k6:** Yük testi ve latency ölçümü.
- **Wireshark:** Paket analizi (Handshake, Frame yapısı, PSH flagleri).

---

## 📊 Analiz Rehberi (Wireshark Odaklı)

### 1. HTTP/1.1 Analizi
- **TCP Handshake:** 3-way handshake (SYN, SYN-ACK, ACK).
- **Keep-Alive:** Aynı TCP stream üzerinden kaç request gitti? `tcp.stream` filtresi ile izle.
- **Pipeline:** Pipelining kapalıyken (varsayılan) response gelmeden yeni request gitmediğini gör.

### 2. HTTP/2 Analizi
- **Upgrade/Preface:** `PRI * HTTP/2.0` magic string'ini yakala.
- **Frames:** `SETTINGS`, `HEADERS`, `DATA` frame'lerini incele.
- **Stream ID:** Aynı bağlantıda farklı Stream ID'leri (`Stream: 1`, `Stream: 3`...) gör.

### 3. gRPC Analizi
- **Content-Type:** `application/grpc` header'ını teyit et.
- **Protobuf Payload:** Body kısmının binary (okunamaz) olduğunu gör.
- **Trailers:** Response sonunda gelen `grpc-status` trailer frame'ini yakala.



# Deney 2: Bağlantı Yönetimi (Connection Management) ve Protokol Analizi

## Hedef
Bu laboratuvar, **HTTP/1.1 vs HTTP/2 vs gRPC** arasındaki farkları salt teorik değil, **"Network Level"** analizle anlamayı hedefler. Sadece "Hangisi daha hızlı?" değil, **"Neden daha hızlı/yavaş?"** sorusuna Wireshark ve düşük seviyeli metriklerle cevap arayacağız.

---

## 🔬 Deney Planı

### Aşama 1: Ortam İzolasyonu (Variable Control)
Sonuçların tutarlı olması için **"Noisy Neighbor"** etkisini minimuma indireceğiz.
- **Container Limits:** `cpus: "0.5"`, `memory: "256m"`. (Saturasyon erken başlasın diye).
- **Runtime Config:** `MinThreads=50` (Thread ramp-up etkisini silmek için).
- **TLS:** **Kapalı (Plaintext)**. (Şifreleme maliyetini denklemden çıkarmak için).
- **Network:** Docker bridge network.

---

### Aşama 2: Senaryolar

#### Senaryo A: Head-of-Line (HOL) Blocking & Multiplexing
*Aynı bağlantı üzerinden, yanıt süresi farklı isteklerin yönetimi.*
- **Kurgu:** İstemci 6 paralel istek atar. İlk istek 2sn sürer, diğerleri 10ms.
- **HTTP/1.1 (Browser Mode):** Tarayıcı limiti (6 connections) dolarsa diğerleri bekler.
- **HTTP/1.1 (Single Conn):** 1. istek bitmeden 2. gitmez (Serial).
- **HTTP/2:** Multiplexing sayesinde 2sn süren istek diğerlerini bloklamaz.

#### Senaryo B: Connection & Resource Overhead
*Yüksek bağlantı sayısının sunucuya etkisi.*
- **Kurgu:** 1000 VU, kısa keep-alive süresi.
- **Ölçüm:** `Active Connections` vs `Memory Usage`.
- **Hipotez:** HTTP/1.1 "Connection Storm" yaratırken, HTTP/2 stabil kalacak.

#### Senaryo C: Payload & Serialization (gRPC vs JSON)
*Büyük veri transferinde binary vs text farkı.*
- **Kurgu:** 50KB'lık kompleks nesne listesi.
- **Ölçüm:** `Network Throughput (MB/s)` vs `CPU Usage`.
- **Hipotez:** gRPC Protobuf, JSON parse maliyetinden %X daha az CPU tüketecek.

---

## 🛠️ Kurulum

### Server (.NET 8/9 Minimal API + gRPC)
Tek proje üzerinde 3 port dinleyecek:
- **Port 5001:** HTTP/1.1 Only
- **Port 5002:** HTTP/2 Only (H2C)
- **Port 5003:** gRPC

### Client (k6 & Wireshark)
- **k6:** Yük testi ve latency ölçümü.
- **Wireshark:** Paket analizi (Handshake, Frame yapısı, PSH flagleri).

---

## 📊 Analiz Rehberi (Wireshark Odaklı)

### 1. HTTP/1.1 Analizi
- **TCP Handshake:** 3-way handshake (SYN, SYN-ACK, ACK).
- **Keep-Alive:** Aynı TCP stream üzerinden kaç request gitti? `tcp.stream` filtresi ile izle.
- **Pipeline:** Pipelining kapalıyken (varsayılan) response gelmeden yeni request gitmediğini gör.

### 2. HTTP/2 Analizi
- **Upgrade/Preface:** `PRI * HTTP/2.0` magic string'ini yakala.
- **Frames:** `SETTINGS`, `HEADERS`, `DATA` frame'lerini incele.
- **Stream ID:** Aynı bağlantıda farklı Stream ID'leri (`Stream: 1`, `Stream: 3`...) gör.

### 3. gRPC Analizi
- **Content-Type:** `application/grpc` header'ını teyit et.
- **Protobuf Payload:** Body kısmının binary (okunamaz) olduğunu gör.
- **Trailers:** Response sonunda gelen `grpc-status` trailer frame'ini yakala.

## Kısa Test Senaryosu

- Aynı endpoint test edildi: `/api/benchmark/fast`
- Keep-Alive kapalı (`Connection: close`) ve Keep-Alive açık senaryoları karşılaştırıldı.
- Her koşuda `1000` ölçüm isteği + `200` ısınma isteği atıldı.
- Eşzamanlılık seviyeleri: `VU=1` ve `VU=10`
- Ölçülen metrikler: `Ort`, `P50`, `P95`, `P99`, `RPS`, `Eklenen_TIME_WAIT`

## Kısa Sonuçlar

### VU = 1

```text
Senaryo       P50(ms)  P95(ms)  P99(ms)  RPS      TIME_WAIT(+)
KeepAliveOff  0.404    0.819    1.783    1065.85  1000
KeepAliveOn   0.284    0.569    1.290    2666.10  1
```

Handshake gecikme maliyeti
Off: tcp.port == 15001 and tcp.flags.syn == 1 and tcp.flags.ack == 1
On: tcp.port == 15002 and tcp.flags.syn == 1 and tcp.flags.ack == 1
Bakılacak alan: tcp.analysis.initial_rtt (SYN/SYN-ACK tarafında).

### VU = 10

```text
Senaryo       P50(ms)  P95(ms)  P99(ms)  RPS       TIME_WAIT(+)
KeepAliveOff  1.23     2.78     7.15     20.06     163
KeepAliveOn   0.64     0.91     1.35     13475.73  10
```

### Hızlı Okuma

- `KeepAliveOn`, her iki VU seviyesinde de daha düşük gecikme veriyor.
- `KeepAliveOn`, daha yüksek RPS üretiyor.
- `KeepAliveOff`, çok daha fazla `TIME_WAIT` oluşturuyor.

### Özet Yorum

- Keep-Alive açık senaryoda hem gecikme (P50/P95/P99) daha düşük hem de throughput (RPS) daha yüksek.
- Keep-Alive kapalı senaryoda TCP bağlantı kur/kapat maliyeti arttığı için `TIME_WAIT` ciddi yükseliyor.
- Wireshark tarafında beklenen gözlem: Keep-Alive kapalıda çok daha fazla SYN/handshake, Keep-Alive açıkta bağlantı tekrar kullanımı.


---------------------------------------------------------------------------------------------------------------------------------------------------

TCP 3-way handshake (SYN, SYN-ACK, ACK)
TCP kapanış (FIN, ACK, RST, TIME_WAIT)
HTTP/1.1 keep-alive mantığı (aynı TCP üstünden çoklu request)