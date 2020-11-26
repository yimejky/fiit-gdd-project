using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(KnockbackController))]
public class Enemy : MonoBehaviour
{
    public enum BotDifficulty { Easy, Medium, Hard }
    public BotDifficulty botDifficulty = BotDifficulty.Medium;
    public float movmentSpeed = 100f;
    public float attackCooldown = 5f;

    protected float range;
    protected float attackRange = 5f;
    protected float actualAttackCooldown = 0f;
    protected GameObject player;
    protected Weapon weapon;
    protected Rigidbody2D rb2D;
    protected KnockbackController knockbackController;

    public virtual void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        knockbackController = gameObject.GetComponent<KnockbackController>();
        player = GameObject.Find("Hero");
        weapon = GameObject.Find("Weapon").GetComponent<Weapon>();
        actualAttackCooldown = 0;
    }

    public virtual void Update()
    {
        actualAttackCooldown -= Time.deltaTime;
    }

    public virtual void FixedUpdate()
    {
        if (knockbackController.canMove)
        {
            range = Vector2.Distance(transform.position, player.transform.position);
            if (range <= attackRange)
            {
                MoveToTarget(player);

                // TODO if close attack hero
                // weapon.Attack();
            }
        }
    }

    protected void MoveToTarget(GameObject target)
    {
        Vector3 relativePos = transform.position - target.transform.position;
        Vector2 velocity = new Vector2(relativePos.x > 0 ? -1 : 1, 0) * movmentSpeed * Time.fixedDeltaTime;
        velocity.y = rb2D.velocity.y;

        // Debug.Log("Debug velocity: " + velocity);
        // Debug.Log($"pos {transform.position}, {player.transform.position}, {transform.position - player.transform.position}");
        transform.rotation = Quaternion.Euler(0, relativePos.x < 0.1 ? 0 : 180, 0);
        rb2D.velocity = velocity;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("enemy trigger enter" + collision.name);
        DetectPlayerTouch(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Debug.Log("enemy trigger stay" + collision.name);
        DetectPlayerTouch(collision);
    }

    void DetectPlayerTouch(Collider2D collision)
    {
        Transform parentTrans = collision.transform.parent;
        if (collision.CompareTag(Constants.HURTBOX_TAG) && parentTrans.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            GameObject parent = parentTrans.gameObject;
            parent.GetComponent<KnockbackController>().Knock(gameObject, true);
        }
    }
}
