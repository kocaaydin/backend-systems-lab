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
