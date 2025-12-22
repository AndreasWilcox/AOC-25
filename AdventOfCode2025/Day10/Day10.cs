
using LpSolveDotNet;

namespace AdventOfCode2025;

public static class Day10
{
	public static void Run()
	{
		Console.WriteLine("Day 10!");
		var machines = File.ReadAllLines($"Day10/TestMachines.txt")
			.Select(ParseMachineInfo)
			.ToList();

		Console.WriteLine($"Finished part 1, password is {Part1.Run(machines)}");
		Console.WriteLine($"Finished part 2, password is {Part2.Run(machines)}");
	}

	static Machine ParseMachineInfo(string info)
	{
		info = info.Replace("[", "").Replace("}", "").Replace("(", "").Replace(")", "");
		var split = info.Split(new[] { ']', '{' });
		var diagram = split[0].ToList().Select(c => c == '#').ToArray();
		var buttons = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ReadButton).ToList();
		var joltage = split[2].Split(',').Select(int.Parse).ToArray();
		return new Machine(new LightState(diagram), buttons, joltage);
	}

	static Button ReadButton(string change) => new(change.Split(',').Select(int.Parse));

	static class Part1
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

	static class Part2
	{
		public static long Run(List<Machine> machines)
		{
			LpSolve.Init();
			return machines.Sum(GetShortestSolution);
		}
		
		static long GetShortestSolution(Machine machine)
		{
			int ignored = 0;
			int numButtons = machine.Buttons.Count;
			int numStates  = machine.JoltageDiagram.Length;
			using var lp = LpSolve.make_lp(0, numButtons);
			lp.set_minim();
			double[] obj = Enumerable.Repeat(1.0, numButtons).Prepend(ignored).ToArray();
			lp.set_obj_fn(obj);
			
			for (int i = 0; i < numButtons; i++)
			{
				lp.set_int(i + 1, true);
				lp.set_lowbo(i + 1, 0);
			}
			
			lp.set_add_rowmode(true);
			for (int i = 0; i < numStates; i++)
			{
				double[] row = machine.Buttons
					.Select(b => b.Numbers.Contains(i) ? 1.0 : 0.0)
					.Prepend(ignored)
					.ToArray();

				lp.add_constraint(row, lpsolve_constr_types.EQ, machine.JoltageDiagram[i]);
			}
			lp.set_add_rowmode(false);
			
			//lp.print_lp();
			
			lp.set_verbose(lpsolve_verbosity.IMPORTANT);

			var s = lp.solve();
			if(s != lpsolve_return.OPTIMAL)
			{
				lp.print_lp();
				throw new Exception("NO OPTIMAL SOLUTION FOUND");
			}

			//PrintPresses(numButtons, lp);
			return (long)(lp.get_objective() + 0.5f);
		}

		static void PrintPresses(int numButtons, LpSolve lp)
		{
			var results = new double[numButtons];
			lp.get_variables(results);
			for (int j = 0; j < numButtons; j++)
			{
				Console.WriteLine(lp.get_col_name(j + 1) + ": " + results[j]);
			}
		}

		static double[] AddIgnored(List<double> row)
		{
			const int Ignored = 0;
			return new List<double>() { Ignored }.Concat(row).ToArray();
		}
	}
}