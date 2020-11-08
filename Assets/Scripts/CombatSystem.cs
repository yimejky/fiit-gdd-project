using UnityEngine;

// inspired by https://www.youtube.com/watch?v=sPiVz1k-fEs
public class CombatSystem : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Attack();
        }
    }

    private void Attack()
    {
        Vector3 attackPosition = attackPoint.position;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
            HealthController enemyHealth = enemy.gameObject.GetComponent<HealthController>();
            enemyHealth.DealDamage(10);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 attackPosition = attackPoint.position;
        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }
}
