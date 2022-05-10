# Tasks
Key takeaways and learnings
1. Task cancellation using CancellationTokenSource.Cancel()
2. Task cancellation using CancellationTokenSource.CancelAfter()
3. Registering a callback for cancelled task. https://docs.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads
4. discards. https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/discards
5. AggregateException: https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/exception-handling-task-parallel-library https://docs.microsoft.com/en-us/archive/msdn-magazine/2009/brownfield/aggregating-exceptions#figure-3-aggregateexception-in-parallel-invocation
6. Cancelling a task with Cancel() or CancelAfter() only cancels queued tasks. To stop a started task, we need to implement cancellation logic in the task by using IsCancellationRequested , maybe? *
