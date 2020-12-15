using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int damage = 10;
    public float attackCooldown = 0.6f;
    public float knockbackTime = 0.3f;
    public Vector2 knockbackPower = new Vector2(10, 10);

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
