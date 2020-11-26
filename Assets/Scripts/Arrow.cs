using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Arrow : MonoBehaviour
{
    public int damage = 10;
    private bool isFrozen = false;
    private float aliveTime = 0f;
    private readonly float angleOffset = 90;
    private readonly float despawnTime = 10f;
    private GameObject creator;
    private Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void Init(GameObject newCreator, Vector3 force)
    {
        creator = newCreator;
        rb2D.AddForce(force, ForceMode2D.Impulse);
    }

    private void Update()
    {
        aliveTime += Time.deltaTime;
        if (aliveTime > despawnTime)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (!isFrozen)
        {
            Vector2 velo = rb2D.velocity;
            float angle = Mathf.Atan2(velo.y, velo.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - angleOffset, Vector3.forward);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFrozen)
        {
            GameObject hitGameObject = collision.gameObject;
            Transform hitParentTrans = collision.transform.parent;
            GameObject hitParentGameObject = hitParentTrans.gameObject;
            bool isHurtbox = hitGameObject.CompareTag(Constants.HURTBOX_TAG);
            bool isPlayer = hitParentGameObject.CompareTag(Constants.PLAYER_TAG);
            bool isEnemy = hitParentGameObject.CompareTag(Constants.ENEMY_TAG);

            // skip if hurtbox parent == creator
            if (isHurtbox && creator == hitParentGameObject)
                return;

            // Debug.Log("freezing arrow trigger enter " + collision.name);
            isFrozen = true;
            rb2D.isKinematic = true;
            rb2D.velocity = Vector3.zero;

            if (isPlayer || isEnemy)
            {
                // Debug.Log($"{hitGameObject.name}: arrow hit player or enemy");
                hitParentGameObject.GetComponent<HealthController>().DealDamage(damage);
                transform.parent = hitParentTrans;
            }
            else if (hitGameObject.CompareTag(Constants.GROUND_TAG))
            {
                transform.parent = hitGameObject.transform;
                Debug.Log($"{gameObject.name}: arrow hit ground");
            }
        }
    }

    public Vector3 CalculateArrowForceVector(Vector2 target, float arrowSpeed, bool isArrowDirect)
    {
        Vector2 source = transform.position;
        // Debug.Log($"Target {target}, Source {source}");

        float y = target.y - source.y;
        float x = (new Vector2(source.x - target.x, 0)).magnitude;
        float g = -Physics2D.gravity.y;
        float v = arrowSpeed;
        float v2 = v * v;
        float v4 = v2 * v2;
        float x2 = x * x;

        float sqrt = v4 - (g * (g * x2 + 2 * y * v2));
        if (sqrt <= 0)
        {
            sqrt = 0.000001f;
        }

        sqrt = Mathf.Sqrt(sqrt);
        sqrt = isArrowDirect ? sqrt * (-1) : sqrt;
        float upper = v2 + sqrt;
        float lower = g * x;
        float angle = Mathf.Atan(upper/lower);

        Vector3 force = new Vector3(v * Mathf.Cos(angle), v * Mathf.Sin(angle), 0);
        // Debug.Log($"arrow Force {force}, sqrt {sqrt}, v {v}, angle {angle}, g {g}, x {x}, upper {upper}, lower {lower}");

        if (target.x - source.x < 0) force.x *= -1;

        return force;
    }
}
