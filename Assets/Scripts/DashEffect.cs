using UnityEngine;

public class DashEffect : MonoBehaviour
{
    public float duration = 0.3f; // 이펙트가 사라지는 시간
    public float fadeSpeed = 1f;

    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void Update()
    {
        Color color = sr.color;
        color.a -= Time.deltaTime * fadeSpeed;
        sr.color = color;

        
        if (sr.color.a <= 0f)
        {
            Destroy(gameObject);
        }
        
    }

    public void SetSprite(Sprite sprite, bool flipX)
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.flipX = flipX;
        sr.color = originalColor;
    }
}
