using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    public List<GameObject> prefabList; // 프리팹 리스트
    public List<Queue<GameObject>> poolList; // 각 프리팹별 풀

    void Awake()
    {
        // 싱글턴 패턴: 인스턴스가 없으면 자신을 할당
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지 (선택)

            // 각 프리팹별로 풀 생성
            poolList = new List<Queue<GameObject>>();
            foreach (var prefab in prefabList)
            {
                poolList.Add(new Queue<GameObject>());
            }
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }

    public GameObject Get(int index)
    {
        if (index < 0 || index >= prefabList.Count)
        {
            Debug.LogError($"PoolManager: index {index}가 prefabList 범위를 벗어났습니다! prefabList.Count={prefabList.Count}");
            return null;
        }
        GameObject obj = null;
        if (poolList[index].Count > 0)
        {
            obj = poolList[index].Dequeue();
            // 이미 Destroy된 오브젝트라면 새로 생성
            if (obj == null)
            {
                obj = Instantiate(prefabList[index]);
            }
        }
        else
        {
            obj = Instantiate(prefabList[index]);
        }
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(int index, GameObject obj)
    {
        obj.SetActive(false);
        poolList[index].Enqueue(obj);
    }

    // 예시: Bullet 꺼내는 함수
    public GameObject GetBullet(Vector3 position, Quaternion rotation)
    {
        // 풀에서 꺼내서 position, rotation 세팅 후 반환
        // (여기서는 Instantiate로 예시)
        // 실제 풀링 구현은 별도 필요
        return Instantiate(Resources.Load<GameObject>("Bullet"), position, rotation);
    }
}
