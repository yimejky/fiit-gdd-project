using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAction : MonoBehaviour, IInteractableObject
{
    public void Interact()
    {
        Destroy(gameObject);
    }
}
