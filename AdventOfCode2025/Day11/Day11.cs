
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

		public static long Run(Devices devices)
		{
			Part2.devices = devices;
			return FindAllPaths("svr", new Path(["svr"])).Count(path => path.Contains("fft") && path.Contains("dac"));
		}

		static List<Path> FindAllPaths(Path path, string current)
		{
			if(current == "out")
				return [new Path([current])];

			//var existingPath = FindCachedPath(current);
			//if(existingPath != null)
			//return [path.Concat(existingPath).ToList()];

			var paths = devices.GetConnections(current).SelectMany(x => FindAllPaths(path, x));

			foreach(var finishedPath in paths)
			{
				CachePath(finishedPath);
			}
		}

		static void CachePath(List<string> path)
		{
		}

		static List<string> FindCachedPath(string current)
		{
			throw new NotImplementedException();
		}

		class Path(List<string> list)
		{
			List<string> path = list;

			public bool Contains(string machine) => path.Contains(machine);

			public Path Concat(string machine) => new Path(path.Concat([machine]).ToList());

			public bool SequenceEquals(Path other) => path.SequenceEqual(other.path);
		}
	}
}