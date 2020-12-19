using UnityEngine;

[CreateAssetMenu()]
public class WeaponConfig : ScriptableObject
{
    public int defaultDamage = 10;
    public float attackCooldown = 0.6f;
    public float knockbackTime = 0.3f;
    public Vector2 knockbackPower = new Vector2(10, 10);
}
