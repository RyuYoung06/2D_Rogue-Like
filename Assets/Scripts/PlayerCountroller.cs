using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //총알
    public GameObject bulletPrefab;

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

    private Animator anim;


    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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

            // 애니메이션
            if (move.magnitude > 0)
            {
                GetComponent<Animator>().SetTrigger("Move");
            }
            else
            {
                GetComponent<Animator>().SetTrigger("Stop");
            }

            anim.SetBool("Move", move.magnitude > 0);

            if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0f && move != Vector2.zero)
            {
                isDashing = true;
                isInvincible = true;
                dashDirection = move;
                dashTime = dashDuration;
                dashCooldownTimer = dashCooldown;

                anim.SetBool("Dash", true);
            }
        }

        //총알 발사
        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    //발사
    void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("bulletPrefab이 Inspector에서 할당되지 않았습니다!");
            return;
        }

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = transform.position;

        Bullet bullet = newBullet.GetComponent<Bullet>();
        if (bullet == null)
        {
            Debug.LogError("bulletPrefab에는 Bullet 스크립트가 없습니다!");
            return;
        }

        bullet.Direction = new Vector2(1, 0);
    }



    private bool isDead = false;

    public bool IsDead()
    {
        return isDead;
    }

    public void Die()
    {
        isDead = true;
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
                anim.SetBool("Dash", false);
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
