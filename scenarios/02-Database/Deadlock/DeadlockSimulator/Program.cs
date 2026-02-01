using System;
using System.Data.SqlClient;
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
        Console.WriteLine("=== Deadlock Simulation ===");
        Console.WriteLine("Preparing Database...");

        try
        {
            SetupDatabase();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting up DB (Make sure MSSQL is running on port 1433): {ex.Message}");
            return;
        }

        Console.WriteLine("Database ready. Starting Deadlock Scenario...");
        Console.WriteLine("Thread 1 will lock Resource A, then want Resource B.");
        Console.WriteLine("Thread 2 will lock Resource B, then want Resource A.");
        Console.WriteLine("-----------------------------------------------------");

        var task1 = Task.Run(() => ExecuteTransaction("Thread-1", 1, 2));
        var task2 = Task.Run(() => ExecuteTransaction("Thread-2", 2, 1));

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
                IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'DeadlockLab')
                    CREATE DATABASE DeadlockLab;
            ";
            cmd.ExecuteNonQuery();
        }

        var labConnectionString = ConnectionString.Replace("master", "DeadlockLab");
        using (var conn = new SqlConnection(labConnectionString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                IF OBJECT_ID('Resources', 'U') IS NOT NULL DROP TABLE Resources;
                CREATE TABLE Resources (Id INT PRIMARY KEY, Name NVARCHAR(50));
                INSERT INTO Resources VALUES (1, 'Resource A'), (2, 'Resource B');
            ";
            cmd.ExecuteNonQuery();
        }
    }
    #endregion

    #region Transaction Logic
    static void ExecuteTransaction(string threadName, int firstResourceId, int secondResourceId)
    {
        var labConnectionString = ConnectionString.Replace("master", "DeadlockLab");

        using (var conn = new SqlConnection(labConnectionString))
        {
            conn.Open();
            using (var trans = conn.BeginTransaction())
            {
                try
                {
                    var cmd = conn.CreateCommand();
                    cmd.Transaction = trans;

                    // 1. Lock First Resource
                    Console.WriteLine($"[{threadName}] Locking Resource {firstResourceId}...");
                    cmd.CommandText = $"UPDATE Resources SET Name = '{threadName}' WHERE Id = {firstResourceId}";
                    cmd.ExecuteNonQuery();
                    Console.WriteLine($"[{threadName}] Locked Resource {firstResourceId}.");

                    // 2. Wait to ensure the other thread locks the other resource
                    Thread.Sleep(2000); 

                    // 3. Try to Lock Second Resource
                    Console.WriteLine($"[{threadName}] Trying to lock Resource {secondResourceId} (Waiting)...");
                    cmd.CommandText = $"UPDATE Resources SET Name = '{threadName}' WHERE Id = {secondResourceId}";
                    cmd.ExecuteNonQuery(); 
                    Console.WriteLine($"[{threadName}] Locked Resource {secondResourceId}.");

                    trans.Commit();
                    Console.WriteLine($"[{threadName}] Transaction Committed successfully!");
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 1205) // Deadlock Victim
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"[{threadName}] DEADLOCK VICTIM! Process was chosen as victim and rolled back.");
                        Console.ResetColor();
                        Console.WriteLine($"[{threadName}] Cause: Cyclic dependency detected by SQL Server.");
                    }
                    else
                    {
                        Console.WriteLine($"[{threadName}] Error: {ex.Message}");
                    }
                }
            }
        }
    }
    #endregion
}
