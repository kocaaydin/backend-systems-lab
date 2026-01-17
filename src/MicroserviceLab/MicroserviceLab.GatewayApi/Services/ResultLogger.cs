using System.Text.Json;

namespace MicroserviceLab.GatewayApi.Services;

public static class ResultLogger
{
    private static readonly string ResultFilePath = "/app/results/microservice_lab_results.json";

    public static async Task LogResultAsync(string scenario, object metrics, string observations)
    {
        var result = new
        {
            Timestamp = DateTime.UtcNow,
            Scenario = scenario,
            Metrics = metrics,
            Observations = observations
        };

        var directory = Path.GetDirectoryName(ResultFilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        List<object> results;
        if (File.Exists(ResultFilePath))
        {
            var existingJson = await File.ReadAllTextAsync(ResultFilePath);
            results = JsonSerializer.Deserialize<List<object>>(existingJson) ?? new List<object>();
        }
        else
        {
            results = new List<object>();
        }

        results.Add(result);

        var json = JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(ResultFilePath, json);
    }
}
