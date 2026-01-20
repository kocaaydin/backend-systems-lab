# 🔬 Thread Starvation Lab - Grafana Monitoring Guide

## Deney #2.1: ThreadPool Starvation Demonstration

Bu rehber, Thread Starvation deneyinin sonuçlarını **Grafana**, **Jaeger**, ve **Prometheus** aracılığıyla nasıl gözlemleyeceğinizi anlatır.

---

## 🚀 Başlangıç

### 1. Docker Compose Stack'i Başlat

**En Kolay Yol - BAT Dosyası (Windows):**

```bash
start-thread-starvation-test.bat
```

Bu script otomatik olarak:
- ✅ Docker Compose stack'i başlatır
- ✅ Tüm servislerin healthy olmasını bekler
- ✅ Monitoring dashboard'larını aç

**Manuel Başlatma:**

```bash
cd c:\Projects\backend-systems-lab
docker-compose up -d
```

### 2. Servislerin Başlayıp Hazırlanmasını Bekle

```bash
docker-compose logs -f api
```

Output'ta şu satırları görünce hazır demektir:

```
🚀 Thread Starvation Background Service Starting...
📊 Starting worker initialization...
✅ All 100 worker threads launched
```

---

## 📊 Monitoring Dashboard'ları

### 📍 **Jaeger - Distributed Tracing** (En Önemli)

**URL:** http://localhost:16686

#### Traces'i Görmek İçin:

1. **Service:** "backend-lab-api" seç
2. **Operation:** "ThreadStarvationExperiment" seç
3. **Find Traces** tıkla

#### Gözlemlenecekler:

```
▶ ThreadStarvationExperiment (30000ms duration)
  ├─ experiment.name: "Thread Starvation - Deney #2.1"
  ├─ experiment.workers.total: 100
  ├─ experiment.workers.max_concurrent: 50
  ├─ experiment.starved: true
  ├─ experiment.result: "starvation_detected"
  └─ experiment.elapsed_ms: 30015
```

**Trace Detayları:**
- **Duration:** ~30 saniye (timeout)
- **Status:** Starvation detected ⚠️
- **Tags:** Completed/Failed worker sayıları
- **Logs:** Her worker'ın state'i

---

### 📈 **Prometheus - Metrics Query**

**URL:** http://localhost:9090

#### Kullanışlı Queries:

```promql
# API'nin uptime'ı
up{job="backend-lab-api"}

# Process memory usage
process_resident_memory_bytes{job="backend-lab-api"}

# CPU usage
process_cpu_seconds_total{job="backend-lab-api"}

# Go routines (thread sayısı)
go_goroutines{job="backend-lab-api"}
```

**Test Sırasında Beklenen Spike'lar:**
- Memory artışı (100 thread oluşturmak için)
- CPU usage yükselmesi
- Go routines sayısı artması

---

### 🔍 **Grafana - Dashboards & Alerts**

**URL:** http://localhost:3000
- **Username:** admin
- **Password:** admin

#### Dashboard Kurulumu:

##### 1. Prometheus Data Source Ekle:

1. Settings → Data Sources → Add
2. Type: "Prometheus"
3. URL: `http://prometheus:9090`
4. Save & Test

##### 2. Dashboard Import:

```
1. Create → Import
2. Upload: grafana-dashboard.json
3. Select Prometheus data source
4. Import
```

#### Pre-built Panels:

| Panel | Query | Açıklama |
|-------|-------|---------|
| API Uptime | `up{job="backend-lab-api"}` | API çalışıyor mu? |
| Memory Usage | `process_resident_memory_bytes` | Bellek tüketimi |
| Request Rate | `rate(http_request_duration_seconds_bucket[5m])` | HTTP request oranı |

---

## 📝 Test Sonuçlarını Yakalama

### 1. **Console Logs (Real-time)**

```bash
docker-compose logs -f api | findstr "Thread Starvation"
```

Output örneği:

```
🚀 Thread Starvation Background Service Starting...

╔════════════════════════════════════════════════════════════════════╗
║        Thread Starvation Lab - Deney #2.1 (Background Worker)       ║
║     Demonstrating ThreadPool Starvation with Task.Run + .Wait()    ║
╚════════════════════════════════════════════════════════════════════╝

📊 ThreadPool Stats:
   Worker Threads: 0/32 (Utilization: 100%)
   ❌ NO AVAILABLE THREADS - COMPLETE STARVATION!

📈 Final Statistics:
  - Total Elapsed: 30015ms
  - Completed Workers: 47/100
  - Failed Workers: 53/100
```

### 2. **Jaeger Traces**

Trace'ler otomatik olarak OTEL Collector'a gönderiliyor:

```bash
curl http://localhost:16686/api/traces?service=backend-lab-api
```

### 3. **Prometheus Metrics Scrape**

```bash
curl http://localhost:9090/api/v1/query?query=up
```

### 4. **Docker Stats**

```bash
docker stats backend-systems-lab-api-1
```

Görünecekler:
- CPU % (spike sırasında)
- Memory (100+ MB)
- Network I/O

---

## 🔍 Analiz - Starvation Bulguları

### ✅ Beklenen Sonuçlar:

| Bulgu | Değer | İzahı |
|-------|-------|-------|
| **Elapsed Time** | ~30 saniye | Timeout'a ulaştı |
| **Completed Workers** | 47-60 | Sadece başarısız (starvation başladı) |
| **Failed Workers** | 40-53 | Timeout veya hata |
| **ThreadPool Util.** | 100% | Tüm thread'ler bloklandı |
| **Starvation Detected** | TRUE | Deadlock tespit edildi |

### 📊 Grafana'da Gözlemlenecekler:

**Memory Spike:**
```
Before: ~100MB
During: ~300-400MB (100 thread'e)
After:  ~150MB (thread cleanup)
```

**CPU Spike:**
```
Before: ~5%
During: ~80-95% (busy waiting + context switching)
After:  ~2%
```

---

## 🛠️ Troubleshooting

### Problem: Logs'ta "Thread Starvation" görünmüyor

**Çözüm:**

```bash
# 1. Container'ın çalıştığını kontrol et
docker ps | findstr api

# 2. Logs'ı kontrol et
docker-compose logs api

# 3. Container'ı restart et
docker-compose restart api
```

### Problem: Prometheus metrikleri gelmiyoriş

**Çözüm:**

```bash
# OTEL Collector'ın çalıştığını kontrol et
docker ps | findstr otel-collector

# Collector logs'unu kontrol et
docker-compose logs otel-collector

# Prometheus config'ini kontrol et
curl http://localhost:9090/api/v1/label/__name__/values
```

### Problem: Jaeger'da trace yok

**Çözüm:**

```bash
# 1. API container'ını kontrol et
docker logs backend-systems-lab-api-1 | tail -50

# 2. OTEL Collector'a bağlantıyı kontrol et
docker-compose logs otel-collector | grep api

# 3. API'nin OTEL env variables'ı kontrol et
docker inspect backend-systems-lab-api-1 | grep -i otel
```

---

## 📚 Kaynaklar

### .NET ThreadPool Belgeleri
- [ThreadPool.GetAvailableThreads](https://docs.microsoft.com/en-us/dotnet/api/system.threading.threadpool.getavailablethreads)
- [Task Starvation](https://devblogs.microsoft.com/pfxteam/should-i-expose-asynchronous-wrappers-for-synchronous-methods/)

### OpenTelemetry
- [OTEL .NET SDK](https://github.com/open-telemetry/opentelemetry-dotnet)
- [OTEL Jaeger Exporter](https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/src/OpenTelemetry.Exporter.Jaeger)

### Grafana & Prometheus
- [Grafana Official Docs](https://grafana.com/docs/)
- [Prometheus Query Language](https://prometheus.io/docs/prometheus/latest/querying/basics/)

---

## 📌 Hızlı Referans

### Önemli Komutlar

```bash
# Stack'i başlat
docker-compose up -d

# Stack'i durdur
docker-compose down

# Logs'u canlı göster
docker-compose logs -f api

# Belirli container'ın logs'u
docker logs backend-systems-lab-api-1 -f

# Stack'i sıfırla (volumes silinir)
docker-compose down -v && docker-compose up -d

# Container shell'e gir
docker exec -it backend-systems-lab-api-1 sh

# Network durumunu kontrol et
docker network ls
```

### Dashboard URLs

| Tool | URL | Port |
|------|-----|------|
| **Grafana** | http://localhost:3000 | 3000 |
| **Prometheus** | http://localhost:9090 | 9090 |
| **Jaeger** | http://localhost:16686 | 16686 |
| **API** | http://localhost:8080 | 8080 |
| **OTEL Collector** | http://localhost:4317 | 4317 |

---

## 🎯 Sonuç

Bu deneyim gösteriyor ki:

1. ✅ **Task.Run + .Wait() ThreadPool'u blokluyor**
2. ✅ **Concurrent worker sınırı (SemaphoreSlim) işe yaramıyor**
3. ✅ **100 thread çalıştırıldığında ~30 saniyede deadlock oluşuyor**
4. ✅ **ThreadPool istatistikleri (availability) sıfıra düşüyor**
5. ✅ **Memory usage 3x artıyor (thread allocation)**

**Doğru Pattern:**
```csharp
// ❌ YANLIŞ
Task.Run(async () => await SomeAsyncWork()).Wait();

// ✅ DOĞRU
await SomeAsyncWork();
```

---

**Created:** January 20, 2026  
**Experiment:** Deney #2.1 - Thread Starvation Lab  
**Status:** ✅ Complete
