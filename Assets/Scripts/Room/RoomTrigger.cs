using UnityEngine;
using System.Collections;

public class RoomTrigger : MonoBehaviour
{
    public DoorController[] doors;
    public float doorCloseDelay = 1f; // ��������� ������ �ð� (��)

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("�÷��̾ �濡 ����");
            StartCoroutine(CloseDoorsAfterDelay());
        }
    }

    private IEnumerator CloseDoorsAfterDelay()
    {
        yield return new WaitForSeconds(doorCloseDelay);

        foreach (DoorController door in doors)
        {
            door.CloseDoor();
        }
    }
}
