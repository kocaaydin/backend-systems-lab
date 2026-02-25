internal static class ConsumerLossWorker
{
    private static readonly object FileLock = new();

    public static async Task RunAsync(string[] args)
    {
        var options = ParseArgs(args);

        Directory.CreateDirectory(options.ResultsDir);
        string runId = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        string resultPath = Path.Combine(options.ResultsDir, $"consumer-loss-{runId}-{options.Scheduler}.txt");

        if (File.Exists(resultPath))
        {
            File.Delete(resultPath);
        }

        ExecuteAsync(options, resultPath);
        await Task.Delay(options.LingerMs);

        WriteSummaryAtTop(resultPath, options.Iterations, options.Scheduler);

        Console.WriteLine($"result_file={resultPath}");
    }

    private static void ExecuteAsync(ConsumerLossOptions options, string resultPath)
    {
        for (int i = 1; i <= options.Iterations; i++)
        {
            KafkaEventListener_MessageReceived(i, options, resultPath);
        }
    }

    private static void KafkaEventListener_MessageReceived(
        int id,
        ConsumerLossOptions options,
        string resultPath)
    {
        Action work = () =>
        {
            try
            {
                // Iteration logu burada yazilir (yalnizca id).
                AppendId(resultPath, id);
                Thread.Sleep(options.WorkMs);
            }
            catch
            {
                // Bu senaryoda sadece id kaydi hedefleniyor.
            }
        };

        if (options.Scheduler == "longrunning")
        {
            Task.Factory.StartNew(work, TaskCreationOptions.LongRunning);
            return;
        }

        Task.Run(work);
    }

    private static void WriteSummaryAtTop(string resultPath, int expectedIterations, string scheduler)
    {
        var lines = File.Exists(resultPath) ? File.ReadAllLines(resultPath) : Array.Empty<string>();
        var loggedIds = new HashSet<int>();

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.StartsWith("summary_", StringComparison.Ordinal) || line.StartsWith("scheduler=", StringComparison.Ordinal))
            {
                continue;
            }

            if (int.TryParse(line.Trim(), out int id))
            {
                loggedIds.Add(id);
            }
        }

        int[] missing = Enumerable.Range(1, expectedIterations)
            .Where(i => !loggedIds.Contains(i))
            .ToArray();
        string missingSample = missing.Length == 0 ? "-" : string.Join(",", missing.Take(50));
        string summary =
            $"summary_expected_total={expectedIterations}\n" +
            $"summary_logged_total={loggedIds.Count}\n" +
            $"summary_missing_count={missing.Length}\n" +
            $"summary_missing_sample={missingSample}\n" +
            $"scheduler={scheduler}\n" +
            "\n";

        File.WriteAllText(resultPath, summary + string.Join(Environment.NewLine, lines) + Environment.NewLine);
        Console.WriteLine(summary.TrimEnd());
    }

    private static void AppendId(string resultPath, int id)
    {
        string line = id + Environment.NewLine;
        lock (FileLock)
        {
            File.AppendAllText(resultPath, line);
        }
    }

    private static ConsumerLossOptions ParseArgs(string[] args)
    {
        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < args.Length; i++)
        {
            if (!args[i].StartsWith("--", StringComparison.Ordinal))
            {
                continue;
            }

            string key = args[i][2..];
            string value = i + 1 < args.Length && !args[i + 1].StartsWith("--", StringComparison.Ordinal)
                ? args[++i]
                : "true";
            dict[key] = value;
        }

        return new ConsumerLossOptions(
            Scheduler: dict.TryGetValue("scheduler", out var scheduler) ? scheduler.ToLowerInvariant() : "taskrun",
            Iterations: dict.TryGetValue("iterations", out var iterations) && int.TryParse(iterations, out var it) ? it : 1000,
            WorkMs: dict.TryGetValue("work-ms", out var workMs) && int.TryParse(workMs, out var wm) ? wm : 200,
            LingerMs: dict.TryGetValue("linger-ms", out var lingerMs) && int.TryParse(lingerMs, out var lm) ? lm : 1000,
            ResultsDir: dict.TryGetValue("results-dir", out var resultsDir)
                ? Path.GetFullPath(resultsDir)
                : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "results")));
    }

    private sealed record ConsumerLossOptions(string Scheduler, int Iterations, int WorkMs, int LingerMs, string ResultsDir);
}
