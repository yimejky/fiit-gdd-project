using UnityEngine;

public class RangedWeapon : Weapon
{
	public float arrowSpeed = 10f;
	public Transform attackPoint;

	private RangedWeaponWielder wielder;

	private void Start()
	{
		wielder = transform.parent.GetComponent<RangedWeaponWielder>();
	}

    public override void Attack()
    {
        if (isOnCooldown())
            return;

		bool isArrowDirect = true;

		GameObject arrowGameobject = Instantiate(Resources.Load("Prefabs/Weapons/Arrow"), attackPoint.position, Quaternion.identity) as GameObject;
		Arrow arrow = arrowGameobject.GetComponent<Arrow>();
		Vector3 force = arrow.CalculateArrowForceVector(wielder.GetRangedAttackDirection(), arrowSpeed, isArrowDirect);
		arrow.Init(gameObject, force);

		base.Attack();
    }
}
public interface RangedWeaponWielder
{
	Vector2 GetRangedAttackDirection();
}
