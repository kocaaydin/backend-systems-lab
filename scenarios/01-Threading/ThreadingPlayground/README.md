# Threading Playground

Bu senaryo, `Task.Run` ile `TaskCreationOptions.LongRunning` farkini
bloklayan is yukunde karsilastirmak icin hazirlandi.

## Calistirma

```bash
dotnet run --project /Users/aydin/Desktop/Projects/backend-systems-lab/scenarios/01-Threading/ThreadingPlayground/ThreadingPlayground.csproj -- longrunning
```

Mode parametresi `longrunning` olmadiginda uygulama sadece kullanim bilgisini yazar.

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
