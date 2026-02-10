# Redis Roadmap

## 🎯 Amaç
Bu roadmap'in amacı, Redis'in sadece "hızlı cache" değil, aynı zamanda trafik yönetimi ve eşzamanlılık kontrolü için nasıl kullanıldığını **neden-sonuç** ilişkisiyle göstermek.

Hedef:
- `Cache hit` oranı düşükse neden latency arttığını,
- Yanlış TTL/invalidation kararlarının neden stale data ürettiğini,
- Lock/rate-limit tasarımının sistem stabilitesini nasıl etkilediğini görmek.

## 🧩 Kavramlar (Nedir?)
- **Cache hit:** İstenen veri cache'de bulundu, DB'ye gidilmedi.
- **Cache miss:** Veri cache'de yok, DB veya kaynak servisten çekildi.
- **TTL (Time To Live):** Cache kaydının otomatik silinme süresi.
- **Invalidation:** Veri değişince ilgili cache anahtarını silme/güncelleme işlemi.
- **Cache stampede:** Aynı anda çok sayıda isteğin cache miss sonrası DB'ye yüklenmesi.
- **Distributed lock:** Dağıtık sistemde aynı kritik işi tek instance'ın yapmasını garanti etmeye çalışan kilit.

## 🧪 Senaryolar

### 1. Cache Aside vs Write Through
- **Case A (Bad):** Sorgudan sonra cache'e yazılıyor, invalidation disiplini zayıf.
- **Case B (Good):** Güncelleme anında kontrollü cache güncelleme/invalidation.
- **Beklenen fark:** Case A'da stale data riski artar, Case B'de veri tazeliği daha stabildir.

### 2. Cache Stampede (Thundering Herd)
- **Case A (Bad):** Aynı anahtar expire olunca binlerce istek DB'ye düşer.
- **Case B (Good):** Jitter TTL + request coalescing + lock.
- **Beklenen fark:** Case A'da ani DB spike, Case B'de kontrollü toparlanma.
- **Yorumlama ipucu:** Miss oranı + DB CPU aynı anda fırlıyorsa genelde stampede vardır.

### 3. Rate Limiting
- **Case A (Bad):** Uygulama içinde in-memory limit (çoklu instance'da tutarsız).
- **Case B (Good):** Redis tabanlı merkezi limit (Sliding Window / Token Bucket).
- **Beklenen fark:** Case B'de instance sayısı artsa da limit tutarlı kalır.

### 4. Distributed Lock
- **Case A (Bad):** Lock timeout kötü seçilir, kritik bölge çakışır.
- **Case B (Good):** Lock süresi + retry/backoff kontrollü.
- **Beklenen fark:** Çift işlem (double process) riski azalır.

## 📊 Ölçülecek Metrikler
- Cache hit/miss oranı
- p95/p99 latency
- Redis command latency
- DB fallback oranı
- Lock contention süresi

## 🧪 Kod Karşılaştırması
**Kötü Kullanım (Stampede):**
```bash
# Key yoksa her istek backend'e gider
GET product:42
```

**İyi Kullanım (Lock + Tek Üretici):**
```bash
# Sadece lock alan instance cache'i doldurur
SET lock:product:42 1 NX EX 5
SET product:42 "payload" EX 60
```

## 🛠️ Nasıl Çalıştırılır?
```bash
cd scenarios/10-Redis
./run.sh
```

Çalışan dosyalar:
- `scenarios/10-Redis/run.sh`
- `scenarios/10-Redis/scripts/01_bad_cache_stampede.sh`
- `scenarios/10-Redis/scripts/02_good_cache_stampede.sh`
- `scenarios/10-Redis/scripts/03_rate_limit_demo.sh`
- `scenarios/10-Redis/k6/RedisLab/cache_endpoint_load.js`

## 🧭 Notlar
- Redis primary DB değildir, cache/state store'dur.
- TTL her zaman iş kuralı ile birlikte tasarlanmalıdır.
- Rate limit ve distributed lock için clock drift ve timeout seçimi kritiktir.
