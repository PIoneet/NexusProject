using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [Header("Unit Settings")]
    public GameObject unitPrefab;
    public MapManager mapManager;
    public int unitXOffset;
    public int unitZOffset;
    private float unitHeight = 0.2f;
    public List<GameObject> units = new List<GameObject>();

    void InitUnitName(GameObject unit, int x, int y)
    {
        unit.name = $"Unit_{x}_{y}";
    }
    
    public void SpawnUnit(int x, int y)
    {
        Vector3 position = mapManager.GetTilePosition(x, y);
        GameObject newUnit = Instantiate(unitPrefab, position, Quaternion.identity, this.transform);
        
        Debug.Log($"Spawned Unit at ({x}, {y}) Position: {position}");
        position.y += unitHeight;
        newUnit.transform.position = position;
        Debug.Log($"Unit Local Position: {newUnit.transform.localPosition}");
        InitUnitName(newUnit, x, y);
    
    
        units.Add(newUnit);
    }

}
