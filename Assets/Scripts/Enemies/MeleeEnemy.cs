using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(KnockbackController), typeof(AudioController))]
public class MeleeEnemy : Enemy, IMeleeWeaponWielder
{
    new MeleeEnemyConfig enemyConfig => GameConfigManager.Get().gameConfig.meleeEnemyConfig;

    protected override void Start()
    {
        base.enemyConfig = enemyConfig;
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
                    if (targetDistance > enemyConfig.attackRange)
                    {
                        FixedMoveToTarget(target, enemyConfig.movementSpeed);
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