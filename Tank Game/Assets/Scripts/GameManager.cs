using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Variables

    // This is an instance variable from the game manager
    public static GameManager instance;

    // This adds a score to the player
    public int score;

    public GameObject player;
    public GameObject playerPrefab;

    // This will apply these variables to a list 
    public List<TankData> enemyTanks;
    public List<TankData> players;

    public List<Transform> playerSpawnpoints;

    public Spawner playerSpawner;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("ERROR: There can only be one GameManager.");
            Destroy(gameObject);
        }
        playerSpawner.enabled = false;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerSpawnpoints()
    {
        Debug.Log("Setting spawns");

        playerSpawnpoints.Clear();

        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Spawnpoints"))
        {
            playerSpawnpoints.Add(gameObject.transform);
        }
        playerSpawner.spawnPoints = playerSpawnpoints.ToArray();
        Debug.Log("Spawning Player");
        playerSpawner.enabled = true;
    }
}
