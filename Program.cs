Console.WriteLine("This shall take a while . . . . . . .");
var cts = new CancellationTokenSource();

try
{
    /*
     * For a time consuming work, wait for it to finish for x seconds
     * Continue the execution if work is not finished within the time
     */
    var task = MyClass.TimeConsumingWork(cts.Token);
    Thread.Sleep(1000);
    if (!task.IsCompleted)
    {
        Console.WriteLine("Initiate Cacnelling >>.");
        cts.Cancel();
    }
    Console.WriteLine("All good comes to those who wait");
}
//https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/exception-handling-task-parallel-library
//https://docs.microsoft.com/en-us/archive/msdn-magazine/2009/brownfield/aggregating-exceptions#figure-3-aggregateexception-in-parallel-invocation
catch (AggregateException aggex)
{
    aggex.Handle((ex) =>
    {
        if (ex is TaskCanceledException)
        {
            Console.WriteLine("Task Cancellation well received.");
            return true;
        }
        return false; //Considered as unhandled exception
    });
}
finally
{
    cts.Dispose();
}

public class MyClass
{
    public static Task TimeConsumingWork(CancellationToken token)
    {
        //https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/discards
        _ = token.Register(CallMeIfCancelled);

        Task? task = null;
        task = Task.Run(() =>
        {
            Console.WriteLine("Long work started.");
            Thread.Sleep(5000);
            Console.WriteLine("Long work Ended.");
        });
        return task;
    }

    private static void CallMeIfCancelled()
    {
        Console.WriteLine("Task Cancelled.");
        throw new TaskCanceledException();
    }
}