public class Subway
{
    // 전체 역 (충정로 2호선, 충정로 5호선 별개 노드 취급)
    private readonly Dictionary<string, List<Station>> stations = new();

    // 역 추가
    public void AddStation(string name, string line)
    {
        Station station = new(name, line);

        if (!stations.ContainsKey(name))
            stations[name] = new List<Station>();

        stations[name].Add(station);
    }

    // 역 사이 연결
    public void ConnectStations(string name1, string line1, string name2, string line2, int time)
    {
        Station? src = FindStation(name1, line1);
        Station? dst = FindStation(name2, line2);

        if(src == null || dst == null)
            throw new ArgumentException("존재하지 않는 역입니다.");

        src.AddNeighbor(dst, time);
        dst.AddNeighbor(src, time);
    }

    // 해당 이름을 가진 역 List 가져오기 (충정로 2호선, 충정로 5호선 별개 노드 취급)
    public List<Station> FindStations(string name)
    {
        if (stations.TryGetValue(name, out List<Station>? result))
            return result;

        return new List<Station>();
    }

    // 특정 역 가져오기
    public Station? FindStation(string name, string line)
    {
        return FindStations(name).FirstOrDefault(station => station.Line == line);
    }

    public (List<Station> Path, int TotalTime) FindShortestPath(string startName, string endName)
    {
        throw new NotImplementedException();
    }
}