# Storage & Consistency Lab

## 🎯 Amaç
Bu laboratuvarın temel amacı, veri katmanının (Data Layer) sistem davranışını nasıl şekillendirdiğini derinlemesine anlamak ve gerçek dünya senaryolarında karşılaşılabilen sorunlara karşı mühendislik reflekslerini geliştirmektir. Sadece "Veritabanı yavaş" demek yerine, sorunun kök nedenini (lock contention, pool exhaustion, network latency vb.) analiz edebilecek yetkinliğe ulaşılması hedeflenmektedir.

## 🏗️ Mimari
Bu deney ortamı, mikroservis mimarisini simüle eden çok katmanlı bir yapıdan oluşur:

`API` → `Order Service` → `Inventory Service` → `Database`

Bu akış üzerinde farklı noktalarda bilinçli hatalar ve darboğazlar yaratılarak sistemin tepkisi ölçülecektir.

## 🧪 Deney Senaryoları

Aşağıdaki senaryolar, veri tutarlılığı ve sistem dayanıklılığı üzerinde testler yapmak için tasarlanmıştır:

### 1. Performans & Kaynak Yönetimi
- **Slow Query Etkisi:** Tek bir yavaş sorgunun zincirleme etkisiyle tüm sistemi nasıl kilitlediğinin gözlemlenmesi.
- **Connection Pool Exhaustion:** Veritabanı bağlantı havuzunun dolmasının, gelen istekleri nasıl boğduğunu ve timeout'lara yol açtığını simüle etmek.
- **Cache Stampede:** Önbelleğin (cache) düşmesi veya süresinin dolması anında sisteme binen ani yükün (spike) sistemi nasıl çökerttiğinin analizi.

### 2. Eşzamanlılık (Concurrency) & Kilitler (Locks)
- **Lock Contention (Kilit Çekişmesi):** Aynı kaynağa erişmeye çalışan işlemlerin birbirini beklemesi sonucu throughput düşüşünün incelenmesi.
- **Deadlock (Ölümcül Kilitlenme):** İki işlemin birbirini beklemesi sonucu sistemin kilitlenmesi.

### 3. Veri Tutarlılığı (Consistency)
- **Replica Lag & Stale Read:** Verinin bir replica'ya geç yazılması sonucu eski verinin okunması durumu.
- **Write Skew & Lost Update:** Eşzamanlı güncellemelerde veri kaybı veya mantıksal tutarsızlıkların (isolation levels kaynaklı) simülasyonu.

## ⚙️ Değişkenler
Her deneyde aşağıdaki parametreler değiştirilerek sonuçlar karşılaştırılacaktır:
- **Timeouts:** İstek ve bağlantı zaman aşımları.
- **Pool Size:** Veritabanı bağlantı havuzu limitleri.
- **Isolation Level:** Read Committed, Repeatable Read, Serializable vb.
- **Retry Policies:** Hata anında tekrar deneme stratejileri.
- **Cache TTL:** Önbellek geçerlilik süreleri.

## 📊 Gözlemlenebilirlik (Observability)
Tüm servisler **OpenTelemetry** ile enstrümante edilecektir. Her bir senaryo için:
- **Traces:** İsteğin yaşam döngüsü ve darboğaz noktaları.
- **Metrics:** Hata oranları, gecikme süreleri (latency), throughput.
- **Logs:** Hata detayları ve sistemin o anki durumu.

## 📝 Metodoloji
Her deney için şu döngü takip edilecektir:
1.  **Hipotez:** "Bu senaryoda sistem X tepkisini vermeli."
2.  **Deney:** Senaryonun çalıştırılması.
3.  **Ölçüm:** Gerçek davranışın gözlemlenmesi (Dashboard & Logs).
4.  **Analiz:** Beklenen ve gerçekleşen arasındaki farkların nedenlerinin dokümante edilmesi.

## 📁 Bu Repodaki Çalıştırılabilir Örnekler
- `scenarios/02-Database/StorageConsistency/README.md`
- `scenarios/02-Database/StorageConsistency/setup_storage_consistency.sql`
- `scenarios/02-Database/StorageConsistency/replica_lag_stale_read_demo.sql`
- `scenarios/02-Database/StorageConsistency/write_skew_lost_update_demo.sql`
- `scenarios/02-Database/StorageConsistency/run_storage_consistency.sh`
