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

internal class Machine(LightState lightsDiagram, List<Button> buttons, int[] joltageDiagram)
{
	public readonly LightState LightsDiagram = lightsDiagram;
	public readonly List<Button> Buttons = buttons;
	public readonly int[] JoltageDiagram = joltageDiagram;
}

internal class Button(IEnumerable<int> numbers)
{
	public List<int> Numbers { get; } = numbers.ToList();
}