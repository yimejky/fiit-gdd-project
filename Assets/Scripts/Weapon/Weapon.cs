using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float attackCooldown = 0.6f;

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
