# Communication Roadmap (Agile + Non-Agile)

Bu roadmap, teknik delivery yanina iletisim kasini sistematik olarak eklemek icin hazirlandi.

## Hedef

- Teknik kararlari daha hizli ve net alinabilir hale getirmek.
- Paydaslarla yanlis anlasilmayi azaltmak.
- Tek basina is yuruturken gorunurluk ve guven olusturmak.

## Track A - Agile Icindeki Iletisim

### Sprint Planning

- Her is kalemi icin `scope`, `non-goal`, `success criteria` yaz.
- Bagimliliklari acik listele.
- Cikti: kisa planning notu (5-8 satir).

### Daily Standup

- Sabit format kullan:
  `Done / Next / Blocker / Need decision`
- Sure: 60-90 saniye.
- Cikti: async takip icin yaziya dokulmus guncelleme.

### Refinement

- Is birimine su 5 soruyu sor:
  `Neden simdi?`, `Basari metrikleri?`, `Kisitlar?`, `Regulasyon etkisi?`, `Ne kapsam disi?`
- Cikti: net acceptance criteria.

### Sprint Review

- Sadece "ne yaptim" degil:
  `Ne etki urettik?`, `Hangi risk kapandi?`, `Sonraki karar ne?`
- Cikti: 1 sayfa review ozeti.

### Retro

- Iletisim kaynakli issue'lari ayri maddede topla.
- Her issue icin 1 aksiyon sahibi belirle.
- Cikti: sonraki sprint icin 1 iletisim deneyi.

## Track B - Agile Disindaki Iletisim

### Stakeholder Alignment

- Proje basinda stakeholder haritasi cikar:
  `urun`, `operasyon`, `guvenlik`, `finans`, `destek`.
- Her stakeholder icin "beklenti/risk/karar ihtiyaci" notu ekle.

### Decision Memo

- Teknik kararlari su formatla yaz:
  `Context / Options / Trade-off / Decision / Business impact`.
- Ozellikle belirsizlikte yazili karar izi birak.

### Incident Communication

- Olay aninda su formati kullan:
  `Impact / Scope / Current status / Next update time / Workaround`.
- Teknik detayi daha sonra, etkiyi once anlat.

### Executive Summary

- Teknik isi ust yonetime 6-8 satirda cevir:
  `Problem`, `Business impact`, `Plan`, `Risk`, `ETA`.
- Haftalik duzende gonder.

### Expectation Management

- "Commit" ve "Forecast" ayir.
- Kayma riski varsa erken bildir, alternatif oner.

## 8 Haftalik Uygulama Plani

1. Hafta 1-2: Daily + Planning formatlarini sabitle.
2. Hafta 3-4: Refinement soru seti ve review ozetini uygulamaya al.
3. Hafta 5-6: Decision memo ve stakeholder mapping'i standartlastir.
4. Hafta 7-8: Incident comms + executive summary pratigi yap.

## Olcum Kriterleri

- Yanlis anlasilan gereksinim sayisi azaliyor mu?
- Blocker'lar daha erken gorunur oluyor mu?
- Karar sureleri kisaliyor mu?
- Review toplantisinda tekrar soru sayisi azaliyor mu?
- Incident guncellemeleri daha az ping gerektiriyor mu?

## Minimum Template Set

- `daily-update.md`
- `decision-memo.md`
- `incident-update.md`
- `weekly-exec-summary.md`
- `stakeholder-map.md`

