namespace Moerenuma.Robot
{
  using System.Collections.Generic;

  public class Obstacles
  {
    private readonly Dictionary<Coordinate, Obstacle> obstacles = new();

    public Obstacles(IEnumerable<Obstacle> initialList)
    {
      foreach (Obstacle ob in initialList)
      {
        this.obstacles.Add(ob.Coordinate, ob);
      }
    }

    public bool ExistsAt(Coordinate position)
    {
      return this.obstacles.ContainsKey(position);
    }
  }
}
