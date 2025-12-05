using System.Diagnostics;

namespace AdventOfCode2025;

public static class Day4
{
	public static void Run()
	{
		Console.WriteLine("Day 4!");
		string[] layoutLines = File.ReadAllLines($"Day4/Layout.txt");
		var layout = CreateLayout(layoutLines);
		var sum = Part2.Run(layout);
		Console.WriteLine($"Finished part 2, password is {sum}");
	}

	static int[,] CreateLayout(string[] layoutLines)
	{
		var layout = new int[layoutLines[0].Length, layoutLines.Length];
		for (int i = 0; i < layoutLines.Length; i++)
		{
			for (int j = 0; j < layoutLines[i].Length; j++)
			{
				layout[j, i] = layoutLines[j][i];
				if(layout[j, i] == '@')
					layout[j, i] = 100;
				else
					layout[j, i] = 0;
			}
		}

		return layout;
	}
	
	static void PrintLayout(int[,] layout)
	{
		Console.WriteLine();
		for (int x = 0; x < layout.GetLength(0); x++)
		{
			for (int y = 0; y < layout.GetLength(1); y++)
			{
				if(layout[x, y] < 5 && layout[x, y] > 0)
					Console.Write("x");
				else if(layout[x, y] > 0)
					Console.Write("@");
				else
					Console.Write(".");
			}
			Console.WriteLine();
		}
	}
	
	static int CountSurrounding(int[,] layout, int x, int y)
	{
		var count = 0;
		for (int dx = -1; dx <= 1; dx++)
		{
			for (int dy = -1; dy <= 1; dy++)
			{
				var newX = x + dx;
				var newY = y + dy;
				if(newX < 0 || newX >= layout.GetLength(0) || newY < 0 || newY >= layout.GetLength(1))
					continue;
					
				if(layout[x + dx, y + dy] > 0)
					count += 1;
			}
		}

		return count;
	}
	
	static int CalculateSurrounding(int[,] layout)
	{
		var sum = 0;
		for (int x = 0; x < layout.GetLength(0); x++)
		{
			for (int y = 0; y < layout.GetLength(1); y++)
			{
				if(layout[x, y] <= 0)
					continue;
				layout[x, y] = CountSurrounding(layout, x, y);
				if(layout[x, y] < 5)
					sum += 1;
			}
		}

		return sum;
	}

	class Part1
	{
		public static long Run(int[,] layout)
		{
			long sum = 0;
			
			PrintLayout(layout);

			sum += CalculateSurrounding(layout);
			
			PrintLayout(layout);

			return sum;
		}


	}
	
	class Part2
	{
		public static long Run(int[,] layout)
		{
			long sum = 0;

			PrintLayout(layout);

			while(true)
			{
				int count = CalculateSurrounding(layout);
				if(count > 0)
				{
					sum += count;
					RemoveFree(layout);
				}
				else
				{
					break;
				}
			}
			
			PrintLayout(layout);

			return sum;
		}

		static void RemoveFree(int[,] layout)
		{
			for (int x = 0; x < layout.GetLength(0); x++)
			{
				for (int y = 0; y < layout.GetLength(1); y++)
				{
					if(layout[x, y] < 5)
						layout[x, y] = 0;
				}
			}
		}
	}
}