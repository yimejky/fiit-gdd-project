using UnityEngine;

public interface IMeleeWeaponWielder 
{
    Vector2 GetMeeleAttackDirection();
}

public interface IRangedWeaponWielder
{
    bool IsArrowDirect { get; }
    float ArrowSpeed { get; }

    Vector2 GetRangedAttackDirection();
}
