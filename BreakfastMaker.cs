public class BreakfastMaker
{
    public BreakfastMaker()
    {
    }

    #region AsyncVoid

    public void StartDevice_OnClick(object sender, EventArgs args)
    {
        DoCleaningAsync().GetAwaiter().GetResult();
    }

    private async Task StartDeviceOnClick(object sender, EventArgs args)
    {
        try
        {
            await DoCleaningAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private async Task DoCleaningAsync()
    {
        Console.WriteLine("Cleaning cycle started.");

        // Simulates an async operation
        await Task.Delay(200);
        
        Console.WriteLine("Cleaning cycle finished.");
    }

    #endregion

    #region Running tasks in parallel
    public async Task MakeBreakfastSlowAsync(int eggsQuantity)
    {
        await BrewCoffeeAsync();
        await MakeScrambledEggsAsync(eggsQuantity);
        await MakeSandwichAsync();
    }

    public Task MakeBreakfastFastAsync(int eggsQuantity)
    {
        var brewCoffeeTask = BrewCoffeeAsync();
        var makeSandwichTask = MakeSandwichAsync();
        var makeEggs = MakeScrambledEggsAsync(eggsQuantity);

        return Task.WhenAll(brewCoffeeTask, makeSandwichTask, makeEggs);
    }

    public async Task MakeBreakfastFastWithResultsAsync(int eggsQuantity)
    {
        var brewCoffeeTask = BrewCoffeeAsync();
        var makeSandwichTask = MakeSandwichAsync();
        var makeEggs = MakeScrambledEggsAsync(eggsQuantity);

        await Task.WhenAll(brewCoffeeTask, makeSandwichTask, makeEggs);


        var coffeeResult = await brewCoffeeTask;
        var sandwichResult = await makeSandwichTask;
        var eggsResult = await makeEggs;
    }

    #endregion
    
    #region Cancellation
    public Task FryBaconAsync(CookingTimer timer)
    {
        return Task.Run(() => 
        {
            for (var i = 1; i < 100; i++)
            {   
                if (timer.Token.IsCancellationRequested)
                {
                    // Do clean up here 
                    timer.Token.ThrowIfCancellationRequested();
                }
                else
                {
                    // Keep frying.
                    Thread.Sleep(300);
                    timer.OnNext(i);
                }
            }
        }, timer.Token);
    }
    #endregion

    #region Food recipes

    private class Coffee{}
    private class Sandwich{}
    private class ScrambledEggs{}

    private async Task<Coffee> BrewCoffeeAsync()
    {
        Console.WriteLine("Starting to brew a coffee");

        await Task.Delay(400);

        Console.WriteLine("Coffee is ready.");

        return new Coffee();
    }

    private async Task<Sandwich> MakeSandwichAsync()
    {
        Console.WriteLine("Starting to make a sandwich.");
        
        await Task.Delay(100);
        
        Console.WriteLine("Adding ham.");
        await Task.Delay(100);
        
        Console.WriteLine("Adding cheese.");
        await Task.Delay(100);
        
        Console.WriteLine("Sandwich is ready.");

        return new Sandwich();
    }

    private async Task<ScrambledEggs> MakeScrambledEggsAsync(int quantity)
    {
        Console.WriteLine("Starting to make scrambled eggs.");
        await TakeEggsOutOfFridgeAsync(quantity);

        await Task.Delay(300);

        Console.WriteLine("Cooking eggs.");
        
        await Task.Delay(300);

        Console.WriteLine("Scrambled eggs ready.");

        return new ScrambledEggs();
    }

    #endregion

    #region Parameter validation
    // This is an interesting case flagged by SonarLint, which I found recently.
    // https://rules.sonarsource.com/csharp/RSPEC-4457
    
    // Noncompliant code
    public async Task NonComplaintTakeEggsOutOfFridgeAsync(int quantity)
    {
        int availableEggs = 1;
        if (availableEggs < quantity)
        {
            throw new ArgumentNullException("There are not enough eggs.");
        }

        await Task.Delay(100);
    }
    
    // Complaint code
    private Task TakeEggsOutOfFridgeAsync(int quantity)
    {
        int availableEggs = 1;
        if (availableEggs < quantity)
        {
            throw new ArgumentNullException("There are not enough eggs.");
        }

        return InternalTakeEggsOutOfFridgeAsync(quantity);
    }

    private async Task InternalTakeEggsOutOfFridgeAsync(int quantity)
    {   
        // Simulates an async operation
        await Task.Delay(50);
    }

    #endregion
}