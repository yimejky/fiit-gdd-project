using UnityEngine;

public class ArcherEnemy : Enemy, IRangedWeaponWielder
{
    new ArcherEnemyConfig enemyConfig => GameConfigManager.Get().gameConfig.archerEnemyConfig; 

    public bool IsArrowDirect => enemyConfig.isArrowDirect;
    public float ArrowSpeed => enemyConfig.arrowSpeed;

    protected override void Start()
    {
        base.enemyConfig = enemyConfig;
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        HandleStatesChanging();

        switch (state)
        {
            case State.Idle:
                {
                    break;
                }
            case State.Attacking:
                {
                    weapon.Attack();
                    break;
                }
        }
    }

    public Vector2 GetRangedAttackDirection()
    {
        return target.transform.position;
    }
}
