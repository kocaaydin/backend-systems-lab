using System;
using System.Threading;
using Polly;
using Polly.CircuitBreaker;

class Program
{
    #region Shared State
    // Simulate a service that is currently down
    static bool _serviceIsHealthy = false;
    #endregion

    #region Main Execution
    static void Main(string[] args)
    {
        Console.WriteLine("=== Circuit Breaker Simulation ===");
        Console.WriteLine("Configuration: Break after 3 failures. Stay broken for 5 seconds.");

        var circuitBreakerPolicy = DefinePolicy();

        RunSimulation(circuitBreakerPolicy);
    }
    #endregion

    #region Policy Definition
    static CircuitBreakerPolicy DefinePolicy()
    {
        // Define a Circuit Breaker Policy: Break after 3 exceptions, keep circuit open for 5 seconds
        return Policy
            .Handle<Exception>()
            .CircuitBreaker(
                exceptionsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(5),
                onBreak: (ex, breakDelay) =>
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"!!! CIRCUIT STATE CHANGE: OPEN (Broken) !!!");
                    Console.WriteLine($"   Reason: {ex.Message}");
                    Console.WriteLine($"   Blocking all requests for {breakDelay.TotalSeconds} seconds.");
                    Console.ResetColor();
                },
                onReset: () =>
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"!!! CIRCUIT STATE CHANGE: CLOSED (Reset) !!!");
                    Console.WriteLine("   Services recovered. Requests allowed again.");
                    Console.ResetColor();
                },
                onHalfOpen: () =>
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("!!! CIRCUIT STATE CHANGE: HALF-OPEN !!!");
                    Console.WriteLine("   Testing service with next request...");
                    Console.ResetColor();
                }
            );
    }
    #endregion

    #region Simulation Logic
    static void RunSimulation(CircuitBreakerPolicy policy)
    {
        // Simulation Loop
        for (int i = 1; i <= 20; i++)
        {
            Console.Write($"Request #{i}: ");
            
            try
            {
                // Try to execute the action through the policy
                policy.Execute(() =>
                {
                    if (policy.CircuitState == CircuitState.Open)
                    {
                        // This technically won't be reached because Polly throws BrokenCircuitException before executing delegate
                        // but it's good for understanding.
                    }

                    CallUnstableService();
                    
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("SUCCESS.");
                    Console.ResetColor();
                });
            }
            catch (BrokenCircuitException)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("BLOCKED by Circuit Breaker. (Fast Failure - No actual call made)");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"FAILED. ({ex.Message})");
                Console.ResetColor();
            }

            // Sleep to visualize the timeline
            if (i == 10) 
            {
                Console.WriteLine("\n[System] Healing the service... Next requests should eventually succeed.\n");
                _serviceIsHealthy = true;
                // Wait enough for the 5s break to expire so we see Half-Open
                Thread.Sleep(6000); 
            }
            else
            {
                Thread.Sleep(500);
            }
        }
    }

    static void CallUnstableService()
    {
        if (!_serviceIsHealthy)
        {
            throw new Exception("500 Internal Server Error");
        }
        // If healthy, do nothing (success)
    }
    #endregion
}
