using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    public static int score = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            score++;
        }
    }
}
