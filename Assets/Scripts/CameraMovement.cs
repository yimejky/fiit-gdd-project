using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject player;
    public Vector3 offset;
    public float smoothTime = 0.3f;
    public float minYCameraPos = 0;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = player.transform.position + offset;
        playerPosition.y = Mathf.Max(minYCameraPos, playerPosition.y);

        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, smoothTime);
    }
}
