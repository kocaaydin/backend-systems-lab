# Storage Scenarios Roadmap

## Mülakat Odaklı Minimal Çerçeve
- Durability: backup/restore ve RPO/RTO farkını anlat.
- Consistency: güçlü tutarlılık vs eventual consistency trade-off.
- Performance: IO-bound darboğazları nasıl okuyacağını belirt.
- Lifecycle: retention/archive/cleanup stratejisini açıkla.
- Metrik: p95 storage latency, throughput, error rate, recovery süresi.

Bu kategori, `StorageLab` örneklerinin senaryo odaklı konumudur.

## İçerik

- `k6/StorageLab/load-test.js`  
  Storage API için yük testi örneği.

- `results/StorageLab/`  
  Storage ile ilgili örnek çıktı dosyaları.

## Senaryo Seti (Önerilen)
1. Yük altında storage latency artışı (IO saturation)
2. Backup/restore tatbikatı ve recovery süresi
3. Retention/archival stratejisinin maliyet/performans etkisi
4. Tutarlılık seviyesi değişiminde okuma-yazma davranışı

## Kullanım

1. İlgili Storage servisini ayağa kaldır.
2. `k6/StorageLab/load-test.js` ile testi çalıştır.
3. Sonuçları `results/StorageLab/` altında incele.
