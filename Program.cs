Console.WriteLine("This shall take a while . . . . . . .");
var cts = new CancellationTokenSource();
//cts.CancelAfter(1000); 

try
{
    var task = MyClass.LongWork(cts.Token);
    Thread.Sleep(1000);
    if (!task.IsCompleted)
    {
        Console.WriteLine("Initiate Cacnelling. . . . . . . . .");
        cts.Cancel();
    }
    Console.WriteLine("All good comes to those who wait");
}
catch (TaskCanceledException)
{
    Console.WriteLine("Arrggghh!! Can't wait");
}
catch (OperationCanceledException)
{
    Console.WriteLine("Arrggghh!! Can't wait");
}
catch(AggregateException)
{
    Console.WriteLine("Multiple exceptions");
}
finally
{
    cts.Dispose();
}

public class MyClass
{
    public static Task LongWork(CancellationToken token)
    {
        Task? task = null;
        _ = token.Register(CallMeIfCancelled);
        task = Task.Run(() =>
        {
            Console.WriteLine("Long work started");
            Thread.Sleep(5000);
            Console.WriteLine("Long work Ended");
        });
        return task;
    }

    private static void CallMeIfCancelled()
    {
        Console.WriteLine("Task Cancelled. . . . . .");
        throw new TaskCanceledException();
    }
}