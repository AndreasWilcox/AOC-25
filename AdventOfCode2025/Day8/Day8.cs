using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2025;

public static class Day8
{
	struct Vector3(int x, int y, int z) : IEquatable<Vector3>
	{
		public int X = x, Y = y, Z = z;

		public Vector3(List<int> list) : this(list[0], list[1], list[2])
		{
		}

		public double DistanceSquared(Vector3 other) => Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2);
		public override string ToString() => $"({X}, {Y}, {Z})";

		public bool Equals(Vector3 other) => X == other.X && Y == other.Y && Z == other.Z;
		public override bool Equals(object? obj) => obj is Vector3 other && Equals(other);
		public override int GetHashCode() => HashCode.Combine(X, Y, Z);
	}
	
	public static void Run()
	{
		Console.WriteLine("Day 8!");
		var lines = File.ReadAllLines($"Day8/Junctions.txt");
		var junctions = lines.Select(x => new Vector3(x.Split(',').Select(int.Parse).ToList())).ToList();
		
		var sum = Part1.Run(junctions);
		Console.WriteLine($"Finished part 1, password is {sum}");
	}

	class Part1
	{
		public static long Run(List<Vector3> junctionPositions)
		{
			var junctions = GetConnectedJunctions(junctionPositions, 1000);

			return junctions
				.OrderByDescending(x => x.CountTreeSize())
				.Take(3)
				.Aggregate<Junction, long>(1, (current, x) => current * x.CountTreeSize());
		}
	}
	
	class Part2
	{
		public static long Run(List<Vector3> junctionPositions)
		{
			long sum = 0;

			var junction = GetConnectedJunctions(junctionPositions, int.MaxValue).Single();

			return junction.Position.X * junction.GetLastConnection().Position.X;
		}
	}

	static List<Junction> GetConnectedJunctions(List<Vector3> junctionPositions, int tries)
	{
		var junctions = junctionPositions.Select(x => new Junction(x)).ToList();
		var edges = junctions.SelectMany(
				(j1, i) => junctions
					.Skip(i + 1)
					.Select(j2 => new Tuple<Junction, Junction, double>(j1, j2, j2.DistanceSquared(j1))))
			.OrderBy(x => x.Item3)
			.Take(tries)
			.ToList();

		var merges = 0;
		foreach(var edge in edges.TakeWhile(edge => merges != tries && junctions.Count != 1))
		{
			merges += 1;
			if(edge.Item1.IsConnectedTo(edge.Item2))
				continue;

			junctions.Remove(edge.Item2);
			foreach (var junction in junctions.ToList())
			{
				if(junction.IsConnectedTo(edge.Item2))
					junctions.Remove(junction);
			}
				
			edge.Item1.Merge(edge.Item2);
			edge.Item2.Merge(edge.Item1);
		}

		junctions = junctions.OrderByDescending(x => x.CountTreeSize()).ToList();
		foreach(var junction in junctions)
		{
			Console.WriteLine($"Circuit: {junction.CountTreeSize()}");
		}

		return junctions;
	}

	class Junction(Vector3 position)
	{
		public Vector3 Position = position;
		readonly List<Junction> connections = [];

		public bool IsConnectedTo(Junction other) => IsConnectedTo(other, this);
		bool IsConnectedTo(Junction junction, Junction previous)
		{
			if(connections.Contains(junction))
				return true;
			return connections.Any(x => x != previous && x.IsConnectedTo(junction, this));
		}

		public int CountTreeSize() => CountJunctions(this);
		int CountJunctions(Junction previous) => 1 + connections.Where(connection => connection != previous).Sum(connection => connection.CountJunctions(this));
			
		public double DistanceSquared(Junction other) => Position.DistanceSquared(other.Position);
		public void Merge(Junction other) => connections.Add(other);
		public Junction GetLastConnection() => connections.Last();
	}
}