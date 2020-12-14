using UnityEngine;

public class AttachToPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsPlayerOrEnemy(collision.gameObject.tag))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsPlayerOrEnemy(collision.gameObject.tag))
        {
            collision.gameObject.transform.parent = null;
        }
    }

    private bool IsPlayerOrEnemy(string tag)
    {
        return tag == Constants.PLAYER_TAG || tag == Constants.ENEMY_TAG;
    }
}
