using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreLayer.BackgroundTasks;

public interface IBackgroundTaskQueue
{
    Task QueueBackgroundWorkItemAsync(Func<CancellationToken, Task> workItem);
    Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
}