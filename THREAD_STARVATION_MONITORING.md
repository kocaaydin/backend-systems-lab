# 🔬 Thread Starvation Lab - İzleme ve Analiz Rehberi

Bu dosya, test sonuçlarını nasıl yorumlayacağınızı ve monitoring araçlarını nasıl kullanacağınızı açıklar.

---

## 🚀 Başlangıç

Tek komutla tüm sistemi başlatın:

```bash
./start-thread-starvation-test.sh
```

---

## 📊 Monitoring Ekranları

### 1. Jaeger (En Önemli)
**Link:** [http://localhost:16686](http://localhost:16686)

*   **Service:** `backend-lab-api`
*   **Operation:** `ThreadStarvationExperiment`
*   **Find Traces:** Butona basarak arayın.

**🔍 Ne Görmelisiniz?**
*   **Süre:** Eğer Starvation varsa `30s` (timeout) süren bir trace görürsünüz.
*   **Durum:** Hata (Error) ikonu veya kırmızı loglar.
*   **Tagler:** `experiment.starved = true`

### 2. Elastic & Kibana (Log Analizi)
**Link:** [http://localhost:5601](http://localhost:5601)

*   Menüden **Discover** sekmesine gidin.
*   Loglarda `NO AVAILABLE THREADS` veya `TIMEOUT` araması yapın.

### 3. Grafana (Görsel Grafikler)
**Link:** [http://localhost:3000](http://localhost:3000)

*   **Login:** admin / admin
*   **Veri Kaynağı:** Prometheus'u ekleyin (`http://prometheus:9090`).
*   **Memory Spike:** Test sırasında bellek kullanımının (RAM) aniden yükseldiğini görebilirsiniz.

---

## 🎯 Deney Sonuçları

### Başarılı Senaryo (Starvation Yok - Yeterli Kaynak)
Eğer sisteminiz güçlüyse (Mac M1/M2/M3 gibi), 100 thread 15-20 saniyede işini bitirir.
*   **Log:** `✅ All workers completed successfully`
*   **Trace Süresi:** < 20s

### Başarısız Senaryo (Starvation Var - Kaynak Tükendi)
Thread limiti düşükse veya yük çok fazlaysa sistem kilitlenir.
*   **Log:** `❌ NO AVAILABLE THREADS - COMPLETE STARVATION!`
*   **Trace Süresi:** 30s (Timeout)

---

## 🧩 Kod Analizi

### Hatalı Kullanım (Anti-Pattern)
`Task.Run` ile işi ThreadPool'a atıp, sonra `.Wait()` ile senkron olarak beklemek o thread'i kilitler. Bu (Sync-over-Async) deadlock'a yol açar.

```csharp
// ❌ YANLIŞ: ThreadPool thread'i meşgul edilirken, sonucunu beklemek için başka thread de bloklanıyor.
Task.Run(async () => await IsYap()).Wait(); 
```

### Doğru Kullanım
Her aşamada `await` kullanarak Thread'in serbest kalmasını sağlamak.

```csharp
// ✅ DOĞRU: İş bitene kadar Thread serbest kalır, havuza döner.
await IsYap();
```
