Console.WriteLine("This shall take a while . . . . . . .");
var cts = new CancellationTokenSource();
var token = cts.Token;
//cts.CancelAfter(10000); //does not call async operation? 

try
{
    MyClassAsync.LongWork(token);
    cts.Cancel();
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

public class MyClassAsync
{
    public static Task LongWork(CancellationToken ct)
    {
        Task? task = null;
        ct.Register(CallMeIfCancelled);
        task = Task.Run(() =>
        {
            Task.Delay(5000);
            Console.WriteLine("Long work started");
            Task.Delay(5000);
        });
        return task;
    }

    private static void CallMeIfCancelled()
    {
        Console.WriteLine("Task Cancelled :( ");
        throw new TaskCanceledException();
    }
}


