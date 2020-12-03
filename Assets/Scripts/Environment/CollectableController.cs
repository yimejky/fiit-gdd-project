using UnityEngine;

public class CollectableController : MonoBehaviour
{
    void Awake()
    {
        BoxCollider2D bc;
        bc = gameObject.AddComponent<BoxCollider2D>() as BoxCollider2D;
        bc.size = new Vector2(1f, 1f);
        bc.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
            Destroy(this.gameObject, 0.1f);
        }
    }
}
