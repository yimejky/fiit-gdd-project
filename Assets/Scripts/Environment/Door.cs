using UnityEngine;

public interface IInteractableObject
{
    void Interact();
}


public class Door : MonoBehaviour, IInteractableObject
{
    public bool isOpen = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D col2d;
    public Sprite openSprite;
    public Sprite closedSprite;

    // Start is called before the first frame update
    void Start()
    {
        col2d = GetComponent<Collider2D>();
        spriteRenderer = transform.Find("DoorSprite").GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = isOpen ? openSprite : closedSprite;
    }

    private void Update()
    {
        if (isOpen && spriteRenderer.sprite != openSprite) Open();
        if (!isOpen && spriteRenderer.sprite != closedSprite) Close();
    }

    public void Open()
    {
        Debug.Log("Door: open");
        isOpen = true;
        spriteRenderer.sprite = openSprite;
        col2d.enabled = false;
    }

    public void Close()
    {
        Debug.Log("Door: close");
        isOpen = false;
        spriteRenderer.sprite = closedSprite;
        col2d.enabled = true;
    }

    public void Interact()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
