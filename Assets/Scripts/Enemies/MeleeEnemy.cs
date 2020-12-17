using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(KnockbackController), typeof(AudioController))]
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
        HandleStatesChanging();

        switch (state)
        {
            case State.Idle:
                {
                    if (knockbackController.canMove && patrol)
                    {
                        patrol.setPatrolEnabled(true);
                    }

                    break;
                }
            case State.Attacking:
                {
                    if (targetDistance > attackRange)
                    {
                        // Debug.Log($"Debug distance {playerDistance}, {attackRange}");
                        if (patrol) patrol.setPatrolEnabled(false);
                        if (knockbackController.canMove) FixedMoveToTarget(target);
                    }
                    else
                    {
                        weapon.Attack();
                    }

                    break;
                }
        }
    }

    public Vector2 GetMeeleAttackDirection()
    {
        return new Vector2(1, 0);
    }
}