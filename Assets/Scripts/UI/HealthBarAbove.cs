using UnityEngine;

public class HealthBarAbove : MonoBehaviour, IHealthBarController
{
    static private int startingOrder = 0;
    static private int StartingOrder
    {
        get {
            startingOrder += 1;
            return startingOrder; 
        }
    }

    public Color color;
    private Transform borderTrans;
    private Transform backgroundTrans;
    private Transform barTrans; 
    private Transform barSpriteTrans;

    void Awake()
    {
        borderTrans = transform.Find("Border");
        backgroundTrans = transform.Find("Background");
        barTrans = transform.Find("Bar");

        barSpriteTrans = barTrans.Find("BarSprite");
        SetColor(color);
        
        int newOrder = StartingOrder * 3 + 10;
        borderTrans.GetComponent<SpriteRenderer>().sortingOrder = newOrder;
        backgroundTrans.GetComponent<SpriteRenderer>().sortingOrder = newOrder + 1;
        barSpriteTrans.GetComponent<SpriteRenderer>().sortingOrder = newOrder + 2;
    }

    void Update()
    {
        Quaternion rot = Quaternion.Euler(0, transform.parent.rotation.eulerAngles.y, 0);
        Debug.Log($"new rotate {rot.eulerAngles}");
        transform.localRotation = rot;
    }

    public void SetSize(float sizeNormalized)
    {
        barTrans.localScale = new Vector3(sizeNormalized, barTrans.localScale.y);
    }

    public void SetColor(Color color)
    {
        barSpriteTrans.GetComponent<SpriteRenderer>().color = color;
    }
}
