Subway subway = new();

subway.AddStation("강남", "2호선");
subway.AddStation("강남", "신분당선");

var stations = subway.FindStations("강남");

foreach (Station station in stations)
    Console.WriteLine(station.Name + " " + station.Line);
