using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [Header("Unit Settings")]
    public MapManager mapManager;
    public int unitXOffset;
    public int unitZOffset;
    private float unitHeight = 0.2f;
    public List<Unit> units = new List<Unit>();
    public Dictionary<Vector2Int, Unit> unitMap = new Dictionary<Vector2Int, Unit>();

    void InitUnitName(GameObject unit, int x, int y)
    {
        unit.name = $"Unit_{x}_{y}";
    }

    void InitUnitPosition()
    {
        foreach(var unit in units)
        {
            Vector2Int randomPos;
            do
            {
                int randXValue = Random.Range(-4, 5);
                int randYValue = Random.Range(-4, 5);
                randomPos = new Vector2Int(randXValue, randYValue);
            } while (unitMap.ContainsKey(randomPos));

            unitMap.Add(randomPos, unit);    
        }
        
    }
    
    public void SpawnUnit(int x, int y)
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        units.Clear();

        //유닛 생성

        foreach(var unit in units)
        {
            Vector3 position = mapManager.GetTilePosition(x, y);
            Unit newUnit = Instantiate(unit, position, Quaternion.identity, this.transform);
        
            Debug.Log($"Spawned Unit at ({x}, {y}) Position: {position}");
            position.y += unitHeight;
            newUnit.transform.position = position;
            Debug.Log($"Unit Local Position: {newUnit.transform.localPosition}");
            InitUnitName(newUnit.gameObject, x, y);
    
            units.Add(newUnit);

        }
    }

}
