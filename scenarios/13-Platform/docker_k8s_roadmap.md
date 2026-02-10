# Docker & Kubernetes Roadmap

## 🎯 Amaç
Container ve orchestration kararlarının (Dockerfile, health checks, resource limits, deploy stratejileri) üretim davranışını nasıl değiştirdiğini görmek.

Hedef:
- "Container çalışıyor ama servis neden sağlıksız?" sorusunu cevaplayabilmek,
- "Neden rollout sonrası trafik bozuldu?" gibi platform kaynaklı sorunları ayıklayabilmek.

## 🧩 Kavramlar (Nedir?)
- **Probe:** Kubernetes'in pod'un sağlığını anlamak için yaptığı kontrol çağrısıdır.
- **Liveness probe:** "Uygulama kill/restart edilmeli mi?" sorusunu cevaplar.
- **Readiness probe:** "Bu pod trafiğe hazır mı?" sorusunu cevaplar. Ready değilse servis trafiği göndermez.
- **Startup probe:** Geç açılan uygulamalarda başlangıç süresince liveness/readiness'i geciktirir; erken restart'ı engeller.
- **Resource request:** Scheduler'a "minimum ihtiyaç" bilgisidir.
- **Resource limit:** Container'ın kullanabileceği üst sınırdır; aşılırsa throttling/OOM görülebilir.
- **Rolling update:** Pod'ları adım adım yeni sürüme geçirir.

## 🧪 Senaryolar

### 1. Dockerfile Anti-Pattern
- **Case A (Bad):** Büyük image, root kullanıcı, tek stage build.
- **Case B (Good):** multi-stage, minimal base image, non-root user.
- **Beklenen fark:** Build/deploy süresi ve güvenlik riski iyileşir.

### 2. Readiness / Liveness / Startup Probe
- **Case A (Bad):** Probe yok veya yanlış endpoint.
- **Case B (Good):** uygulama yaşam döngüsüne uygun probe seti.
- **Beklenen fark:** false restart azalır, rollout daha stabil olur.
- **Yorumlama ipucu:** Pod `Running` olabilir ama `Ready` olmayabilir. Trafik alamıyorsa önce readiness'e bak.

### 3. Resource Limit & Throttling
- **Case A (Bad):** limitsiz pod/containers.
- **Case B (Good):** doğru CPU/memory request-limit.
- **Beklenen fark:** node komşu etkisi azalır, tahmin edilebilir performans artar.

### 4. Deployment Strategy
- **Case A (Bad):** uygunsuz rolling update parametreleri.
- **Case B (Good):** kontrollü rollout + rollback planı.
- **Beklenen fark:** partial failure etkisi sınırlanır.

## 📊 Ölçülecek Metrikler
- Restart count
- OOMKilled / throttling oranı
- Deployment success duration
- Error rate during rollout

## 🧪 Kod Karşılaştırması
**Kötü Kullanım (Probe hatalı):**
```yaml
livenessProbe:
  httpGet:
    path: /not-exists
```

**İyi Kullanım (Readiness + Liveness + Startup):**
```yaml
readinessProbe: ...
livenessProbe: ...
startupProbe: ...
```

## 🛠️ Nasıl Çalıştırılır?
```bash
cd scenarios/13-Platform
./run.sh
```

Çalışan dosyalar:
- `scenarios/13-Platform/run.sh`
- `scenarios/13-Platform/manifests/bad-deployment.yaml`
- `scenarios/13-Platform/manifests/good-deployment.yaml`
- `scenarios/13-Platform/scripts/compare_manifests.sh`
- `scenarios/13-Platform/k6/PlatformLab/health_check_load.js`

## 🧭 Notlar
- Config image içinde değil, runtime objelerinde tutulmalı.
- Tek servis/az trafik için Kubernetes her zaman şart değildir.
- Health check tasarımı, kod kalitesi kadar önemlidir.
