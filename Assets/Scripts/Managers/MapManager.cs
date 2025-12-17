using UnityEngine; // 유니티 기본 네임스페이스
using System.Collections.Generic; // 리스트, 딕셔너리 사용을 위해 필수

public class MapManager : MonoBehaviour
{
    [Header("Map Settings")]
    public GameObject tilePrefab; // Kenney 3D 타일 프리팹을 여기에 연결
    public int mapWidth = 10;     // 가로 크기
    public int mapHeight = 10;    // 세로 크기

    [Header("Tile Spacing (조절 필요)")]
    // Kenney 에셋 기준 대략적인 값입니다. 
    // 타일 사이가 벌어지거나 겹치면 이 값을 인스펙터에서 미세조정하세요.
   [SerializeField]
    private float xOffset = 1.732f; // 가로 간격 (루트3)
    [SerializeField]
    private float zOffset = 1.5f;   // 세로 간격 (1.5)

    // 생성된 타일들을 저장해두는 딕셔너리 (좌표로 타일 찾기용)
    // 키: "x,y" 문자열, 값: 실제 타일 스크립트
    private Dictionary<string, HexTile> tileMap = new Dictionary<string, HexTile>();

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        // 맵이 이미 있다면 정리 (재시작 시 오류 방지)
        foreach(Transform child in transform) //이터레이터 순회
        {
            Destroy(child.gameObject);
        }
        tileMap.Clear();

        // (0,0) 타일을 중심으로 만들기 위해 시작 인덱스를 음수로 설정
        int startX = -mapWidth / 2;
        int endX = startX + mapWidth;
        int startY = -mapHeight / 2;
        int endY = startY + mapHeight;

        // 이중 반복문으로 격자 생성
        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                // 1. 육각형 배치를 위한 좌표 계산 (Offset Coordinate)
                float xPos = x * xOffset;
                float zPos = y * zOffset;
                //float randomYvalue = Random.Range(0f, 0.5f); 
                float yPos = 0f; 

                if(x == 0 && y == 0)
                {
                    yPos += 0.3f;
                }
    
                // 2. 홀수 행(y가 1, 3, 5... 또는 -1, -3...)일 때는 가로로 반 칸 밀어주기
                if (y % 2 != 0)
                {
                    xPos += xOffset / 2f;
                }

                if(x == -4 && y == -3)
                {
                    yPos += 0.2f;
                }
                
                // 3. 3D 월드 좌표 생성 (바닥에 깔리니까 y는 0)
                Vector3 spawnPos = new Vector3(xPos, yPos, zPos);
                // 4. 타일 생성 (Instantiate)
                GameObject newTileObj = Instantiate(tilePrefab, spawnPos, Quaternion.identity);
                
                // 생성된 타일을 MapManager의 자식으로 정리
                newTileObj.transform.SetParent(this.transform);

                // 5. HexTile 데이터 세팅 및 저장
                HexTile tileScript = newTileObj.GetComponent<HexTile>(); // 프리팹에 안 붙여놨다면 여기서 붙임
                tileScript.Init(x, y);

                // 딕셔너리에 저장 (나중에 "2,3 타일 줘!" 하면 바로 찾기 위함)
                tileMap.Add($"{x},{y}", tileScript);
            }
        }
    }
    
    // 나중에 쓸 함수: 좌표로 타일 가져오기
    public HexTile GetTileAt(int x, int y)
    {
        string key = $"{x},{y}";
        if (tileMap.ContainsKey(key))
            return tileMap[key];
        return null;
    }
}
