using UnityEngine;

[CreateAssetMenu()]
public class EnemyConfig : ScriptableObject
{
    public int touchDamage = 10;
    public int movementSpeed = 100;
    public float attackCooldown = 5;
    public float defaultStartAttackingStateDistance = 5;
    public float endAttackingStateDistance = 15;
    public float touchKnockbackTime = 0.3f;
    public Vector2 touchKnockbackPower = new Vector2(2, 2);
}