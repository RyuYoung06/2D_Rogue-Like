using UnityEngine;
using System.Collections;

public class RoomTrigger : MonoBehaviour
{
    public Room room;
    public float closeDelay = 0.5f; // 문 닫는 딜레이(초)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CloseDoorsWithDelay());
            room.OnPlayerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var door in room.doors)
            {
                door.SetTriggerMode(false); // 문을 벽처럼 만듦
            }
        }
    }

    IEnumerator CloseDoorsWithDelay()
    {
        yield return new WaitForSeconds(closeDelay);
        foreach (var door in room.doors)
        {
            door.SetTriggerMode(false); // 문을 벽처럼 만듦
        }
    }
}