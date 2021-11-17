namespace Moerenuma.Robot
{
  public class Obstacle
  {
    public Obstacle(int row, int col)
    {
      this.Coordinate = new Coordinate(row, col);
    }

    public int Size { get; set; }

    public Coordinate Coordinate { get; set; }
  }
}
