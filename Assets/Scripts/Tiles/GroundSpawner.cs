using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTile;
    public GameObject groundTileSplit;
    public GameObject groundTileMiddle;
    Vector3 NextSpawnPoint;
    public float TileAmount;
    public int TileDestroyTimer;
    public float PickupSpawnRate;
    public GameObject rail1;
    public GameObject rail2;
    public GameObject rail3;
    public GameObject rail4;
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
    void SpawnRail(GameObject RailSpawnPoint)
    {
        int Roll = Random.Range(1,5);
        GameObject Rail;
        Quaternion RailRotation = new(0,0,0,1);
        switch (Roll)
        {
            case 1:
                Rail = Instantiate(rail1,NextSpawnPoint,RailRotation);
                break;
            case 2:
                Rail = Instantiate(rail2,NextSpawnPoint, RailRotation);
                break;
            case 3:
                Rail = Instantiate(rail3, NextSpawnPoint, RailRotation);
                break;
            case 4:
                Rail = Instantiate(rail4, NextSpawnPoint, RailRotation);
                break;
            default:
                Rail = Instantiate(rail1, groundTile.transform);
                break;
        }
    }
}
