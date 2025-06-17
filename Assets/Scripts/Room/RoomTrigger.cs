using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public DoorController[] Doors;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� �� ����"); // Ȯ�ο�
            foreach (var door in Doors)
            {
                door.CloseDoor(); // �� �ݱ�
            }
        }
    }
}
