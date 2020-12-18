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
       // Debug.Log($"melee before edge {isBeforeEdge}");

        switch (state)
        {
            case State.Idle:
                {
                    break;
                }
            case State.Attacking:
                {
                    if (targetDistance > attackRange)
                    {
                        Debug.Log($"Debug attacking mode, {attackRange}");
                        FixedMoveToTarget(target, movementSpeed);
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