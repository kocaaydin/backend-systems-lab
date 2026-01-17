# Vendor-Lock & Advanced Observability Lab

## 🎯 Amaç
Bu lab çalışmasının temel amacı, modern mikroservis mimarilerinde **Gözlemlenebilirlik (Observability)** stratejilerini karşılaştırmak, **Vendor-Lock (Sağlayıcı Bağımlılığı)** risklerini uygulamalı olarak görmek ve **Gerçek Hayat Senaryoları (Real-World Problems)** ile başa çıkma reflekslerini geliştirmektir.

Amacımız, sadece "metrik toplamak" değil, toplanan verinin sistemin sağlığı hakkında nasıl kritik kararlar almamızı sağladığını deneyimlemektir. Özellikle, bir Observability sağlayıcısına (örneğin New Relic, Datadog) bağımlı kalmak ile açık standartlar (OpenTelemetry) kullanmanın farklarını canlı olarak test edeceğiz.

## 🏗️ Mimari ve Senaryolar
Deney ortamı, tipik bir e-ticaret akışını (`Gateway` → `Order Service` → `Payment Service`) simüle edecektir. Bu akış üzerinde iki ana yapılandırma karşılaştırılacaktır:

### 1. Vendor-Lock Senaryosu 🔒
*   **Tanım:** Servisler, belirli bir sağlayıcının (örn. New Relic) özel SDK'larını (Agent) kullanır.
*   **Akış:** Servisler -> New Relic Agent -> New Relic Cloud.
*   **Risk:** New Relic'ten vazgeçmek istendiğinde tüm kod tabanının değiştirilmesi gerekir.

### 2. Vendor-Free (OpenTelemetry) Senaryosu 🔓
*   **Tanım:** Servisler, platformdan bağımsız **OpenTelemetry SDK** kullanır.
*   **Akış:** Servisler -> OpenTelemetry Collector -> (Jaeger, Prometheus, ELK Stack ve Opsiyonel olarak New Relic).
*   **Avantaj:** Kod değişikliği yapmadan verinin hedefi (sink) değiştirilebilir.

## 🧪 İleri Seviye Observability Senaryoları
Sistemi sadece kurup izlemekle kalmayacağız; aşağıdaki gibi **kaos ve gerçek hayat problemleri** yaratarak gözlemlenebilirliğin gücünü test edeceğiz:

### 1. Cache & Latency Problemleri
*   **Cache Stampede / Thundering Herd:** Önbelleğin aniden boşalmasıyla oluşan ani yük artışı ve sistemin kitlenmesi.
*   **Cold Start:** Yeni açılan servislerin (warm-up eksikliği) ilk isteklerdeki yavaşlığı.
*   **GC Pause Spikes:** Garbage Collection kaynaklı anlık "donmalar" ve bunların p99 latency üzerindeki etkisi.

### 2. Deployment & Partial Failures
*   **Half-Broken State:** Rolling update sırasında yeni versiyonun hatalı olması ve trafiğin bir kısmının hata alması (%50 up, %50 down).
*   **Backward Compatibility Break:** Yeni servis versiyonunun eski veri formatını anlayamaması.
*   **"Her Şey Yeşil Ama..." Paradoksu:** Dashboard'larda tüm servisler "UP" görünürken, kullanıcıların hata alması (Semantic Errors).

### 3. Resource & Pool Sorunları
*   **Connection Pool Starvation:** Veritabanı bağlantılarının tükenmesi ve isteklerin kuyrukta beklemesi.
*   **Metrics Lie (Metriklerin Yalan Söylemesi):** Ortalama (Average) değerlerin iyi görünmesi ama p99'un felaket olması durumu.

## ⚙️ Değişkenler
Her senaryoda aşağıdaki parametreler değiştirilerek sistem davranışı analiz edilecektir:
*   **Load Pattern:** Sabit yük, ani spike (\`thundering herd\`), yavaş artan yük.
*   **Collector Config:** Verinin gönderileceği hedefler (sadece Jaeger, hem Jaeger hem NR vb.).
*   **Feature Flags:** Canlı sistemde kod değiştirmeden davranışın değiştirilmesi.

## 📊 Gözlemlenebilirlik Araçları
*   **Tracing:** Jaeger (Distributed Tracing).
*   **Metrics:** Prometheus + Grafana.
*   **Logs:** Elasticsearch + Kibana (veya Loki).
*   **Instrumentation:** OpenTelemetry SDK (.NET).
*   **Collection:** OpenTelemetry Collector.

## 📝 Çıktı ve Analiz
Tüm deneylerin sonuçları `results/` klasöründe JSON formatında saklanacaktır.
*   **Dosya:** `results/observability_lab_results.json`
*   **Format:** `{ "timestamp": "ISO8601", "scenario": "CacheStampede", "metrics": { ... }, "observations": "..." }`