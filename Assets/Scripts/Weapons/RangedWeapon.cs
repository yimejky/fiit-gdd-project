using System;
using UnityEngine;

public class RangedWeapon : Weapon
{
	public Transform attackPoint;

	private GameObject wielderGameObject;
	public new void Update()
	{
		base.Update();
		wielderGameObject = transform.parent.gameObject;
	}

    public override void Attack()
    {
		IRangedWeaponWielder wielder = wielderGameObject.GetComponent<IRangedWeaponWielder>();
		if (wielder == null)
        {
			Debug.Log("RangedWeapon: missing wielder");
			return;
		}

        if (isOnCooldown())
            return;

		GameObject arrowGameobject = Instantiate(Resources.Load("Prefabs/Weapons/Arrow"), attackPoint.position, Quaternion.identity) as GameObject;
		Arrow arrow = arrowGameobject.GetComponent<Arrow>();
		try {
			// allow fire even if cant reach
			bool shouldIgnoreCantReach = wielderGameObject.CompareTag(Constants.PLAYER_TAG);

			Vector3 force = arrow.CalculateArrowForceVector(wielder.GetRangedAttackDirection(), wielder.ArrowSpeed, wielder.IsArrowDirect, shouldIgnoreCantReach);
			arrow.Init(transform.parent.gameObject, force, damage, knockbackPower, knockbackTime);
			Debug.Log($"Ranged Weapon {force}, {knockbackPower}");
		} catch (Exception)
		{
			Debug.Log($"Ranged Weapon cant reach");
			Destroy(arrowGameobject);
		}

		base.Attack();
	}
}
