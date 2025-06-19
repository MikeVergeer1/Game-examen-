using UnityEngine;
using System.Collections;

public class Levels : MonoBehaviour
{
    public static Levels Instance;

    public GameObject rocketObstacle;

    public GameObject saturnObstacle;
    public float saturnMoveSpeed = 2f;
    public float saturnMoveRange = 5f;

    public GameObject ufoObstacle;

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
    }

    public void OnScoreChanged(int score)
    {
        if (score == lastScore) return;
        lastScore = score;

        StartCoroutine(HandleLevelChange(score));
    }

    private IEnumerator HandleLevelChange(int score)
    {
        float rocketHeight = 2 + (score * 2f);
        float fastSpeed = score * 1f;

        if (rocketObstacle != null)
            StartCoroutine(FlyRocketUpwardAndDown(rocketObstacle.transform, rocketHeight, fastSpeed));

        yield return new WaitForSeconds(2f);

        if (score == 2 && basketTransform != null)
        {
            Vector3 pos = basketTransform.position;
            pos.z -= 2f;
            basketTransform.position = pos;
            LifeManager.Instance.SetLives(3);
        }
        else if (score == 3 && basketTransform != null)
        {
            Vector3 pos = basketTransform.position;
            pos.z += 2f;
            pos.x -= 5f;
            basketTransform.Rotate(0f, 20f, 0f);
            basketTransform.position = pos;
            LifeManager.Instance.SetLives(4);
        }
        else if (score == 4 && basketTransform != null)
        {
            Vector3 pos = basketTransform.position;
            pos.z -= 2f;
            pos.x += 5f;
            basketTransform.Rotate(0f, -20f, 0f);
            basketTransform.position = pos;

            saturnObstacle.SetActive(true);
            saturnMoveRange = 2f;
            StartCoroutine(MoveSaturnLeftRight(saturnObstacle.transform, saturnMoveRange, saturnMoveSpeed));

            LifeManager.Instance.SetLives(3);
        }
        else if (score == 5)
        {
            saturnObstacle.SetActive(false);
            ufoObstacle.SetActive(true);
            StartCoroutine(MoveUfoUpAndDown(ufoObstacle.transform, saturnMoveRange, saturnMoveSpeed));
            LifeManager.Instance.SetLives(3);
        }
    }

    private IEnumerator FlyRocketUpwardAndDown(Transform rocketTransform, float targetHeight, float fastSpeed)
    {
        float slowSpeed = 3f;

        while (rocketTransform.position.y < targetHeight)
        {
            rocketTransform.position += Vector3.up * fastSpeed * Time.deltaTime;
            yield return null;
        }

        Vector3 pos = rocketTransform.position;
        pos.y = targetHeight;
        rocketTransform.position = pos;

        while (rocketTransform.position.y > 3f)
        {
            rocketTransform.position -= Vector3.up * slowSpeed * Time.deltaTime;
            if (rocketTransform.position.y < 3f)
            {
                pos = rocketTransform.position;
                pos.y = 3f;
                rocketTransform.position = pos;
                break;
            }
            yield return null;
        }
    }

    private IEnumerator MoveSaturnLeftRight(Transform saturn, float range, float speed)
    {
        Vector3 startPos = saturn.position;
        bool movingRight = true;

        while (saturn.gameObject.activeSelf)
        {
            float step = speed * Time.deltaTime;

            if (movingRight)
            {
                saturn.position += Vector3.right * step;
                if (saturn.position.x >= startPos.x + range)
                    movingRight = false;
            }
            else
            {
                saturn.position -= Vector3.right * step;
                if (saturn.position.x <= startPos.x - range)
                    movingRight = true;
            }

            yield return null;
        }
    }
    private IEnumerator MoveUfoUpAndDown(Transform ufoTransform, float range, float speed)
    {
        Vector3 startPos = ufoTransform.position;
        bool movingUp = true;

        while (ufoTransform.gameObject.activeSelf)
        {
            float step = speed * Time.deltaTime;

            if (movingUp)
            {
                ufoTransform.position += Vector3.up * step;
                if (ufoTransform.position.y >= startPos.y + range)
                    movingUp = false;
            }
            else
            {
                ufoTransform.position -= Vector3.up * step;
                if (ufoTransform.position.y <= startPos.y - range)
                    movingUp = true;
            }

            yield return null;
        }
    }
}