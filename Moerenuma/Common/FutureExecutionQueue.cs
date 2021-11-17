namespace Moerenuma.Common
{
  using System;
  using System.Collections.Concurrent;
  using System.Threading;
  using System.Threading.Tasks;

  public class FutureExecutionQueue : IDisposable
  {
    private readonly SemaphoreSlim semephore;
    private readonly Timer timer;
    private readonly CancellationToken token;
    private ConcurrentQueue<FutureExecutionRecord> queue;
    private DateTime nextExecutionTime;

    public FutureExecutionQueue(CancellationToken token)
    {
      this.token = token;
      this.nextExecutionTime = DateTime.MaxValue;
      this.queue = new ConcurrentQueue<FutureExecutionRecord>();
      this.semephore = new SemaphoreSlim(0, 1);
      this.timer = new Timer((o) => this.TimerCallback());
    }

    public void Enqueue(int delayMs, Action act)
    {
      this.token.ThrowIfCancellationRequested();
      this.EnqueueRecord(new FutureExecutionRecord(delayMs, act));
    }

    public async Task ExecuteAsync()
    {
      while (true)
      {
        await this.semephore.WaitAsync(this.token);
        ConcurrentQueue<FutureExecutionRecord> workToDo = Interlocked.Exchange(ref this.queue, new ConcurrentQueue<FutureExecutionRecord>());
        while (workToDo.Count > 0)
        {
          this.token.ThrowIfCancellationRequested();
          if (workToDo.TryDequeue(out FutureExecutionRecord record))
          {
            if (record.ShouldExecute)
            {
              record.Execute();
            }
            else
            {
              this.EnqueueRecord(record);
            }
          }
        }
      }
    }

    public void Dispose()
    {
      this.timer.Dispose();
      this.semephore.Dispose();
    }

    private void EnqueueRecord(FutureExecutionRecord record)
    {
      this.queue.Enqueue(record);
      if (record.DueTime < this.nextExecutionTime)
      {
        this.timer.Change(record.DelayTime, Timeout.InfiniteTimeSpan);
        this.nextExecutionTime = record.DueTime;
      }
    }

    private void TimerCallback()
    {
      this.nextExecutionTime = DateTime.MaxValue;
      if (this.semephore.CurrentCount == 0)
      {
        this.semephore.Release();
      }
    }

    private record FutureExecutionRecord
    {
      public FutureExecutionRecord(int delayMs, Action action)
      {
        this.Action = action;
        this.DueTime = DateTime.UtcNow + TimeSpan.FromMilliseconds(delayMs);
      }

      public Action Action { get; private set; }

      public TimeSpan DelayTime
      {
        get
        {
          DateTime now = DateTime.UtcNow;
          return now > this.DueTime ? TimeSpan.Zero : this.DueTime - now;
        }
      }

      public DateTime DueTime { get; private set; }

      public bool ShouldExecute => DateTime.UtcNow >= this.DueTime;

      public void Execute() => this.Action();
    }
  }
}
