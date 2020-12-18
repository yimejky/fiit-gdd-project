using UnityEngine.UI;
using UnityEngine;

public class UIAction : MonoBehaviour
{
    public float displayDistance = 3;
    public Transform prerequisiteDead;
    public string prerequisiteText;

    private PlayerController player;
    private Canvas canvas;
    private string originalText;
    private Text text;
    private bool textChanged = false;

    void Start()
    {
        player = GameObject.Find("Hero").GetComponent<PlayerController>();
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        canvas.gameObject.SetActive(false);
        text = canvas.transform.Find("Text").GetComponent<Text>();
        originalText = text.text;
        text.text = prerequisiteText != "" && prerequisiteText != null ? prerequisiteText : originalText;
    }

    void Update()
    {
        if (prerequisiteDead != null)
        {
            if (prerequisiteText == null || prerequisiteText == "") return;
        }
        else
        {
            if (!textChanged) {
                text.text = originalText;
                textChanged = true;
            }
        }

        float distance = Vector2.Distance(player.transform.position, transform.position);
        float activeUIdistance = player.closestUI ? Vector2.Distance(player.transform.position, player.closestUI.transform.position) : Mathf.Infinity;

        if (distance <= displayDistance && activeUIdistance >= distance)
        {
            canvas.gameObject.SetActive(true);
            player.closestUI = this;

            if (Input.GetButtonDown("Interact") && !player.isPaused && prerequisiteDead == null)
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
