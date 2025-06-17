using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float dashSpeed = 10f; // ������ ��� �ӵ�
    public float dashDuration = 0.2f; // �� ����
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

    //����Ʈ ���� 
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
        //���
        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0f && move != Vector2.zero)
        {
            isDashing = true;
            isInvincible = true;
            dashDirection = move;
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;

            anim.SetBool("Dash", true);

            CreateDashEffect(); // ����Ʈ ����
        }


        // ��ٿ� ����
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        // �̵� �Է�
        move = Vector2.zero;

        if (Input.GetKey(KeyCode.A)) move += Vector2.left;
        if (Input.GetKey(KeyCode.D)) move += Vector2.right;
        if (Input.GetKey(KeyCode.W)) move += Vector2.up;
        if (Input.GetKey(KeyCode.S)) move += Vector2.down;

        move = move.normalized;

        // ���⿡ ���� ������
        if (move.x < 0) sprite.flipX = true;
        else if (move.x > 0) sprite.flipX = false;



        // �ִϸ��̼�
        anim.SetBool("Move", move.magnitude > 0);

        // ��� �Է�
        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0f && move != Vector2.zero)
        {
            isDashing = true;
            isInvincible = true;
            dashDirection = move;
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;

            anim.SetBool("Dash", true);
        }

        // �Ѿ� �߻�
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

        // SpriteRenderer ���� ����
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
        DashEffect dashEffect = effect.GetComponent<DashEffect>();

    }


    void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("bulletPrefab�� Inspector���� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = transform.position;

        Bullet bullet = newBullet.GetComponent<Bullet>();
        if (bullet == null)
        {
            Debug.LogError("bulletPrefab���� Bullet ��ũ��Ʈ�� �����ϴ�!");
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

            // ������ ó�� ����
            // GameDataManager.instance.playerData.colectedItems.Add(item.GetItem());
            // GameDataManager.instance.SaveData(GameDataManager.instance.playerData);

            Destroy(collision.gameObject);
        }
    }
}
