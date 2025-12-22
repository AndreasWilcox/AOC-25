using System.Diagnostics;

namespace AdventOfCode2025;

public static class Day1
{
	public static void Run()
	{
		Console.WriteLine("Day 1!");
		string[] readAllLines = File.ReadAllLines("Day1/TestRotations.txt");

		Method1(readAllLines);
		Method2(readAllLines);
	}

	static void Method1(string[] readAllLines)
	{
		var num = 50;
		var count = 0;
		
		foreach (var line in readAllLines)
		{
			try
			{
				var direction = line[0];
				var change = Int32.Parse(line.Substring(1));

				change *= direction == 'L' ? -1 : 1;
				num = (num + change + 100) % 100;
				if(num == 0)
					count += 1;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		Console.WriteLine($"Finished part 1, password is {count}");
	}

	static void Method2(string[] readAllLines)
	{
		var num = 50;
		var count = 0;

		for (int i = 0; i < readAllLines.Length; i++)
		{
			string line = readAllLines[i];
			var oldNum = num;
			var direction = line[0];
			var change = Int32.Parse(line.Substring(1));

			change *= direction == 'L' ? -1 : 1;
			count += abs(change / 100);
			change %= 100;
			var newNum = num + change;

			var high = change > 0 ? newNum : num;
			var low = change < 0 ? newNum : num;
			var passed = Inbetween(0, high, low) || Inbetween(100, high, low);
			if(oldNum != 0 && passed)
				count += 1;

			num = (newNum + 100) % 100;

			//Console.WriteLine($"[{i + 1}] {change}, {oldNum} to {num}. Count {count}");
		}

		Console.WriteLine($"Finished part 2, password is {count}");
	}

	static int abs(int val) => val < 0 ? -val : val;

	static bool Inbetween(int val, int high, int low) => val <= high && val >= low;
}