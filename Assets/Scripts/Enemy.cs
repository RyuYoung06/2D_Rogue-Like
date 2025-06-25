using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public float attackRange = 3.0f; // 적의 공격 범위
    public Rigidbody2D target;
    public float hp = 10f; // 적의 체력
    public float attackPower = 2f; // 적의 공격력
    public GameObject damageTextPrefab; // 인스펙터에서 프리팹 할당
    public Transform damageTextSpawnPoint; // (선택) 텍스트가 뜰 위치
    public Room room; // Room 참조
    public bool isActive = false; // 움직임 활성화 플래그
    public GameObject coinPrefab; // Inspector에서 코인 프리팹 할당

    private bool isLive = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriter;
    private Animator anim;

    private bool isAttacking = false;
    private float attackCooldown = 5.0f; // 공격 쿨다운 (초)
    private float lastAttackTime = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // 자동으로 Player의 Rigidbody2D를 찾아서 할당
        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.GetComponent<Rigidbody2D>();
            }
        }
    }

    void OnEnable()
    {
        isActive = false; // 재활성화될 때 항상 비활성화 상태로 시작
    }

    private void FixedUpdate()
    {
        if (!isActive) return;

        if (!isLive || target == null)
            return;

        Vector2 dirVec = target.position - rb.position;
        float distance = dirVec.magnitude;

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

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

            if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)
            {
                anim.SetTrigger("Attack");
                isAttacking = true;
                lastAttackTime = Time.time;
            }
        }
        else
        {
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

    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
        // 공격 애니메이션이 끝날 때 플레이어에게 피해를 준다
        if (isLive && target != null)
        {
            Player player = target.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(attackPower);
                Debug.Log("공격성공");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isLive) return;
        hp -= damage;
        ShowDamageText(damage);
        Debug.Log($"적 데미지: {damage}, 남은 HP: {hp}");
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (coinPrefab != null)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
        if (room != null)
        {
            room.OnEnemyDied(this);
        }
        gameObject.SetActive(false);
    }

    void ShowDamageText(float damage)
    {
        if (damageTextPrefab != null)
        {
            Vector3 spawnPos = damageTextSpawnPoint != null ? damageTextSpawnPoint.position : transform.position;
            GameObject obj = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity, damageTextPrefab.transform.parent);
            DamageText dmgText = obj.GetComponent<DamageText>();
            if (dmgText != null)
                dmgText.SetText(damage);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        TakeDamage(collision.GetComponent<Bullet>().damage);
    }
}
