using JetBrains.Annotations;
using UnityEngine;

// inspired by https://www.youtube.com/watch?v=sPiVz1k-fEs
public class CombatSystem : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.8f;
    public LayerMask enemyLayers;
    public Vector2 knockbackPower = new Vector2(150, 150);
    public int damage = 10;
    
    private bool attackInput = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            attackInput = true;
        }
    }

    void FixedUpdate()
    {
        if (attackInput)
        {
            Attack();
        }
        attackInput = false;
    }

    private void Attack()
    {
        Vector3 attackPosition = attackPoint.position;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayers);
        foreach (Collider2D enemyCol in hitEnemies)
        {
            Debug.Log("Hit " + enemyCol.name);

            GameObject enemy = enemyCol.gameObject;
            Rigidbody2D enemyRigid = enemy.GetComponent<Rigidbody2D>();
            HealthController enemyHealth = enemy.GetComponent<HealthController>();

            enemyHealth.DealDamage(damage);
            int knockbackXDir = enemy.transform.position.x - attackPoint.transform.position.x < 0 ? -1 : 1;
            Vector3 knockbackForce = new Vector3(knockbackXDir * knockbackPower.x, knockbackPower.y, 0);
            Debug.Log("attack knock " + knockbackForce);
            enemyRigid.AddForce(knockbackForce, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackPosition = attackPoint.position;
        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }
}
