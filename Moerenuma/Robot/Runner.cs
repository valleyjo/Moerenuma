namespace Moerenuma.Robot
{
  using System;
  using System.Collections.Generic;
  using Microsoft.Extensions.Logging;

  public static class Runner
  {
    public static void RunLoop(ILogger log)
    {
      var obstacles = new Obstacles(new List<Obstacle>()
      {
        new Obstacle(0, 1),
        new Obstacle(1, 1),
      });

      var robot = new Robot(log, obstacles);
      string inputValue;
      bool shouldRun = true;
      do
      {
        log.LogInformation("-----------------------------------------------");
        log.LogInformation(robot.ToString());
        log.LogInformation("Enter a command:");
        inputValue = Console.ReadLine();
        log.LogInformation($"You entered: '{inputValue}");
        ParseCommand(inputValue, robot, log);

        shouldRun = !string.Equals(inputValue, "q", StringComparison.OrdinalIgnoreCase);
      }
      while (shouldRun);
    }

    private static void PrintIntroMessages(ILogger logger)
    {
      logger.LogInformation("Starting Robot Control Program");
      logger.LogInformation("Enter commands to control the robot:");
      logger.LogInformation("f: move forward");
      logger.LogInformation("r: turn the robot right");
      logger.LogInformation("l: flip the robot");
      logger.LogInformation("?: help message");
      logger.LogInformation("q: quit the program");
    }

    private static void ParseCommand(string command, Robot robot, ILogger log)
    {
      if (string.Equals(command, "?", StringComparison.OrdinalIgnoreCase))
      {
        PrintIntroMessages(log);
        return;
      }

      bool valid = robot.ExecuteInstructions(command);
      if (!valid)
      {
        log.LogWarning("Enter '?' for help.");
      }
    }
  }
}
