using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Variables
    [SerializeField]
    private Dropdown mapType;
    [SerializeField]
    private Dropdown players;

    [SerializeField]
    private InputField mapSeed;
    [SerializeField]
    private InputField player1Name;
    [SerializeField]
    private InputField player2Name;

    [SerializeField]
    private Text highScoreText;

    private AudioSource buttonPressed;

    
    private void Start()
    {
        // This spawns in a listener depending on how many players spawn in
        mapType.onValueChanged.AddListener(delegate
        {
            MapTypeDropdownValueChanged();
        });
        players.onValueChanged.AddListener(delegate
        {
            PlayersDropdownValueChanged();
        });

        // This will load the high score for the game on the screen
        GameManager.instance.LoadScores();

        // This will display high score from the game
        highScoreText.text = GameManager.instance.highScoreText;
    }

    // This will load the Game Scene.
    public void PlayGame()
    {
        if (mapType.value == 2)
        {
            GameManager.instance.currentSeed = int.Parse(mapSeed.text);
        }

        GameManager.instance.player1Name = player1Name.text;
        if (GameManager.instance.isMultiplayer)
        {
            GameManager.instance.player2Name = player2Name.text;
        }

        SceneManager.LoadScene("GameScene");
    }

    // This will display the options menu and hide the main menu
    public void GoToOptionsMenu()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    // This will take the player back to the main menu 
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // This will quit the application
    public void QuitGame()
    {
        Application.Quit();
    }

    //Ouput the new value of the Dropdown into Text
    void MapTypeDropdownValueChanged()
    {
        //Map of the day
        if (mapType.value == 0)
        {
            mapSeed.gameObject.SetActive(false);
            GameManager.instance.currentMapType = MapGenerator.MapType.mapOfTheDay;
        }
        //Random
        if (mapType.value == 1)
        {
            mapSeed.gameObject.SetActive(false);
            GameManager.instance.currentMapType = MapGenerator.MapType.random;
        }
        //Custom
        if (mapType.value == 2)
        {
            mapSeed.gameObject.SetActive(true);
            GameManager.instance.currentMapType = MapGenerator.MapType.custom;
        }
    }

    // This is checking how many players are getting spawned in
    void PlayersDropdownValueChanged()
    {
        if(players.value == 0)
        {
            player2Name.gameObject.SetActive(false);
            GameManager.instance.isMultiplayer = false;
        }
        if (players.value == 1)
        {
            player2Name.gameObject.SetActive(true);
            GameManager.instance.isMultiplayer = true;
        }
    }

}
