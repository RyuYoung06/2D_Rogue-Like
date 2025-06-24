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

    private Vector2 move; // 현재 프레임 이동 방향
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

            // 애니메이션 방향 및 이동 여부 설정
            if (move != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
                anim.SetFloat("MoveX", move.x);
                anim.SetFloat("MoveY", move.y);
                anim.SetFloat("LastMoveX", move.x);
                anim.SetFloat("LastMoveY", move.y);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }

        // 대시 입력
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            Vector2 nextVec = move * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + nextVec);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        rb.velocity = move * dashSpeed;

        anim.SetFloat("MoveX", move.x);
        anim.SetFloat("MoveY", move.y);
        anim.SetBool("isDashing", true);

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        isDashing = false;
        anim.SetBool("isDashing", false);
    }
}
