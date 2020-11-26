using UnityEngine;

public class Weapon : MonoBehaviour
{
    public LayerMask enemyLayer;
    public Transform attackPoint;
    public float attackRange = 0.8f;
    public int damage = 10;
    public Vector2 knockbackPower = new Vector2(10, 10);
    public float knockbackTime = 0.3f;
    public float attackCooldown = 0.6f;

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
            Vector2 swordDirection = (GetComponentInParent<PlayerController>()).GetSwordDirection();
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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayer);
        foreach (Collider2D enemyCol in hitEnemies)
        {
            Debug.Log("Hit " + enemyCol.name);
            GameObject enemyGameObject = enemyCol.gameObject;
            HealthController enemyHealth = enemyGameObject.GetComponent<HealthController>();
            Enemy enemy = enemyGameObject.GetComponent<Enemy>();

            int knockbackXDir = enemyGameObject.transform.position.x - attackPoint.transform.position.x < 0 ? -1 : 1;
            Vector2 knockbackDir = new Vector2(knockbackXDir, 1);

            enemyHealth.DealDamage(damage);
            enemy.Knockback(knockbackDir, knockbackPower, knockbackTime);
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
