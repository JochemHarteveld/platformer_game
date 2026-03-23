using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        // Set camera background to dark grey
        Camera cam = GetComponent<Camera>();
        if (cam != null)
        {
            cam.backgroundColor = new Color(0.15f, 0.15f, 0.15f, 1f);
        }
    }

    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -10);
    }
}