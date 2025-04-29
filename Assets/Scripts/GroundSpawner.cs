using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTile;
    public GameObject groundTileSplit;
    public GameObject groundTileMiddle;
    Vector3 NextSpawnPoint;
    public float TileAmount;
    public int TileDestroyTimer;

    void Start()
    {
        for(int i = 0;i<TileAmount;i++)
        {
            SpawnTile();
        }
    }
    public void SpawnTile()
    {
        int TileIndex =Random.Range(1, 4);
        GameObject Temp;
        switch (TileIndex)
        {
            case 1:
                Temp = Instantiate(groundTile, NextSpawnPoint, Quaternion.identity);
                break;
            case 2:
                Temp = Instantiate(groundTileSplit, NextSpawnPoint, Quaternion.identity);
                break;
            case 3:
                Temp = Instantiate(groundTileMiddle, NextSpawnPoint, Quaternion.identity);
                break;
            default:
                Temp = Instantiate(groundTile, NextSpawnPoint, Quaternion.identity);
                break;
        }
        NextSpawnPoint = Temp.transform.GetChild(1).transform.position;
    }
}
