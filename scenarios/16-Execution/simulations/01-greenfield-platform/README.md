# Simulation 01 - Greenfield Platform

## Product Brief

"Yeni bir B2B siparis platformu sifirdan kurulacak.
Ilk fazda: musteri, urun, siparis ve odeme akislarini calistir.
Sistem mikroservis mimarisiyle tasarlanacak ve yatay buyumeye uygun olacak."

## Teknik Beklentiler

- Servis sinirlarini belirle (bounded context).
- Sync vs async iletisim kararlarini ver.
- Veri sahipligini servis bazinda ayir.
- Event akisini tasarla (en az 3 domain event).
- Retry/timeout/idempotency standartlarini tanimla.
- Go-live icin minimum gozlemlenebilirlik setini cikar.

## Beklenen Calisma Sekli

1. `templates/brief.md` ile problemi cercevele.
2. Isi 1-2 gunluk tasklara bol.
3. `templates/design.md` ile en az 2 alternatif cikar.
4. `templates/test-plan.md` ile riskleri teste bagla.
5. `templates/runbook.md` ile deployment + rollback planini yaz.
6. `templates/status-update.md` ile gun sonu raporu yaz.

## Basari Kriterleri

- Kapsam ve non-goals net.
- Mikroservis sinirlari ve veri sahipligi tutarli.
- Karar teknik ve operasyonel trade-off ile savunulmus.
- Test plani riskleri kapatiyor.
- Rollback ve alert adimlari acik.
