namespace AdventOfCode2025;


internal class LightState
{
	public int Count;
	readonly bool[] state;
	
	public LightState(bool[] state)
	{
		this.state = state;
	}

	public LightState(int length)
	{
		state = new bool[length];
	}

	LightState(LightState other)
	{
		Count = other.Count;
		state = other.state.ToArray();
	}

	public int Length() => state.Length;

	public LightState Modify(Button button)
	{
		var copy = new LightState(this);
		foreach (int index in button.Numbers)
			copy.state[index] = !copy.state[index];
		copy.Count += 1;
		return copy;
	}

	bool Equals(LightState other) => state.SequenceEqual(other.state);
	public override bool Equals(object? obj)
	{
		if(obj is null) return false;
		if(ReferenceEquals(this, obj)) return true;
		if(obj.GetType() != GetType()) return false;
		return Equals((LightState)obj);
	}
	public override int GetHashCode() => state.GetHashCode();
}

internal class JoltageState
{
	public int Count;
	readonly int[] state;
	
	public JoltageState(int[] state)
	{
		this.state = state;
	}

	public JoltageState(int length)
	{
		state = new int[length];
	}

	JoltageState(JoltageState other)
	{
		Count = other.Count;
		state = other.state.ToArray();
	}

	public int Length() => state.Length;

	public JoltageState Modify(Button button)
	{
		var copy = new JoltageState(this);
		foreach (int index in button.Numbers)
			copy.state[index] += 1;
		copy.Count += 1;
		return copy;
	}

	public bool EqualsState(JoltageState other) => state.SequenceEqual(other.state);

	public bool IsAnyTooHigh(JoltageState other)
	{
		for (int i = 0; i < state.Length; i++)
		{
			if(state[i] > other.state[i])
				return true;
		}
		return false;
	}

	public int StateDiff(JoltageState targetState)
	{
		var sum = 0;
		for (int i = 0; i < state.Length; i++)
		{
			sum += targetState.state[i] - state[i];
		}
		return sum;
	}
}

internal class Machine(LightState lightsDiagram, List<Button> buttons, JoltageState joltageDiagram)
{
	public readonly LightState LightsDiagram = lightsDiagram;
	public readonly List<Button> Buttons = buttons;
	public readonly JoltageState JoltageDiagram = joltageDiagram;
}

internal class Button(IEnumerable<int> numbers)
{
	public List<int> Numbers { get; } = numbers.ToList();
}