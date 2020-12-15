using UnityEngine;

public class ArcherEnemy : Enemy, IRangedWeaponWielder
{
    public bool isArrowDirect = true;
    public float arrowSpeed = 10f;

    public bool IsArrowDirect { get => isArrowDirect; set => isArrowDirect = value; }
    public float ArrowSpeed { get => arrowSpeed; set => arrowSpeed = value; }

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
