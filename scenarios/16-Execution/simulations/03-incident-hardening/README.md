# Simulation 03 - Production Incident Hardening

## Incident Brief

"Yeni mikroservisler uretimde; trafik artisinda timeout ve duplicate order goruluyor.
Hedef: sistem tasarimini bozmadan kalici hardening plani cikarmak."

## Teknik Beklentiler

- Sorunu siniflandir: network, db, queue, code path.
- Blast radius analizi yap.
- Kisa vadeli mitigation + uzun vadeli cozum ayir.
- Retry budget ve circuit breaker esiklerini belirle.
- SLO/SLI tabanli alarm kurali oner.

## Beklenen Calisma Sekli

1. `templates/brief.md` ile incident framing yap.
2. `templates/design.md` icinde hardening seceneklerini karsilastir.
3. `templates/test-plan.md` ile regression ve load testlerini yaz.
4. `templates/runbook.md` ile rollback ve recovery adimlarini netlestir.
5. `templates/status-update.md` ile paydas guncellemesi hazirla.

## Basari Kriterleri

- Kisa vadeli risk azaltimi acik.
- Kök neden ve kalici aksiyon ayrimi net.
- Tekrarlanabilir bir operasyon plani var.
- Sonraki incident icin runbook daha guclu hale geliyor.

