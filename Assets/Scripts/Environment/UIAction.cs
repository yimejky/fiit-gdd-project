using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAction : MonoBehaviour
{
    public float displayDistance = 3;

    private PlayerController player;
    private Canvas canvas;

    void Start()
    {
        player = GameObject.Find("Hero").GetComponent<PlayerController>();
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        canvas.gameObject.SetActive(false);
    }

    void Update()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
        float activeUIdistance = player.closestUI ? Vector2.Distance(player.transform.position, player.closestUI.transform.position) : Mathf.Infinity;

        if (distance <= displayDistance && activeUIdistance >= distance)
        {
            canvas.gameObject.SetActive(true);
            player.closestUI = this;

            if (!player.isPaused && Input.GetButtonDown("Interact"))
            {
                Debug.Log("Interact " + gameObject.name);
                gameObject.GetComponent<IInteractableObject>().Interact();
            }
        }
        else
        {
            canvas.gameObject.SetActive(false);
            if (player.closestUI == this)
            {
                player.closestUI = null;
            }
        }
    }
}
