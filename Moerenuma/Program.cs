namespace Moerenuma
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using Microsoft.Extensions.Logging;

  public class Program
  {
    public static void Main(string[] args)
    {
      ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
      {
        builder.AddSimpleConsole(options =>
        {
          options.IncludeScopes = true;
          options.SingleLine = true;
          options.TimestampFormat = "[hh:mm:ss.fff] ";
        });
      });

      ILogger logger = loggerFactory.CreateLogger(nameof(Program));
      var cts = new CancellationTokenSource();

      logger.LogInformation("Starting program");
      Console.ReadLine();
      logger.LogInformation("Ending program");
    }
  }
}
