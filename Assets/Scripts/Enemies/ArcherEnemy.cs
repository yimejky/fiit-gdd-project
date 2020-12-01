using UnityEngine;

public class ArcherEnemy : Enemy, IRangedWeaponWielder
{
    public bool isArrowDirect = true;
    public float arrowSpeed = 10f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (knockbackController.canMove && playerDistance <= startAttackDistance)
        {
            // Debug.Log($"Range {range} {attackRange}, debug {actualAttackCooldown}");
            weapon.Attack();
        }
    }

    public Vector2 GetRangedAttackDirection()
    {
        return player.transform.position;
    }
}
