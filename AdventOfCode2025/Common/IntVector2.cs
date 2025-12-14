namespace AdventOfCode2025;

public struct IntVector2(long x, long y) : IEquatable<IntVector2>
{
	public readonly long X = x;
	public readonly long Y = y;
	public double Magnitude => Math.Sqrt(X * X + Y * Y);
	
	public double Dot(IntVector2 other) => X * other.X + Y * other.Y;

	public override string ToString() => $"({X}, {Y})";

	public static IntVector2 Parse(string line)
	{
		string[] split = line.Split(',');
		return new IntVector2(int.Parse(split[0]), int.Parse(split[1]));
	}
	
	public static IntVector2 operator +(IntVector2 a, IntVector2 b) => new(a.X + b.X, a.Y + b.Y);
	public static IntVector2 operator -(IntVector2 a, IntVector2 b) => new(a.X - b.X, a.Y - b.Y);

	public bool Equals(IntVector2 other) => X == other.X && Y == other.Y;
	public override bool Equals(object? obj) => obj is IntVector2 other && Equals(other);
	public override int GetHashCode() => HashCode.Combine(X, Y);

}