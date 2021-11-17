namespace Moerenuma.Robot
{
  using System;

  public class Coordinate : IEquatable<Coordinate>
  {
    public Coordinate(int row, int col)
    {
      this.Row = row;
      this.Col = col;
    }

    public int Row { get; private set; }

    public int Col { get; private set; }

    public bool Equals(Coordinate other)
    {
      return this.Row == other.Row && this.Col == other.Col;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(this.Row, this.Col);
    }

    public override string ToString()
    {
      return $"[{this.Row}, {this.Col}]";
    }
  }
}
