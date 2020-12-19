using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(KnockbackController), typeof(AudioController))]
public class Enemy : MonoBehaviour
{
    public enum State { Idle, Attacking }

    public bool isBeforeEdge = false;
    public Weapon weapon;
    public State state = State.Idle;
    public GameObject target;

    public float startAttackingStateDistance = -1f;
    public float endAttackingStateDistance = -1f;

    protected float targetDistance = 999f;
    protected float actualAttackCooldown = 0f;
    protected readonly int mapBottomLimit = -50;
    protected readonly float edgeDetectionMaxDistance = 3f;
    protected Rigidbody2D rb2D;
    protected KnockbackController knockbackController;
    protected AudioController audioController;
    protected Patrol patrol;

    protected EnemyConfig enemyConfig;
    private GameObject player;

    protected virtual void Start()
    {
        player = GameObject.Find("Hero");

        rb2D = gameObject.GetComponent<Rigidbody2D>();
        knockbackController = gameObject.GetComponent<KnockbackController>();
        actualAttackCooldown = 0;

        Debug.Log($"Start attacking distance {startAttackingStateDistance}, {endAttackingStateDistance}");
        if (startAttackingStateDistance < 0)
            startAttackingStateDistance = enemyConfig.defaultStartAttackingStateDistance;
        if (endAttackingStateDistance < 0)
            endAttackingStateDistance = enemyConfig.defaultEndAttackingStateDistance;

        Debug.Log($"Start attacking distance {startAttackingStateDistance}, {endAttackingStateDistance}");


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
        CheckIfAtEdge();
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
                        // Debug.Log($"Enemy going to attacking state {gameObject.name}");
                        SetAttackingState(player);
                        patrol?.setPatrolEnabled(false);
                    }
                    break;
                }
            case State.Attacking:
                {
                    targetDistance = Vector2.Distance(transform.position, target.transform.position);
                    if (targetDistance > endAttackingStateDistance)
                    {
                        // Debug.Log($"Enemy going to idle state {gameObject.name}");
                        SetIdleState();
                        patrol?.setPatrolEnabled(true);
                    }

                    break;
                }
        }
    }

    public void FixedMoveToTarget(GameObject target, float speed, bool useVelocity=true)
    {        
        FixedMoveToTarget(target.transform.position, speed, useVelocity);
    }

    public void FixedMoveToTarget(Vector3 position, float speed, bool useVelocity=true)
    {
        // pause movement if in knockback or at edge
        if (!knockbackController.canMove)
        {
            // Debug.Log($"Cant move, knock {knockbackController.canMove}, before edge {isBeforeEdge}");
            return;
        }

        Vector2 relativePos = transform.position - position;
        transform.rotation = Quaternion.Euler(0, relativePos.x < 0.1 ? 0 : 180, 0);

        if (isBeforeEdge)
        {
            // Debug.Log($"Cant move before edge {isBeforeEdge}");
            return;
        }

        if (useVelocity)
        {
            float x = (relativePos.x > 0 ? -1 : 1) * speed * Time.fixedDeltaTime;
            Vector2 velocity = new Vector2(x, rb2D.velocity.y);

            // Debug.Log("Debug velocity: " + velocity);
            // Debug.Log($"pos {transform.position}, {position}, {transform.position - position}");
            rb2D.velocity = velocity;
        } 
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.fixedDeltaTime);
        }
    }

    void RecieveDamage(GameObject enemy, GameObject attacker, int actualHealth)
    {
        if (target == null && attacker != null && actualHealth > 0)
        {
            SetAttackingState(attacker);
        }
    }

    public void SetAttackingState(GameObject attacker)
    {
        state = State.Attacking;
        target = attacker;
    }

    public void SetIdleState()
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

    void CheckIfAtEdge()
    {
        Vector2 pos = transform.position;
        pos.x += (transform.rotation.eulerAngles.y == 0 ? 1 : -1) * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(pos, -Vector2.up);

        if (hit.collider == null)
        {
            isBeforeEdge = true;
            return;
        } else {
            float distance = Mathf.Abs(hit.point.y - transform.position.y);
            // Debug.Log($"edge checking: {distance}");
            if (distance > edgeDetectionMaxDistance)
            {
                isBeforeEdge = true;
                return;
            }
        }

        isBeforeEdge = false;
    }

    void OnPlayerTouch(Collider2D collision)
    {
        GameObject parent = collision.transform.parent?.gameObject;
        if (collision.CompareTag(Constants.HURTBOX_TAG) && parent.CompareTag(Constants.PLAYER_TAG))
        {
            parent.GetComponent<HealthController>().DealDamage(gameObject, enemyConfig.touchDamage);
            parent.GetComponent<KnockbackController>().Knock(gameObject.transform.position, enemyConfig.touchKnockbackPower, enemyConfig.touchKnockbackTime);
        }
    }

    private void OnDrawGizmos()
    {   
        Gizmos.color = Color.green;
        Vector2 pos = transform.position;
        pos.x += (transform.rotation.eulerAngles.y == 0 ? 1 : -1) * 0.5f;
        Gizmos.DrawRay(pos, -Vector2.up);
    }
}
