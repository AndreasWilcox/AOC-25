namespace AdventOfCode2025;

public struct IntVector2(int x, int y) : IEquatable<IntVector2>
{
	public readonly int X = x;
	public readonly int Y = y;

	public bool Equals(IntVector2 other) => X == other.X && Y == other.Y;
	public override bool Equals(object? obj) => obj is IntVector2 other && Equals(other);
	public override int GetHashCode() => HashCode.Combine(X, Y);

	public static IntVector2 Parse(string line)
	{
		string[] split = line.Split(',');
		return new IntVector2(int.Parse(split[0]), int.Parse(split[1]));
	}
}