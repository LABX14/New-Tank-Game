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
    public string player1Name;
    public string player2Name;

    [Header("UI Settings")]
    public GameObject player1DeathScreen;
    public Text player1RespawnText;
    public Text player1Lives;
    public Text player1Score;

    public GameObject player2DeathScreen;
    public Text player2RespawnText;
    public Text player2Lives;
    public Text player2Score;
    public GameObject restartButton;

    public string highScoreText;

    [Header("High Score Settings")]
    public List<ScoreData> scores;


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
                if (players[0].spawner != null)
                {
                    Destroy(players[0].spawner);
                }
                restartButton.SetActive(true);
                player1Lives.text = " ";
                player1RespawnText.text = "Game Over";
            }
            else
            {
                player1Lives.text = "Lives: " + players[0].lives;
                player1RespawnText.text = "Respawning in : " + (players[0].spawner.nextSpawnTime - Time.time).ToString("0");
            }
        }
        else if (player1DeathScreen.activeSelf)
        {
            player1DeathScreen.SetActive(false);
        }
        else
        {
            player1Score.text = "Score: " + players[0].data.score;
        }

        if (isMultiplayer)
        {
            if (players[1].isDead)
            {
                player1DeathScreen.SetActive(true);
                if (players[1].lives <= 0)
                {
                    if (players[1].spawner != null)
                    {
                        Destroy(players[1].spawner);
                    }
                    player2Lives.text = " ";
                    player2RespawnText.text = "Game Over";
                }
                else
                {
                    player2Lives.text = "Lives: " + players[1].lives;
                    player2RespawnText.text = "Respawning in : " + (players[0].spawner.nextSpawnTime - Time.time).ToString("0.00");
                }
            }
            else if (player2DeathScreen.activeSelf)
            {
                player2DeathScreen.SetActive(false);
            }
            else
            {
                player2Score.text = "Score: " + players[1].data.score;
            }
        }
    }

    public void StartGameplay()
    {
        players.Clear();

        players.Add(new Player(gameObject.AddComponent<Spawner>(), playerRespawnTime, playerPrefab, player1InputScheme));
        SetPlayerSpawnpoints(players[0].spawner);
        player1Lives.text = "Lives: " + playerLives;

        if (isMultiplayer)
        {
            players.Add(new Player(gameObject.AddComponent<Spawner>(), playerRespawnTime, playerPrefab, player2InputScheme));
            SetPlayerSpawnpoints(players[1].spawner);
            player2Lives.text = "Lives: " + playerLives;
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
        
        Destroy(players[0].spawner);
        if (isMultiplayer)
        {
            Destroy(players[1].spawner);
        }

        SaveScores();

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void SaveScores()
    {
        //Add both player's scores
        scores.Add(new ScoreData(player1Name, players[0].score));
        if (isMultiplayer)
        {
            scores.Add(new ScoreData(player2Name, players[1].score));
        }
        //Sort the list of scores
        scores.Sort();
        //Show from highest to least
        scores.Reverse();
        //Only show the top three
        scores = scores.GetRange(0, 3);
        //Display them
        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetString("name" + i, scores[i].name);
            PlayerPrefs.SetFloat("score" + i, scores[i].score);
        }
        highScoreText = "HIGHSCORES\n";
        for (int i = 0; i < scores.Count; i++)
        {
            highScoreText += scores[i].name + ": " + scores[i].score + "\n";
        }
        PlayerPrefs.SetString("highScoreText", highScoreText);
        PlayerPrefs.Save();
    }

    public void LoadScores()
    {
        scores.Clear();
        for (int i = 0; i < 3; i++)
        {
            scores.Add(new ScoreData(PlayerPrefs.GetString("name" + i), PlayerPrefs.GetFloat("score" + i)));
        }
        //Get the display
        highScoreText = PlayerPrefs.GetString("highScoreText");
    }

    private void OnApplicationQuit()
    {
        //Save on quit
        SaveScores();
    }

}
