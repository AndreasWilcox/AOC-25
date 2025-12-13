using System.Diagnostics.CodeAnalysis;

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
		
		var sum = Part2.Run(map);
		Console.WriteLine($"Finished part 2, password is {sum}");
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
		static char[,] readMap;
		static List<TreeNode> toVisit = new();
		static Dictionary<IntVector2, TreeNode> visited = new();
		static long sum;

		public static long Run(char[,] map)
		{
			readMap = map;
			sum = 0;

			var start = new TreeNode();
			for(var x = 0; x < map.GetLength(0); x++)
				if(map[x, 0] == 'S')
					start = new TreeNode { Pos = FindBelow(new IntVector2(x, 0)).Value };
			
			toVisit.Add(start);
			while(toVisit.Count > 0)
			{
				var node = toVisit[0];
				toVisit.RemoveAt(0);
				var leftPos = FindBelow(new IntVector2(node.Pos.X - 1, node.Pos.Y));
				if(leftPos.HasValue)
					AddChild(leftPos, node, true);

				var rightPos = FindBelow(new IntVector2(node.Pos.X + 1, node.Pos.Y));
				if(rightPos.HasValue)
					AddChild(rightPos, node, false);

				visited.TryAdd(node.Pos, node);
			}
			
			sum = Visit(start);
			
			return sum;
		}

		static void AddChild([DisallowNull] IntVector2? position, TreeNode node, bool addToStart)
		{
			var child = new TreeNode { Pos = position.Value };
			if(visited.TryGetValue(position.Value, out var value))
				child = value;
			else if(addToStart)
				toVisit.Insert(0, child);
			else
				toVisit.Add(child);

			visited.TryAdd(child.Pos, child);
			node.TryAdd(child);
		}

		static IntVector2? FindBelow(IntVector2 p)
		{
			if(p.X < 0 || p.X >= readMap.GetLength(0))
				return null;
			
			for(var dy = 1; p.Y + dy < readMap.GetLength(1); dy++)
			{
				var pos = new IntVector2(p.X, p.Y + dy);
				
				if(readMap[p.X, p.Y + dy] == '^')
				{
					return pos;
				}
			}

			return null;
		}

		static long Visit(TreeNode node)
		{
			if(node.Value >= 0)
				return node.Value;
				
			long value = Math.Max(0, 2 - node.Children.Count);
			foreach(var child in node.Children)
				value += Visit(child);
			node.Value = value;
			return value;
		}
	}

	class TreeNode
	{
		public IntVector2 Pos;
		public readonly List<TreeNode> Children = new();
		public long Value { get; set; } = -1;

		public void TryAdd(TreeNode child)
		{
			foreach(var c in child.Children)
				if(c.Pos.Equals(child.Pos))
					return;
			
			Children.Add(child);
		}
	}
}