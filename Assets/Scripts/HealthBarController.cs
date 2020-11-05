using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    private Transform bar;

    void Start()
    {
        bar = transform.Find("Bar");
    }


    public void SetSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, bar.localScale.y);
    }
}
