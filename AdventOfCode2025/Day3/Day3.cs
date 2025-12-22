namespace AdventOfCode2025;

public static class Day3
{
	public static void Run()
	{
		Console.WriteLine("Day 3!");
		string[] batteries = File.ReadAllLines($"Day3/TestBatteries.txt");
		Console.WriteLine($"Finished part 1, password is {Part1.Run(batteries)}");
		Console.WriteLine($"Finished part 2, password is {Part2.Run(batteries)}");
	}

	static class Part1
	{
		public static long Run(string[] batteries)
		{
			long sum = 0;
			foreach (string range in batteries)
			{
				var largest = new int[2];

				for (int i = 0; i < range.Length; i++)
				{
					char c = range[i];
					var value = int.Parse(c.ToString());
					if(value > largest[0] && i != range.Length - 1)
					{
						largest[0] = value;
						largest[1] = 0;
					}
					else if(value > largest[1])
					{
						largest[1] = value;
					}
				}

				sum += int.Parse($"{largest[0]}{largest[1]}");
				//Console.WriteLine($"{range} - {largest[0]}{largest[1]}");
			}

			return sum;
		}
	}
	
	static class Part2
	{
		public static long Run(string[] batteries)
		{
			var count = 12;
			long sum = 0;
			foreach (string range in batteries)
			{
				var largest = new int[count];

				for (int i = 0; i < range.Length; i++)
				{
					char c = range[i];
					var value = int.Parse(c.ToString());
					for (int j = 0; j < count; j++)
					{
						if(value > largest[j] && i < range.Length - (count - j - 1))
						{
							largest[j] = value;
							ResetAfter(ref largest, j);
							break;
						}
					}
				}

				var sumLine = largest.Aggregate("", (current, i) => current + $"{i}");
				sum += Int64.Parse(sumLine);
				//Console.WriteLine($"{range} - {sumLine}");
			}

			return sum;;
		}

		static void ResetAfter(ref int[] list, int index)
		{
			for (int i = index + 1; i < list.Length; i++)
				list[i] = 0;
		}
	}
}