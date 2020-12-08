using UnityEngine;

public class CollectableController : MonoBehaviour, IInteractableObject
{
    public void Interact()
    {
        // TODO Add object to inventory
        Destroy(gameObject, 0.1f);
    }
}
