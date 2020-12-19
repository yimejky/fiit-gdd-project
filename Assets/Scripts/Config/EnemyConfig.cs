using UnityEngine;

[CreateAssetMenu()]
public class EnemyConfig : ScriptableObject
{
    public int touchDamage = 10;
    public int movementSpeed = 100;
    public float defaultStartAttackingStateDistance = 5f;
    public float defaultEndAttackingStateDistance = 15f;
    public float touchKnockbackTime = 0.3f;
    public Vector2 touchKnockbackPower = new Vector2(2, 2);
}