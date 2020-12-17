using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(KnockbackController), typeof(AudioController))]
public class Enemy : MonoBehaviour
{
    public enum State { Idle, Attacking }
    public State state = State.Idle;
    public GameObject target;

    public float movmentSpeed = 100f;
    public float attackCooldown = 5f;
    public int touchDamage = 10;
    public Vector2 touchKnockbackPower = new Vector2(10, 10);
    public float touchKnockbackTime = 0.3f;
    public Weapon weapon;

    protected float targetDistance = 999f;
    protected float startAttackingStateDistance = 5f;
    protected float endAttackingStateDistance = 15f;
    protected float actualAttackCooldown = 0f;
    protected readonly int mapBottomLimit = -50;
    protected Rigidbody2D rb2D;
    protected KnockbackController knockbackController;
    protected AudioController audioController;
    protected Patrol patrol;

    private GameObject player;

    protected virtual void Start()
    {
        player = GameObject.Find("Hero");

        rb2D = gameObject.GetComponent<Rigidbody2D>();
        knockbackController = gameObject.GetComponent<KnockbackController>();
        actualAttackCooldown = 0;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Patrol>() != null)
            {
                patrol = child.GetComponent<Patrol>();
                break;
            }
        }

        GetComponent<HealthController>().HealthUpdateEvent.AddListener(RecieveDamage);
    }

    protected virtual void Update()
    {
        actualAttackCooldown -= Time.deltaTime;
        if (transform.position.y < mapBottomLimit)
        {
            GetComponent<HealthController>().DealDamage(null, 10000);
        }
    }

    protected virtual void FixedUpdate()
    {
    }

    protected void HandleStatesChanging()
    {
        switch (state)
        {
            case State.Idle:
                {
                    float playerDistance = Vector2.Distance(transform.position, player.transform.position);
                    if (playerDistance < startAttackingStateDistance)
                    {
                        Debug.Log($"Enemy going to attacking state {gameObject.name}");
                        SetAttackingState(player);
                    }
                    break;
                }
            case State.Attacking:
                {
                    targetDistance = Vector2.Distance(transform.position, target.transform.position);
                    if (targetDistance > endAttackingStateDistance)
                    {
                        Debug.Log($"Enemy going to idle state {gameObject.name}");
                        SetIdleState();
                    }

                    break;
                }
        }
    }

    protected void FixedMoveToTarget(GameObject target)
    {
        if (patrol && patrol.patrolEnabled)
        {
            Debug.Log("Moving on patrol cancelled");
            return;
        }

        Vector2 relativePos = transform.position - target.transform.position;
        float x = (relativePos.x > 0 ? -1 : 1) * movmentSpeed * Time.fixedDeltaTime;
        Vector2 velocity = new Vector2(x, rb2D.velocity.y);

        // Debug.Log("Debug velocity: " + velocity);
        // Debug.Log($"pos {transform.position}, {player.transform.position}, {transform.position - player.transform.position}");
        transform.rotation = Quaternion.Euler(0, relativePos.x < 0.1 ? 0 : 180, 0);
        rb2D.velocity = velocity;
    }

    void RecieveDamage(GameObject attacker, int actualHealth)
    {
        if (target == null && attacker != null && actualHealth > 0)
        {
            SetAttackingState(attacker);
        }
    }

    private void SetAttackingState(GameObject attacker)
    {
        state = State.Attacking;
        target = attacker;
    }

    private void SetIdleState()
    {
        state = State.Idle;
        target = null;
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
            parent.GetComponent<HealthController>().DealDamage(gameObject, touchDamage);
            parent.GetComponent<KnockbackController>().Knock(gameObject.transform.position, touchKnockbackPower, touchKnockbackTime);
        }
    }
}
