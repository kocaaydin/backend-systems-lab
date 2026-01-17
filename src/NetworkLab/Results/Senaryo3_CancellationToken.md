# NetworkLab - Senaryo 3: CancellationToken ve Zombi İstek Önleme

## 🎯 Client Timeout vs Server İşlem Süresi

**Test Tarihi:** 18 Ocak 2026, 00:11  
**Test Varyasyonları:** 2 farklı timeout konfigürasyonu

---

## 📊 Test Sonuçları Özeti

### Test 3.1: Kısa Timeout (3s timeout, 10s işlem)

| Metrik | WITH CancellationToken | WITHOUT CancellationToken | Fark |
|--------|------------------------|---------------------------|------|
| **Client Timeout** | 3s | 3s | - |
| **Server Durdurma** | 3s ✅ | 10s ❌ | 7s kaynak israfı |
| **Zombi İstek** | Hayır ✅ | Evet ❌ | Kritik |
| **Log Mesajı** | "cancelled by client" | "completed (even if...)" | - |

### Test 3.2: Orta Timeout (5s timeout, 15s işlem)

| Metrik | WITH CancellationToken | WITHOUT CancellationToken | Fark |
|--------|------------------------|---------------------------|------|
| **Client Timeout** | 5s | 5s | - |
| **Server Durdurma** | 5s ✅ | 15s ❌ | 10s kaynak israfı |
| **Zombi İstek** | Hayır ✅ | Evet ❌ | Kritik |
| **Kaynak İsrafı** | 0s | 10s | %67 israf |

---

## 🔍 CancellationToken Nedir?

**CancellationToken**, .NET'te asenkron işlemlerin iptal edilmesini sağlayan bir mekanizmadır.

### Nasıl Çalışır?

1. **Client bağlantıyı keser** (timeout, cancel, close)
2. **ASP.NET Core otomatik sinyal gönderir** (CancellationToken.IsCancellationRequested = true)
3. **Server kodu kontrol eder** ve işlemi durdurur
4. **Kaynaklar serbest bırakılır** (CPU, memory, DB connections)

### Kod Karşılaştırması

**✅ İyi Kullanım:**
```csharp
public async Task<IActionResult> LongProcess(
    int durationSeconds,
    CancellationToken cancellationToken)  // ← Parametre ekle
{
    for (int i = 0; i < durationSeconds; i++)
    {
        cancellationToken.ThrowIfCancellationRequested();  // ← Kontrol et
        await Task.Delay(1000, cancellationToken);  // ← Token'ı geç
    }
    return Ok("Completed");
}
```

**❌ Kötü Kullanım:**
```csharp
public async Task<IActionResult> LongProcess(
    int durationSeconds)  // ← Token yok!
{
    for (int i = 0; i < durationSeconds; i++)
    {
        await Task.Delay(1000);  // ← İptal edilemiyor
    }
    return Ok("Completed");  // ← Client zaten gitmiş!
}
```

---

## 📈 Zombi İstek Birikimi

### Senaryo: Saniyede 100 İstek, %10 Timeout

**WITHOUT CancellationToken:**
```
Timeout oranı: 10 istek/s
Her biri 7s fazladan çalışıyor
Zombi birikim: 10 × 7 = 70 zombi/saniye

1 dakika sonra: 70 × 60 = 4,200 zombi istek!
```

**Sonuç:**
- CPU %100'e çıkar
- Memory tükenir
- DB connection pool dolar
- **Server çöker** 💥

**WITH CancellationToken:**
```
Timeout oranı: 10 istek/s
Hepsi hemen durur
Zombi birikim: 0

1 dakika sonra: 0 zombi istek
```

**Sonuç:**
- CPU normal
- Memory stabil
- DB connections serbest
- **Server kararlı** ✅

---

## 🔑 Timeout Süreleri ve Etki

### Test 3.1: 3s Timeout, 10s İşlem

**Kaynak İsrafı:**
- WITHOUT token: 7 saniye (10 - 3)
- İsraf oranı: %70

**Production etki (100 req/s, %10 timeout):**
- Zombi/saniye: 10 × 7 = 70
- 1 dakikada: 4,200 zombi
- **Orta risk** ⚠️

### Test 3.2: 5s Timeout, 15s İşlem

**Kaynak İsrafı:**
- WITHOUT token: 10 saniye (15 - 5)
- İsraf oranı: %67

**Production etki (100 req/s, %10 timeout):**
- Zombi/saniye: 10 × 10 = 100
- 1 dakikada: 6,000 zombi
- **Yüksek risk** ❌

---

## ⚠️ Gerçek Dünya Senaryoları

### Senaryo 1: API Gateway Timeout
```
API Gateway: 30s timeout
Backend işlem: 60s (ağır query)
Client timeout: 30s

WITHOUT token: 30s kaynak israfı × istek sayısı
WITH token: 0s israf
```

### Senaryo 2: Mobil Uygulama
```
Mobil network: Kararsız
User: Uygulamayı kapatır
Server: Hala işliyor...

WITHOUT token: İşlem tamamlanana kadar (dakikalar)
WITH token: Anında durur
```

### Senaryo 3: Microservice Chain
```
Service A → Service B → Service C
Service A timeout: 5s
Service B işlem: 10s

WITHOUT token: Service B zombi kalır
WITH token: Tüm chain temiz durur
```

---

## 🎯 Best Practices

### 1. Her Async Method'a Token Ekle
```csharp
public async Task<T> MyMethod(CancellationToken cancellationToken = default)
{
    // İşlemler...
}
```

### 2. Token'ı Her Async Call'a Geç
```csharp
await _httpClient.GetAsync(url, cancellationToken);
await _dbContext.SaveChangesAsync(cancellationToken);
await Task.Delay(1000, cancellationToken);
```

### 3. Loop'larda Kontrol Et
```csharp
for (int i = 0; i < 1000; i++)
{
    cancellationToken.ThrowIfCancellationRequested();
    // İşlem...
}
```

### 4. Try-Catch ile Yakala (Opsiyonel)
```csharp
try
{
    await LongRunningTask(cancellationToken);
}
catch (OperationCanceledException)
{
    _logger.LogInformation("Operation cancelled by user");
    // Cleanup...
}
```

---

## 📊 Sonuç Tablosu

| Timeout Config | Kaynak İsrafı (WITHOUT) | Zombi Risk | Tavsiye |
|----------------|-------------------------|------------|---------|
| **3s / 10s** | 7s (%70) | Orta ⚠️ | Token kullan |
| **5s / 15s** | 10s (%67) | Yüksek ❌ | **Mutlaka** token kullan |

---

## ✅ Sonuç

**CancellationToken kullanımı:**
- ✅ Zombi istekleri önler
- ✅ Kaynakları anında serbest bırakır
- ✅ Server kararlılığını sağlar
- ✅ Production'da **zorunlu**

**CancellationToken kullanmamak:**
- ❌ Zombi istek birikimi
- ❌ Kaynak tükenmesi
- ❌ Server çökmesi riski
- ❌ Production'da **felaket**

**Tavsiye:**
- 🎯 **Her async method'a CancellationToken ekle**
- 🎯 ASP.NET Core otomatik sağlıyor - kullan!
- 🎯 Load test'te zombi birikimini kontrol et
- 🎯 Production'da timeout metriklerini izle

---

*Test tarihi: 18 Ocak 2026, 00:11 UTC*  
*Ortam: Docker on macOS, .NET 8.0*
