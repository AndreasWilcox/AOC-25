namespace AdventOfCode2025;

public static class Day7
{
	public static void Run()
	{
		Console.WriteLine("Day 7!");
		var mapLines = File.ReadAllLines($"Day7/Map.txt");
		var map = new char[mapLines[0].Length, mapLines.Length];
		for(var row = 0; row < mapLines.Length; row++)
		{
			for(var column = 0; column < mapLines[0].Length; column++)
				map[column, row] = mapLines[row][column];
		}
		
		var sum = Part1.Run(map);
		Console.WriteLine($"Finished part 1, password is {sum}");
	}

	class Part1
	{
		static char[,] readMap;
		static char[,] writeMap;
		static bool changed = true;

		public static long Run(char[,] map)
		{
			readMap = map;
			long sum = 0;

			writeMap = readMap.Clone() as char[,];

			while(changed)
			{
				changed = false;
				for(var x = 0; x < map.GetLength(0); x++)
				{
					for(var y = 0; y < map.GetLength(1); y++)
					{
						if(map[x, 0] == 'S')
							TrySet(x, 1, '|');
						
						if(Get(x, y) != '|')
							continue;

						var below = Get(x, y + 1);
						TrySet(x, y + 1, '|');
						if(below == '^')
						{
							var left = TrySet(x + 1, y + 1, '|');
							var right = TrySet(x - 1, y + 1, '|');
							if(left || right)
								sum += 1;
						}
					}
				}
				
				//Print(writeMap);
				//Console.WriteLine($"Sum: {sum}");

				readMap = writeMap.Clone() as char[,];
			}

			return sum;
		}

		static void Print(char[,] map)
		{
			for(var y = 0; y < map.GetLength(1); y++)
			{
				for(var x = 0; x < map.GetLength(0); x++)
				{
					Console.Write(map[x, y]);
				}
				Console.WriteLine();
			}
		}

		static char Get(int x, int y)
		{
			if(!IsValid(x, y))
				return '.';
			return readMap[x, y];
		}

		static bool TrySet(int x, int y, char character)
		{
			if(!IsValid(x, y) || readMap[x, y] != '.')
				return false;
			
			writeMap[x, y] = character;
			changed = true;
			return true;
		}

		static bool IsValid(int x, int y)
		{
			return x >= 0 && x < readMap.GetLength(0) && y >= 0 && y < readMap.GetLength(1);
		}
	}
	
	class Part2
	{
		public static long Run(long[,] map)
		{
			long sum = 0;
			return sum;
		}
	}
}