using UnityEngine;
using UnityEngine.SceneManagement;

//mainmenu.cs
// This script handles the main menu functionality, including starting the game and quitting the application.
public class Mainmenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        Score.score = 1;
    }

    public void QuitGame()
    {
        Debug.Log("Game has been closed");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}
