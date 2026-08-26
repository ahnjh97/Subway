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

        if (src == null || dst == null)
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

    // 최단시간 및 경로 찾기
    public (List<Station> Path, int TotalTime) FindShortestPath(string startName, string endName)
    {
        List<Station> startStations = FindStations(startName);
        List<Station> endStations = FindStations(endName);

        // 예외처리
        const String errorStr = "존재하지 않는 역입니다.";

        if (startStations.Count == 0 && endStations.Count == 0)
            throw new ArgumentException("출발역과 도착역이 " + errorStr);
        else if (startStations.Count == 0)
            throw new ArgumentException("출발역이 " + errorStr);
        else if (endStations.Count == 0)
            throw new ArgumentException("도착역이 " + errorStr);

        if (startName == endName)
            throw new ArgumentException("출발역과 도착역이 동일합니다.");

        // 다익스트라
        Dictionary<Station, int> distances = new();
        Dictionary<Station, Station?> previous = new();

        PriorityQueue<Station, int> pq = new();

        foreach (List<Station> stations in stations.Values)
        {
            foreach (Station station in stations)
            {
                distances[station] = int.MaxValue;
                previous[station] = null;
            }
        }

        // 충정로 2호선, 충정로 5호선 처럼 모든 출발역 후보를 시작점으로 추가
        foreach (Station src in startStations)
        {
            distances[src] = 0;
            pq.Enqueue(src, 0);
        }

        Station? dst = null;

        while (pq.TryDequeue(out Station? cur, out int curDist))
        {
            if (cur == null)
                continue;

            if (curDist > distances[cur])
                continue;

            // 도착역 후보 중 하나에 도착했다면 종료
            if (endStations.Contains(cur))
            {
                dst = cur;
                break;
            }

            foreach (var neighbor in cur.Neighbors)
            {
                Station next = neighbor.Key;
                int travelTime = neighbor.Value;

                int newDist = curDist + travelTime;

                if (newDist < distances[next])
                {
                    distances[next] = newDist;
                    previous[next] = cur;

                    pq.Enqueue(next, newDist);
                }
            }
        }

        // 도착역까지 갈 수 없는 경우
        if (dst == null)
            throw new InvalidOperationException("도착역까지 이동할 수 없습니다.");

        // 최단 경로 복원
        List<Station> path = new();

        Station? curStation = dst;

        while (curStation != null)
        {
            path.Add(curStation);
            curStation = previous[curStation];
        }

        // 도착 -> 출발 순서로 들어갔으므로 뒤집기
        path.Reverse();

        return (path, distances[dst]);
    }
}