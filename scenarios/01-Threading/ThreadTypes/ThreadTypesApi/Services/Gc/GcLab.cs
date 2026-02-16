using System.Collections.Concurrent;

namespace ThreadTypesApi.Services.Gc;

public class GcLab
{
    // Holds references to prevent GC until cleared
    private readonly ConcurrentBag<FinalizableObject> _objects = new();
    private readonly ConcurrentBag<StandardObject> _standardObjects = new();

    public void Allocate(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _objects.Add(new FinalizableObject());
        }
    }

    public void AllocateStandard(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _standardObjects.Add(new StandardObject());
        }
    }

    public void Clear()
    {
        // Clearing the bag makes objects eligible for collection
        _objects.Clear();
        _standardObjects.Clear();
    }

    public string RunStandardScenario()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("=== GC Scenario: STANDARD ===");
        
        // 0. Reset & Baseline
        Clear();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        AppendStandardStats(sb, "1. BASLANGIC DURUMU");
        
        // 1. Allocate
        int count = 10000;
        AllocateStandard(count);
        AppendStandardStats(sb, $"2. {count} STANDARD OBJE YARATILDI (Yikici Metod YOK)");
        
        // 2. Clear References
        Clear();
        sb.AppendLine("\n>>> Referanslar Silindi (Objeler artik sahipsiz)");
        
        // 3. Trigger GC
        sb.AppendLine(">>> GC Tetikleniyor...");
        GC.Collect();
        sb.AppendLine(">>> GC tetiklendi. (Finalizer bekleme yok)");

        // 4. Final Stats
        AppendStandardStats(sb, "3. GC SONRASI DURUM (SONUC)");
        
        sb.AppendLine("\n---------------------------------------------------");
        sb.AppendLine("[YORUM] Bellek hemen dustu mu?");
        sb.AppendLine("       EVET -> Standart objeler aninda reclaim edildi.");
        sb.AppendLine("       Finalizer kuyrugu olmadigi icin bekleme yasanmadi.");

        return sb.ToString();
    }

    public string RunFinalizerScenario()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("=== GC Scenario: FINALIZER ===");
        
        // 0. Reset & Baseline
        Clear();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        AppendFinalizerStats(sb, "1. BASLANGIC DURUMU");
        
        // 1. Allocate
        int count = 10000;
        Allocate(count);
        AppendFinalizerStats(sb, $"2. {count} FINALIZABLE OBJE YARATILDI (Yikici Metod VAR)");
        
        // 2. Clear References
        Clear();
        sb.AppendLine("\n>>> Referanslar Silindi (Objeler artik sahipsiz)");
        
        // 3. Trigger GC
        sb.AppendLine(">>> GC Tetikleniyor...");
        GC.Collect();
        sb.AppendLine(">>> GC tetiklendi. Finalizer thread bekleniyor...");
        GC.WaitForPendingFinalizers(); 
        GC.Collect();

        // 4. Final Stats
        AppendFinalizerStats(sb, "3. GC SONRASI DURUM (SONUC)");
        
        sb.AppendLine("\n---------------------------------------------------");
        sb.AppendLine($"[YORUM] Finalized Count ARTTI MI? ({FinalizableObject.FinalizedCount} > 0)");
        sb.AppendLine("       EVET -> Finalizer Thread devreye girdi ve kuyrugu temizledi.");

        return sb.ToString();
    }

    private void AppendStandardStats(System.Text.StringBuilder sb, string stepName)
    {
        long mem = GC.GetTotalMemory(false);
        sb.AppendLine($"\n{stepName}");
        sb.AppendLine(new string('-', 40));
        sb.AppendLine($"Memory          : {mem / 1024.0:F2} KB");
        sb.AppendLine($"Alive (Std)     : {_standardObjects.Count}");
    }

    private void AppendFinalizerStats(System.Text.StringBuilder sb, string stepName)
    {
        long mem = GC.GetTotalMemory(false);
        sb.AppendLine($"\n{stepName}");
        sb.AppendLine(new string('-', 40));
        sb.AppendLine($"Memory          : {mem / 1024.0:F2} KB");
        sb.AppendLine($"Alive (Fin)     : {_objects.Count}");
        sb.AppendLine($"Finalized Count : {FinalizableObject.FinalizedCount}");
    }

    public string RunGenerationsScenario()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("=== GC Scenario: GENERATIONS ===");
        
        // 0. Reset
        Clear();
        GC.Collect();
        GC.Collect(); // Çift collect ile tertemiz bir sayfa aç
        
        sb.AppendLine("1. BASLANGIC");
        sb.AppendLine("----------------------------------------");
        sb.AppendLine("Yeni bir 'Survivor' (Hayatta Kalan) obje yaratiliyor...");
        
        // 1. Create Object (Born in Gen 0)
        var survivor = new StandardObject();
        // Bu objeyi listeye ekleyerek "Root" ediyoruz, yani GC'nin silmesini engelliyoruz.
        _standardObjects.Add(survivor);
        
        AppendGenerationStats(sb, "Objenin Dogumu (Gen 0)", survivor);

        // 2. Trigger GC 0 -> Promote to Gen 1
        sb.AppendLine("\n>>> GC.Collect(0) cagrildi (Sadece Gen 0 temizligi)...");
        GC.Collect(0);
        
        AppendGenerationStats(sb, "1. GC Sonrasi (Terfi: Gen 1)", survivor);
        
        // 3. Trigger GC 1 -> Promote to Gen 2
        sb.AppendLine("\n>>> GC.Collect(1) cagrildi (Gen 0 + Gen 1 temizligi)...");
        GC.Collect(1);
        
        AppendGenerationStats(sb, "2. GC Sonrasi (Terfi: Gen 2)", survivor);
        
        // 4. Trigger GC 2 (Full GC) -> Stay in Gen 2
        sb.AppendLine("\n>>> GC.Collect(2) cagrildi (Full GC)...");
        GC.Collect(2);
        
        AppendGenerationStats(sb, "Full GC Sonrasi (Hala Gen 2)", survivor);
        
        sb.AppendLine("\n---------------------------------------------------");
        sb.AppendLine("[ANALIZ]");
        sb.AppendLine("- Obje referansi tutuldugu surece her GC'den sag cikar.");
        sb.AppendLine("- Sag cikan obje bir us nesile (Gen 0 -> 1 -> 2) terfi eder.");
        sb.AppendLine("- Gen 2 son duraktir. Buradaki objeler 'Yasli' (Long-Lived) kabul edilir.");

        // Temizlik
        Clear(); 
        
        return sb.ToString();
    }

    public string RunSmallObjectFreeze()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("=== GC Scenario: FULL GC FREEZE (SMALL OBJECTS) ===");
        
        PrepareCleanState();

        var (monitorThread, pauseMonitor) = StartFreezeMonitor();
        
        sb.AppendLine("1. YUKLEME (ALLOCATION)" + Environment.NewLine + "----------------------------------------");
        sb.AppendLine(">>> 10.000.000 Kucuk obje yaratiliyor...(LinkedList mantigiyla birbirine bagli)");

        var list = new LinkedList<byte[]>();
        
        for(int i=0; i < 10000000; i++) list.AddLast(new byte[10]); 
        
        _keepAlive = list; // Rooting

        sb.AppendLine($">>> Allocation Bitti. Memory: {GC.GetTotalMemory(false)/1024/1024} MB");

        // Allocation sirasindaki max donmayi kaydet ve sifirla
        long allocationMaxPause = pauseMonitor.MaxPauseMs;
        pauseMonitor.ResetMaxPause();

        PromoteToGen2(sb);
        
        long gcDuration = TriggerFullGc(sb);
        
        pauseMonitor.Stop();
        monitorThread.Join();

        sb.AppendLine("\n4. SONUCLAR" + Environment.NewLine + "----------------------------------------");
        sb.AppendLine($"Allocation (Yukleme) Sirasinda Max Donma : {allocationMaxPause} ms (CPU Yuku)");
        sb.AppendLine($"GC Tetiklendiginde Max Donma             : {pauseMonitor.MaxPauseMs} ms (Stop-The-World)");
        sb.AppendLine($"GC.Collect() Suresi (Main Thread)        : {gcDuration} ms");
       
        sb.AppendLine("\n[ANALIZ]");
        sb.AppendLine("- Yukleme sirasinda islemci %100 olsa bile donma cok kucuktur (3-5ms).");
        sb.AppendLine("- Ancak GC calistiginda 'Stop-The-World' gerceklesir ve sure uzar.");
        
        PrepareCleanState();
        return sb.ToString();
    }

    // GC tarafindan taranmasi icin objeleri burada tutuyoruz.
    private object _keepAlive;

    public string RunLargeObjectFreeze()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("=== GC Scenario: FULL GC FREEZE (LARGE OBJECTS) ===");
        
        PrepareCleanState();
        var (monitorThread, pauseMonitor) = StartFreezeMonitor();
        
        sb.AppendLine("1. YUKLEME (ALLOCATION)" + Environment.NewLine + "----------------------------------------");
        sb.AppendLine(">>> 50 tane DEV (10MB) array yaratiliyor...");
        
        var bigList = new List<byte[]>();
        for(int i=0; i<50; i++) bigList.Add(new byte[10 * 1024 * 1024]); // 10 MB
        
        _keepAlive = bigList; // Rooting
        sb.AppendLine($">>> Allocation Bitti. Memory: {GC.GetTotalMemory(false)/1024/1024} MB");

        // Allocation sirasindaki max donmayi kaydet ve sifirla
        long allocationMaxPause = pauseMonitor.MaxPauseMs;
        pauseMonitor.ResetMaxPause();

        PromoteToGen2(sb);
        long gcDuration = TriggerFullGc(sb);
        
        pauseMonitor.Stop();
        monitorThread.Join();

        sb.AppendLine("\n4. SONUCLAR" + Environment.NewLine + "----------------------------------------");
        sb.AppendLine($"Allocation (Yukleme) Sirasinda Max Donma : {allocationMaxPause} ms (CPU Yuku)");
        sb.AppendLine($"GC Tetiklendiginde Max Donma             : {pauseMonitor.MaxPauseMs} ms (Stop-The-World)");
        sb.AppendLine($"GC.Collect() Suresi (Main Thread)        : {gcDuration} ms");
        
        sb.AppendLine("\n[ANALIZ]");
        sb.AppendLine("- Buyuk (LOH) objeler oldugu icin sayisal olarak azdilar.");
        sb.AppendLine("- GC grafigi hizli taradi, bu yuzden donma suresi 'Small Objects' senaryosuna gore COK DAHA KISA olmali.");
        
        PrepareCleanState();
        return sb.ToString();
    }

    private void PrepareCleanState()
    {
        _keepAlive = null; // Eski referansi birak
        Clear();
        GC.Collect();
        GC.Collect();
    }

    private void PromoteToGen2(System.Text.StringBuilder sb)
    {
        sb.AppendLine("\n2. YASLANDIRMA (PROMOTION -> GEN 2)");
        sb.AppendLine(">>> GC.Collect() x2 cagriliyor...");
        
        long memBefore = GC.GetTotalMemory(false)/1024/1024;
        
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        long memAfter = GC.GetTotalMemory(false)/1024/1024;
        
        sb.AppendLine($">>> Yaslandirma Bitti. Artik objeler Gen 2'de.");
        sb.AppendLine($"Memory (Gen 0+1 Temizligi Sonrasi): {memAfter} MB (Baslangic: {memBefore} MB)");
        
        if (memAfter > 50) sb.AppendLine("[YORUM] Bellek hala yuksek -> Nesneler olmedi, GEN 2'ye terfi etti.");
    }

    private long TriggerFullGc(System.Text.StringBuilder sb)
    {
        sb.AppendLine("\n3. FULL GC TETIKLEME (STOP-THE-WORLD)");
        sb.AppendLine(">>> Monitor Thread izlemede...");
        sb.AppendLine(">>> GC.Collect(2) cagriliyor (Default Mod - Concurrent Erisim Mumkun)...");
        
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        // Default davranisi gormek icin Force ediyoruz ama Compacting/Blocking zorlamiyoruz.
        // Runtime kendi karar versin (Muhtemelen Background GC yapacak).
        GC.Collect(2, GCCollectionMode.Forced); 
        stopwatch.Stop();
        
        return stopwatch.ElapsedMilliseconds;
    }

    private (Thread, FreezeMonitor) StartFreezeMonitor()
    {
        var monitor = new FreezeMonitor();
        var thread = new Thread(monitor.Run);
        thread.IsBackground = true;
        thread.Start();
        return (thread, monitor);
    }

    private class FreezeMonitor
    {
        public long MaxPauseMs { get; private set; }
        private volatile bool _running = true;

        public void Stop() => _running = false;
        public void ResetMaxPause() => MaxPauseMs = 0; // Reset

        public void Run()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            while (_running)
            {
                var before = sw.ElapsedMilliseconds;
                Thread.Sleep(1); 
                var after = sw.ElapsedMilliseconds;
                
                var delta = after - before;
                // Eger beklendiginden (1ms) cok uzun surerse (ornek > 3ms) kaydet
                if (delta > 3) 
                {
                    if (delta > MaxPauseMs) MaxPauseMs = delta;
                }
            }
        }
    }

    private void AppendGenerationStats(System.Text.StringBuilder sb, string stepName, object obj)
    {
        int gen = GC.GetGeneration(obj);
        sb.AppendLine($"\n{stepName}");
        sb.AppendLine(new string('-', 30));
        sb.AppendLine($"Generation      : {gen}");
        sb.AppendLine($"Is Alive?       : Yes");
        
        if (gen == 0) sb.AppendLine("Yorum           : Bebek obje. En kucuk sarsintida (GC) olebilir veya terfi edebilir.");
        if (gen == 1) sb.AppendLine("Yorum           : Genc obje. Gen 0 temizliginden kurtuldu.");
        if (gen == 2) sb.AppendLine("Yorum           : Yasli obje. Artik ona dokunmak (temizlemek) cok maliyetli (Full GC).");
    }
}
