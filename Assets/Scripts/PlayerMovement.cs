using System;
using UnityEngine;

// inspired by https://github.com/Brackeys/2D-Character-Controller
public class PlayerMovement : MonoBehaviour
{
	public float speed;
	public float maxSpeed;
	public float jumpPower;
	public string groundTag = "Ground";
	public Transform groundCollider;

	private float groundRadius = 0.05f;
	private Rigidbody2D rb2D;
	private bool isGrounded = true;
	private Vector2 movement = Vector2.zero;

	private float xInput = 0f;
	private bool jumpInput = false;

	void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if (Input.GetButtonDown("Jump"))
		{
			jumpInput = true;
		}

		isGrounded = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCollider.position, groundRadius);
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
		if (rb2D.velocity.magnitude < maxSpeed && Math.Abs(xInput) > 0)
		{
			float horizontalVelocity = xInput * Time.fixedDeltaTime * speed;
			movement = new Vector2(horizontalVelocity, rb2D.velocity.y);
			rb2D.velocity = movement;
			// Debug.Log($"Movement {movement}");
		}


		if (isGrounded && jumpInput)
		{
			isGrounded = false;
			movement = new Vector2(0, jumpPower);
			rb2D.AddForce(movement);
			// Debug.Log($"Jump {movement}");
		}
		jumpInput = false;

		// Debug.Log($"Rigid {rb2D.velocity}, {Physics2D.gravity.y}");
	}
}