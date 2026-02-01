# Microservice Observability Lab

## 🎯 Amaç
Bu laboratuvarın amacı, dağıtık sistemlerde **gözlemlenebilirlik (observability)** reflekslerini geliştirmek ve farklı iletişim desenlerinin (HTTP, RabbitMQ, Kafka) sistem davranışına etkilerini canlı olarak deneyimlemektir.

Hedef; araç öğrenmek değil, "Trace nerede koptu?", "Consumer yavaşlarsa ne olur?", "Collector çökerse veri kaybı yaşanır mı?" gibi sorulara yanıt verebilen bir mühendislik sezgisi kazandırmaktır.

## 🏗️ Mimari ve Senaryolar
Laboratuvar ortamı, en az üç servisli bir zincir akışı üzerine kuruludur:
`Gateway` → `Core Service` → `Downstream Service` (veya Queue)

### 1. Senaryo A: Senkron Zincir (HTTP)
*   **Akış:** Gateway -> (HTTP) -> Core -> (HTTP) -> Downstream
*   **Odak:** Distributed Tracing, Context Propagation, Latency analizi.
*   **Deneyler:**
    *   Trace Context'in bir servisten diğerine (Header ile) taşınması.
    *   Bir servis yavaşladığında zincirdeki diğer servislerin durumu.

### 2. Senaryo B: Asenkron Akış (RabbitMQ)
*   **Akış:** Gateway -> (Publish Message) -> RabbitMQ -> (Consumer) -> Core Service
*   **Odak:** Asenkron iletişimde trace takibi, Producer-Consumer hız farkları.
*   **Deneyler:**
    *   RabbitMQ down olduğunda veri kaybı.
    *   Consumer Lag analizi.

### 3. Senaryo C: Asenkron Akış (Kafka)
*   **Akış:** Gateway -> (Produce Event) -> Kafka -> (Consume) -> Core Service
*   **Odak:** Yüksek throughput, Log-based storage davranışı.
*   **Deneyler:**
    *   Kafka Broker down simülasyonu.
    *   Partitioning ve Ordering etkileri.

## 🧪 Kaos ve Failure Senaryoları
*   **Collector Down:** OpenTelemetry Collector devre dışı bırakıldığında uygulama performansı etkilenir mi? Veri tamponlanır mı?
*   **Network Partition:** Servisler arası iletişim koptuğunda trace bütünlüğü.
*   **Backpressure:** Downstream servis yavaşladığında kuyrukların (Queue) davranışı.

## ⚙️ Değişkenler
*   **Sampling Rate:** %1 vs %100 örnekleme oranının hata yakalamaya etkisi.
*   **Buffer Size:** OTel Exporter bellek limitleri.

## 📝 Çıktı ve Analiz
Sonuçlar, diğer lablarda olduğu gibi `results/microservice_lab_results.json` dosyasına kaydedilecektir.
