# Threading Playground

Bu senaryo, `Task.Run` ile `TaskCreationOptions.LongRunning` farkini
bloklayan is yukunde karsilastirmak icin hazirlandi.

## Calistirma

```bash
dotnet run --project /Users/aydin/Desktop/Projects/backend-systems-lab/scenarios/01-Threading/ThreadingPlayground/ThreadingPlayground.csproj -- longrunning
```

Mode parametresi `longrunning` olmadiginda uygulama sadece kullanim bilgisini yazar.

## Consumer Loss Senaryosu

`consumer-loss` modu, fire-and-forget task akisinda id degerlerini tek bir
`txt` dosyasina alt alta yazar ve dosyanin en ustune eksik iteration ozetini ekler.

Calistirma (tek scheduler):

```bash
dotnet run --project /Users/aydin/Desktop/Projects/backend-systems-lab/scenarios/01-Threading/ThreadingPlayground/ThreadingPlayground.csproj -- consumer-loss --scheduler taskrun --iterations 10000 --work-ms 250 --linger-ms 1500
```

Iki scheduler'i birden calistirma:

```bash
bash /Users/aydin/Desktop/Projects/backend-systems-lab/scenarios/01-Threading/ThreadingPlayground/run_consumer_loss_test.sh 10000 250 1500
```

Docker ile calistirma:

```bash
bash /Users/aydin/Desktop/Projects/backend-systems-lab/scenarios/01-Threading/ThreadingPlayground/run_consumer_loss_test_docker.sh 10000 250 1500
```

Uretilen ciktilar:
- `results/consumer-loss-<timestamp>-taskrun.txt`
- `results/consumer-loss-<timestamp>-longrunning.txt`

## Beklenen Cikti (Ornek)

```text
Task.Run vs LongRunning (Dongulu Ornek)

Toplam is: 200, her is: Thread.Sleep(200)
Not: Bu test bloklayan isi simule eder.

Sonuc:
Task.Run     -> ThreadPool: 200, Dedicated: 0, Toplam Sure: 3xxx ms
LongRunning  -> ThreadPool: 0, Dedicated: 200, Toplam Sure: 2xx ms
```

Not: Sure degerleri makine yukune gore degisebilir.
