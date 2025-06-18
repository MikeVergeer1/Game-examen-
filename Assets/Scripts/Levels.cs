using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;


public class Levels : MonoBehaviour
{
    public static Levels Instance;

    public GameObject rocketObstacle;
    public Transform basketTransform;

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

        if (rocketObstacle != null)
            rocketObstacle.SetActive(false);
    }

    public void OnScoreChanged(int score)
    {
        if (score == lastScore) return;
        lastScore = score;

        StartCoroutine(HandleLevelChange(score));
    }

    private System.Collections.IEnumerator HandleLevelChange(int score)
    {
        yield return new WaitForSeconds(1f);

        if (score == 1)
        {
            if (basketTransform != null)
            {
                Vector3 pos = basketTransform.position;
                pos.z -= 2f;
                basketTransform.position = pos;
            }
        }
        else if (score == 2)
        {
            if (basketTransform != null)
            {
                Vector3 pos = basketTransform.position;
                pos.z += 2f;
                pos.x -= 5f;
                basketTransform.Rotate(0f, 20f, 0f);
                basketTransform.position = pos;
            }
        }
        else if (score == 3)
        {
            if (basketTransform != null)
            {
                Vector3 pos = basketTransform.position;
                pos.z -= 2f;
                pos.x += 5f;
                basketTransform.Rotate(0f, -20f, 0f);
                basketTransform.position = pos;
            }
            if (rocketObstacle != null)
            {
                rocketObstacle.SetActive(true);
                rocketObstacle.transform.position = new Vector3(-3f, rocketObstacle.transform.position.y, rocketObstacle.transform.position.z);
                StartCoroutine(MoveRocket(rocketObstacle.transform));
            }
        }

        IEnumerator MoveRocket(Transform rocketTransform)
        {
            float duration = 2f;
            Vector3 startPos = new Vector3(-3f, rocketTransform.position.y, rocketTransform.position.z);
            Vector3 endPos = new Vector3(2f, rocketTransform.position.y, rocketTransform.position.z);

            while (true)
            {
                float t = Mathf.PingPong(Time.time / duration, 1f);
                rocketTransform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }
        }



        // Meer leverls hier toevoegen
    }

}
