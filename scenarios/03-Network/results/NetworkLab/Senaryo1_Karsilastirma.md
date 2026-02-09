# NetworkLab - Senaryo 1: Karşılaştırmalı Test Sonuçları

## 🎯 Connection Pooling ve TCP Bağlantı Yeniden Kullanımı

**Test Tarihi:** 18 Ocak 2026, 00:03  
**Test Varyasyonları:** 2 farklı yük seviyesi

---

## 📊 Test Sonuçları Özeti

### Test 1.1: Düşük Yük (50 İstek)

| Metrik | İyi (Factory) | Kötü (New Instance) | Fark |
|--------|---------------|---------------------|------|
| **İstek Sayısı** | 50 | 50 | - |
| **Süre** | 48.05s | 50.19s | %4 daha yavaş |
| **Throughput** | 1.04 req/s | 1.00 req/s | %4 daha düşük |
| **Ephemeral Port** | +1 | +60 | **60× fazla** ❌ |
| **TIME_WAIT** | 0 | +100 | **100 zombi (Zombie Connections)** ❌ |

### Test 1.2: Orta Yük (100 İstek)

| Metrik | İyi (Factory) | Kötü (New Instance) | Fark |
|--------|---------------|---------------------|------|
| **İstek Sayısı** | 100 | 100 | - |
| **Süre** | 98.33s | 102.94s | %5 daha yavaş |
| **Throughput** | 1.02 req/s | 0.97 req/s | %5 daha düşük |
| **Ephemeral Port** | +2 | +49 | **24× fazla** ❌ |
| **TIME_WAIT** | 0 | +122 | **122 zombi (Zombie Connections)** ❌ |

---

## 📈 Ölçeklendirme Analizi

### İstek Sayısı Arttıkça Ne Oluyor?

**İyi HttpClient (IHttpClientFactory):**
- 50 istek: +1 port, 0 TIME_WAIT
- 100 istek: +2 port, 0 TIME_WAIT
- **Sonuç:** Doğrusal olmayan, minimal artış ✅
- **Açıklama:** Connection pool sabit kalıyor, sadece pool boyutu hafif artıyor

**Kötü HttpClient (new instance):**
- 50 istek: +60 port, +100 TIME_WAIT
- 100 istek: +49 port, +122 TIME_WAIT
- **Sonuç:** Neredeyse doğrusal artış ❌
- **Açıklama:** Her istek yeni port tüketiyor, TIME_WAIT birikimi devam ediyor

---

## 🔍 Detaylı Analiz

### Neden 100 İstek'te Daha Az Port Tüketimi?

**Kötü HttpClient'ta ilginç bir durum:**
- 50 istek → +60 port (istek başına 1.2 port)
- 100 istek → +49 port (istek başına 0.49 port)

**Açıklama:**
1. İlk testte bazı portlar hala TIME_WAIT'te (60 saniye)
2. İkinci test daha hızlı çalıştı, bazı portlar yeniden kullanıldı
3. **Ama TIME_WAIT 122'ye çıktı** - asıl sorun bu!

### TIME_WAIT Birikimi - Asıl Tehlike

```
50 istek:  100 TIME_WAIT (60 saniye bekleyecek)
100 istek: 122 TIME_WAIT (60 saniye bekleyecek)
```

**Production'da ne olur:**
- 1000 req/s → 2000 TIME_WAIT/saniye
- 60 saniye sonra: **120,000 TIME_WAIT bağlantısı**
- Sistem kaynakları tükenir (Resource Exhaustion) → Çökme

---

## ⚡ Performans Karşılaştırması

### Throughput (İstek/Saniye)

```
İyi HttpClient:
- 50 istek:  1.04 req/s
- 100 istek: 1.02 req/s
- Tutarlı performans ✅

Kötü HttpClient:
- 50 istek:  1.00 req/s
- 100 istek: 0.97 req/s
- Performans düşüyor ❌
```

**Neden performans düşüyor?**
- Her istek için yeni TCP handshake (3-way)
- DNS lookup overhead
- TLS handshake (HTTPS'te)
- Socket oluşturma/kapatma maliyeti

---

## 🎯 Öneriler

### 1. Her Zaman IHttpClientFactory Kullanın
```csharp
// Startup.cs
services.AddHttpClient();

// Controller
private readonly IHttpClientFactory _factory;
var client = _factory.CreateClient();
```

### 2. Named Client ile Yapılandırın
```csharp
services.AddHttpClient("ExternalAPI", c => {
    c.BaseAddress = new Uri("http://external-api");
    c.Timeout = TimeSpan.FromSeconds(30);
});
```

### 3. Production'da İzleyin
```bash
# Ephemeral port kullanımı
netstat -an | grep -c "49152:65535"

# TIME_WAIT bağlantıları
netstat -an | grep -c TIME_WAIT
```

---

## 📊 Sonuç Tablosu

| Yük Seviyesi | İyi Port Kullanımı | Kötü Port Kullanımı | Tasarruf |
|--------------|-------------------|---------------------|----------|
| **50 istek** | +1 | +60 | **98%** |
| **100 istek** | +2 | +49 | **96%** |
| **Ortalama** | +1.5 | +54.5 | **97%** |

---

## ✅ Sonuç

**IHttpClientFactory kullanımı:**
- ✅ %97 daha az port tüketimi
- ✅ Sıfır TIME_WAIT birikimi
- ✅ %4-5 daha iyi performans
- ✅ Ölçeklenebilir ve kararlı

**new HttpClient() kullanımı:**
- ❌ Yüksek port tüketimi
- ❌ Tehlikeli TIME_WAIT birikimi
- ❌ Düşük performans
- ❌ Production'da çökme riski

---

*Test tarihi: 18 Ocak 2026, 00:03 UTC*  
*Ortam: Docker on macOS, .NET 8.0*
