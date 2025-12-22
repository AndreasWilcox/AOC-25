namespace AdventOfCode2025;

public static class Day6
{
	public static void Run()
	{
		Console.WriteLine("Day 6!");
		string[] math = File.ReadAllLines($"Day6/TestMath.txt");

		Console.WriteLine($"Finished part 1, password is {Part1.Run(math)}");
		Console.WriteLine($"Finished part 2, password is {Part2.Run(math)}");
	}

	static long[] GetLineNumbers(string line) => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

	static class Part1
	{
		public static long Run(string[] math)
		{
			var rows = math.Length;
			var columns = GetLineNumbers(math[0]).Length;
			long[,] numbers = new long[columns, rows - 1];
		
			for(var y = 0; y < rows - 1; y++)
			{
				var line = GetLineNumbers(math[y]);
				SetLine(numbers, line, y);
			}
		
			char[] operations = math[rows - 1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(char.Parse).ToArray();
			
			var sums = new long[numbers.GetLength(0)];

			for(var column = 0; column < sums.Length; column++)
			{
				if(operations[column] == '+')
					sums[column] = GetColumn(numbers, column).Aggregate((x, y) => x + y);
				else if(operations[column] == '*')
					sums[column] = GetColumn(numbers, column).Aggregate((x, y) => x * y);
			}

			return sums.Sum();
		}

		static void SetLine(long[,] numbers, long[] line, int row)
		{
			for(var x = 0; x < line.Length; x++)
				numbers[x, row] = line[x];
		}

		static long[] GetColumn(long[,] numbers, int column)
		{
			var length = numbers.GetLength(1);
			var values = new long[length];
			for(var y = 0; y < length; y++)
				values[y] = numbers[column, y];
			return values;
		}
	}
	
	static class Part2
	{
		public static long Run(string[] math)
		{
			var data = math.Select(x => x.ToArray()).ToArray();
			var numberLines = math.Length - 1;
			var operations = math[^1];
			var sums = new long[data[0].Length];

			var sumIndex = -1;
			var currentOperation = ' ';
			var numbers = new List<long>();
			for(var column = 0; ; column++)
			{
				var isFinished = column == operations.Length;
				if(isFinished)
				{
					sums[sumIndex] = CalculateOperation(currentOperation, numbers);
					break;
				}
				
				if(operations[column] != ' ')
				{
					if(sumIndex >= 0)
						sums[sumIndex] = CalculateOperation(currentOperation, numbers);
					numbers.Clear();
					currentOperation = operations[column];
					sumIndex += 1;
				}
				
				var number = "";
				for(var row = 0; row < numberLines; row++)
				{
					var value = data[row][column];
					if(value != ' ')
						number += value;
				}
				if(number.Length > 0)
					numbers.Add(long.Parse(number));
			}

			return sums.Sum();
		}

		static long CalculateOperation(char operation, List<long> numbers)
		{
			return operation switch
			{
				'+' => numbers.Aggregate((x, y) => x + y),
				'*' => numbers.Aggregate((x, y) => x * y),
				_ => 0
			};
		}
	}
}