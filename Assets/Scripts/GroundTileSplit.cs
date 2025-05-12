using Unity.VisualScripting;
using UnityEngine;

public class GroundTileSplit : MonoBehaviour
{
    GroundSpawner groundSpawner;
    GameEngine gameEngine;
    public GameObject pointsPrefab;
    public GameObject ObstaclePrefab1;
    public GameObject ObstaclePrefab2;
    public GameObject ObstaclePrefab3;
    public GameObject PickupPrefab1;
    public GameObject PickupPrefab2;
    public GameObject PickupPrefab3;
    Quaternion ObstacleRotation;
    public int PickupSpawnRate;

    void Start()
    {
        gameEngine = GameObject.FindFirstObjectByType<GameEngine>();
        groundSpawner = GameObject.FindFirstObjectByType<GroundSpawner>();
        PickupSpawnRate = gameEngine.PickupSpawnRate; SpawnObstacles();
        SpawnPoints();
        SpawnPickups();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            groundSpawner.SpawnTile();
            Destroy(gameObject, 2);
        }
    }
    void SpawnObstacles()
    {
        int ObstacleSpawnLocation = Random.Range(2, 5);
        if (ObstacleSpawnLocation ==2)
            ObstacleRotation = new(0, 0, 0, 1);
        else ObstacleRotation = new(0, 180, 0, 1);
        Transform SpawnPoint = transform.GetChild(ObstacleSpawnLocation).transform;
        int ObstacleType = Random.Range(1, 4);
        GameObject temp;
        switch (ObstacleType)
        {
            case 1:
                temp = ObstaclePrefab1;
                break;
            case 2:
                temp = ObstaclePrefab2;
                break;
            case 3:
                temp = ObstaclePrefab3;
                break;
            default:
                temp = null;
                break;
        }
        if (temp != null)
            Instantiate(temp, SpawnPoint.position, Quaternion.identity, transform);
    }
    void SpawnPoints()
    {
        Transform SpawnPoint = transform.GetChild(1).transform;
        Instantiate(pointsPrefab, SpawnPoint.position,Quaternion.identity,transform);
    }
    void SpawnPickups()
    {
        int Roll = Random.Range(0, PickupSpawnRate);
        if (Roll != 0)
            return;
        int PickUpType = Random.Range(1, 4);
        GameObject temp;
        switch (PickUpType)
        {
            case 1:
                temp = PickupPrefab1;
                break;
            case 2:
                temp = PickupPrefab2;
                break;
            case 3:
                temp = PickupPrefab3;
                break;
            default:
                temp = null;
                break;
        }
        if (temp != null)
            Instantiate(temp, RandomColliderPoint(GetComponent<Collider>()), Quaternion.identity, transform);
    }

    Vector3 RandomColliderPoint (Collider collider)
    {
        Vector3 point = new Vector3(Random.Range(collider.bounds.min.x,collider.bounds.max.x), 1, Random.Range(collider.bounds.min.z, collider.bounds.max.z));
        return point;
    }
}
