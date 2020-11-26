using System;
using System.Collections;
using UnityEngine;

// inspired by https://github.com/Brackeys/2D-Character-Controller
public class PlayerController : MonoBehaviour
{
	public bool isFlipped = false;
	public float speed = 400f;
	public float maxSpeed = 5f;
	public float jumpPower = 500f;
	public Transform groundCollider;
	public Animator animator;
	public Transform attackPoint;

	private bool isGrounded = true;
	private float hitboxSize = 0.70f;
	private Vector3 groundColliderOffset;
	private Rigidbody2D rb2D;

	private float xInput = 0f;
	private bool jumpInput = false;
	private bool canMove = true;
	private Vector2 defaultAttackPosition;

	void Awake()
	{
		rb2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		defaultAttackPosition = attackPoint.localPosition;
	}
	 
	void Update()
	{
		// syncing isFlipped with transform rotation
		if (isFlipped && transform.rotation.eulerAngles.y != 180) transform.rotation = Quaternion.Euler(0, 180, 0);
		if (!isFlipped && transform.rotation.eulerAngles.y == 180) transform.rotation = Quaternion.Euler(0, 0, 0);

		if (Input.GetButtonDown("Jump"))
		{
			jumpInput = true;
		}

		CheckIfOnGround();
		
		attackPoint.localPosition = calculateAttackPoint(); 
	}

	void FixedUpdate()
	{
		HandleXInput();
		HandleJumpInput();
		// Debug.Log($"Rigid {rb2D.velocity}, {rb2D.velocity.magnitude}, {Physics2D.gravity.y}");
	}

	public void Knockback(Vector2 knockbackDir, Vector2 knockbackPower, float knockbackTime)
	{
		StartCoroutine(KnockbackCoroutine(knockbackDir, knockbackPower, knockbackTime));
	}

	public IEnumerator KnockbackCoroutine(Vector2 knockbackDir, Vector2 knockbackPower, float knockbackTime)
	{
		Vector3 knockbackForce = knockbackDir * knockbackPower;

		canMove = false;
		rb2D.velocity = Vector2.zero;
		rb2D.AddForce(knockbackForce, ForceMode2D.Impulse);
		// Debug.Log("player knockback " + knockbackForce);
		yield return new WaitForSeconds(knockbackTime);
		canMove = true;
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
		if (canMove)
        {
			xInput = Input.GetAxis("Horizontal");
			float xInputAbs = Math.Abs(xInput);
			animator.SetFloat("Speed", xInputAbs);

			if (xInputAbs > 0)
			{
				float horizontalSpeed = xInput * Time.fixedDeltaTime * speed;
				Vector2 movement = new Vector2(horizontalSpeed, rb2D.velocity.y);
				movement.x = Mathf.Clamp(movement.x, -maxSpeed, maxSpeed);

				rb2D.velocity = movement;
				isFlipped = rb2D.velocity.x < 0;
				// Debug.Log($"Movement {movement}, velo {rb2D.velocity}, mag {rb2D.velocity.magnitude}");
			}
		}
		// Debug.Log($"Movement velo {rb2D.velocity}, mag {rb2D.velocity.magnitude}");
	}
	private void HandleJumpInput()
    {
		if (isGrounded && jumpInput)
		{
			isGrounded = false;
			Vector2 movement = new Vector2(0, jumpPower);
			rb2D.AddForce(movement, ForceMode2D.Impulse);
			// Debug.Log($"Jump {movement}");
		}
		jumpInput = false;
	}

	private void OnDrawGizmosSelected()
    {
		float boxSize = 0.05f;
		Vector3 boxSizeVec = new Vector3(boxSize, boxSize, boxSize);
		Gizmos.DrawWireCube(groundCollider.position - groundColliderOffset, boxSizeVec);
		Gizmos.DrawWireCube(groundCollider.position + groundColliderOffset, boxSizeVec);
    }
}
