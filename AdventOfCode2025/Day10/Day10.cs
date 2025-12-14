
namespace AdventOfCode2025;

public static class Day10
{
	public static void Run()
	{
		Console.WriteLine("Day 10!");
		var machines = File.ReadAllLines($"Day10/Machines.txt")
			.Select(ParseMachineInfo)
			.ToList();
		
		var sum = Part2.Run(machines);
		Console.WriteLine($"Finished part 2, password is {sum}");
	}

	static Machine ParseMachineInfo(string info)
	{
		info = info.Replace("[", "").Replace("}", "").Replace("(", "").Replace(")", "");
		var split = info.Split(new[] { ']', '{' });
		var diagram = split[0].ToList().Select(c => c == '#').ToArray();
		var buttons = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ReadButton).ToList();
		var joltage = split[2].Split(',').Select(int.Parse).ToArray();
		return new Machine(new LightState(diagram), buttons, new JoltageState(joltage));
	}

	static Button ReadButton(string change) => new(change.Split(',').Select(int.Parse));

	class Part1
	{
		public static long Run(List<Machine> machines)
		{
			return machines.Sum(GetShortestSolution);
		}

		static long GetShortestSolution(Machine machine)
		{
			var q = new Queue<LightState>();
			q.Enqueue(new LightState(machine.LightsDiagram.Length()));

			while (true)
			{
				var state = q.Dequeue();
				foreach (LightState modified in machine.Buttons.Select(button => state.Modify(button)))
				{
					if(modified.Equals(machine.LightsDiagram))
						return modified.Count;
					q.Enqueue(modified);
				}
			}
		}
	}

	class Part2
	{
		public static long Run(List<Machine> machines)
		{
			long sum = 0;
			for (int i = 0; i < machines.Count; i++)
			{
				var v = GetShortestSolution(machines[i]);
				sum += v;
			}
			return sum;
		}
		
		static long GetShortestSolution(Machine machine)
		{
			var states = new List<JoltageState> { new(machine.JoltageDiagram.Length()) };

			while (true)
			{
				
				/*states.Sort((x, y) => x.Count-y.Count + (x.StateDiff(machine.JoltageDiagram) - y.StateDiff(machine.JoltageDiagram)));
				var state = states[0];
				states.RemoveAt(0);
				foreach (JoltageState modified in machine.Buttons.Select(button => state.Modify(button)))
				{
					if(modified.EqualsState(machine.JoltageDiagram))
						return modified.Count;
					if(modified.IsAnyTooHigh(machine.JoltageDiagram))
						continue;

					var sameCount = states.Count(x => modified.EqualsState(x));
					states.RemoveAll(x => modified.EqualsState(x) && modified.Count < sameCount);
					if(sameCount != 0 && sameCount != states.Count)
						continue;
					
					states.Add(modified);
				}*/
			}
		}
	}
}