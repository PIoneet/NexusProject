using UnityEngine;

public class Unit : MonoBehaviour
{
    public int x;
    public int y;

    public void Init(int _x, int _y)
    {
        x = _x;
        y = _y;
        // 나중에 여기에 체력 초기화나 팀 설정 등을 넣으면 됩니다.
    }
}
