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
    
    public void SpawnUnit(int x, int y)
    {

        foreach(var unit in unitMap.Values)
        {
            Destroy(unit.gameObject);
        }
        unitMap.Clear();

        //유닛 생성

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
            Vector3 position = mapManager.GetTilePosition(randomPos.x, randomPos.y);
            position.y += unitHeight;
            Unit newUnit = Instantiate(unit, position, Quaternion.identity, this.transform);
        
            Debug.Log($"Spawned Unit at ({randomPos.x}, {randomPos.y}) Position: {position}");
            Debug.Log($"Unit Local Position: {newUnit.transform.localPosition}");
            
            InitUnitName(newUnit.gameObject, randomPos.x, randomPos.y);

        }
    }

}
