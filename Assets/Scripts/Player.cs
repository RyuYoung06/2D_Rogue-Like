using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    private Vector2 move; // �÷��̾� �̵� ����
    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDashing)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            move = new Vector2(moveX, moveY).normalized;

        }

        // Ű���� �Է� �ޱ�
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        // �ִϸ��̼� ���� ����
        if (move.x != 0 || move.y != 0)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);

        // �����̰� �ִ��� ����
        bool isMoving = input != Vector2.zero;
        anim.SetBool("isMoving", isMoving);

        // ������ �ٶ� ���� ����
        if (isMoving)
        {
            anim.SetFloat("LastMoveX", input.x);
            anim.SetFloat("LastMoveY", input.y);
        }


        // ��� �Է�
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        rb.velocity = move * dashSpeed;
        anim.SetTrigger("Dash");

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        isDashing = false;
    }
}
