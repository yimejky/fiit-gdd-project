using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float attackCooldown = 0.6f;
    public int damage = 10;
    public Vector2 knockbackPower = new Vector2(10, 10);
    public float knockbackTime = 0.3f;

    private float currentCooldown = 0;

    public virtual void Attack()
    {
        currentCooldown = attackCooldown;
    }

    protected bool isOnCooldown()
    {
        return currentCooldown > 0;
    }

    protected void Update()
    {
        currentCooldown -= Time.deltaTime;
    }
}
