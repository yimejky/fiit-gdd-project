using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public int damage = 10;
    protected float aliveTime = 0f;
    protected readonly float angleOffset = 90;
    protected readonly float despawnTime = 10f;
    protected GameObject creator;
    protected Rigidbody2D rb2D;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = 0;
    }

    protected virtual void Update()
    {
        if (despawnTime > 0)
        {
            aliveTime += Time.deltaTime;
            if (aliveTime > despawnTime)
            {
                Destroy(gameObject);
            }
        }
    }

    protected virtual void FixedUpdate()
    {

    }

    public void Init(GameObject newCreator, Vector3 force)
    {
        creator = newCreator;
        rb2D.AddForce(force, ForceMode2D.Impulse);
    }

    protected bool IsHurtboxAndCreator(Collider2D collision)
    {
        GameObject hitGameObject = collision.gameObject;
        Transform hitParentTrans = collision.transform.parent;
        GameObject hitParentGameObject = hitParentTrans.gameObject;
        bool isHurtbox = hitGameObject.CompareTag(Constants.HURTBOX_TAG);

        return isHurtbox && creator == hitParentGameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsHurtboxAndCreator(collision))
            return;

        Transform hitParentTrans = collision.transform.parent;
        GameObject hitParentGameObject = hitParentTrans.gameObject;
        bool hasHealthController = hitParentGameObject.GetComponent<HealthController>() != null;

        // Debug.Log("freezing arrow trigger enter " + collision.name);
        if (hasHealthController)
        {
            // Debug.Log($"{hitGameObject.name}: arrow hit player or enemy");
            hitParentGameObject.GetComponent<HealthController>().DealDamage(damage);
        }

        Destroy(gameObject);
    }
}
