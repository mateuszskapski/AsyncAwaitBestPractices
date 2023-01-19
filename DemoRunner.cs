using System.Diagnostics;

public static class DemoRunner
{
    public async static Task SlowVsFastBreakfast()
    {
        var breakfastMaker = new BreakfastMaker();

        var sw = new Stopwatch();

        sw.Restart();
        await breakfastMaker.MakeBreakfastSlowAsync(1);
        Console.WriteLine($"Making breakfast step by step took: {sw.ElapsedMilliseconds}ms");
        Console.WriteLine();

        sw.Restart();
        await breakfastMaker.MakeBreakfastFastAsync(1);
        Console.WriteLine($"Making breakfast in parallel took: {sw.ElapsedMilliseconds}ms");
        Console.WriteLine();
    }

    public async static Task CancellationTokenDemo()
    {
        var breakfastMaker = new BreakfastMaker();
        try
        {
            await breakfastMaker.FryBaconAsync(new CookingTimer());
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("FryBacon was cancelled.");
        }
    }

    public static Task ReturnTask()
    {
        return new ReturnTask().MethodReturnTask();
    } 

    public static Task AlwaysAwaitTask()
    {
        return new AlwaysAwaitTask().MethodAlwaysAwaitTask();
    }
}