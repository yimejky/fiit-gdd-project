using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.8f;
    public Vector2 knockbackPower = new Vector2(10, 10);
    public int damage = 10;
    public LayerMask enemyLayer;

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
            GameObject enemy = enemyCol.gameObject;
            Rigidbody2D enemyRigid = enemy.GetComponent<Rigidbody2D>();
            HealthController enemyHealth = enemy.GetComponent<HealthController>();

            enemyHealth.DealDamage(damage);
            int knockbackXDir = enemy.transform.position.x - attackPoint.transform.position.x < 0 ? -1 : 1;
            Vector3 knockbackForce = new Vector3(knockbackXDir * knockbackPower.x, knockbackPower.y, 0);
            // Debug.Log("attack knock " + knockbackForce);
            enemyRigid.AddForce(knockbackForce, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 attackPosition = attackPoint.position;
        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }
}
