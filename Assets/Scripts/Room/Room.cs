using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Door[] doors; // Inspector에서 Door 1, Door 2 ... 순서대로 할당
    public List<Enemy> enemies = new List<Enemy>(); // 방에 있는 몬스터들
    private bool isCleared = false;

    void Start()
    {
        enemies.Clear();
        enemies.AddRange(GetComponentsInChildren<Enemy>());
        foreach (var enemy in enemies)
        {
            if (enemy != null)
                enemy.room = this;
        }
    }

    // 플레이어가 방에 들어왔을 때 호출
    public void OnPlayerEnter()
    {
        Debug.Log("플레이어가 방에 들어옴");
        if (!isCleared)
        {
            CloseDoors();
            ActivateEnemies();
        }
    }

    void ActivateEnemies()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
                enemy.isActive = true;
        }
    }

    // Enemy가 죽을 때마다 호출
    public void OnEnemyDied(Enemy enemy)
    {
        enemies.Remove(enemy);
        Debug.Log("OnEnemyDied 호출됨, 남은 적 수: " + enemies.Count);
        if (enemies.Count == 0 && !isCleared)
        {
            Debug.Log("OpenDoor1 호출");
            OpenDoor1();
            isCleared = true;
        }
    }

    void CloseDoors()
    {
        Debug.Log("문 닫기 시도");
        foreach (var door in doors)
        {
            door.Close();
        }
    }

    void OpenDoor1()
    {
        Debug.Log("Door 1 열림 시도");
        if (doors != null && doors.Length > 1 && doors[1] != null)
        {
            doors[1].Open();
        }
    }
}