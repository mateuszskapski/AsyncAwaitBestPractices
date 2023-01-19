public class CookingTimer
{
    private readonly CancellationTokenSource _cts;
    public CancellationToken Token => _cts.Token;

    public int CancellationInterval {get; set;} = 10; 

    public CookingTimer()
    {
        _cts = new CancellationTokenSource();
        _cts.Token.Register(() => Console.WriteLine("Cooking cancellation has been requested."));
    }

    public void OnNext(int timeInterval)
    {
        Console.WriteLine($"Cooking time: {timeInterval}");
        if (timeInterval == CancellationInterval)
        {
            _cts.Cancel();
        }
    }
}