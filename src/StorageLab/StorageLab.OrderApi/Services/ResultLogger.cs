using System.Text.Json;

namespace StorageLab.OrderApi.Services;

public static class ResultLogger
{
    private static readonly string ResultFilePath = "/app/results/storage_lab_results.json";

    public static async Task LogResultAsync(string scenario, object metrics, string observations)
    {
        var result = new
        {
            timestamp = DateTime.UtcNow.ToString("o"),
            scenario = scenario,
            metrics = metrics,
            observations = observations
        };

        var json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        
        // Ensure directory exists
        var dir = Path.GetDirectoryName(ResultFilePath);
        if (dir != null && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

        // Append to file (in a real scenario we might want a list, but appending lines for simplicity or reading/writing list)
        // For Valid JSON Array, we need to read, add, write. 
        // Let's stick to appending NEWLINE delimited JSON (NDJSON) or just appending to a text file for now to be safe with locking.
        // Actually, user wants "json olarak dosya", let's make it a valid JSON array.
        
        List<object> results = new();
        if (File.Exists(ResultFilePath))
        {
            try 
            {
                var content = await File.ReadAllTextAsync(ResultFilePath);
                if (!string.IsNullOrWhiteSpace(content))
                    results = JsonSerializer.Deserialize<List<object>>(content) ?? new();
            }
            catch { /* File might be corrupted or empty, start fresh */ }
        }

        results.Add(result);
        await File.WriteAllTextAsync(ResultFilePath, JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true }));
    }
}
