using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2025;

public static class Day8
{
	class Circuits
	{
		public List<int> Connected;
		public List<int> Sizes;
		public long Part2;
	}

	public static void Run()
	{
		Console.WriteLine("Day 8!");
		var lines = File.ReadAllLines($"Day8/Junctions.txt");
		var junctions = lines.Select(x => new Vector3(x.Split(',').Select(int.Parse).ToList())).ToList();

		Console.WriteLine($"Finished part 1, password is {Part1.Run(junctions)}");
		Console.WriteLine($"Finished part 2, password is {Part2.Run(junctions)}");
	}

	class Part1
	{
		public static long Run(List<Vector3> junctionPositions)
		{
			var circuits = GetConnectedCircuits(junctionPositions, 1000);

			return circuits.Sizes.Select(x => (long)x)
				.OrderByDescending(x => x)
				.Take(3)
				.Aggregate<long, long>(1, (current, x) => current * x);
		}
	}
	
	class Part2
	{
		public static long Run(List<Vector3> junctionPositions)
		{
			var circuits = GetConnectedCircuits(junctionPositions, int.MaxValue);
			return circuits.Part2;
		}
	}

	static Circuits GetConnectedCircuits(List<Vector3> junctionPositions, int tries)
	{
		var edges = junctionPositions.SelectMany(
				(c1, i) => junctionPositions
					.Skip(i + 1)
					.Select((c2, j) => new Tuple<int, int, double>(i, i + j + 1, c2.DistanceSquared(c1))))
			.OrderBy(x => x.Item3)
			.Take(tries)
			.ToList();

		var c = new Circuits
		{
			Connected = Enumerable.Range(0, junctionPositions.Count).ToList(),
			Sizes = Enumerable.Repeat(1, junctionPositions.Count).ToList()
		};
		var mergeAttempts = 0;
		var groupsLeft = junctionPositions.Count;
		foreach(var edge in edges.TakeWhile(_ => mergeAttempts != tries))
		{
			mergeAttempts += 1;
			var a = FindSet(c, edge.Item1);
			var b = FindSet(c, edge.Item2);
			if(a == b)
				continue;
			if(c.Sizes[a] < c.Sizes[b])
				(a, b) = (b, a);

			c.Connected[b] = a;
			c.Sizes[a] += c.Sizes[b];

			groupsLeft -= 1;
			if(groupsLeft == 1)
			{
				c.Part2 = junctionPositions[edge.Item1].X * junctionPositions[edge.Item2].X;
				break;
			}
		}

		return c;
	}

	static int FindSet(Circuits c, int item)
	{
		if(item == c.Connected[item])
			return item;
		return c.Connected[item] = FindSet(c, c.Connected[item]);
	}
}