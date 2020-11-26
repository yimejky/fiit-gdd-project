using UnityEngine;

public class ArcherEnemy : Enemy
{
    public bool arrowDirect = true;
    public float arrowSpeed = 10f;

    public override void Start()
    {
        base.Start();
    }

    public override void FixedUpdate()
    {
        if (canMove)
        {
            range = Vector2.Distance(transform.position, player.transform.position);
            if (range <= attackRange)
            {
                ShootArrow();
            }
        }
    }

    private void ShootArrow()
    {
        if (actualAttackCooldown <= 0)
        {
            GameObject arrowGameobject = Instantiate(Resources.Load("Prefabs/Arrow"), transform.position, Quaternion.identity) as GameObject;
            Arrow arrow = arrowGameobject.GetComponent<Arrow>();
            Vector3 force = CalculateArrowForceVector(player.transform.position, arrow.transform.position, arrowSpeed, arrowDirect);

            if (force.magnitude > 0)
            {
                arrow.Init(gameObject, force);
                actualAttackCooldown = attackCooldown;
            }
        }
    }

    private Vector3 CalculateArrowForceVector(Vector3 target, Vector3 source, float speed, bool directFire)
    {
        float y = target.y - source.y;
        float x = (new Vector3(source.x - target.x, 0, 0)).magnitude;
        float g = -Physics2D.gravity.y;
        float v = speed;
        float v2 = v * v;
        float v4 = v2 * v2;
        float x2 = x * x;

        float sqrt = v4 - (g * (g * x2 + 2 * y * v2));
        if (sqrt <= 0)
        {
            return Vector3.zero;
        }

        sqrt = Mathf.Sqrt(sqrt);
        sqrt = directFire ? sqrt * (-1) : sqrt;
        float angle = Mathf.Atan((v2 + sqrt) / (g * x));

        Vector3 force = new Vector3(v * Mathf.Cos(angle), v * Mathf.Sin(angle), 0);
        if (target.x - source.x < 0) force.x *= -1;

        return force;
    }
}
