using UnityEngine;

public class ActionCollider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        gameObject.GetComponent<IInteractableObject>().Interact();
    }
}
