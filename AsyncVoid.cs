public class AsyncVoid
{
    private Task DoWork()
    {
        throw new InvalidOperationException("This operation is not allowed.");
    }

    private async void DoSomeWork(Action<Exception> action)
    {
        try
        {
            await DoWork();
        }
        catch (Exception ex)
        {
            action.Invoke(ex);
        }
    }

    public void DoMoreWork(Action<Exception> action)
    {
        DoSomeWork(action);
    }
}