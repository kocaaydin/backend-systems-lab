# 🏁 NetworkLab: Port Exhaustion (Crash Scenario) Raporu

Bu rapor, **Bad HttpClient** kullanımının sistemi nasıl bir "çöküş (System Crash)" noktasına sürüklediğini gösteren stres testi sonuçlarını içerir.

## 🧪 Test Ortamı ve Konfigürasyon

- **Hedef Servis:** `http://external-api:80` (Yerel Docker container)
- **Kullanılan Araç:** k6 (Stress Testing)
- **İstemci Yapılandırması:** `new HttpClient()` (Her istekte yeni instance)
- **Sistem Kısıtlaması (Simüle Edilmiş):** `ulimit -n 512` (Dosya ve socket limiti)
- **Yük:** 200 Eşzamanlı Kullanıcı (VU)
- **Süre:** 15 Saniye

---

## 📊 Özet Sonuçlar

| Metrik | Değer |
|--------|-------|
| **Toplam İstek (Attempted)** | 1.422 |
| **Başarılı İstek** | 0 (%0) |
| **Başarısız İstek** | 1.422 (%100) 💥 |
| **Hata Mesajı** | `dial tcp: connect: cannot assign requested address` |
| **Sistem Durumu** | **KİLİTLENDİ (DEADLOCK / UNRESPONSIVE)** |

---

## 🔍 Neden Çöktü? (Root Cause Analysis: Port Exhaustion)

### Port Exhaustion Mekanizması
Concurrency'yi (eşzamanlılık) kontrol altında tutarak ve `ulimit`'i 512'ye düşürerek **saf port tükenmesini** izole ettik.
- **Sonuç:** Boşta bekleyen portlar (TIME_WAIT) ve aktif socket'ler toplam limiti (512) saniyeler içinde aştı.
- **Hata:** İşletim sistemi yeni bağlantı açmak isteyen uygulamaya "Socket açacak yerim kalmadı" (`cannot assign requested address`) dedi.
- **Ders:** Port exhaustion sadece bir "yavaşlama" değil, uygulamanın dış dünyaya tamamen kapanmasıdır.

---

## 📈 Karşılaştırma: Gerçek vs Teorik

| Durum | Teorik Tahmin | Gerçek Test (k6) |
|-------|--------------|------------------|
| **Çökme Süresi** | 13.6 saniye | **< 3 saniye** (yük seviyesine bağlı) |
| **Hata Türü** | Port Tükenmesi | Port Exhaustion + I/O Timeout |
| **Etki** | Yavaşlama | Tam Hizmet Kesintisi |

---

## ⚠️ Kritik Çıkarımlar

1. **Öngörülemezlik:** Port exhaustion sadece bir "yavaşlama" değil, uygulamanın dış dünyaya tamamen kapanmasıdır.
2. **Kısıtlı Kaynaklar:** Docker containerları veya cloud enviromentları (Azure, AWS) varsayılan port limitlerine sahiptir. Bu limitler aşıldığında uygulama restart olsa bile düzelmez (çünkü TIME_WAIT portları hala meşgul eder).
3. **Çözüm:** `IHttpClientFactory` kullanımı bu testte **%100 başarı** sağlardı. Çünkü binlerce istek olsa dahi toplamda sadece 10-20 socket kullanılır, limit asla aşılmaz.

---

## 🛠️ Nasıl Reproduce Edilir?

```bash
# 1. Limitleri ayarla (docker-compose.yml)
ulimits:
  nofile: 512

# 2. k6 Stress testini çalıştır
./scenarios/03-Network/scripts/NetworkLab/run_k6_port_exhaustion.sh
```

---

*Hata Analiz Notu:*
Sisteminizde `dial: i/o timeout` veya `address already in use` hatalarını görüyorsanız, ilk bakmanız gereken yer HttpClient yaşam döngüsüdür.

**Tarih:** 18 Ocak 2026  
**Durum:** ✅ Raporlandı / 💥 Çöküş Onaylandı
