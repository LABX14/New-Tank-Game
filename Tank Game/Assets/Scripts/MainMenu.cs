using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // This will load into the next scene
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
}
