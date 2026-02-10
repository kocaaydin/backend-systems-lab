# Security & API Roadmap

## 🎯 Amaç
Güvenlik ve API tasarım kararlarının sadece "teori" değil, sistem davranışı ve risk seviyesi üzerinde nasıl etkiler ürettiğini göstermek.

Hedef:
- Authentication/Authorization hatalarını ayırabilmek,
- CORS/TLS yanlışlarının gerçek saldırı yüzeyine nasıl dönüştüğünü görmek,
- OWASP odaklı temel test refleksini kazanmak.

## 🧩 Kavramlar (Nedir?)
- **Authentication (AuthN):** "Kimsin?" sorusunun cevabı (token, login, kimlik doğrulama).
- **Authorization (AuthZ):** "Neye yetkin var?" sorusunun cevabı (rol/policy).
- **CORS:** Tarayıcının cross-origin istekleri için uyguladığı güvenlik politikası.
- **TLS:** Client-server trafiğini şifreleyen protokol; sertifika zinciriyle kimlik doğrulama sağlar.
- **OWASP Top 10:** En yaygın web uygulama güvenlik risklerinin referans listesi.

## 🧪 Senaryolar

### 1. AuthN vs AuthZ Ayrımı
- **Case A (Bad):** Token var diye tüm endpoint'lere erişim.
- **Case B (Good):** role/policy bazlı authorization.
- **Beklenen fark:** Yetkisiz erişim engellenir, audit izi netleşir.

### 2. CORS Misconfiguration
- **Case A (Bad):** `AllowAnyOrigin + AllowCredentials`.
- **Case B (Good):** explicit origin listesi ve method/header kontrolü.
- **Beklenen fark:** Cross-origin risk yüzeyi daralır.
- **Yorumlama ipucu:** `AllowAnyOrigin` + `Credentials` birlikte kullanımı tarayıcı tabanlı riskleri büyütür.

### 3. TLS / Sertifika Hataları
- **Case A (Bad):** self-signed/expired cert ile üretim benzeri kullanım.
- **Case B (Good):** doğru sertifika zinciri ve TLS policy.
- **Beklenen fark:** MITM riski azalır, client handshake hataları düşer.

### 4. OWASP Top 10 Mini Kontrol Seti
- Broken access control
- Injection
- Security misconfiguration
- Sensitive data exposure

## 📊 Ölçülecek Metrikler
- Unauthorized/forbidden oranı
- Security event log sayısı
- Failed TLS handshake sayısı
- Endpoint bazlı risk seviyesi

## 🧪 Kod Karşılaştırması
**Kötü Kullanım (CORS):**
```csharp
AllowAnyOrigin().AllowCredentials()
```

**İyi Kullanım (Explicit Origin):**
```csharp
WithOrigins("https://app.company.com").AllowCredentials()
```

## 🛠️ Nasıl Çalıştırılır?
```bash
cd scenarios/14-Security
./run.sh
```

Çalışan dosyalar:
- `scenarios/14-Security/run.sh`
- `scenarios/14-Security/scripts/01_authn_authz_examples.sh`
- `scenarios/14-Security/scripts/02_cors_examples.sh`
- `scenarios/14-Security/scripts/03_tls_check.sh`
- `scenarios/14-Security/k6/SecurityLab/auth_load.js`

## 🧭 Notlar
- Authentication kullanıcıyı doğrular, authorization yetkiyi belirler.
- Güvenlik testleri CI aşamasında otomatikleştirilmelidir.
- CORS bir güvenlik katmanı değil, tarayıcı politika mekanizmasıdır.
