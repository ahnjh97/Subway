Subway subway = new();

// 호선별 역 이름
Dictionary<string, List<string>> lineStations = new()
{
    { "1호선", new List<string> { "용산", "남영", "서울역", "시청", "종각", 
        "종로3가", "종로5가", "동대문", "동묘앞", "신설동",
        "제기동", "청량리" } },
    { "2호선", new List<string> { "당산", "합정", "홍대입구", "신촌", "이대",
        "아현", "충정로", "시청", "을지로입구", "을지로3가",
        "을지로4가", "동대문역사문화공원", "신당", "상왕십리", "왕십리",
        "한양대" } },
    { "3호선", new List<string> { "경복궁", "안국", "종로3가", "을지로3가", "충무로",
        "동대입구", "약수", "금호", "옥수" } },
    { "4호선", new List<string> { "이촌", "신용산", "삼각지", "숙대입구", "서울역",
        "회현", "명동", "충무로", "동대문역사문화공원", "동대문",
        "혜화" } },
    { "5호선", new List<string> { "마포", "공덕", "애오개", "충정로", "서대문",
        "광화문", "종로3가", "을지로4가", "동대문역사문화공원", "청구",
        "신금호", "행당", "왕십리", "마장" } },
    { "6호선", new List<string> { "망원", "합정", "상수", "광흥창", "대흥",
        "공덕", "효창공원앞", "삼각지", "녹사평", "이태원",
        "한강진", "버티고개", "약수", "청구", "신당",
        "동묘앞", "창신" } }
};

// 호선별 구간시간
Dictionary<string, List<int>> lineTimes = new()
{
    { "1호선", new List<int> { 110, 120, 120, 100, 90, 90, 90, 80, 80, 90, 100 } },
    { "2호선", new List<int> { 170, 100, 110, 90, 90, 90, 110, 90, 90, 80, 100, 100, 100, 90, 100 } },
    { "3호선", new List<int> { 100, 90, 70, 80, 100, 90, 90, 90 } },
    { "4호선", new List<int> { 100, 90, 100, 100, 90, 90, 80, 100, 90, 90 } },
    { "5호선", new List<int> { 100, 110, 100, 90, 120, 100, 90, 90, 100, 100, 100, 100, 100 } },
    { "6호선", new List<int> { 100, 100, 100, 100, 110, 100, 130, 110, 90, 100, 110, 90, 90, 90, 100, 90 } }
};

// 역 객체 생성 및 같은 호선끼리 연결
foreach (var (lineName, stationNames) in lineStations)
{
    if (!lineTimes.TryGetValue(lineName, out var times))
        continue;

    // 역 이름 개수가 구간시간 개수보다 1개 더 많아야 함
    if(stationNames.Count != times.Count + 1)
    {
        Console.WriteLine(lineName + "의 정보가 잘못되었습니다.");
        return;
    }

    // 역 객체 생성 및 추가
    foreach (var stationName in stationNames)
    {
        subway.AddStation(stationName, lineName);
    }

    // 같은 호선의 인접 역 연결
    for (int i = 0; i < times.Count; i++)
    {
        subway.ConnectStations(stationNames[i], lineName, stationNames[i + 1], lineName, times[i]);
    }
}

// 환승 연결 (ex: 서울역1호선 - 서울역 4호선)
var lines = lineStations.ToList();

for(int i = 0; i < lines.Count; i++)
{
    var (srcLineName, srcStationNames) = lines[i];

    for(int j = i + 1; j < lines.Count; j++)
    {
        var (dstLineName, dstStationNames) = lines[j];

        foreach (var stationName in srcStationNames)
        {
            if (dstStationNames.Contains(stationName))
            {
                subway.ConnectStations(
                    stationName, srcLineName,
                    stationName, dstLineName,
                    180
                );
            }
        }
    }
}

while (true)
{
    Console.Write("출발 역: ");
    string? src = Console.ReadLine();

    Console.Write("도착 역: ");
    string? dst = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(src) || string.IsNullOrWhiteSpace(dst))
    {
        Console.WriteLine("역 이름을 입력해주세요.");
        continue;
    }

    try
    {
        src = src.Trim();
        dst = dst.Trim();

        var (path, time) = subway.FindShortestPath(src, dst);
        Console.WriteLine("[탐색 결과] " + src + " -> " + dst);
        Console.Write("이동경로: ");

        for(int i = 0; i < path.Count; i++)
        {
            Console.Write(path[i].Name);

            // 다음 노드가 같은 역인데 호선이 다르면 환승
            if (i < path.Count - 1 &&
                path[i].Name == path[i + 1].Name &&
                path[i].Line != path[i + 1].Line)
            {
                Console.Write("(환승)");

                // 다음 노드는 같은 역이므로 건너뜀
                i++;
            }

            if (i < path.Count - 1)
                Console.Write("->");
        }

        Console.WriteLine();
        Console.WriteLine("총 소요 시간: " + time / 60 + "분 " + time % 60 + "초");
        break;
    }
    catch (ArgumentException e)
    {
        Console.WriteLine(e.Message);
        Console.WriteLine();
    }
    catch (InvalidOperationException e)
    {
        Console.WriteLine(e.Message);
        Console.WriteLine();
    }
}