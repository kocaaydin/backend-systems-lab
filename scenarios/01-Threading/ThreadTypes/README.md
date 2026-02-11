# ThreadTypes

Bu proje, thread turlerini ve davranis farklarini en basit gozlemle gormek icin hazirlandi.

## Calistirma

```bash
dotnet run --project scenarios/01-Threading/ThreadTypes/ThreadTypesApi/ThreadTypesApi.csproj
```

API adresi:

- `http://localhost:8091`

## Hangi Ornek Ne Gosterir?

1. Thread turu bilgisi
- `GET /thread-types/info`
- Main/ThreadPool ayrimini ve thread id bilgisini gorursun.

2. ThreadPool vs Dedicated CPU is
- `GET /thread-types/cpu-heavy-threadpool?n=200000`
- `GET /thread-types/cpu-heavy-dedicated?n=200000`
- Ayni CPU isinde hangi thread modelinin nasil davrandigini gorursun.

3. Starvation davranisi
- `GET /thread-types/starvation/blocking?blockMs=5000`
- Paralelde cok sayida cagri ile `/thread-types/fast` gecikmesini gozle.

4. Async I/O vs CPU
- `GET /thread-types/io-async?delayMs=400`
- `GET /thread-types/cpu-heavy-threadpool?n=200000`
- Bekleme (I/O) ve CPU isinin farkli etkisini gor.

5. Cancellation
- `GET /thread-types/cpu-cancellable?n=400000`
- Istegi client tarafindan iptal ederek 499 donusunu gozle.

6. Queue + Backpressure (Tek Endpoint)
- `POST /thread-types/queue/enqueue?items=20&capacity=5&workMs=300`
- Bounded channel doldugunda producer bekler.
- `backpressureDetected` ve `producerWaitMs` alanlari backpressure'i gosterir.

7. Finalizer / GC
- `POST /thread-types/finalizer/create?count=50000`
- `GET /thread-types/finalizer/stats`
- `POST /thread-types/finalizer/collect`
- Finalizer thread etkisini sayaclarla gor.

## Hızlı curl Ornekleri

```bash
curl -s http://localhost:8091/thread-types/info | jq
curl -s "http://localhost:8091/thread-types/cpu-heavy-threadpool?n=200000" | jq
curl -s "http://localhost:8091/thread-types/cpu-heavy-dedicated?n=200000" | jq
curl -s -X POST "http://localhost:8091/thread-types/queue/enqueue?items=20&capacity=5&workMs=250" | jq
```

## Demo Scriptleri

- Tek seferde temel tur:
  - `bash scenarios/01-Threading/ThreadTypes/scripts/quick_overview.sh`
- Ayrı çalıştırman gerekenler:
  - `bash scenarios/01-Threading/ThreadTypes/scripts/starvation_observation.sh`
  - `bash scenarios/01-Threading/ThreadTypes/scripts/cancellation_observation.sh`
  - `bash scenarios/01-Threading/ThreadTypes/scripts/finalizer_observation.sh`

## Gozlem Ipuclari

- Console loglarini izle:
  - `START endpoint=...`
  - `END endpoint=... elapsedMs=... tid=... pool=...`
- Ayni endpointi ardarda calistirip `tid` ve `pool` alanlarini karsilastir.
