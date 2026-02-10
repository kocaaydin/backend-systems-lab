# Storage Consistency Senaryolari

Bu klasor, `storage_consistency_roadmap.md` icindeki eksik kalan consistency orneklerini calistirilabilir hale getirir.

## Dosyalar
- `setup_storage_consistency.sql`: Lab tablolarini olusturur.
- `replica_lag_stale_read_demo.sql`: Replica lag / stale read simulasyonu.
- `write_skew_lost_update_demo.sql`: Lost update ve write skew simulasyonlari.
- `run_storage_consistency.sh`: Tum SQL akisini sirayla calistirir.

## Calistirma
```bash
cd scenarios/02-Database/StorageConsistency
./run_storage_consistency.sh
```

## Beklenen Cikti
- `replica_lag_stale_read_demo.sql`:
  - Sync oncesi `MAIN` ve `REPLICA_SHADOW` farkli stock gosterir.
  - Sync sonrasi degerler eslenir.
- `write_skew_lost_update_demo.sql`:
  - Lost update bolumunde beklenen stok kaymasi gorulur.
  - RowVersion bolumunde ikinci update 0 row etkiler.
  - Write skew bolumunde iki doktor da nobetten cikabilir (kural ihlali).
