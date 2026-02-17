# Simulation 02 - Order Workflow at Scale

## Product Brief

"Siparis platformu buyudu. Gunde 1M siparis hedefi var.
Checkout -> payment -> inventory -> shipping akisi dagitik servisler uzerinden ilerleyecek.
Hedef: tutarli, izlenebilir ve yeniden denenebilir bir workflow."

## Teknik Beklentiler

- Saga/choreography vs orchestration karari.
- Mesajlasma modelini belirle (topic/queue, consumer group, retry queue).
- Idempotency key ve dedup stratejisi tanimla.
- Basarisiz adimlar icin compensation akisini yaz.
- P95/P99 gecikme hedeflerini koy.

## Beklenen Calisma Sekli

1. `templates/brief.md` ile problem ve hedefleri yaz.
2. `templates/design.md` icinde en az 2 workflow alternatifi cikar.
3. `templates/test-plan.md` ile failure senaryolari tasarla.
4. `templates/runbook.md` icinde incident triage adimlarini yaz.
5. `templates/status-update.md` ile asenkron iletisimi tamamla.

## Basari Kriterleri

- Veri kaybi olmadan akisin tamamlanmasi.
- En az bir compensation senaryosunun net tanimi.
- Retry storm olusmadan hata yonetimi.
- Operasyon ekibi icin izlenebilirlik adimlari net.

