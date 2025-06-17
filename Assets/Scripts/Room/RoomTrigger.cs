using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public DoorController[] Doors;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 방 입장"); // 확인용
            foreach (var door in Doors)
            {
                door.CloseDoor(); // 문 닫기
            }
        }
    }
}
