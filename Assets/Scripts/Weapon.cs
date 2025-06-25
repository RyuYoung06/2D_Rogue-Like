using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public float speed;
    public Scanner scanner;

    public int maxShots = 4;         // 연속 발사 가능 횟수
    public float cooldownTime = 1.5f; // 쿨타임(초)

    private int shotCount = 0;
    private float cooldownTimer = 0f;
    private bool isCooldown = false;

    void Start()
    {
        Init();
    }
    
    void Update()
    {
        // 쿨타임 처리
        if (isCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownTime)
            {
                isCooldown = false;
                shotCount = 0;
                cooldownTimer = 0f;
                Debug.Log("쿨타임 종료");
            }
            return; // 쿨타임 중에는 발사 불가
        }

        // 발사 입력
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("발사 입력 감지");
            ShootAtNearestEnemy();
            shotCount++;

            if (shotCount >= maxShots)
            {
                isCooldown = true;
                cooldownTimer = 0f;
                Debug.Log("쿨타임 시작");
            }
        }
    }

    public void Init()
    {
        switch(id){
            case 0:
                speed = 10f;
                 break;
            default:
                break;
        }
    }

    void ShootAtNearestEnemy()
    {
        if (scanner == null || scanner.nearestTarget == null)
        {
            Debug.Log("타겟이 없어 총알 발사 실패");
            return;
        }

        Vector3 dir = (scanner.nearestTarget.position - transform.position).normalized;

        Transform bullet = GameDataManager.instance.poolManager.Get(prefabId).transform;
        bullet.position = transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.rotation = Quaternion.Euler(0, 0, angle);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = dir * speed;
        }

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Init(damage, -1);
        }

        Debug.Log("총알 발사!");
    }
}
