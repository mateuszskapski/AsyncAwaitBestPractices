public class TaskDemos
{
    System.Collections.Concurrent.ConcurrentDictionary<int, int> executions = new System.Collections.Concurrent.ConcurrentDictionary<int, int>();
    public async Task WhenAllDemo(int taskCount, TimeSpan duration)
    {
        var tasks = new List<Task>(taskCount);

        for (int i = 0; i < taskCount; i++)
        {
            tasks.Add(DoWorkAsync(duration, i));
        }

        Task entireTask = Task.WhenAll(tasks);
        while (await Task.WhenAny(entireTask, Task.Delay(100)) != entireTask)
        {
            Console.Write(".");
        }
        Console.WriteLine();

        var results = executions.GroupBy(x => x.Value, x => x.Value, (thread, threads) => new 
        {
            Key = thread, 
            Count = threads.Count()
        }).OrderByDescending(x => x.Count);

        Console.WriteLine($"ThreadPoolCount: {ThreadPool.ThreadCount}");
        ThreadPool.GetAvailableThreads(out int workerThreads, out int ioThreads);
        Console.WriteLine($"ThreadPoolAvailable: workerThreads {workerThreads}, IO Threads {ioThreads}");
        ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxIOThreads);
        Console.WriteLine($"ThreadPoolMax: workerThreads {maxWorkerThreads}, IO Threads {maxIOThreads}");

        foreach (var exec in results)
        {
            Console.WriteLine($"ThreadId: {exec.Key}, ExecCount: {exec.Count}");
        }
    }

    async Task DoWorkAsync(TimeSpan duration, int taskId)
    {
        await Task.Delay(duration);
        executions.TryAdd(taskId, Thread.CurrentThread.ManagedThreadId);
    }

}