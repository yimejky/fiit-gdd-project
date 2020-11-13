using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage = 10;
    public Vector2 knockbackPower = new Vector2(150, 150);
    public float knockbackTime = 0.3f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("enemy trigger enter" + collision.name);
        DetectHeroTouch(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("enemy trigger stay" + collision.name);
        DetectHeroTouch(collision);
    }

    void DetectHeroTouch(Collider2D collision)
    {
        if (collision.CompareTag(Constants.HURTBOX_TAG))
        {
            GameObject hero = collision.transform.root.gameObject;
            PlayerController playerController = hero.GetComponent<PlayerController>();
            HealthController playerHealth = hero.GetComponent<HealthController>();

            int knockbackXDir = hero.transform.position.x - transform.position.x < 0 ? -1 : 1;
            Vector2 knockbackDir = new Vector2(knockbackXDir, 0);

            playerHealth.DealDamage(damage);
            playerController.Knockback(knockbackDir, knockbackPower, knockbackTime);
        }
    }
}
