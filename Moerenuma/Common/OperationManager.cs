namespace Moerenuma.Common
{
  using System;
  using System.Collections.Generic;
  using System.Threading;
  using System.Threading.Tasks;

  public class OperationManager<T>
  {
    private readonly int maxPendingCalls;
    private readonly Func<T, Task> doAsync;

    public OperationManager(int maxPendingCalls, Func<T, Task> doAsync)
    {
      if (maxPendingCalls <= 0)
      {
        throw new ArgumentOutOfRangeException(nameof(maxPendingCalls));
      }

      this.maxPendingCalls = maxPendingCalls;
      this.doAsync = doAsync;
    }

    public async Task RunAsync(IEnumerable<T> items, CancellationToken token)
    {
      var semaphore = new AsyncSemaphore(this.maxPendingCalls);
      var pending = new Queue<Task>();
      var exceptions = new List<Exception>();
      foreach (T item in items)
      {
        await semaphore.WaitAsync();
        token.ThrowIfCancellationRequested();
        pending.Enqueue(this.CallAsync(semaphore, item));
        HandleCompletedCalls(pending, exceptions);
        if (exceptions.Count > 0)
        {
          throw new AggregateException(exceptions).Flatten();
        }
      }

      await Task.WhenAll(pending);
    }

    private static void HandleCompletedCalls(Queue<Task> pending, IList<Exception> exceptions)
    {
      while (pending.Count > 0)
      {
        if (!pending.Peek().IsCompleted)
        {
          return;
        }

        Task task = pending.Dequeue();
        if (task.IsFaulted)
        {
          exceptions.Add(task.Exception);
        }
      }
    }

    private async Task CallAsync(AsyncSemaphore semaphore, T item)
    {
      try
      {
        await this.doAsync(item);
      }
      finally
      {
        semaphore.Release();
      }
    }
  }
}
