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


    // These are all of the player settings that is found on the game Manager
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

    // These are the UI settings 
    [Header("UI Settings")]
    public GameObject player1DeathScreen;
    public Text player1RespawnText;
    public Text player1Lives;
    public Text player1Score;

    public GameObject player2Canvas;
    public GameObject player2DeathScreen;
    public Text player2RespawnText;
    public Text player2Lives;
    public Text player2Score;
    public GameObject restartButton;

    public string highScoreText;

    [Header("High Score Settings")]
    public List<ScoreData> scores;

    [Header("Audio Settings")]
    public float sfxVolume;
    public float bgmVolume;
    public AudioSource bgmAudioSource;

    private bool isGameActive = false;

    public MapGenerator.MapType currentMapType;
    public int currentSeed;


    // This prevents the GameManager from being destroyed after it is loaded into the scene
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

        bgmAudioSource = GameObject.FindGameObjectWithTag("Background Music").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameActive) { return; }
        
        // If the player 1 dies, the player's character will get destroyed
        if (players[0].isDead) 
        {
            player1DeathScreen.SetActive(true);
            if (players[0].lives <= 0)
            {
                if (players[0].spawner != null)
                {
                    Destroy(players[0].spawner);
                }

                // The restart button will display and be available for the player to click on if they wish to continue
                restartButton.SetActive(true);
                player1Lives.text = " ";
                player1RespawnText.text = "Game Over";
            }
            else
            {
                // If the player still has lives after it does, then the player will see a screen that is going to tell that they are respawning instead
                player1Lives.text = "Lives: " + players[0].lives;
                player1RespawnText.text = "Respawning in : " + (players[0].spawner.nextSpawnTime - Time.time).ToString("0");
            }
        }

        // If player one's death screen is active, 
        else if (player1DeathScreen.activeSelf)
        {
            // then set death screen to false
            player1DeathScreen.SetActive(false);
        }

        // else show the screen that will display the player's score
        else
        {
            player1Score.text = "Score: " + players[0].data.score;
        }

        // If in Multiplayer game mode and if the player 1 is dead, the player 2 death screen will be set to active
        if (isMultiplayer)
        {
            if (players[1].isDead)
            {
                player2DeathScreen.SetActive(true);
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
                    // Else the player 2 lives are being displayed and show the respawn text with countdown
                    player2Lives.text = "Lives: " + players[1].lives;
                    player2RespawnText.text = "Respawning in : " + (players[1].spawner.nextSpawnTime - Time.time).ToString("0");
                }
            }

            // Display the player 2 score if the player 2 death screen is showing 
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

    // At the start of the game, clear all the players then add in a new player to the scene
    public void StartGameplay()
    {
        players.Clear();

        // give that player their respawn time, the character they will be playing as, and that player's controls
        players.Add(new Player(gameObject.AddComponent<Spawner>(), playerRespawnTime, playerPrefab, player1InputScheme));
        SetPlayerSpawnpoints(players[0].spawner);
        player1Lives.text = "Lives: " + playerLives;

        if (isMultiplayer)
        {
            // Give player their respawn time, the character they will be playing as, and their different controls if game mode is Multiplayer
            player2Canvas.SetActive(true);
            players.Add(new Player(gameObject.AddComponent<Spawner>(), playerRespawnTime, playerPrefab, player2InputScheme));
            SetPlayerSpawnpoints(players[1].spawner);
            player2Lives.text = "Lives: " + playerLives;
            player1Camera.rect = new Rect(0, 0.5f, 1, 0.5f);
        }
        else
        {
            player2Canvas.SetActive(false);
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

        // This is looking for all of the game objects within the scene labeled as "Spawnpoints" and then spawning in the player at one of those locations
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Spawnpoints"))
        {
            playerSpawnpoints.Add(gameObject.transform);
        }

        spawner.spawnPoints = playerSpawnpoints.ToArray();
    }

    // This will restart the game
    public void RestartGame()
    {
        // the game is no longer active 
        isGameActive = false;
        // the restart button will have spawned in
        restartButton.SetActive(false);
        // destroy the players
        Destroy(players[0].spawner);
        // in Multiplayer, destroy player 2
        if (isMultiplayer)
        {
            Destroy(players[1].spawner);
        }

        SaveScores();

        player1Camera.rect = new Rect(0, 0, 1, 1);

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


    // This will get the high score from the game and display it on the main menu 
    public void LoadScores()
    {


        scores.Clear();
        for (int i = 0; i < 3; i++)
        {
            // This will take the name and the score and display them in the main menu
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

    // Set the background music volume from the game scene to the value of the slider from main menu
    private void OnLevelWasLoaded(int level)
    {
        bgmAudioSource = GameObject.FindGameObjectWithTag("Background Music").GetComponent<AudioSource>();
        bgmAudioSource.volume = bgmVolume;
    }

}
