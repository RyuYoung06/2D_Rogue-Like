using UnityEngine;
using System.Collections;

public class RoomTrigger : MonoBehaviour
{
    public DoorController[] doors;
    public float doorCloseDelay = 1f; // 닫히기까지 딜레이 시간 (초)

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("플레이어가 방에 들어옴");
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
