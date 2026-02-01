using System;
using System.Threading;
using Polly;
using Polly.Retry;

class Program
{
    #region Shared State
    static int _attempts = 0;
    static Random _random = new Random();
    #endregion

    #region Main Execution
    static void Main(string[] args)
    {
        Console.WriteLine("=== Transient Failure & Retry with Jitter Simulation ===");
        Console.WriteLine("Scenario: Service is overloaded and succeeds only occasionally.");
        Console.WriteLine("Strategy: Exponential Backoff + Jitter to avoid compounding load.\n");

        var retryPolicy = DefinePolicy();

        RunSimulation(retryPolicy);
    }
    #endregion

    #region Policy Definition
    static RetryPolicy DefinePolicy()
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount: 5,
                sleepDurationProvider: retryAttempt => 
                {
                    // Exponential Backoff: 2^1, 2^2, 2^3...
                    var baseDelay = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                    
                    // Jitter: Add random milliseconds to avoid "Retry Storms" (synchronized retries)
                    var jitter = TimeSpan.FromMilliseconds(_random.Next(0, 1000));
                    
                    var totalDelay = baseDelay + jitter;
                    
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"   -> Retry #{retryAttempt} scheduled in {totalDelay.TotalSeconds:F2}s (Backoff {baseDelay.TotalSeconds}s + Jitter {jitter.TotalMilliseconds}ms)");
                    Console.ResetColor();

                    return totalDelay;
                },
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"   [Log] Failure detected: {exception.Message}. Waiting...");
                }
            );
    }
    #endregion

    #region Simulation Logic
    static void RunSimulation(RetryPolicy retryPolicy)
    {
        try
        {
            retryPolicy.Execute(() =>
            {
                CallFlakyService();
            });

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[Success] Operation completed successfully.");
            Console.ResetColor();
        }
        catch (Exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[Failure] All retries exhausted. Operation failed.");
            Console.ResetColor();
        }
    }

    static void CallFlakyService()
    {
        _attempts++;
        Console.Write($"Attempt #{_attempts}: Calling Service... ");

        // Succeed only on the 4th attempt
        if (_attempts < 4)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FAIL (503 Service Unavailable)");
            Console.ResetColor();
            throw new Exception("503 Service Unavailable");
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("SUCCESS (200 OK)");
        Console.ResetColor();
    }
    #endregion
}
