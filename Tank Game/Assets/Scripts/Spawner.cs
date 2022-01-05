using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Variables
    public GameObject prefabToSpawn;
    public float spawnDelay;
    private float nextSpawnTime;
    [SerializeField]
    private Transform[] spawnPoints;
    private GameObject spawnedObject;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time + spawnDelay;
        Debug.Log(spawnPoints.Length);
    }

    // Update is called once per frame
    void Update()
    {
        // If it is there is nothing spawns
        if (spawnedObject == null)
        {
            // And it is time to spawn
            if (Time.time > nextSpawnTime)
            {

                // Spawn it and set the next time
                spawnedObject = Instantiate(prefabToSpawn, spawnPoints[Random.Range(0,spawnPoints.Length)].position, Quaternion.identity) as GameObject;
                nextSpawnTime = Time.time + spawnDelay;
            }
        }
        else
        {
            // Otherwise, the object still exists, so postpone the spawn
            nextSpawnTime = Time.time + spawnDelay;
        }
    }
}
