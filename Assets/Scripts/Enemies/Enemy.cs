using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(KnockbackController))]
public class Enemy : MonoBehaviour
{
    public enum BotDifficulty { Easy, Medium, Hard }
    public BotDifficulty botDifficulty = BotDifficulty.Medium;
    public float movmentSpeed = 100f;
    public float attackCooldown = 5f;
    public int touchDamage = 10;
    public Vector2 touchKnockbackPower = new Vector2(10, 10);
    public float touchKnockbackTime = 0.3f;
    public Weapon weapon;

    protected float playerDistance = 999f;
    protected float startAttackDistance = 5f;
    protected float actualAttackCooldown = 0f;
    protected GameObject player;
    protected Rigidbody2D rb2D;
    protected KnockbackController knockbackController;

    protected virtual void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        knockbackController = gameObject.GetComponent<KnockbackController>();
        player = GameObject.Find("Hero");
        actualAttackCooldown = 0;
    }

    protected virtual void Update()
    {
        actualAttackCooldown -= Time.deltaTime;
        playerDistance = Vector2.Distance(transform.position, player.transform.position);
    }

    protected virtual void FixedUpdate()
    {

    }

    protected void MoveToTarget(GameObject target)
    {
        Vector2 relativePos = transform.position - target.transform.position;
        float x = (relativePos.x > 0 ? -1 : 1) * movmentSpeed * Time.fixedDeltaTime;
        Vector2 velocity = new Vector2(x, rb2D.velocity.y);

        // Debug.Log("Debug velocity: " + velocity);
        // Debug.Log($"pos {transform.position}, {player.transform.position}, {transform.position - player.transform.position}");
        transform.rotation = Quaternion.Euler(0, relativePos.x < 0.1 ? 0 : 180, 0);
        rb2D.velocity = velocity;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("enemy trigger enter" + collision.name);
        OnPlayerTouch(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Debug.Log("enemy trigger stay" + collision.name);
        OnPlayerTouch(collision);
    }

    void OnPlayerTouch(Collider2D collision)
    {
        GameObject parent = collision.transform.parent?.gameObject;
        if (collision.CompareTag(Constants.HURTBOX_TAG) && parent.CompareTag(Constants.PLAYER_TAG))
        {
            parent.GetComponent<HealthController>().DealDamage(touchDamage);
            parent.GetComponent<KnockbackController>().Knock(gameObject, touchKnockbackPower, touchKnockbackTime);
        }
    }
}
