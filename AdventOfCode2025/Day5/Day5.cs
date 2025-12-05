using System.Diagnostics;

namespace AdventOfCode2025;

public static class Day5
{
	struct IntRange(long low, long high) : IEquatable<IntRange>
	{
		public readonly long Low = low;
		public readonly long High = high;

		public bool Contains(long val) => val >= Low && val <= High;
		
		public bool Equals(IntRange other) => Low == other.Low && High == other.High;

		public override bool Equals(object? obj) => obj is IntRange other && Equals(other);

		public override int GetHashCode() => HashCode.Combine(Low, High);
	}
	
	public static void Run()
	{
		Console.WriteLine("Day 5!");
		string[] ingredientsInfo = File.ReadAllLines($"Day5/Ingredients.txt");
		var splitFound = false;
		List<IntRange> fresh = new();
		List<long> ingredients = new();
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
				ingredients.Add(Int64.Parse(line));
		}
		
		var sum = Part2.Run(fresh);
		Console.WriteLine($"Finished part 2, password is {sum}");
	}

	static void AddRanges(string range, List<IntRange> fresh)
	{
		var split = range.Split('-');
		var low = Int64.Parse(split[0]);
		var high = Int64.Parse(split[1]);
		fresh.Add(new IntRange(low, high));
	}

	class Part1
	{
		public static long Run(List<IntRange> fresh, List<long> ingredients)
		{
			long sum = 0;

			foreach (var val in ingredients)
			{
				foreach (IntRange range in fresh)
				{
					if(!range.Contains(val))
						continue;
					
					Console.WriteLine($"{val} is fresh");
					sum += 1;
					break;
				}
			}
			
			return sum;
		}
	}
	
	class Part2
	{
		public static long Run(List<IntRange> fresh)
		{
			long sum = 0;

			List<IntRange> checkedRanges = new();
			foreach (var range in fresh)
			{
				for (long value = range.Low; value <= range.High; value++)
				{
					if(!IsInOtherRange(value, checkedRanges))
						sum += 1;
				}

				checkedRanges.Add(range);
			}
			
			return sum;
		}

		static bool IsInOtherRange(long value, List<IntRange> fresh)
		{
			foreach (IntRange freshRange in fresh)
			{
				if(freshRange.Contains(value))
					return true;
			}

			return false;
		}
	}
}