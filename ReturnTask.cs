public class ReturnTask
{
    public Task MethodReturnTask()
    {
        Console.WriteLine("About to call method one");
        
        return MethodOne();
    }

    private Task MethodOne()
    {
        Console.WriteLine("In method one.");
        return MethodTwo();
    }

    private Task MethodTwo()
    {
        Console.WriteLine("In method two");
        return MethodThree();
    }

    private Task MethodThree()
    {
        Console.WriteLine("In method three");
        return MethodFour();
    }

    private async Task MethodFour()
    {
        Console.WriteLine("In method four");
        await Task.Delay(100);

        await Task.Delay(1);
    }

    #region Pitfalls

    Task DoNotReturnTaskFromTryCatch()
    {
        try
        {
            return MethodFour();
        }
        catch (Exception)
        {
            return ProcessExceptionAsync();
        }
    }

    private Task ProcessExceptionAsync()
    {
        throw new NotImplementedException();
    }

    Task ProcessData(Stream stream) => Task.Delay(100);

    Task DoNotReturnTaskFromUsingStatement()
    {
        using (var ms = new MemoryStream())
        {
            return ProcessData(ms);
        }
    }

    #endregion
}

public class AlwaysAwaitTask
{
    public async Task MethodAlwaysAwaitTask()
    {
        Console.WriteLine("About to call method one");
        await MethodOne();
    }

    private async Task MethodOne()
    {
        await MethodTwo();
    }

    private async Task MethodTwo()
    {
        Console.WriteLine("In method two");
        await MethodThree();
    }

    private async Task MethodThree()
    {
        Console.WriteLine("In method three");
        await MethodFour();
    }

    private async Task MethodFour()
    {
        Console.WriteLine("In method four");

        await Task.Delay(100);
    }
}