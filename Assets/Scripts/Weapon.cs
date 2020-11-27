using UnityEngine;

public class Weapon : MonoBehaviour
{
    public LayerMask hurtboxLayer;
    public Transform attackPoint;
    public float attackRange = 0.8f;
    public float attackCooldown = 0.6f;
    public int damage = 10;

    private Animator weaponAnimator;
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    private float animationCooldown;
    private float actualAttackCooldown = 0;

    private void Start()
    {
        weaponAnimator = GetComponent<Animator>();
        defaultPosition = transform.localPosition;
        defaultRotation = transform.localRotation;
        actualAttackCooldown = 0;
    }
    private void Update()
    {
        actualAttackCooldown -= Time.deltaTime;
        animationCooldown -= Time.deltaTime;
        if (animationCooldown < 0)
        {
            transform.localPosition = defaultPosition;
            transform.localRotation = defaultRotation;
        }
    }

    private void WeaponAnimation()
    {
        float angle;
        try
        {
            Vector2 swordDirection = GetComponentInParent<PlayerController>().GetSwordDirection();
            transform.localPosition = attackPoint.transform.localPosition;
            angle = 90 * swordDirection.y / (swordDirection.x + 1);
        }
        catch
        {
            angle = 0;
        }
        
        transform.Rotate(new Vector3(0, 0, angle));
        weaponAnimator.Play("Attack");
        // TODO make this adjustable to different weapons
        animationCooldown = 0.42f;
    }

    public void Attack()
    {
        if (actualAttackCooldown > 0)
            return;

        WeaponAnimation();

        Vector3 attackPosition = attackPoint.position;
        Collider2D[] hurtboxes = Physics2D.OverlapCircleAll(attackPosition, attackRange, hurtboxLayer);
        foreach (Collider2D hurtbox in hurtboxes)
        {
            GameObject parent = hurtbox.transform.parent.gameObject;
            if (parent == transform.parent.gameObject)
                continue;

            Debug.Log("Hit " + hurtbox.name);
            parent.GetComponent<HealthController>().DealDamage(damage);
            parent.GetComponent<KnockbackController>().Knock(gameObject, false);
            break;
        }

        actualAttackCooldown = attackCooldown;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 attackPosition = attackPoint.position;
        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }
}
