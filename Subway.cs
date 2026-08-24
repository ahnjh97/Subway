public class Subway
{
    // 전체 역 (충정로 2호선, 충정로 5호선 별개 노드 취급)
    private readonly Dictionary<string, List<Station>> stations = new();

    // 역 추가
    public void AddStation(string name, string line)
    {

    }

    // 역 사이 연결
    public void ConnectStations(string src, string dst, string line, int time)
    {

    }

    // 해당 이름을 가진 역 List 가져오기 (충정로 2호선, 충정로 5호선 별개 노드 취급)
    public List<Station> FindStations(string name)
    {
        throw new NotImplementedException();
    }

    public (List<Station> Path, int TotalTime) FindShortestPath(string startName, string endName)
    {
        throw new NotImplementedException();
    }
}