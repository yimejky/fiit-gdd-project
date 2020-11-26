using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Arrow : MonoBehaviour
{
    public int damage = 10;

    private Rigidbody2D rb2D;
    private readonly float angleOffset = 90;
    private bool isFrozen = false;
    private float aliveTime = 0f;
    private readonly float despawnTime = 10f;
    private GameObject creator;

    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void Init(GameObject newCreator, Vector2 force)
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
            bool isHurtbox = hitGameObject.CompareTag(Constants.HURTBOX_TAG);
            bool isCreatorAndHitEnemy = creator.CompareTag(Constants.ENEMY_TAG) && hitGameObject.CompareTag(Constants.ENEMY_TAG);
            Transform hitParentTrans = collision.transform.parent;
            GameObject hitParentGameObject = hitParentTrans.gameObject;

            // skip hurtbox/parent or creator is hitobject
            if ((isHurtbox && creator == hitParentGameObject) || creator == hitGameObject || isCreatorAndHitEnemy)
            {
                return;
            }

            Debug.Log("freezing arrow trigger enter " + collision.name);
            isFrozen = true;
            rb2D.isKinematic = true;
            rb2D.velocity = Vector3.zero;

            if (isHurtbox && hitParentGameObject.CompareTag(Constants.PLAYER_TAG))
            {
                Debug.Log($"{hitGameObject.name}: arrow hit player");
                HealthController health = hitParentGameObject.GetComponent<HealthController>();
                health.DealDamage(damage);
                transform.parent = hitParentTrans;
            }
            else if (hitGameObject.CompareTag(Constants.ENEMY_TAG))
            {
                Debug.Log($"{hitGameObject.name}: arrow hit enemy");
                HealthController health = hitGameObject.GetComponent<HealthController>();
                health.DealDamage(damage);
                transform.parent = hitGameObject.transform;
            }
            else
            {
                Debug.Log($"{gameObject.name}: arrow hit ground");
            }
        }
    }
}
