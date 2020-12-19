using UnityEngine;

public interface IHealthBarController
{
    void SetSize(float sizeNormalized);

    void SetColor(Color color);

    void SetText(string newText);
}