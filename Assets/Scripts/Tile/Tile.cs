using UnityEngine;

public class T : MonoBehaviour
{
    public TileData tileData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Tile Name: " + tileData.tileName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
