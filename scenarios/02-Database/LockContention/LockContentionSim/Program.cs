using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    #region Configuration
    private const string ConnectionString = "Server=localhost,1433;Database=master;User Id=sa;Password=VeryStrongPassword123!;TrustServerCertificate=True;";
    #endregion

    #region Main Execution
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Lock Contention Simulation ===");
        
        try
        {
            SetupDatabase();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting up DB: {ex.Message}");
            return;
        }

        Console.WriteLine("Scenario: Thread A holds a lock for 5 seconds. Thread B tries to read.");
        Console.WriteLine("Expectation: Thread B should be blocked for at least 5 seconds.");
        Console.WriteLine("-----------------------------------------------------");

        var task1 = Task.Run(RunBlockingTransaction);
        
        // Give Thread A a head start to ensure it acquires the lock
        Thread.Sleep(1000); 
        
        var task2 = Task.Run(RunBlockedTransaction);

        await Task.WhenAll(task1, task2);
        
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine("Simulation Complete.");
    }
    #endregion

    #region Database Setup
    static void SetupDatabase()
    {
        using (var conn = new SqlConnection(ConnectionString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'LockLab')
                    CREATE DATABASE LockLab;
            ";
            cmd.ExecuteNonQuery();
        }

        var labConnectionString = ConnectionString.Replace("master", "LockLab");
        using (var conn = new SqlConnection(labConnectionString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                IF OBJECT_ID('SharedResource', 'U') IS NOT NULL DROP TABLE SharedResource;
                CREATE TABLE SharedResource (Id INT PRIMARY KEY, Data NVARCHAR(50));
                INSERT INTO SharedResource VALUES (1, 'Original Data');
            ";
            cmd.ExecuteNonQuery();
        }
    }
    #endregion

    #region Transaction Logic
    static void RunBlockingTransaction()
    {
        var labConnectionString = ConnectionString.Replace("master", "LockLab");
        using (var conn = new SqlConnection(labConnectionString))
        {
            conn.Open();
            using (var trans = conn.BeginTransaction())
            {
                Console.WriteLine("[Thread A] Started Transaction. locking row ID=1...");
                var cmd = conn.CreateCommand();
                cmd.Transaction = trans;
                cmd.CommandText = "UPDATE SharedResource SET Data = 'Updated Data' WHERE Id = 1";
                cmd.ExecuteNonQuery();
                
                Console.WriteLine("[Thread A] Row ID=1 Locked. Sleeping for 5 seconds (holding lock)...");
                Thread.Sleep(5000);
                
                trans.Commit();
                Console.WriteLine("[Thread A] Transaction Committed. Lock released.");
            }
        }
    }

    static void RunBlockedTransaction()
    {
        var labConnectionString = ConnectionString.Replace("master", "LockLab");
        using (var conn = new SqlConnection(labConnectionString))
        {
            conn.Open();
            Console.WriteLine("[Thread B] Attempting to read row ID=1...");
            var stopwatch = Stopwatch.StartNew();
            
            var cmd = conn.CreateCommand();
            // Default Isolation Level is ReadCommitted, so it will wait for the exclusive lock to be released
            cmd.CommandText = "SELECT Data FROM SharedResource WHERE Id = 1";
            var result = cmd.ExecuteScalar();
            
            stopwatch.Stop();
            Console.WriteLine($"[Thread B] Read finished. Value: '{result}'");
            
            if (stopwatch.ElapsedMilliseconds > 4000)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            
            Console.WriteLine($"[Thread B] Duration: {stopwatch.ElapsedMilliseconds} ms");
            Console.ResetColor();
        }
    }
    #endregion
}
