using UnityEngine;

public class DoorController : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OpenDoor()
    {
        if (boxCollider != null) boxCollider.isTrigger = true;
        if (spriteRenderer != null) spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f); // ¾îµÓ°Ô
    }

    public void CloseDoor()
    {
        if (boxCollider != null) boxCollider.isTrigger = false;
        if (spriteRenderer != null) spriteRenderer.color = Color.white;
    }
}
