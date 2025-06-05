using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float dashSpeed = 500f;
    public float dashDuration = 20f;
    public float dashCooldown = 1f;

    private Vector2 move;
    private Vector2 dashDirection;

    private bool isDashing = false;
    private float dashTime;
    private float dashCooldownTimer;

    private bool isInvincible = false;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 쿨다운 타이머 감소
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        if (!isDashing)
        {
            move = Vector2.zero;

            if (Input.GetKey(KeyCode.A)) move += Vector2.left;
            if (Input.GetKey(KeyCode.D)) move += Vector2.right;
            if (Input.GetKey(KeyCode.W)) move += Vector2.up;
            if (Input.GetKey(KeyCode.S)) move += Vector2.down;

            move = move.normalized;

            if (move.x < 0) sprite.flipX = true;
            else if (move.x > 0) sprite.flipX = false;

            // 대시 시작 조건
            if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0f && move != Vector2.zero)
            {
                Debug.Log("대시 시작!");
                isDashing = true;
                isInvincible = true;
                dashDirection = move;
                dashTime = dashDuration;
                dashCooldownTimer = dashCooldown;
            }
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = dashDirection * dashSpeed;
            dashTime -= Time.fixedDeltaTime;

            if (dashTime <= 0f)
            {
                isDashing = false;
                isInvincible = false;
            }
        }
        else
        {
            rb.velocity = move * speed;
        }
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}
