namespace ThreadTypesApi.Services;

public sealed class CpuCalculator
{
    public int CountPrimes(int n)
    {
        var count = 0;
        for (var i = 2; i <= n; i++)
        {
            if (IsPrime(i)) count++;
        }

        return count;
    }

    public int CountPrimesCancellable(int n, CancellationToken cancellationToken, int checkEvery = 200)
    {
        if (checkEvery <= 0) checkEvery = 1;

        var count = 0;
        for (var i = 2; i <= n; i++)
        {
            if (i % checkEvery == 0) cancellationToken.ThrowIfCancellationRequested();

            if (IsPrime(i)) count++;
        }

        return count;
    }

    private static bool IsPrime(int x)
    {
        if (x < 2) return false;
        if (x == 2) return true;
        if (x % 2 == 0) return false;

        var limit = (int)Math.Sqrt(x);
        for (var i = 3; i <= limit; i += 2)
        {
            if (x % i == 0) return false;
        }

        return true;
    }
}
