using System.Collections;
using UnityEngine;

public class KnockbackController : MonoBehaviour
{
    public bool canMove = true;
    public int knockbackDamage = 10;
    public Vector2 knockbackPower = new Vector2(10, 10);
    public float knockbackTime = 0.3f;

    public void Knock(GameObject attacker, bool giveDamage=false)
    {
        Rigidbody2D rb2D = gameObject.GetComponent<Rigidbody2D>();
        int knockbackXDir = gameObject.transform.position.x - attacker.transform.position.x < 0 ? -1 : 1;
        Vector2 knockbackDir = new Vector2(knockbackXDir, 1);

        if (giveDamage)
        {
            HealthController health = gameObject.GetComponent<HealthController>();
            health.DealDamage(knockbackDamage);
        }

        StartCoroutine(KnockbackCoroutine(rb2D, knockbackDir, knockbackPower, knockbackTime));
    }

    private IEnumerator KnockbackCoroutine(Rigidbody2D rb2D, Vector2 knockbackDir, Vector2 knockbackPower, float knockbackTime)
    {
        Vector3 knockbackForce = knockbackDir * knockbackPower;

        canMove = false;
        rb2D.velocity = Vector2.zero;
        rb2D.AddForce(knockbackForce, ForceMode2D.Impulse);
        // Debug.Log("player knockback " + knockbackForce);
        yield return new WaitForSeconds(knockbackTime);
        canMove = true;
    }
}
