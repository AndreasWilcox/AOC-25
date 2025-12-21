
using System.Text.RegularExpressions;

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
		static int SearchedFound;
		static List<string> searchedMachines;
		static Devices devices;
		static readonly Dictionary<(string, int), PathTree> cache = new();

		public static long Run(Devices devices)
		{
			Part2.devices = devices;
			searchedMachines = ["fft", "dac"];
			SearchedFound = (1 << searchedMachines.Count) - 1;
			var searchThroughPaths = SearchThroughPaths("svr", 0);
			return searchThroughPaths.MatchedBranches;
		}

		static PathTree SearchThroughPaths(string current, int found)
		{
			var currentPath = new PathTree(current);
			if(current == "out")
			{
				currentPath.Branches = 1;
				return currentPath;
			}

			found |= MatchSearched(current);
			if(found == SearchedFound)
				currentPath.FoundBoth = true;
			
			var cachedTree = FindCachedPathTree(current, found);
			if(cachedTree != null)
				return cachedTree;

			var children = devices.GetConnections(current).Select(x => SearchThroughPaths(x, found));

			foreach(var child in children)
				currentPath.AddChildTree(child);
			
			return CachePath(current, found, currentPath);
		}

		static int MatchSearched(string current)
		{
			for(var i = 0; i < searchedMachines.Count; i++)
			{
				var searchedMachine = searchedMachines[i];
				if(searchedMachine == current)
					return 1 << i;
			}

			return 0;
		}

		static PathTree CachePath(string current, int found, PathTree child)
		{
			cache[(current, found)] = child;
			return child;
		}

		static PathTree? FindCachedPathTree(string current, int found) => cache.GetValueOrDefault((current, found));

		class PathTree(string current)
		{
			string name = current;
			readonly List<PathTree> paths = [];
			public long Branches;
			public long MatchedBranches;
			public bool FoundBoth;

			public void AddChildTree(PathTree child)
			{
				paths.Add(child);
				Branches += child.Branches;
				MatchedBranches += FoundBoth ? child.Branches : child.MatchedBranches;
			}
		}
	}
}