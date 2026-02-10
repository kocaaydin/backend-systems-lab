# CPU Bound Lab

Bu lab, ayni endpoint uzerinde `n` degerini buyutup CPU maliyeti arttiginda latency'nin nasil degistigini gosterir.

Kisa fikir:
- `n` kucukken islem hafif -> cevaplar daha hizli
- `n` buyukken CPU daha uzun mesgul -> cevaplar daha yavas

Bu sayede "CPU-bound is yukunde neden latency artar?" sorusunu net goruruz.

## Neyi Test Ediyoruz?

- Endpoint: `/experiments/cpu?n=<N>`
- Islem: brute-force prime counting
- Karsilastirma: `N=20000` vs `N=200000`
- K6 yuk profili: sabit `RPS=20`, `duration=30s`

Neden sabit RPS?
- Cunku ayni trafik seviyesinde sadece is maliyetini (`n`) degistirmek istiyoruz.
- Boylece farkin kaynagi daha net olur.

## Calistirma

Varsayilan karsilastirma:

```bash
bash scenarios/01-Threading/CpuBound/scripts/run.sh
```

Tek bir `N` degeri:

```bash
bash scenarios/01-Threading/CpuBound/scripts/run.sh 20000
```

Birden fazla `N` degeri:

```bash
bash scenarios/01-Threading/CpuBound/scripts/run.sh 20000 200000
```

Ortam degiskenleri (opsiyonel):

```bash
REPEAT_COUNT=5 DURATION=45s RPS_VALUE=25 bash scenarios/01-Threading/CpuBound/scripts/run.sh 20000 200000
```

## Scriptler

- `scenarios/01-Threading/CpuBound/scripts/run.sh`
- `scenarios/01-Threading/CpuBound/scripts/run_n.sh`
- `scenarios/01-Threading/CpuBound/scripts/run_n_20000.sh`
- `scenarios/01-Threading/CpuBound/scripts/run_n_200000.sh`
- `scenarios/01-Threading/CpuBound/scripts/run_n_compare.sh`

Script davranisi:
- Her `N` senaryosu `REPEAT_COUNT` kadar tekrar edilir (varsayilan 3).
- Her tekrar oncesi API yeniden baslatilir.
- API `/health` hazir olana kadar beklenir.
- Sonra k6 kosar ve summary dosyasi yazar.

## Sonuc Formati

Her `N` icin:

- 3 tekrar summary:
  - `scenarios/01-Threading/CpuBound/results/k6-n-<N>-run-1-summary.json`
  - `scenarios/01-Threading/CpuBound/results/k6-n-<N>-run-2-summary.json`
  - `scenarios/01-Threading/CpuBound/results/k6-n-<N>-run-3-summary.json`
- 1 ortalama dosyasi:
  - `scenarios/01-Threading/CpuBound/results/k6-n-<N>-average.json`

`average.json` alanlari:

- `avg_of_avg_ms`: 3 kosunun ortalama latency ortalamasi (ms)
- `avg_of_p95_ms`: 3 kosunun p95 latency ortalamasi (ms)
- `median_p95_ms`: 3 kosudaki p95 degerlerinin ortancasi (outlier'a daha dayanikli)
- `avg_fail_rate`: `http_req_failed` ortalamasi
- `avg_http_reqs_rate`: gerceklesen istek hizi
- `dropped_iterations`: hedef yukun yetismeyen istek sayilari (her kosu icin)

## Yorumlama

- Daha buyuk `N` -> CPU islemi daha pahali -> `avg_of_avg_ms` ve `avg_of_p95_ms` genelde yukselir.
- Karsilastirma yaparken su kontrolleri ekle:
  - `dropped_iterations` (mümkünse 0 olmali)
  - `avg_http_reqs_rate` (hedef RPS'e yakin olmali)

Pratik yorum sirasi:
1. `dropped_iterations` kontrol et. Yuk dusuyorsa metrik yorumu zayiflar.
2. `avg_http_reqs_rate` hedefe yakin mi bak.
3. Sonra `avg_of_p95_ms` ve `median_p95_ms` karsilastir.
4. En son `avg_fail_rate` ile hataya dusmus mu kontrol et.

Neden tekrar + restart yapiyoruz?
- Tek kosu gürültülü olabilir (scheduler jitter, anlik dalgalanma).
- Restart ile onceki kosudan kalan uygulama state etkisini azaltiriz.
- 3 tekrar ortalamasi daha stabil sonuc verir.

Not:
- Connection reuse (keep-alive), JIT, cache, pool gibi etkiler performansi degistirebilir.
- Bu nedenle tek kosu yerine tekrarli olcum tercih edilir.

## Hizli Kontrol

```bash
ls -1 scenarios/01-Threading/CpuBound/results
```

```bash
cat scenarios/01-Threading/CpuBound/results/k6-n-20000-average.json
cat scenarios/01-Threading/CpuBound/results/k6-n-200000-average.json
```
