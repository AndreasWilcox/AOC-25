namespace AdventOfCode2025;

public static class Day2
{
	public static void Run()
	{
		Console.WriteLine("Day 2!");
		string[] ranges = File.ReadAllLines("Day2/TestRanges.txt");
		Console.WriteLine($"Finished part 1, password is {Part1.Run(ranges)}");
		Console.WriteLine($"Finished part 2, password is {Part2.Run(ranges)}");
	}

	static class Part1
	{
		public static long Run(string[] ranges)
		{
			long sum = 0;
			foreach (string range in ranges)
			{
				var split = range.Split('-');
				var low = Int64.Parse(split[0]);
				var high = Int64.Parse(split[1]);

				foreach (var i in Enumerable.Range(0, (int)(high - low + 1)))
				{
					var num = i + low;
					if(IsRepeating(num.ToString()))
					{
						sum += num;
						//Console.WriteLine($"{num} -> {sum}");
					}
				}
			}

			return sum;
		}
		
		static bool IsRepeating(string current)
		{
			for (int i = 1; i <= Math.Ceiling(current.Length / 2f); i++)
			{
				var repeat = current.Substring(0, i);
				var count = CountInstances(current, repeat);
				
				if(count == 2)
					return true;
			}

			return false;
		}

		static int CountInstances(string current, string repeat)
		{
			var count = 0;

			if(current.Length % repeat.Length != 0)
				return 0;
			
			for (int i = 0; i <= current.Length - repeat.Length; i += repeat.Length)
			{
				if(current.Substring(i, repeat.Length) == repeat)
					count += 1;
				else
					return 0;
			}

			return count;
		}
	}
	
	static class Part2
	{
		public static long Run(string[] ranges)
		{
			long sum = 0;
			foreach (string range in ranges)
			{
				var split = range.Split('-');
				var low = Int64.Parse(split[0]);
				var high = Int64.Parse(split[1]);

				foreach (var i in Enumerable.Range(0, (int)(high - low + 1)))
				{
					var num = i + low;
					if(IsRepeating(num.ToString()))
					{
						sum += num;
						//Console.WriteLine($"{num} -> {sum}");
					}
				}
			}

			return sum;
		}
		
		static bool IsRepeating(string current)
		{
			for (int i = 1; i <= Math.Ceiling(current.Length / 2f); i++)
			{
				var repeat = current.Substring(0, i);
				var count = CountInstances(current, repeat);
				
				if(count >= 2)
					return true;
			}

			return false;
		}

		static int CountInstances(string current, string repeat)
		{
			var count = 0;
			
			if(current.Length % repeat.Length != 0)
				return 0;
			
			for (int i = 0; i <= current.Length - repeat.Length; i += repeat.Length)
			{
				if(current.Substring(i, repeat.Length) == repeat)
					count += 1;
				else
					return 0;
			}

			return count;
		}
	}
}