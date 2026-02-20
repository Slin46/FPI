using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float bobFrequency = 8f;
    public float bobAmplitude = 0.05f;
    public float returnSpeed = 6f;

    Vector3 startPos;
    float timer;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float inputAmount =
            Mathf.Abs(Input.GetAxis("Horizontal")) +
            Mathf.Abs(Input.GetAxis("Vertical"));

        if (inputAmount > 0.1f)
        {
            timer += Time.deltaTime * bobFrequency;
            float bob = Mathf.Sin(timer) * bobAmplitude;
            transform.localPosition = startPos + new Vector3(0, bob, 0);
        }
        else
        {
            timer = 0;
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                startPos,
                Time.deltaTime * returnSpeed
            );
        }
    }
}
