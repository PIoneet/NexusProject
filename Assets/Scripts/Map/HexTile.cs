using UnityEngine;

// 이 스크립트는 3D 타일 프리팹(FBX)에 직접 컴포넌트로 붙여주세요.
public class HexTile : MonoBehaviour
{
    // 타일의 좌표 (나중에 길찾기나 이동에 쓰임)
    public int x;
    public int y;

    // 타일 초기화 함수
    public void Init(int _x, int _y)
    {
        x = _x;
        y = _y;
        
        // 디버깅용: 씬 뷰에서 타일 이름을 좌표로 바꿔서 보기 편하게 함
        name = $"Tile_{x}_{y}";
    }
}
