namespace Moerenuma
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using Moerenuma.Common;

  public static class FutureExecutionQueueRunner
  {
    private const string DateTimeFormat = "hh:mm:ss.ffff";

    public static void Run()
    {
      var cts = new CancellationTokenSource();
      var feq = new FutureExecutionQueue(cts.Token);
      Task queueTask = feq.ExecuteAsync();

      Console.WriteLine("Enter -1 to exit");
      Console.WriteLine("Enter -2 to cancel the TokenSource");
      LogWithTime("Enter the task callback delay in ms: ");
      while (true)
      {
        string input = Console.ReadLine();
        int delayMs = int.Parse(input);
        DateTime expectedDueTime = DateTime.Now + TimeSpan.FromMilliseconds(delayMs);
        LogWithTime($"Entered '{delayMs}'. Expected due back at {expectedDueTime.ToString(DateTimeFormat)}");

        if (delayMs == -1)
        {
          break;
        }
        else if (delayMs == 5)
        {
          // special value to indicate a delay on the execution
          // can simulate executing a lot of actions
          string msg = $"callback executed for delay '{delayMs}'";
          feq.Enqueue(delayMs, () =>
          {
            Thread.Sleep(5000);
            LogWithTime(msg);
          });
        }
        else if (delayMs == -2)
        {
          cts.Cancel();
          Console.WriteLine("Cancelling the token");
        }
        else
        {
          string msg = $"callback executed for delay '{delayMs}'";
          feq.Enqueue(delayMs, () => LogWithTime(msg));
        }
      }

      Console.WriteLine("Exiting...");
    }

    private static void LogWithTime(string msg) => Console.WriteLine($"[{DateTime.Now.ToString(DateTimeFormat)}]: {msg}");
  }
}
