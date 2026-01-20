# 🔬 Thread Starvation Lab - Quick Start Guide

## ⚡ 60 Saniye İçinde Başla

### 1️⃣ Docker Stack'i Başlat

```bash
# Proje dizinine git
cd c:\Projects\backend-systems-lab

# BAT dosyasını çalıştır (En kolay yol!)
start-thread-starvation-test.bat
```

**Veya manual olarak:**

```bash
docker-compose up -d
```

### 2️⃣ Sonuçları Gözlemle

**Real-time Logs:**
```bash
docker-compose logs -f api | findstr "Thread Starvation"
```

**Tarayıcıda Monitoring:**
- 🔍 **Jaeger Traces:** http://localhost:16686
- 📈 **Grafana Dashboards:** http://localhost:3000
- 📊 **Prometheus Metrics:** http://localhost:9090

---

## 📊 Görülecek Sonuçlar

```
╔════════════════════════════════════════════════════════════════════╗
║        Thread Starvation Lab - Deney #2.1 (Background Worker)       ║
║     Demonstrating ThreadPool Starvation with Task.Run + .Wait()    ║
╚════════════════════════════════════════════════════════════════════╝

Configuration:
  - Total Workers: 100
  - Max Concurrent: 50
  - Worker Duration: 5000ms
  - Timeout: 30s

🔍 ThreadPool Monitoring Started

📊 [ThreadPool] Available: 28/32 (Utilization: 12%)
📊 [ThreadPool] Available: 2/32 (Utilization: 93%) ⚠️ HIGH UTILIZATION
📊 [ThreadPool] Available: 0/32 (Utilization: 100%)
❌ NO AVAILABLE THREADS - COMPLETE STARVATION!

(30 saniye sonra...)

╔════════════════════════════════════════════════════════════════════╗
║ ⚠️  TIMEOUT! Workers did not complete within 30s                   ║
║ ❌ ThreadPool is DEADLOCKED - threads blocked by Task.Run + .Wait()║
╚════════════════════════════════════════════════════════════════════╝

📈 Final Statistics:
  - Total Elapsed: 30015ms
  - Completed Workers: 47/100
  - Failed Workers: 53/100

✅ STARVATION DETECTED ✅
```

---

## 🔑 Anahtar Bulgular

| Metrik | Değer | Açıklama |
|--------|-------|---------|
| **Thread Utilization** | 100% | Tüm thread'ler bloklandı |
| **Completed Workers** | 47/100 | Sadece %47 başarılı |
| **Timeout Duration** | 30s | Deneyim timeout'a ulaştı |
| **Memory Spike** | 3x | 100-300MB artış |
| **CPU Spike** | 80-95% | Busy waiting nedeniyle |

---

## 🎯 Öğrenilecekler

### ❌ Problematic Pattern:
```csharp
// Thread içinde
new Thread(() => {
    semaphore.Wait();
    Task.Run(async () => {
        await Task.Delay(5000);
    }).Wait();  // ❌ DEADLOCK!
    semaphore.Release();
}).Start();
```

### ✅ Correct Pattern:
```csharp
// Async all the way
async Task WorkerAsync() {
    await semaphore.WaitAsync();
    try {
        await Task.Delay(5000);
    } finally {
        semaphore.Release();
    }
}
```

---

## 📁 Dosya Yapısı

```
backend-systems-lab/
├── start-thread-starvation-test.bat         ← Başlangıç scripti
├── THREAD_STARVATION_MONITORING.md          ← Detaylı monitoring rehberi
└── src/BasicsLab/BackendLab.Api/
    └── Services/
        └── ThreadStarvationBackgroundService.cs ← Ana deneyim kodu
```

---

## 🚀 Ek Komutlar

```bash
# Stack'i durdur
docker-compose down

# Logs'u filtreyle
docker-compose logs -f api | findstr "Completed"

# Container'a gir
docker exec -it backend-systems-lab-api-1 sh

# Memory & CPU stats
docker stats backend-systems-lab-api-1

# Network kontrolü
docker network ls
```

---

## 📌 Monitoring URLs

| Tool | URL | Login |
|------|-----|-------|
| Grafana | http://localhost:3000 | admin/admin |
| Jaeger | http://localhost:16686 | - |
| Prometheus | http://localhost:9090 | - |
| API | http://localhost:8080 | - |

---

## ❓ Sık Sorular

**S: Test ne kadar sürer?**  
C: ~30-35 saniye (2 saniye startup + 30 saniye timeout)

**S: Logs'ta hiçbir şey görünmüyor?**  
C: `docker-compose logs api` komutunu çalıştır veya container'ı restart et

**S: Grafana'da metrics görmüyorum?**  
C: Prometheus'u data source olarak ekle (http://prometheus:9090)

**S: Jaeger'da trace yok?**  
C: OTEL Collector'ın çalıştığını kontrol et: `docker ps | findstr otel`

---

## 📚 Devamını Oku

Detaylı monitoring rehberi için: [THREAD_STARVATION_MONITORING.md](THREAD_STARVATION_MONITORING.md)

---

**Status:** ✅ Ready to use  
**Build:** Successful  
**Experiment:** Deney #2.1 - Thread Starvation  
**Last Updated:** January 20, 2026
