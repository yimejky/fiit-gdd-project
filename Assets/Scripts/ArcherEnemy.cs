using UnityEngine;

public class ArcherEnemy : Enemy
{
    public bool isArrowDirect = true;
    public float arrowSpeed = 10f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (knockbackController.canMove && playerDistance <= startAttackDistance)
        {
            // Debug.Log($"Range {range} {attackRange}, debug {actualAttackCooldown}");
            ShootArrow();
        }
    }

    private void ShootArrow()
    {
        if (actualAttackCooldown > 0)
            return;

        GameObject arrowGameobject = Instantiate(Resources.Load("Prefabs/Arrow"), transform.position, Quaternion.identity) as GameObject;
        Arrow arrow = arrowGameobject.GetComponent<Arrow>();
        Vector3 force = arrow.CalculateArrowForceVector(player.transform.position, arrowSpeed, isArrowDirect);
        arrow.Init(gameObject, force);
        actualAttackCooldown = attackCooldown;
    }
}
