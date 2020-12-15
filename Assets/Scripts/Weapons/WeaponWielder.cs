using UnityEngine;

public interface IMeleeWeaponWielder 
{
    Vector2 GetMeeleAttackDirection();
}

public interface IRangedWeaponWielder
{
    bool IsArrowDirect { get; set; }
    float ArrowSpeed { get; set; }

    Vector2 GetRangedAttackDirection();
}
