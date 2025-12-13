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

		public double Distance(Vector3 other) => Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2));
		public override string ToString() => $"({X}, {Y}, {Z})";

		public bool Equals(Vector3 other) => X == other.X && Y == other.Y && Z == other.Z;
		public override bool Equals(object? obj) => obj is Vector3 other && Equals(other);
		public override int GetHashCode() => HashCode.Combine(X, Y, Z);
	}
	
	public static void Run()
	{
		Console.WriteLine("Day 8!");
		var lines = File.ReadAllLines($"Day8/TestJunctions.txt");
		var junctions = lines.Select(x => new Vector3(x.Split(',').Select(int.Parse).ToList())).ToList();
		
		var sum = Part1.Run(junctions);
		Console.WriteLine($"Finished part 1, password is {sum}");
	}

	class Part1
	{
		public static long Run(List<Vector3> junctions)
		{
			long sum = 0;
			var c1 = new Vector3();
			var c2 = new Vector3();
			var available = junctions.ToList();
			var circuits = new List<Circuit>();
			for(var i = 0; i < 10; i++)
			{
				var closestDistance = double.MaxValue;
				foreach(var junction in available)
				{
					foreach(var other in junctions)
					{
						var distance = junction.Distance(other);
						if(junction.Equals(other) || !(closestDistance > distance))
							continue;
						
						closestDistance = distance;
						c1 = junction;
						c2 = other;
					}
				}

				var circuit = circuits.FirstOrDefault(x => x.HasJunction(c2));
				if(circuit != null)
				{
					circuit.Add(c1);
				}
				else
				{
					circuits.Add(new Circuit(c1, c2));
				}

				available.Remove(c1);
				available.Remove(c2);
            }
			return sum;
		}

		class Circuit
		{
			List<Vector3> Junctions = new();

			public Circuit(Vector3 j1, Vector3 j2)
			{
				Junctions.Add(j1);
				Junctions.Add(j2);
			}

			public bool HasJunction(Vector3 junction) => Junctions.Contains(junction);
			public void Add(Vector3 junction) => Junctions.Add(junction);
		}
	}
	
	class Part2
	{
		public static long Run(List<Vector3> junctions)
		{
			long sum = 0;
			return sum;
		}
	}
}