using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
    public static int score = 0;
    private bool canScore = true;

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
        Debug.Log("Scored! New score: " + score);
        Levels.Instance.OnScoreChanged(score);

        yield return new WaitForSeconds(2f); 
        canScore = true;
    }
}
