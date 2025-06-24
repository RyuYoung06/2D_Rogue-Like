using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public float attackRange = 3.0f; // 공격 거리
    public Rigidbody2D target;

    private bool isLive = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriter;
    private Animator anim;

    private bool isAttacking = false;
    private float attackCooldown = 5.0f; // 공격 쿨다운 시간 (초)
    private float lastAttackTime = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isLive || target == null)
            return;

        Vector2 dirVec = target.position - rb.position;
        float distance = dirVec.magnitude;

        // 현재 애니메이터 상태 확인 (0은 기본 레이어)
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        // 공격 애니메이션 중이면 이동 및 추가 공격 트리거 방지
        if (state.IsName("Attack"))
        {
            isAttacking = true;
            anim.SetBool("isWalking", false);
            return;
        }
        else
        {
            isAttacking = false;
        }

        if (distance <= attackRange)
        {
            anim.SetBool("isWalking", false);

            // 쿨다운 체크 후 공격 트리거
            if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)
            {
                anim.SetTrigger("Attack");
                isAttacking = true;
                lastAttackTime = Time.time;
            }
        }
        else
        {
            // 공격범위 밖이면 이동
            isAttacking = false;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + nextVec);

            anim.SetBool("isWalking", true);
        }
    }

    private void LateUpdate()
    {
        if (!isLive || target == null)
            return;

        spriter.flipX = target.position.x < rb.position.x;
    }

    // 공격 애니메이션 종료 시 Animation Event로 호출
    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
    }
}
