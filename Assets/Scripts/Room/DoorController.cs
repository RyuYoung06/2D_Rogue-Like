using UnityEngine;

public class Door : MonoBehaviour
{
    public Collider2D col;

    void Awake()
    {
        if (col == null)
            col = GetComponent<Collider2D>();
    }

    public void Open()
    {
        Debug.Log("Door 열림");
        if (col != null)
        {
            col.enabled = false; // 문 열릴 때 Collider 비활성화
        }
        // 문 열리는 애니메이션/사운드 등 추가 가능
    }

    public void Close()
    {
        col.enabled = true; // 문 닫힐 때 Collider 활성화
    }

    public void SetTriggerMode(bool isTrigger)
    {
        col.isTrigger = isTrigger;
    }
}