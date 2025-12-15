
namespace AdventOfCode2025;

public static class Day11
{
	public class Devices
	{
		readonly Dictionary<string, List<string>> devices = new();

		public void Add(string name, List<string> connections) => devices[name] = connections;

		public List<string> GetConnections(string current) => devices[current];
	}
	public static void Run()
	{
		Console.WriteLine("Day 11!");
		var devices = ParseDevices(File.ReadAllLines($"Day11/Output.txt"));
		
		var sum = Part2.Run(devices);
		Console.WriteLine($"Finished part 1, password is {sum}");
	}

	static Devices ParseDevices(string[] lines)
	{
		var devices = new Devices();
		foreach(var line in lines)
		{
			var split = line.Split(": ");
			devices.Add(split[0], split[1].Split(' ').ToList());
		}

		return devices;
	}

	class Part1
	{
		public static long Run(Devices devices) => CountPaths(devices, "you");

		static long CountPaths(Devices devices, string current)
		{
			if(current == "out")
				return 1;
			return devices.GetConnections(current).Sum(x => CountPaths(devices, x));
		}
	}

	class Part2
	{
		static Devices devices;
		static Dictionary<string, List<Path>> cache = new();

		public static long Run(Devices devices)
		{
			Part2.devices = devices;
			return FindAllPaths("svr").Count(path => path.Contains("fft") && path.Contains("dac"));
		}

		static IEnumerable<Path> FindAllPaths(string current)
		{
			var currentPath = new Path().AddFront(current);
			if(current == "out")
			{
				yield return currentPath;
				yield break;
			}

			var cachedPaths = FindCachedPaths(current);
			if(cachedPaths != null)
			{
				foreach (Path path in cachedPaths)
					yield return path.Copy().AddFront(current);
				yield break;
			}

			var paths = devices.GetConnections(current).SelectMany(FindAllPaths);

			foreach(var finishedPath in paths)
			{
				var path = finishedPath.Copy().AddFront(current);
				CachePath(current, path);
				yield return path;
			}
		}

		static void CachePath(string current, Path path)
		{
			if(!cache.ContainsKey(current))
				cache.Add(current, []);
			cache[current].Add(path);
		}

		static List<Path>? FindCachedPaths(string current) => cache.GetValueOrDefault(current);

		class Path
		{
			List<Path> paths = new();

			public bool Contains(string machine) => paths.Contains(machine);

			public Path AddFront(string current)
			{
				paths.Insert(0, current);
				return this;
			}

			public Path Copy() => new() { paths = paths.ToList() };
		}
	}
}