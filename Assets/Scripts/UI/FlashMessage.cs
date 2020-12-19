using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashMessage : MonoBehaviour
{
    Text textUI;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        textUI = gameObject.GetComponent<Text>();
        animator = gameObject.GetComponent<Animator>();
       
        Color color = textUI.color;
        textUI.color = color;
    }

    public void ShowMessage(string text)
    {
        StartCoroutine(ShowMessageCoroutine(text));
    }

    public IEnumerator ShowMessageCoroutine(string text, float showTime=4f)
    {
        // Debug.Log($"Message log {text}");
        textUI.text = text;

        animator.Play("FadeIn");
        yield return new WaitForSeconds(showTime);
        animator.Play("FadeOut");
    }
}
