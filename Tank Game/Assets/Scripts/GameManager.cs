using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Variables

    // This is an instance variable from the game manager
    public static GameManager instance;

    // This adds a score to the player
    public int score;

    public bool isMultiplayer;



    // This will apply these variables to a list 
    public List<TankData> enemyTanks;

    public List<Transform> playerSpawnpoints;
    //public List<Transform> powerUpSpawnpoints;
    //public Spawner powerUpSpawner;

    [Header("Player Settings")]
    public List<Player> players;
    public GameObject playerPrefab;
    public float playerRespawnTime;
    public InputController.InputScheme player1InputScheme;
    public InputController.InputScheme player2InputScheme;
    public Camera player1Camera;
    public Camera player2Camera;
    public int playerLives = 3;

    [Header("UI Settings")]
    public GameObject player1DeathScreen;
    public Text player1RespawnText;
    public GameObject player2DeathScreen;
    public Text player2RespawnText;
    public GameObject restartButton;


    private bool isGameActive = false;

    public MapGenerator.MapType currentMapType;
    public int currentSeed;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        players = new List<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameActive) { return; }

        if (players[0].isDead) 
        {
            player1DeathScreen.SetActive(true);
            if (players[0].lives <= 0)
            {
                restartButton.SetActive(true);
                player1RespawnText.text = "Game Over";
            }
            else
            {
                player1RespawnText.text = "Respawning in : " + (players[0].spawner.nextSpawnTime - Time.time).ToString("0");
            }
        }
        else if (player1DeathScreen.activeSelf)
        {
            player1DeathScreen.SetActive(false);
        }

        if (isMultiplayer)
        {
            if (players[1].isDead)
            {
                player1DeathScreen.SetActive(true);
                if (players[1].lives <= 0)
                {
                    player2RespawnText.text = "Game Over";
                }
                else
                {
                    player2RespawnText.text = "Respawning in : " + (players[0].spawner.nextSpawnTime - Time.time).ToString("0.00");
                }
            }
            else if (player2DeathScreen.activeSelf)
            {
                player2DeathScreen.SetActive(false);
            }
        }
    }

    public void StartGameplay()
    {


        players.Clear();

        players.Add(new Player(gameObject.AddComponent<Spawner>(), playerRespawnTime, playerPrefab, player1InputScheme));
        SetPlayerSpawnpoints(players[0].spawner);

        if (isMultiplayer)
        {
            players.Add(new Player(gameObject.AddComponent<Spawner>(), playerRespawnTime, playerPrefab, player2InputScheme));
            SetPlayerSpawnpoints(players[1].spawner);
        }

        foreach(Player player in players)
        {
            player.lives = playerLives;
        }

        isGameActive = true;
    }

    // Set the spawn points for the players on the map after the map is generated
    public void SetPlayerSpawnpoints(Spawner spawner)
    {

        playerSpawnpoints.Clear();

        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Spawnpoints"))
        {
            playerSpawnpoints.Add(gameObject.transform);
        }

        spawner.spawnPoints = playerSpawnpoints.ToArray();
    }

    public void RestartGame()
    {
        isGameActive = false;
        restartButton.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
