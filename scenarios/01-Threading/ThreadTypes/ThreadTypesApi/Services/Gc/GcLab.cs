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
}
