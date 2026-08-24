public class Station(string name, string line)
{
    public string Name { get; } = name;
    public string Line { get; } = line;

    public Dictionary<Station, int> Neighbors { get; } = new();

    public void AddNeighbor(Station target, int time)
    {
        Neighbors[target] = time;
    }
}