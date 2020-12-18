using UnityEngine;

public class AttachToPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsAttachable(collision.gameObject.tag))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsAttachable(collision.gameObject.tag))
        {
            collision.gameObject.transform.parent = null;
        }
    }

    private bool IsAttachable(string tag)
    {
        return tag == Constants.PLAYER_TAG || tag == Constants.ENEMY_TAG || tag == Constants.ENVIRONMENT_TAG;
    }
}
