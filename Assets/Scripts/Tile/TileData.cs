using UnityEngine;

[CreateAssetMenu(fileName = "New Tile Data", menuName = "SRPG/TileData")]
public class TileData : ScriptableObject
{
    public string tileName;
    
    [Header("외형 정보")]
    public Mesh tileMesh;       // 3D 모델 형태 (육각형, 산 모양 등)
    public Material tileMaterial; // 색깔 및 텍스처 (초록색, 회색 등)

    [Header("게임 스탯")]
    public int maxHP;
    public bool isWalkable;
}
