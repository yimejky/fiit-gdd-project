using UnityEngine;

public class HealthBarAbove : MonoBehaviour, IHealthBarController
{
    public Color color;
    private Transform bar;
    private Transform barSprite;
    private SpriteRenderer barSpriteSprite;

    void Awake()
    {
        bar = transform.Find("Bar");
        barSprite = bar.Find("BarSprite");
        barSpriteSprite = barSprite.GetComponent<SpriteRenderer>();
        SetColor(color);
    }

    public void SetSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, bar.localScale.y);
    }

    public void SetColor(Color color)
    {
        barSpriteSprite.color = color;
    }
}
