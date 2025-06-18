using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        Score.score = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game has been closed");
    }
}
