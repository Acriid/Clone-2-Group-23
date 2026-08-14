using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Loads the PlayTest scene.
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayTest_Scene");
    }

    // Quits the game.
    public void QuitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
}
