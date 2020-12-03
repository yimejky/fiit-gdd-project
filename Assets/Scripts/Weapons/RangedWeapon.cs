using UnityEngine;

public class RangedWeapon : Weapon
{
	public float arrowSpeed = 10f;
	public Transform attackPoint;

	private IRangedWeaponWielder wielder;
	public new void Update()
	{
		base.Update();
		wielder = transform.parent.GetComponent<IRangedWeaponWielder>();
	}

    public override void Attack()
    {
		if (wielder == null)
        {
			Debug.Log("RangedWeapon: missing wielder");
			return;
		}

        if (isOnCooldown())
            return;

		bool isArrowDirect = true;

		GameObject arrowGameobject = Instantiate(Resources.Load("Prefabs/Weapons/Arrow"), attackPoint.position, Quaternion.identity) as GameObject;
		Arrow arrow = arrowGameobject.GetComponent<Arrow>();
		Vector3 force = arrow.CalculateArrowForceVector(wielder.GetRangedAttackDirection(), arrowSpeed, isArrowDirect);
		Debug.Log($"Ranged Weapon {force}");
		arrow.Init(transform.parent.gameObject, force);

		base.Attack();
    }
}
