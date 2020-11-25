using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum BotDifficulty { Easy, Medium, Hard }
    public BotDifficulty botDifficulty = BotDifficulty.Medium;
    public float speed = 0f;
    public GameObject player;

    public int damage = 10;
    public Vector2 knockbackPower = new Vector2(150, 150);
    public float knockbackTime = 0.3f;

    private bool canMove = true;
    private float range;
    private float startRange = 5f;
    private Rigidbody2D rb2D;

    private void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            range = Vector2.Distance(transform.position, player.transform.position);
            if (range <= startRange)
            {
                Vector3 relativePos = transform.position - player.transform.position;
                Vector2 velocity = new Vector2(relativePos.x > 0 ? -1 : 1, 0) * speed * Time.fixedDeltaTime;
                velocity.y = rb2D.velocity.y;

                // Debug.Log("Debug velocity: " + velocity);
                // Debug.Log($"pos {transform.position}, {player.transform.position}, {transform.position - player.transform.position}");
                rb2D.velocity = velocity;
            }
        }
    }

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
            Vector2 knockbackDir = new Vector2(knockbackXDir, 1);

            playerHealth.DealDamage(damage);
            playerController.Knockback(knockbackDir, knockbackPower, knockbackTime);
        }
    }

    public void Knockback(Vector2 knockbackDir, Vector2 knockbackPower, float knockbackTime)
    {
        StartCoroutine(KnockbackCoroutine(knockbackDir, knockbackPower, knockbackTime));
    }

    private IEnumerator KnockbackCoroutine(Vector2 knockbackDir, Vector2 knockbackPower, float knockbackTime)
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
