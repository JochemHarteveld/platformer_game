using UnityEngine;

/// <summary>
/// Enemy that moves horizontally between leftLimit and rightLimit while
/// bobbing vertically using a sine wave.
/// Colour: cyan (set via SpriteRenderer in Start).
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class EnemySine : MonoBehaviour
{
    public float horizontalSpeed = 2f;
    public float leftLimit;
    public float rightLimit;

    [Header("Sine Bob")]
    public float bobAmplitude = 0.8f;
    public float bobFrequency = 2f;

    private float startY;

    void Start()
    {
        startY = transform.position.y;
        GetComponent<SpriteRenderer>().color = Color.cyan;
    }

    void Update()
    {
        transform.Translate(Vector2.right * horizontalSpeed * Time.deltaTime);

        if (transform.position.x > rightLimit || transform.position.x < leftLimit)
        {
            horizontalSpeed *= -1;
        }

        float y = startY + Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
