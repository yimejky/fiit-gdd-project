using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    public Vector3 offset;
    public bool isFollowing = false;
    public float smoothTime = 0.3f;
    public float minYCameraPos = 0;
    private Vector3 velocity = Vector3.zero;
    private string PlayerTag = "Player";

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag(PlayerTag);
        isFollowing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            Vector3 playerPosition = player.transform.position + offset;
            playerPosition.y = Mathf.Max(minYCameraPos, playerPosition.y);
            transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, smoothTime);
        }
    }
}
