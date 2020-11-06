using System;
using UnityEngine;

// inspired by https://github.com/Brackeys/2D-Character-Controller
public class PlayerController : MonoBehaviour
{
	public bool isFlipped = false;
	public float speed = 400f;
	public float jumpPower = 1500f;
	public string groundTag = "Ground";
	public Transform groundCollider;
	public Animator animator;

	private bool isGrounded = true;
	private float hitboxSize = 0.70f;
	private Vector3 groundColliderOffset;
	private Rigidbody2D rb2D;
	private SpriteRenderer spriteRenderer;

	private float xInput = 0f;
	private bool jumpInput = false;

	void Awake()
	{
		rb2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		spriteRenderer.flipX = isFlipped;
		groundColliderOffset = new Vector3(hitboxSize / 2f - 0.01f, 0, 0);

		if (Input.GetButtonDown("Jump"))
		{
			jumpInput = true;
		}

		isGrounded = false;
		Collider2D[] colliders = Physics2D.OverlapAreaAll(groundCollider.position - groundColliderOffset, groundCollider.position + groundColliderOffset);
		for (int i = 0; i < colliders.Length; i++)
		{
			GameObject col = colliders[i].gameObject;
			if (col != gameObject && col.CompareTag(groundTag))
			{
				isGrounded = true;
			}
		}
	}

	void FixedUpdate()
	{
		xInput = Input.GetAxis("Horizontal");
		float xInputAbs = Math.Abs(xInput);
		animator.SetFloat("Speed", xInputAbs);

		if (xInputAbs > 0)
		{
			float horizontalVelocity = xInput * Time.fixedDeltaTime * speed;
			Vector2 movement = new Vector2(horizontalVelocity, rb2D.velocity.y);
			isFlipped = movement.x < 0;
			rb2D.velocity = movement;
			// Debug.Log($"Movement {movement}");
		}

		if (isGrounded && jumpInput)
		{
			isGrounded = false;
			Vector2 movement = new Vector2(0, jumpPower);
			rb2D.AddForce(movement);
			// Debug.Log($"Jump {movement}");
		}
		jumpInput = false;

		Debug.Log($"Rigid {rb2D.velocity}, {rb2D.velocity.magnitude}, {Physics2D.gravity.y}");
	}


    private void OnDrawGizmosSelected()
    {
		float boxSize = 0.05f;
		Vector3 boxSizeVec = new Vector3(boxSize, boxSize, boxSize);
		Gizmos.DrawWireCube(groundCollider.position - groundColliderOffset, boxSizeVec);
		Gizmos.DrawWireCube(groundCollider.position + groundColliderOffset, boxSizeVec);
    }
}