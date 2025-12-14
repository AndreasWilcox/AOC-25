
using System.Numerics;

namespace AdventOfCode2025;

public static class Day9
{
	public static void Run()
	{
		Console.WriteLine("Day 9!");
		var tiles = File.ReadAllLines($"Day9/Tiles.txt")
			.Select(IntVector2.Parse)
			.ToList();
		
		var sum = Part2.Run(tiles);
		Console.WriteLine($"Finished part 2, password is {sum}");
	}
	
	static long GetSize(IntVector2 a, IntVector2 b) => (long)(Math.Abs(a.X - b.X) + 1) * (Math.Abs(a.Y - b.Y) + 1);

	class Part1
	{
		public static long Run(List<IntVector2> tiles)
		{
			long largest = 0;
			var remaining = tiles.ToList();
			foreach (var tile in tiles)
			{
				foreach (IntVector2 other in remaining)
				{
					var size = GetSize(tile, other);
					if(size > largest)
						largest = size;
				}
			}

			return largest;
		}

	}

	class Part2
	{
		class Line(IntVector2 start, IntVector2 end)
		{
			public readonly IntVector2 Start = start;
			public readonly IntVector2 End = end;
		}
		
		public static long Run(List<IntVector2> tiles)
		{
			var areaLines = GenerateAreaLines(tiles);

			long sum = 0;
			
			var options = tiles.SelectMany(
					(a, i) => tiles
						.Skip(i + 1)
						.Select(b => new Tuple<IntVector2, IntVector2, double>(a, b, GetSize(b, a))))
				.OrderByDescending(x => x.Item3)
				.ToList();

			foreach(var option in options)
			{
				if(AnyLinesCollideSHOULDBEWRONG(areaLines, option.Item1, option.Item2))
					continue;

				Console.WriteLine($"Found between {option.Item1} and {option.Item2} with size {option.Item3}");
				return (long)option.Item3;
			}

			return sum;
		}

		static bool AnyLinesCollideSHOULDBEWRONG(List<Line> areaLines, IntVector2 rectStart, IntVector2 rectEnd)
		{
			//var rectPoints = GenerateRectanglePoints(rectStart, rectEnd).ToList();
			foreach(var area in areaLines)
			{
				if(Intersects(rectStart, rectEnd, area))
					return true;
			}

			return false;
		}

		static bool AnyLinesCollide(List<Line> areaLines, IntVector2 rectStart, IntVector2 rectEnd)
		{
			var rectPoints = GenerateRectanglePoints(rectStart, rectEnd).ToList();
			foreach(var area in areaLines)
			{
				if(rectPoints.Any(x => PointToTheLeftOfLine(x, area)))
					return true;
			}

			return false;
		}

		static bool Intersects(IntVector2 rectStart, IntVector2 rectEnd, Line areaLine)
		{
			var lMinX = Math.Min(areaLine.Start.X, areaLine.End.X);
			var lMinY = Math.Min(areaLine.Start.Y, areaLine.End.Y);
			var lMaxX = Math.Max(areaLine.Start.X, areaLine.End.X);
			var lMaxY = Math.Max(areaLine.Start.Y, areaLine.End.Y);
			
			var rMinX = Math.Min(rectStart.X, rectEnd.X);
			var rMinY = Math.Min(rectStart.Y, rectEnd.Y);
			var rMaxX = Math.Max(rectStart.X, rectEnd.X);
			var rMaxY = Math.Max(rectStart.Y, rectEnd.Y);

			return rMinX < lMaxX && rMaxX > lMinX && rMinY < lMaxY && rMaxY > lMinY;
		}

		static bool PointToTheLeftOfLine(IntVector2 point, Line line)
		{
			var toPoint = point - line.Start;
			var toEnd = line.End - line.Start;
			
			// Check if 'outside' line
			var t = (float)(toPoint.X*toEnd.X + toPoint.Y*toEnd.Y) / (toEnd.X*toEnd.X + toEnd.Y*toEnd.Y);
			if(t is < 0 or > 1)
				return false;

			// Cross
			var result = (line.End.X - line.Start.X)*(point.Y - line.Start.Y) - (line.End.Y - line.Start.Y)*(point.X - line.Start.X);
			var ret = result < 0;
			return ret;
		}

		static List<Line> GenerateAreaLines(List<IntVector2> tiles)
		{
			var areaLines = new List<Line>();
			var last = tiles.Last();
			foreach(var tile in tiles)
			{
				areaLines.Add(new Line(last, tile));
				last = tile;
			}

			return areaLines;
		}

		static IEnumerable<IntVector2> GenerateRectanglePoints(IntVector2 optionItem1, IntVector2 optionItem2)
		{
			yield return new IntVector2(optionItem1.X, optionItem1.Y);
			yield return new IntVector2(optionItem2.X, optionItem1.Y);
			yield return new IntVector2(optionItem2.X, optionItem2.Y);
			yield return new IntVector2(optionItem1.X, optionItem2.Y);
		}
	}
}