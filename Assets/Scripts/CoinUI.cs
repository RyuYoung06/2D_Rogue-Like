using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    void Start()
    {
        UpdateCoinUI();
    }

    void Update()
    {
        // 코인 값이 바뀔 수 있으니 실시간으로 갱신
        UpdateCoinUI();
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = "코인: " + GameDataManager.instance.coin.ToString();
    }
}