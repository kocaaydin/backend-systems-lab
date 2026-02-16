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




Şu şekilde olursa değerli olur:
	•	Aynı endpoint
	•	Keep-Alive açık vs kapalı
	•	1000 istek
	•	P50 / P95 / P99
	•	Açılan TCP connection sayısı
	•	Handshake sayısı (Wireshark ekran görüntüsü)

Ve şu soruya cevap verirsen:

