using UnityEngine;
using System.Collections;

public class MovementManager : MonoBehaviour
{
    public static MovementManager Instance;

    public Transform rocketTransform;
    public Transform saturnTransform;
    public Transform ufoTransform;
    public Transform basketTransform;
    public Transform[] cloudTransforms;

    public float cloudSpeed = 2f;
    public float rocketDownY = 3f;

    private Vector3 basketStart;
    private Vector3[] cloudStarts;

    private Coroutine basketRoutine;
    private Coroutine cloudRoutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        basketStart = basketTransform.position;

        cloudStarts = new Vector3[cloudTransforms.Length];
        for (int i = 0; i < cloudTransforms.Length; i++)
            cloudStarts[i] = cloudTransforms[i].position;
    }

    public void LaunchRocket(float targetY, float upSpeed)
    {
        StartCoroutine(RocketRoutine(targetY, upSpeed));
    }

    public void StartSaturn(float range, float speed)
    {
        StartCoroutine(PingPong(saturnTransform, Vector3.right, range, speed));
    }

    public void StartUfo(float range, float speed)
    {
        StartCoroutine(PingPong(ufoTransform, Vector3.up, range, speed));
    }

    public void StartBasket(Vector3 axis, float range, float speed)
    {
        StopBasket();
        basketRoutine = StartCoroutine(PingPong(basketTransform, axis, range, speed, true, basketStart, false));
    }

    public void StartBasketBackOnly(float range, float speed)
    {
        StopBasket();
        basketRoutine = StartCoroutine(PingPong(basketTransform, Vector3.forward, range, speed, true, basketStart, true));
    }

    public void StopBasket()
    {
        if (basketRoutine != null) StopCoroutine(basketRoutine);
        basketTransform.position = basketStart;
        basketTransform.rotation = Quaternion.identity;
    }

    public void SetBasketOffset(Vector3 offset, float yRotation = 0f)
    {
        StopBasket();
        basketTransform.position = basketStart + offset;
        basketTransform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    public void StartClouds(float range)
    {
        if (cloudRoutine != null) StopCoroutine(cloudRoutine);
        cloudRoutine = StartCoroutine(CloudRoutine(range, cloudSpeed));
    }

    private IEnumerator RocketRoutine(float targetY, float upSpeed)
    {
        float downSpeed = 3f;

        while (rocketTransform.position.y < targetY)
        {
            rocketTransform.position += Vector3.up * upSpeed * Time.deltaTime;
            yield return null;
        }

        while (rocketTransform.position.y > rocketDownY)
        {
            rocketTransform.position -= Vector3.up * downSpeed * Time.deltaTime;
            yield return null;
        }

        Vector3 p = rocketTransform.position;
        p.y = rocketDownY;
        rocketTransform.position = p;
    }

    private IEnumerator PingPong(Transform obj, Vector3 axis, float range, float speed, bool loop = false, Vector3? startPosOverride = null, bool oneSided = false)
    {
        Vector3 start = startPosOverride ?? obj.position;
        bool forward = true;
        Vector3 direction = axis.normalized;

        float minOffset = oneSided ? -range : -range;
        float maxOffset = oneSided ? 0f      :  range;

        while (loop || obj.gameObject.activeSelf)
        {
            obj.position += direction * (forward ? 1 : -1) * speed * Time.deltaTime;

            float offset = Vector3.Dot(obj.position - start, direction);

            if (forward && offset >= maxOffset)
                forward = false;
            else if (!forward && offset <= minOffset)
                forward = true;

            yield return null;
        }
    }

    private IEnumerator CloudRoutine(float range, float speed)
    {
        bool[] movingRight = new bool[cloudTransforms.Length];

        for (int i = 0; i < movingRight.Length; i++)
            movingRight[i] = true;

        while (true)
        {
            float step = speed * Time.deltaTime;

            for (int i = 0; i < cloudTransforms.Length; i++)
            {
                Transform cloud = cloudTransforms[i];
                Vector3 start = cloudStarts[i];

                Vector3 dir = movingRight[i] ? Vector3.right : Vector3.left;
                cloud.position += dir * step;

                float offset = cloud.position.x - start.x;

                if (movingRight[i] && offset >= range)
                    movingRight[i] = false;
                else if (!movingRight[i] && offset <= -range)
                    movingRight[i] = true;
            }

            yield return null;
        }
    }
}