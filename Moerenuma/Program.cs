namespace Moerenuma
{
  using System.Threading;
  using Microsoft.Extensions.Logging;
  using Moerenuma.Robot;

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

      Runner.RunLoop(logger);

      logger.LogInformation("Ending program");
    }
  }
}
