using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour, IHealthBarController
{
    public Color color;
    private Text text;
    private Image fillImage;
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        fillImage = transform.Find("Fill").GetComponent<Image>();
        text = transform.Find("Text").GetComponent<Text>();

        SetColor(color);
        SetText("100/100");
    }

    public void SetSize(float sizeNormalized)
    {
        if (slider != null)
            slider.value = sizeNormalized;
    }

    public void SetColor(Color color)
    {
        if (fillImage != null) 
            fillImage.color = color;
    }

    public void SetText(string newText)
    {
        if (text != null)
            text.text = newText;
    }
}