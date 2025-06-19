using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float dashSpeed = 10f; // 조정된 대시 속도
    public float dashDuration = 0.2f; // 초 단위
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    private Vector2 move;
    private Vector2 dashDirection;

    private bool isDashing = false;
    private float dashTime;
    private float dashCooldownTimer;

    private bool isInvincible = false;

    public GameObject bulletPrefab;

    //이펙트 생성 
    public GameObject dashEffectPrefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //대시
        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0f && move != Vector2.zero)
        {
            isDashing = true;
            isInvincible = true;
            dashDirection = move;
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;

            anim.SetBool("Dash", true);

            CreateDashEffect(); // 이펙트 생성
        }


        // 쿨다운 감소
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        // 이동 입력
        move = Vector2.zero;
        move.x = Input.GetAxisRaw("Horizontal"); // A, D 또는 ←, →
        move.y = Input.GetAxisRaw("Vertical");   // W, S 또는 ↑, ↓
        move = move.normalized; // 대각선 이동 시 속도 보정
        move = move.normalized;

        // 방향에 따라 뒤집기
        if (move.x < 0) sprite.flipX = true;
        else if (move.x > 0) sprite.flipX = false;



        // 애니메이션
        move.x = Input.GetAxisRaw("Horizontal"); // -1 (왼쪽), 0, 1 (오른쪽)
        move.y = Input.GetAxisRaw("Vertical");   // -1 (아래), 0, 1 (위)

        move = move.normalized; // 대각선 이동 시 속도 일정하게 유지

        // 애니메이터에 방향 넘겨주기
        anim.SetFloat("MoveX", move.x);
        anim.SetFloat("MoveY", move.y);

        // 캐릭터가 움직이고 있을 때만 isMoving을 true로
        bool isMoving = move != Vector2.zero;
        anim.SetBool("isMoving", isMoving);

        // 대시 입력
        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0f && move != Vector2.zero)
        {
            isDashing = true;
            isInvincible = true;
            dashDirection = move;
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;

            anim.SetBool("Dash", true);
        }

        // 총알 발사
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            dashTime -= Time.fixedDeltaTime;
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);

            if (dashTime <= 0f)
            {
                isDashing = false;
                isInvincible = false;
                anim.SetBool("Dash", false);
            }
        }
        else
        {
            rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
        }
    }


    void CreateDashEffect()
    {
        Debug.Log("Dash");
        GameObject effect = Instantiate(dashEffectPrefab, transform.position, Quaternion.identity);

        // SpriteRenderer 상태 복사
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
        DashEffect dashEffect = effect.GetComponent<DashEffect>();

    }


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

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPos - transform.position).normalized;
        bullet.Direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            ItemObject item = collision.GetComponent<ItemObject>();

            // 아이템 처리 예시
            // GameDataManager.instance.playerData.colectedItems.Add(item.GetItem());
            // GameDataManager.instance.SaveData(GameDataManager.instance.playerData);

            Destroy(collision.gameObject);
        }
    }
}
