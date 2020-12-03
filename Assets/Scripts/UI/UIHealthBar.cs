using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour, IHealthBarController
{
    public Color color;
    private Image fillImage;
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        fillImage = transform.Find("Fill").GetComponent<Image>();
        SetColor(color);
    }

    public void SetSize(float sizeNormalized)
    {
        slider.value = sizeNormalized;
    }

    public void SetColor(Color color)
    {
        fillImage.color = color;
    }
}