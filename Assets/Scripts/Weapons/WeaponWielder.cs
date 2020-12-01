using UnityEngine;

public interface IMeleeWeaponWielder 
{
    Vector2 GetMeeleAttackDirection();
}

public interface IRangedWeaponWielder
{
    Vector2 GetRangedAttackDirection();
}