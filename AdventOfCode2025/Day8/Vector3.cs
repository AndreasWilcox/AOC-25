namespace AdventOfCode2025;

struct Vector3(int x, int y, int z) : IEquatable<Vector3>
{
    public long X = x, Y = y, Z = z;

    public Vector3(List<int> list) : this(list[0], list[1], list[2])
    {
    }

    public double DistanceSquared(Vector3 other) => Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2);
    public override string ToString() => $"({X}, {Y}, {Z})";

    public bool Equals(Vector3 other) => X == other.X && Y == other.Y && Z == other.Z;
    public override bool Equals(object? obj) => obj is Vector3 other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);
}