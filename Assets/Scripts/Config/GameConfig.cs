using UnityEngine;

[CreateAssetMenu()]
public class GameConfig : ScriptableObject
{
    public HealthControllerConfig meleeEnemyHealth;
    public MeleeEnemyConfig meleeEnemyConfig;
    public WeaponConfig meleeEnemyWeapon;

    public HealthControllerConfig archerEnemyHealth;
    public ArcherEnemyConfig archerEnemyConfig;
    public WeaponConfig archerEnemyWeapon;

    public HealthControllerConfig playerHealth;
    public PlayerControllerConfig playerConfig;
    public WeaponConfig playerMeleeWeapon;
    public WeaponConfig playerRangedWeapon;

    public HealthControllerConfig destroyableBlockHealth;
}
