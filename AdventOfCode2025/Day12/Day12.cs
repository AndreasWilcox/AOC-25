
namespace AdventOfCode2025;

public static class Day12
{
	public static void Run()
	{
		Console.WriteLine("Day 12!");
		var spaces = ParseSpaces(File.ReadAllLines($"Day12/TestShapes.txt")).ToList();

		Console.WriteLine($"Finished, password is {RunFull(spaces)}");
	}

	static long RunFull(List<Space> spaces)
	{
		var sum = 0;
		
		foreach(var space in spaces)
		{
			var totalSpace = (space.Size.X - space.Size.X % 3) * (space.Size.Y - space.Size.Y % 3);
			var requiredSpace = 9 * space.GiftsRequired.Sum(x => x);
			if(totalSpace >= requiredSpace)
				sum += 1;
		}

		return sum;
	}

	static IEnumerable<Space> ParseSpaces(string[] lines)
	{
		var data = lines.SkipWhile(x => !x.Contains("---")).Skip(1);
		foreach(var line in data)
		{
			var lineSplit = line.Split(": ");
			var sizeSplit = lineSplit[0].Split("x");
			var giftsSplit = lineSplit[1].Split(" ");
			var space = new Space
			{
				Size = new IntVector2(int.Parse(sizeSplit[0]), int.Parse(sizeSplit[1])),
				GiftsRequired = giftsSplit.Select(int.Parse).ToList()
			};
			yield return space;
		}
	}

	class Space
	{
		public IntVector2 Size;
		public List<int> GiftsRequired = new();
	}
}