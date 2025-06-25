using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1; // 코인 가치

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameDataManager.instance.AddCoin(value);
            Destroy(gameObject);
        }
    }
}