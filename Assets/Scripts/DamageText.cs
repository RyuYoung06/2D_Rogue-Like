using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float destroyTime = 1f;
    private TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(float damage)
    {
        text.text = damage.ToString();
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0f)
            Destroy(gameObject);
    }
}