namespace Moerenuma.Robot
{
  using Microsoft.Extensions.Logging;

  public class Robot
  {
    private readonly Obstacles obstacles;
    private readonly ILogger logger;

    public Robot(ILogger log, Obstacles obstacles)
    {
      this.Direction = Direction.Right;
      this.obstacles = obstacles;
      this.logger = log;
    }

    public int Row { get; private set; }

    public int Col { get; private set; }

    public Direction Direction { get; private set; }

    public bool ExecuteInstructions(string instructions)
    {
      bool valid = true;
      foreach (char instruction in instructions)
      {
        valid &= this.ExecuteInstruction(instruction);
      }

      return valid;
    }

    public bool ExecuteInstruction(char keyChar)
    {
      bool isValid = true;
      switch (keyChar)
      {
        case 'f':
          this.MoveForward();
          break;
        case 'r':
          this.Rotate(clockwise: true);
          break;
        case 'l':
          this.Rotate(clockwise: false);
          break;
        default:
          this.logger.LogWarning($"Input '{keyChar}' not recognized");
          isValid = false;
          break;
      }

      return isValid;
    }

    public void Rotate(bool clockwise)
    {
      int delta = clockwise ? 1 : -1;

      int newDirection = (int)this.Direction + delta;

      if (newDirection < 0)
      {
        newDirection = 3;
      }

      if (newDirection > 3)
      {
        newDirection = 0;
      }

      this.Direction = (Direction)newDirection;
    }

    public void MoveForward()
    {
      int initialRow = this.Row;
      int initialCol = this.Col;

      switch (this.Direction)
      {
        case Direction.Right:
          this.Col++;
          break;
        case Direction.Left:
          this.Col--;
          break;
        case Direction.Up:
          this.Row++;
          break;
        case Direction.Down:
          this.Row--;
          break;
      }

      var newPosition = new Coordinate(this.Row, this.Col);
      if (this.obstacles.ExistsAt(newPosition))
      {
        this.Row = initialRow;
        this.Col = initialCol;
        this.logger.LogWarning($"Obstacle at position {newPosition}");
      }
    }

    public override string ToString()
    {
      return $"Robot at [{this.Row}, {this.Col}] facing {this.Direction}";
    }
  }
}
