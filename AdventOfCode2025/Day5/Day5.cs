using System.Diagnostics;

namespace AdventOfCode2025;

public static class Day5
{
	public static void Run()
	{
		Console.WriteLine("Day 5!");
		string[] ingredientsInfo = File.ReadAllLines($"Day5/TestIngredients.txt");
		var splitFound = false;
		List<IntRange> fresh = new();
		List<ulong> ingredients = new();
		foreach (string line in ingredientsInfo)
		{
			if(string.IsNullOrEmpty(line))
			{
				splitFound = true;
				continue;
			}
			
			if(!splitFound)
				AddRanges(line, fresh);
			else
				ingredients.Add(ulong.Parse(line));
		}

		Console.WriteLine($"Finished part 1, password is {Part1.Run(fresh, ingredients)}");
		Console.WriteLine($"Finished part 2, password is {Part2.Run(fresh)}");
	}

	static void AddRanges(string range, List<IntRange> fresh)
	{
		var split = range.Split('-');
		var low = ulong.Parse(split[0]);
		var high = ulong.Parse(split[1]);
		fresh.Add(new IntRange(low, high));
	}

	static class Part1
	{
		public static ulong Run(List<IntRange> fresh, List<ulong> ingredients)
		{
			ulong sum = 0;

			foreach (var val in ingredients)
			{
				foreach (IntRange range in fresh)
				{
					if(!range.Contains(val))
						continue;
					
					//Console.WriteLine($"{val} is fresh");
					sum += 1;
					break;
				}
			}
			
			return sum;
		}
	}
	
	static class Part2
	{
		public static ulong Run(List<IntRange> fresh)
		{
			ulong sum = 0;

			fresh = fresh.OrderBy(x => x.Low).ToList();
			for (int i = 0; i < fresh.Count; i++)
			{
				IntRange current = fresh[i];
				for (int j = i; j < fresh.Count; j++)
				{
					IntRange other = fresh[j];
					if(current == other || !current.Overlaps(other))
						continue;
					
					current.Extend(other);
					fresh.RemoveAt(j);
					j -= 1;
				}
			}

			foreach (var range in fresh)
				sum += range.High - range.Low + 1;

			return sum;
		}
	}
	
	class IntRange(ulong low, ulong high)
	{
		public ulong Low = low;
		public ulong High = high;

		public IntRange(IntRange range) : this(range.Low, range.High)
		{
		}

		public bool Contains(ulong val) => val >= Low && val <= High;
		
		public bool Overlaps(IntRange other) => Contains(other.Low) || Contains(other.High) || other.Contains(Low) || other.Contains(High);

		public void Extend(IntRange other)
		{
			Low = Math.Min(Low, other.Low);
			High = Math.Max(High, other.High);
		}
	}
}