MyLogger.LogWithThreadId("This shall take a while . . . . . . .");
var tokenSource = new CancellationTokenSource();
var tokenSourceWithExpiry = new CancellationTokenSource(1000);// Same as CancelAfter()

try
{
    /*
     * For a time consuming work, wait for it to finish for x seconds
     * Continue the execution if work is not finished within the time
     */
    var task = MyClass.TimeConsumingWork(tokenSource.Token);
    Thread.Sleep(1000);
    if (!task.IsCompleted)
    {
        MyLogger.LogWithThreadId("Initiate Cacnelling >>.");
        tokenSource.Cancel();
    }
}
//https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/exception-handling-task-parallel-library
//https://docs.microsoft.com/en-us/archive/msdn-magazine/2009/brownfield/aggregating-exceptions#figure-3-aggregateexception-in-parallel-invocation
catch (AggregateException aggex)
{
    aggex.Handle((ex) =>
    {
        if (ex is TaskCanceledException)
        {
            MyLogger.LogWithThreadId("Task Cancellation well received.");
            Thread.Sleep(5000);
            return true;
        }
        return false; //Considered as unhandled exception
    });
}
finally
{
    tokenSource.Dispose();
    tokenSourceWithExpiry.Dispose();
}

/*
* For a time consuming work, set the expiry time.
* When expired, cancel the task
*/
AppDomain.CurrentDomain.UnhandledException +=
    (sender, e) => { MyLogger.LogWithThreadId("Task cancellation on other thread."); };
_ = MyClass.TimeConsumingWork(tokenSourceWithExpiry.Token);
Thread.Sleep(5000);
MyLogger.LogWithThreadId("All good comes to those who wait");
Thread.Sleep(5000);

public class MyClass
{
    public static Task TimeConsumingWork(CancellationToken token)
    {
        //https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/discards
        _ = token.Register(CallMeIfCancelled);

        Task? task = null;
        task = Task.Run(() =>
        {
            MyLogger.LogWithThreadId("Long work started.");
            Thread.Sleep(5000);
            MyLogger.LogWithThreadId("Long work Ended.");
        });
        return task;
    }

    private static void CallMeIfCancelled()
    {
        MyLogger.LogWithThreadId("Task Cancelled.");
        throw new TaskCanceledException();
    }
}

public class MyLogger
{
    public static void LogWithThreadId(string? message)
    {
        //Console.WriteLine($"On Thread {Thread.CurrentThread.ManagedThreadId}: {message}");
        Console.WriteLine($"Thread {Environment.CurrentManagedThreadId}: {message}");
    }
}