# Thread Pool / Worker Pool Roadmap (Deney + Stres Test)

## Amaç
- Teoride bilinen kavramları pratikte gözlemlemek.
- Yanlış thread/pool kararlarının latency ve cevap sürelerine etkisini görmek.
- Her senaryoyu tek komutla çalıştırıp çıktıyı terminalden yorumlayabilmek.

## 0. Thread Türleri (Temel Harita)
- `Main Thread`: Uygulamanın giriş thread'i.
- `ThreadPool Thread`: Runtime tarafından yönetilen worker thread'ler.
- `Dedicated Thread`: Uygulamanın özel işi için açtığı manuel thread.
- `GC Threads`: GC işini yapan runtime thread'leri.
- `Finalizer Thread`: Finalizer kuyruğunu tüketen özel thread.

Not:
- Starvation belirtileri çoğunlukla `ThreadPool` üzerinde görünür.
- Thread modelini seçmek (ThreadPool vs Dedicated) mimari karardır.

## 1. Tek Pool ile Başla
Ne yapıyor:
- `cpu-heavy-threadpool` yükü altında `/fast` endpoint gecikmesini gösterir.

Ne gözlemlemeliyim:
- Baseline hızlı yanıt.
- Yük altına girince `/fast` latency artışı.


## 2. Starvation'ı Bilinçli Üret
Ne yapıyor:
- `starvation/blocking` endpoint'iyle pool thread'lerini meşgul eder.

Ne gözlemlemeliyim:
- `/fast` cevap süresinde bozulma.
- Sistem ayakta olsa da cevapların gecikmesi.

Dosyalar:
- SH: `scenarios/01-Threading/ThreadTypes/scripts/02_induce_starvation.sh`
- K6: `scenarios/01-Threading/ThreadTypes/k6/02_induce_starvation.js`

## 3. Worker Pool Ayrımı (ThreadPool vs Dedicated)
Ne yapıyor:
- Aynı CPU işini önce `ThreadPool`, sonra `Dedicated` modelinde koşturur.

Ne gözlemlemeliyim:
- Yük altında hangi modelin `/fast` üzerine daha fazla etkisi var?
- Aynı iş tipi için farklı model seçiminin sonucu.

Dosyalar:
- SH: `scenarios/01-Threading/ThreadTypes/scripts/03_separate_worker_pool_dedicated.sh`
- K6: `scenarios/01-Threading/ThreadTypes/k6/03_threadpool_vs_dedicated.js`

## 4. Queue + Worker Simülasyonu
Ne yapıyor:
- Bounded queue ile backpressure davranışını test eder.
- Dar kapasite/yavaş işleme ile geniş kapasite/hızlı işleme karşılaştırılır.

Ne gözlemlemeliyim:
- `producer` bekliyor mu?
- Kapasite ve iş süresi değişince kuyruk davranışı nasıl değişiyor?

Dosyalar:
- SH: `scenarios/01-Threading/ThreadTypes/scripts/04_queue_backpressure.sh`
- K6: `scenarios/01-Threading/ThreadTypes/k6/04_queue_backpressure.js`

## 5. Failure Senaryosu (Kill + Retry)
Ne yapıyor:
- Uzun istek sırasında API process kesilir.
- API yeniden başlatılıp aynı istek retry edilir.

Ne gözlemlemeliyim:
- İlk denemede hata/yarım kalma.
- Retry sonrası toparlanma.

Dosyalar:
- SH: `scenarios/01-Threading/ThreadTypes/scripts/05_failure_retry_simulation.sh`
- K6: `scenarios/01-Threading/ThreadTypes/k6/05_failure_retry.js`

## 6. Çok Seviyeli Karşılaştırma
Ne yapıyor:
- `vus=1,4,8,12` seviyelerinde ThreadPool vs Dedicated karşılaştırması yapar.

Ne gözlemlemeliyim:
- Yük arttıkça model farkı netleşiyor mu?
- Özellikle p95/p99 trendi nasıl değişiyor?

Dosyalar:
- SH: `scenarios/01-Threading/ThreadTypes/scripts/06_compare_threadpool_vs_dedicated.sh`
- K6: `scenarios/01-Threading/ThreadTypes/k6/06_compare_threadpool_vs_dedicated.js`

## 7. GC Threads + Finalizer Thread
Ne yapıyor:
- `create -> stats -> collect -> stats` akışıyla GC/finalizer etkisini görünür yapar.

Ne gözlemlemeliyim:
- `finalized` sayısı collect sonrası artıyor mu?
- Finalizer queue davranışı beklendiği gibi mi?

Dosyalar:
- SH: `scenarios/01-Threading/ThreadTypes/scripts/07_gc_threads_observation.sh`
- K6: `scenarios/01-Threading/ThreadTypes/k6/07_gc_finalizer_observation.js`

## 8. Request Cancellation Propagation
Ne yapıyor:
- Kısa timeout ile cancellable endpoint'e istek gönderir.

Ne gözlemlemeliyim:
- Client timeout olduğunda iş gerçekten kesiliyor mu?
- Cancellation zinciri taşınıyor mu?

Dosyalar:
- SH: `scenarios/01-Threading/ThreadTypes/scripts/08_request_cancellation_propagation.sh`
- K6: `scenarios/01-Threading/ThreadTypes/k6/08_request_cancellation_propagation.js`

## 9. Fire-and-Forget Risk Simülasyonu
Ne yapıyor:
- Non-cancellable timeout ve cancellable timeout davranışlarını kıyaslar.

Ne gözlemlemeliyim:
- Timeout sonrası sistem etkisi (özellikle `/fast` gecikmesi).
- Cancellable ve non-cancellable farkı.

Dosyalar:
- SH: `scenarios/01-Threading/ThreadTypes/scripts/09_fire_and_forget_risk_simulation.sh`
- K6: `scenarios/01-Threading/ThreadTypes/k6/09_fire_and_forget_risk.js`

## 10. Graceful Shutdown + Queue Drain
Ne yapıyor:
- Uzun queue isteği çalışırken API'ye `TERM` gönderir.

Ne gözlemlemeliyim:
- İstek tamamlandı mı, yoksa yarım mı kaldı?
- Kapanış davranışı öngörülebilir mi?

Dosyalar:
- SH: `scenarios/01-Threading/ThreadTypes/scripts/10_graceful_shutdown_queue_drain.sh`
- K6: `scenarios/01-Threading/ThreadTypes/k6/10_graceful_shutdown_queue_drain.js`

## 11. Birden Fazla ThreadPool Yorumu (Multi-Process)
Ne yapıyor:
- Aynı API'den iki ayrı process başlatır.
- A processine ağır yük verilir, A ve B'de `/fast` probu alınır.

Ne gözlemlemeliyim:
- A yük altındayken B izolasyonu korunuyor mu?
- Process başına ayrı threadpool davranışı pratikte nasıl görünüyor?

Dosyalar:
- SH: `scenarios/01-Threading/ThreadTypes/scripts/11_multiple_threadpools_multi_process.sh`
- K6 (yük): `scenarios/01-Threading/ThreadTypes/k6/11_multi_process_heavy_a.js`
- K6 (probe): `scenarios/01-Threading/ThreadTypes/k6/11_multi_process_fast_probe.js`

## Hızlı Kullanım
Ön koşul:
- `docker`, `docker compose`, `curl`
- Not: Bu senaryolarda API ve k6 testleri docker compose ile çalışır.

Çalıştırma:
- `cd scenarios/01-Threading/ThreadTypes/scripts`
- `./01_single_pool_baseline.sh`
- `./02_induce_starvation.sh`
- `./11_multiple_threadpools_multi_process.sh`
