﻿using System;
using UnityEngine;

// inspired by https://github.com/Brackeys/2D-Character-Controller
[RequireComponent(typeof(Rigidbody2D), typeof(KnockbackController))]
public class PlayerController : MonoBehaviour, IMeleeWeaponWielder, IRangedWeaponWielder
{
	public bool isPaused;
	public bool isFlipped = false;
	public float speed = 400f;
	public float maxSpeed = 5f;
	public float jumpPower = 500f;
	public Transform groundCollider;
	public Animator animator;

	private bool isGrounded = true;
	private float hitboxSize = 0.70f;
	private float xInput = 0f;
	private Weapon weapon;
	private Vector3 groundColliderOffset;
	private Rigidbody2D rb2D;
	private KnockbackController knockbackController;

    void Awake()
	{
		foreach (Transform child in transform)
		{
			if (child.tag == "Weapon" && child.gameObject.activeSelf)
            {
				weapon = child.GetComponent<Weapon>();
				break;
			}
		}
		
		rb2D = gameObject.GetComponent<Rigidbody2D>();
		knockbackController = gameObject.GetComponent<KnockbackController>();
		animator = GetComponent<Animator>();
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
	}

	void FixedUpdate()
	{
		HandleXInput();
		// Debug.Log($"Rigid {rb2D.velocity}, {rb2D.velocity.magnitude}, {Physics2D.gravity.y}");
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

	private void OnDrawGizmosSelected()
    {
		float boxSize = 0.05f;
		Vector3 boxSizeVec = new Vector3(boxSize, boxSize, boxSize);
		Gizmos.DrawWireCube(groundCollider.position - groundColliderOffset, boxSizeVec);
		Gizmos.DrawWireCube(groundCollider.position + groundColliderOffset, boxSizeVec);
    }

    public Vector2 GetMeeleAttackDirection()
    {
		float threshold = 0.1f;
		float xInput = Input.GetAxis("Horizontal");
		float yInput = Input.GetAxis("Vertical");

		return new Vector2(Math.Abs(xInput) >= threshold || Math.Abs(yInput) < threshold ? 1 : 0, Math.Abs(yInput) < threshold ? 0 : yInput > 0 ? 1 : -1);
	}

    public Vector2 GetRangedAttackDirection()
    {
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
