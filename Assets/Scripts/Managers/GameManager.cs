using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UnitManager unitManager;
    public MapManager mapManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mapManager.GenerateGrid();
        unitManager.SpawnUnit(unitManager.unitXOffset, unitManager.unitZOffset);
    }
}
