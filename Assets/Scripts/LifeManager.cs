using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance;

    public TextMeshProUGUI livesText;
    public int startingLives = 3;
    private int currentLives;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            currentLives = startingLives;
            UpdateLivesUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLives(int value)
    {
        currentLives = value;
        UpdateLivesUI();
    }

    public void LoseLife()
    {
        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = $"Balls: {currentLives}";
        }
    }
}
