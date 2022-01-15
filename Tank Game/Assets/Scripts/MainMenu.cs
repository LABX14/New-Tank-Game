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
    private InputField mapSeed;

    private void Start()
    {
        mapType.onValueChanged.AddListener(delegate {
            DropdownValueChanged();
        });
    }

    // This will load the Game Scene.
    public void PlayGame ()
    {
        if (mapType.value == 2)
        {
            GameManager.instance.currentSeed = int.Parse(mapSeed.text);
        }
        SceneManager.LoadScene("GameScene");
    }

    // This will display the options menu and hide the main menu
    public void GoToOptionsMenu ()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    // This will take the player back to the main menu 
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // This will quit the application
    public void QuitGame ()
    {
        Application.Quit();
    }

    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged()
    {
        //Map of the day
        if(mapType.value == 0)
        {
            mapSeed.gameObject.SetActive(false);
            GameManager.instance.currentMapType = MapGenerator.MapType.mapOfTheDay;
        }
        //Random
        if(mapType.value == 1)
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
}
