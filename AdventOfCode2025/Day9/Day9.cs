
namespace AdventOfCode2025;

public static class Day9
{
	public static void Run()
	{
		Console.WriteLine("Day 9!");
		var tiles = File.ReadAllLines($"Day9/Tiles.txt")
			.Select(IntVector2.Parse)
			.ToList();
		
		var sum = Part1.Run(tiles);
		Console.WriteLine($"Finished part 2, password is {sum}");
	}

	class Part1
	{
		public static long Run(List<IntVector2> tiles)
		{
			long largest = 0;
			var remaining = tiles.ToList();
			foreach (var tile in tiles)
			{
				foreach (IntVector2 other in remaining)
				{
					var size = (long)(Math.Abs(tile.X - other.X) + 1) * (Math.Abs(tile.Y - other.Y) + 1);
					if(size > largest)
						largest = size;
				}
			}

			return largest;
		}
	}

	class Part2
	{
		public static long Run(char[,] map)
		{
			long sum = 0;

			return sum;
		}
	}
}