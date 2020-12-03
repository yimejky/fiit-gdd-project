using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(HealthController))]
public class KnockbackController : MonoBehaviour
{
    public bool canMove = true;
    public bool disabled = false;

    public void Knock(GameObject attacker, Vector2 knockbackPower, float knockbackTime=0.3f)
    {
        if (disabled)
        {
            return;
        }

        Rigidbody2D rb2D = gameObject.GetComponent<Rigidbody2D>();
        int knockbackXDir = gameObject.transform.position.x - attacker.transform.position.x < 0 ? -1 : 1;
        Vector2 knockbackDir = new Vector2(knockbackXDir, 1);

        StartCoroutine(KnockbackCoroutine(rb2D, knockbackDir, knockbackPower, knockbackTime));
    }

    private IEnumerator KnockbackCoroutine(Rigidbody2D rb2D, Vector2 knockbackDir, Vector2 knockbackPower, float knockbackTime)
    {
        Vector3 knockbackForce = knockbackDir * knockbackPower;
        
        canMove = false;
        if (rb2D.bodyType != RigidbodyType2D.Static)
        {
            rb2D.velocity = new Vector2(0, 0);
            rb2D.AddForce(knockbackForce, ForceMode2D.Impulse);
        }

        // Debug.Log("player knockback " + knockbackForce);
        if (knockbackTime > 0)
        {
            yield return new WaitForSeconds(knockbackTime);
        }

        canMove = true;
    }
}
