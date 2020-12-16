using System;
using System.Collections.Generic;
using UnityEngine;

// inspired by https://github.com/Brackeys/2D-Character-Controller
[RequireComponent(typeof(Rigidbody2D), typeof(KnockbackController))]
public class PlayerController : MonoBehaviour, IMeleeWeaponWielder, IRangedWeaponWielder, StatsObserver
{
    public bool isPaused = false;
    public bool isFlipped = false;
    public bool isArrowDirect = true;
    public float arrowSpeed = 10f;
    public float speed = 400f;
    public float maxSpeed = 5f;
    public float jumpPower = 500f;
    public float interactRange = 1.0f;
    public Transform groundCollider;
    public Animator animator;
    public LayerMask interactableLayer;

    public bool IsArrowDirect { get => isArrowDirect; set => isArrowDirect = value; }
    public float ArrowSpeed { get => arrowSpeed; set => arrowSpeed = value; }

    [HideInInspector]
    private bool isGrounded = true;
    private float hitboxSize = 0.70f;
    private float xInput = 0f;
    public UIAction closestUI;
    private Weapon activeWeapon;
    private List<Weapon> weapons;
    private Vector3 groundColliderOffset;
    private Rigidbody2D rb2D;
    private KnockbackController knockbackController;

    private readonly int weaponCoefficient = 5;
    private readonly int healtCoefficient = 20;
    private readonly int mapBottomLimit = -50;

    void Awake()
    {
        weapons = new List<Weapon>();
        foreach (Transform child in transform)
        {
            if (child.tag == Constants.WEAPON_TAG)
            {
                weapons.Add(child.GetComponent<Weapon>());
            }
        }

        foreach (Weapon weapon in weapons)
        {
            if (!activeWeapon && weapon.gameObject.activeSelf)
            {
                activeWeapon = weapon;
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
        }

        rb2D = gameObject.GetComponent<Rigidbody2D>();
        knockbackController = gameObject.GetComponent<KnockbackController>();
        animator = GetComponent<Animator>();
        StatsUpgrades.Instance.Subscribe(this);
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
            activeWeapon.Attack();
        }

        if (!isPaused && Input.GetButtonDown("SwitchWeapon"))
        {
            HandleWeaponSwitch();
        }

        CheckIfOnGround();
        // Debug.Log($"CheckIfOnGround: {isGrounded}");

        if (transform.position.y < mapBottomLimit)
        {
            GetComponent<HealthController>().DealDamage(null, 10000);
            rb2D.velocity = new Vector2(0, 0);
        }
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

            // triming y velocity before jump
            Vector2 vel = rb2D.velocity;
            vel.y = 0;
            rb2D.velocity = vel;
            rb2D.AddForce(movement, ForceMode2D.Impulse);
            // Debug.Log($"Jump {movement}");
        }
    }

    private void HandleObjectInteraction()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRange, interactableLayer);
        Debug.Log($"handle Interact {colliders.Length}");
        foreach (Collider2D col in colliders)
        {
            Debug.Log($"Interact in {col.name}");
            IInteractableObject interactable = col.transform.parent.GetComponent<IInteractableObject>();
            if (interactable == null)
                continue;

            interactable.Interact();

            break;
        }
    }

    private void HandleWeaponSwitch()
    {
        if (weapons.Count < 2) return;
        int oldIndex = weapons.FindIndex(activeWeapon.Equals);
        int newIndex = (oldIndex + 1) % weapons.Count;
        activeWeapon.gameObject.SetActive(false);
        activeWeapon = weapons[newIndex];
        activeWeapon.gameObject.SetActive(true);
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

    public void StatsUpdate(string name, int value)
    {
        Debug.Log("Controller update call: " + name + "; value: " + value);

        switch (name)
        {
            case "health":
                HealthUpgrade(value);
                break;
            case "sword":
            case "bow":
                WeaponUpgrade(name, value);
                break;
            default:
                break;
        }
    }

    private void HealthUpgrade(int value)
    {
        HealthController health = GetComponent<HealthController>();
        health.actualHealth += value * healtCoefficient;
        health.maxHealth += value * healtCoefficient;
    }

    private void WeaponUpgrade(string name, int value)
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.gameObject.name.ToLower() == name)
                weapon.damage += value * weaponCoefficient;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 boxSizeVec = Vector3.one * 0.05f;
        Gizmos.color = Color.blue;
        // Ground checking visual helpers
        Gizmos.DrawWireCube(groundCollider.position - groundColliderOffset, boxSizeVec);
        Gizmos.DrawWireCube(groundCollider.position + groundColliderOffset, boxSizeVec);

        // Interact range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }

    private void OnDestroy()
    {
        StatsUpgrades.Instance.Unsubscribe(this);
    }
}
