using UnityEngine;

public class Weapon : MonoBehaviour
{
    public LayerMask enemyLayer;
    public Transform attackPoint;
    public float attackRange = 0.8f;
    public int damage = 10;
    public Vector2 knockbackPower = new Vector2(150, 150);
    public float knockbackTime = 0.3f;

    private Animator weaponAnimator;

    private void Start()
    {
        weaponAnimator = GetComponent<Animator>();
    }

    public void Attack()
    {
        weaponAnimator.Play("Attack");

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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 attackPosition = attackPoint.position;
        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }
}
