using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
    public static int score = 1;
    private bool canScore = true;

    private BallThower ballThower;

    void Start()
    {
        ballThower = FindObjectOfType<BallThower>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canScore)
        {
            StartCoroutine(ScoreCooldown());
        }
    }

    private IEnumerator ScoreCooldown()
    {
        canScore = false;
        score++;

        ballThower?.OnScored();                 // prevent life loss
        Levels.Instance?.OnScoreChanged(score); // if you’re using Levels.cs

        yield return new WaitForSeconds(2f);
        canScore = true;
    }
}
