# 🔬 Thread Starvation Lab - Hızlı Başlangıç

## ⚡ Hemen Başla

Terminalde şu komutu çalıştır:

```bash
./start-thread-starvation-test.sh
```

---

## 2️⃣ Sonuçları Gözlemle

**Tarayıcıda Monitoring:**
- 🔍 **Jaeger Traces:** [http://localhost:16686](http://localhost:16686)
- 🪵 **Kibana Logs:** [http://localhost:5601](http://localhost:5601)
- 📈 **Grafana Metrics:** [http://localhost:3000](http://localhost:3000) (admin/admin)

---

## 📊 Özet Sonuç

Test yaklaşık 30 saniye sürer. Loglarda şunlardan birini görmelisiniz:

1.  **✅ Başarılı:** `All workers completed successfully` (Kaynak yeterliydi).
2.  **❌ Starvation:** `NO AVAILABLE THREADS` veya `TIMEOUT` (Sistem kilitlendi).

Detaylar için: [THREAD_STARVATION_MONITORING.md](THREAD_STARVATION_MONITORING.md)
