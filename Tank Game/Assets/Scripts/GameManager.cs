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
    public List<Transform> powerUpSpawnpoints;

    public Spawner playerSpawner;
    public Spawner powerUpSpawner;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
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

    // Set the spawn points for the players on the map after the map is generated
    public void SetPlayerSpawnpoints()
    {

        playerSpawnpoints.Clear();

        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Spawnpoints"))
        {
            playerSpawnpoints.Add(gameObject.transform);
        }
        playerSpawner.spawnPoints = playerSpawnpoints.ToArray();
        playerSpawner.enabled = true;
    }
}
