Subway subway = new();

subway.AddStation("강남", "2호선");
subway.AddStation("강남", "신분당선");

subway.ConnectStations("강남", "2호선", "강남", "신분당선", 180);

var stations = subway.FindStations("강남");

foreach (Station station in stations)
{
    Console.WriteLine(station.Name + " " + station.Line);
    foreach(var kvp in station.Neighbors)
    {
        Station neighbor = kvp.Key;
        Console.WriteLine("이웃: " + neighbor.Name + " " + neighbor.Line + ", 소요시간: " + kvp.Value);
    }
    Console.WriteLine();
}

try
{
    subway.FindShortestPath("교대", "교대");
}
catch(ArgumentException e)
{
    Console.WriteLine("오류: " + e.Message);
}
    
