using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string hurtboxTag = "Hurtbox";
    public int damage = 10;
    public Vector2 knockbackPower = new Vector2(150, 150);
    public float knockbackTime = 0.3f;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        Debug.Log("Enemy trigger " + collider2D.name);

        if (collider2D.CompareTag(hurtboxTag))
        {
            GameObject hero = collider2D.transform.root.gameObject;
            PlayerController playerController = hero.GetComponent<PlayerController>();
            HealthController playerHealth = hero.GetComponent<HealthController>();

            int knockbackXDir = hero.transform.position.x - transform.position.x < 0 ? -1 : 1;
            Vector2 knockbackDir = new Vector2(knockbackXDir, 0);

            playerHealth.DealDamage(damage);
            playerController.Knockback(knockbackDir, knockbackPower, knockbackTime);
        }
    }
}
