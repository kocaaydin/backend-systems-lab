# 16 - Execution Simulation Lab

## Hedef

Teknik konuyu bilmenin otesinde su becerileri pratik etmek:

- Problem framing
- Task breakdown
- Design writing
- Debugging discipline
- Testing strategy
- Production readiness
- Delivery hygiene
- Asenkron iletisim

## Simulasyon Tipleri

- Incident odakli: mevcut serviste sorun bulma ve kalici iyilestirme.
- Greenfield odakli: sifirdan servis/mikroservis tasarlama ve teslim etme.

## Kullanim

1. `simulations/` altindan bir senaryo sec.
2. `templates/` altindaki 5 dosyayi kopyalayip senaryo klasorune koy.
3. Her dosyayi doldurup senaryoyu bastan sona tamamla.
4. Bitince kendine mini retrospektif yaz:
- Nerede takildin?
- Hangi karar gec verildi?
- Sonraki turda neyi kisaltacaksin?

## Zorunlu Cikti Seti

- `brief.md`
- `design.md`
- `test-plan.md`
- `runbook.md`
- `status-update.md`

## Degerlendirme Rubrigi (Kisa)

- Acik kapsam: net mi?
- Risk odagi: erken ele alinmis mi?
- Karar kalitesi: trade-off yazilmis mi?
- Kanit: test + olcum var mi?
- Operasyon: rollback/alert plani var mi?

## Onerilen Siralama

1. `simulations/01-greenfield-platform`
2. `simulations/02-order-workflow`
3. `simulations/03-incident-hardening`

## Ek Roadmap

- Iletisim odakli plan: `communication_roadmap.md`
