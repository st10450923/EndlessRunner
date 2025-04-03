using Unity.VisualScripting;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;
    public GameObject ObstaclePrefab;
    public GameObject pointsPrefab;
    public GameObject PickupPrefab;
    public int PickupSpawnRate;

    void Start()
    {
        groundSpawner = GameObject.FindFirstObjectByType<GroundSpawner>();
        SpawnObstacles();
        SpawnPoints();
        SpawnPickups();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            groundSpawner.SpawnTile();
            Destroy(gameObject, 1);
        }
    } 

    void SpawnObstacles()
    {
        int ObstacleSpawnIndex = Random.Range(2, 5);
        Transform SpawnPoint = transform.GetChild(ObstacleSpawnIndex).transform;
        Instantiate(ObstaclePrefab, SpawnPoint.position, Quaternion.identity, transform);
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
        GameObject temp = Instantiate(PickupPrefab,transform);
        temp.transform.position = RandomColliderPoint(GetComponent<Collider>());

    }

    Vector3 RandomColliderPoint (Collider collider)
    {
        Vector3 point = new Vector3(Random.Range(collider.bounds.min.x,collider.bounds.max.x), 1, Random.Range(collider.bounds.min.z, collider.bounds.max.z));
        return point;
    }
}
