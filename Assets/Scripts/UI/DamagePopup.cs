using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour
{
    public float aliveTime = 0.5f;
    private Animator animator;
    private bool isAlive = true;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.speed = 6;
        animator.Play("FadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        if (aliveTime > 0)
        {
            aliveTime -= Time.deltaTime;
        } else if (isAlive)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        isAlive = false;
        animator.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
