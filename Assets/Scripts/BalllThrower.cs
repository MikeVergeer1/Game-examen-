using UnityEngine;

public class BallThower : MonoBehaviour
{
    public float MaxBallSpeed = 40;
    public float MinSwipeDistance = 30f;
    public float PickupSmooth = 80f;
    public float resetDelay = 2f;

    private Rigidbody rb;
    private Vector2 startPos, endPos;
    private float startTime, endTime, swipeDistance, swipeTime;
    private Vector3 launchAngle;
    private float BallSpeed;
    private bool holding, thrown;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        ResetBall();
    }

    void Update()
    {
        if (holding)
            PickupBall();

        if (thrown)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f)
                && hit.transform == transform)
            {
                holding = true;
                startTime = Time.time;
                startPos = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0) && holding)
        {
            holding = false;
            endTime = Time.time;
            endPos = Input.mousePosition;
            swipeDistance = (endPos - startPos).magnitude;
            swipeTime = endTime - startTime;

            if (swipeTime > 0f && swipeTime < 10f && swipeDistance > MinSwipeDistance)
            {
                CalculateSpeed();
                CalculateAngle();

                rb.AddForce(new Vector3(
                    launchAngle.x * BallSpeed * 5f,
                    launchAngle.y * BallSpeed * 3f,
                    launchAngle.z * BallSpeed
                ));
                rb.useGravity = true;
                thrown = true;
            }
            else
            {
                ResetBall();
            }
        }
    }

    private void PickupBall()
    {
        Vector3 mp = Input.mousePosition;
        mp.z = Camera.main.nearClipPlane * 5f;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mp);
        transform.position = Vector3.Lerp(
            transform.position,
            worldPoint,
            PickupSmooth * Time.deltaTime
        );
    }

    private void CalculateSpeed()
    {
        BallSpeed = (swipeDistance / swipeTime) * 10f;
        BallSpeed = Mathf.Min(BallSpeed, MaxBallSpeed);
    }

    private void CalculateAngle()
    {
        Vector3 screenPoint = new Vector3(endPos.x, endPos.y + 50f, Camera.main.nearClipPlane + 5f);
        launchAngle = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    public void OnScored()
    {
        thrown = false;
        Invoke(nameof(ResetBall), resetDelay);
    }

    public void ResetBall()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;

        transform.position = initialPosition;
        transform.rotation = initialRotation;

        startPos = endPos = Vector2.zero;
        startTime = endTime = swipeDistance = swipeTime = 0f;
        launchAngle = Vector3.zero;
        holding = thrown = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!thrown) return;

        if (collision.collider.CompareTag("Floor"))
        {
            LifeManager.Instance?.LoseLife();
            thrown = false;
            Invoke(nameof(ResetBall), resetDelay);
        }
    }
}
