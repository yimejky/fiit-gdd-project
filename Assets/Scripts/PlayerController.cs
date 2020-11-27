using System;
using UnityEngine;

// inspired by https://github.com/Brackeys/2D-Character-Controller
[RequireComponent(typeof(Rigidbody2D), typeof(KnockbackController))]
public class PlayerController : MonoBehaviour
{
	public bool isPaused;
	public bool isFlipped = false;
	public float speed = 400f;
	public float maxSpeed = 5f;
	public float jumpPower = 500f;
	public Transform groundCollider;
	public Transform attackPoint;
	public Animator animator;

	private bool isGrounded = true;
	private float hitboxSize = 0.70f;
	private float xInput = 0f;
	private Weapon weapon;
	private Vector3 defaultAttackPosition;
	private Vector3 groundColliderOffset;
	private Rigidbody2D rb2D;
	private KnockbackController knockbackController;

    void Awake()
	{
		weapon = transform.Find("Sword").GetComponent<Weapon>();
		rb2D = gameObject.GetComponent<Rigidbody2D>();
		knockbackController = gameObject.GetComponent<KnockbackController>();
		animator = GetComponent<Animator>();
		defaultAttackPosition = attackPoint.localPosition;
	}
	 
	void Update()
	{
		isPaused = Time.timeScale <= 0;

		// syncing isFlipped with transform rotation
		if (isFlipped && transform.rotation.eulerAngles.y != 180) transform.rotation = Quaternion.Euler(0, 180, 0);
		if (!isFlipped && transform.rotation.eulerAngles.y == 180) transform.rotation = Quaternion.Euler(0, 0, 0);

		if (!isPaused && Input.GetButtonDown("Jump"))
		{
			HandleJumpInput();
		}

		if (!isPaused && Input.GetButtonDown("Attack"))
		{
			weapon.Attack();
		}

		CheckIfOnGround();
		
		attackPoint.localPosition = calculateAttackPoint();
		HandleArrowInput();
	}

	void FixedUpdate()
	{
		HandleXInput();
		// Debug.Log($"Rigid {rb2D.velocity}, {rb2D.velocity.magnitude}, {Physics2D.gravity.y}");
	}

	public Vector2 GetSwordDirection(float threshold = 0.1f)
    {
		int x = Math.Abs(rb2D.velocity.x) < threshold && Math.Abs(rb2D.velocity.y) > threshold ? 0 : 1;
		int y = rb2D.velocity.y > threshold ? 1 : rb2D.velocity.y < -threshold ? -1 : 0;

		return new Vector2(x, y);
	}

	private Vector2 calculateAttackPoint()
    {
		Vector2 swordDirection = GetSwordDirection();
		return new Vector2(defaultAttackPosition.x * swordDirection.x, defaultAttackPosition.y + 0.8f * swordDirection.y);
	}

	private void CheckIfOnGround()
	{
		groundColliderOffset = new Vector3(hitboxSize / 2f - 0.01f, 0, 0);
		isGrounded = false;
		Collider2D[] colliders = Physics2D.OverlapAreaAll(groundCollider.position - groundColliderOffset, groundCollider.position + groundColliderOffset);
		for (int i = 0; i < colliders.Length; i++)
		{
			GameObject col = colliders[i].gameObject;
			if (col != gameObject && col.CompareTag(Constants.GROUND_TAG))
			{
				isGrounded = true;
			}
		}

		if (animator.GetBool("IsOnGround") != isGrounded) animator.SetBool("IsOnGround", isGrounded); ;
	}

	private void HandleXInput()
    {
		xInput = Input.GetAxis("Horizontal");
		float xInputAbs = Math.Abs(xInput);
		animator.SetFloat("Speed", xInputAbs);
		if (!isPaused && knockbackController.canMove && xInputAbs > 0)
        {
			float horizontalSpeed = xInput * Time.fixedDeltaTime * speed;
			Vector2 movement = new Vector3(horizontalSpeed, rb2D.velocity.y, 0);
			movement.x = Mathf.Clamp(movement.x, -maxSpeed, maxSpeed);

			rb2D.velocity = movement;
			isFlipped = rb2D.velocity.x < 0;
			// Debug.Log($"Movement {movement}, velo {rb2D.velocity}, mag {rb2D.velocity.magnitude}");
		}
		// Debug.Log($"Movement velo {rb2D.velocity}, mag {rb2D.velocity.magnitude}");
	}
	private void HandleJumpInput()
    {
		if (!isPaused && isGrounded)
		{
			isGrounded = false;
			Vector2 movement = new Vector2(0, jumpPower);
			rb2D.AddForce(movement, ForceMode2D.Impulse);
			// Debug.Log($"Jump {movement}");
		}
	}

	private void HandleArrowInput()
	{
		if (!isPaused && Input.GetButtonDown("ArrowTest"))
		{
			float projectileSpeed = 25f;

			GameObject projectileGameobject = Instantiate(Resources.Load("Prefabs/Weapons/Projectile"), transform.position, Quaternion.identity) as GameObject;
			Projectile projectile = projectileGameobject.GetComponent<Projectile>();
			Vector3 dir = -(transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized * projectileSpeed;
			projectile.Init(gameObject, new Vector3(dir.x, dir.y, 0));

			float arrowSpeed = 10f;
			bool isArrowDirect = true;

			GameObject arrowGameobject = Instantiate(Resources.Load("Prefabs/Weapons/Arrow"), transform.position, Quaternion.identity) as GameObject;
			Arrow arrow = arrowGameobject.GetComponent<Arrow>();
			Vector3 force = arrow.CalculateArrowForceVector(Camera.main.ScreenToWorldPoint(Input.mousePosition), arrowSpeed, isArrowDirect);
			arrow.Init(gameObject, force);
		}
	}

	private void OnDrawGizmosSelected()
    {
		float boxSize = 0.05f;
		Vector3 boxSizeVec = new Vector3(boxSize, boxSize, boxSize);
		Gizmos.DrawWireCube(groundCollider.position - groundColliderOffset, boxSizeVec);
		Gizmos.DrawWireCube(groundCollider.position + groundColliderOffset, boxSizeVec);
    }
}
