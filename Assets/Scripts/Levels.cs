using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// levels.cs
// This script manages the game levels and handles the progression based on the player's score.
public class Levels : MonoBehaviour
{
    public static Levels Instance;

    public GameObject saturnObstacle;
    public GameObject ufoObstacle;

    private int lastScore = -1;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void OnScoreChanged(int score)
    {
        if (score == lastScore) return;
        lastScore = score;
        StartCoroutine(HandleLevel(score));
    }

    IEnumerator HandleLevel(int score)
    {
        float h = 2 + score * 2f;
        float s = score * 1f;

        if (score >= 1)
            MovementManager.Instance.LaunchRocket(h, s);

        yield return new WaitForSeconds(2f);

        if (score == 2)
        {
            MovementManager.Instance.SetBasketOffset(new Vector3(0f, 0f, -2f));
            LifeManager.Instance.SetLives(3);
        }
        else if (score == 3)
        {
            MovementManager.Instance.SetBasketOffset(new Vector3(-5f, 0f, 2f), 20f);
            LifeManager.Instance.SetLives(5);
        }
        else if (score == 4)
        {
            MovementManager.Instance.SetBasketOffset(new Vector3(0f, 0f, 0f), 0f);
            saturnObstacle.SetActive(true);
            MovementManager.Instance.StartSaturn(2f, 2f);
            LifeManager.Instance.SetLives(3);
        }
        else if (score == 5)
        {
            saturnObstacle.SetActive(false);
            ufoObstacle.SetActive(true);
            MovementManager.Instance.StartUfo(2f, 2f);
            LifeManager.Instance.SetLives(3);
        }
        else if (score == 6)
        {
            ufoObstacle.SetActive(false);
            MovementManager.Instance.StartBasketBackOnly(8f, 2f);
            LifeManager.Instance.SetLives(5);
        }
        else if (score == 7)
        {
            MovementManager.Instance.StopBasket();
            MovementManager.Instance.StartBasket(Vector3.up, 1f, 2f);
            LifeManager.Instance.SetLives(5);

        }
        else if (score == 8)
        {
            MovementManager.Instance.StartBasket(Vector3.right, 3f, 2f);
            LifeManager.Instance.SetLives(6);
        }
        else if (score == 9)
        {
            MovementManager.Instance.StopBasket();
            MovementManager.Instance.SetBasketOffset(new Vector3(0f, 0f, -3f), 180f);
            LifeManager.Instance.SetLives(8);
        }
        else if (score == 10)
        {
            MovementManager.Instance.StartClouds(6f);
            LifeManager.Instance.SetLives(10);
        }
        else if (score == 11)
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}
