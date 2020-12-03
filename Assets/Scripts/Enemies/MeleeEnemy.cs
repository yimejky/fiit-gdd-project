using UnityEngine;

public class MeleeEnemy : Enemy, IMeleeWeaponWielder
{
    private float attackRange = 1.5f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (knockbackController.canMove && playerDistance <= startAttackDistance)
        {
            if (playerDistance > attackRange)
            {
                // Debug.Log($"Debug distance {playerDistance}, {attackRange}");
                patrol.setPatrolEnabled(false);
                MoveToTarget(player);
            } else
            {
                weapon.Attack();
            }
        }

        if (playerDistance > startAttackDistance && patrol != null)
        {
            patrol.setPatrolEnabled(true);
        }
    }

    public Vector2 GetMeeleAttackDirection()
    {
        return new Vector2(1, 0);
    }
}