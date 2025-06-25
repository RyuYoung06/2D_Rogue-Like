using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class PlayerData
{
    public List<string> colectedItems = new List<string>();
    public int stage = 1;
}

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;
    public PlayerData playerData;
    public PoolManager poolManager;
    public int coin = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCoin();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveData(PlayerData playerData)
    {
        string filePath = Application.persistentDataPath + "/player_data.json";
        string json = JsonUtility.ToJson(playerData, true);
        System.IO.File.WriteAllText(filePath, json);
        Debug.Log("���� ������ �����: " + json);

    }
    public PlayerData LoadData()
    {
        string filePath = Application.persistentDataPath + "/player_data.json";
        if(System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("���� ������ �ε��: " + json);
            return playerData;
        }
        else
        {
            Debug.LogWarning("����� ���� �����Ͱ� �����ϴ�.");
            return new PlayerData();
        }
    }
    public void GameStart()
    {
        PlayerData playerData = LoadData();
        if(playerData == null)
        {
            playerData = new PlayerData();
            SceneManager.LoadScene("Level_1");

        }
        else
        {
            SceneManager.LoadScene("Level_1" + playerData.stage);
        }
    }
    public void PlayerDead()
    {
        PlayerData playerData = LoadData();
        if(playerData != null)
        {
            playerData.stage = 1;

            foreach (string item in playerData.colectedItems.ToList())
            {
                if(UnityEngine.Random.Range(0, 1) == 0)
                {
                    playerData.colectedItems.Remove(item);
                }
            }

            SaveData(playerData);
        }
        SceneManager.LoadScene("GameOver");
    }

    void SomeFunction()
    {
        // PoolManager의 싱글턴 인스턴스 사용
        //PoolManager.Instance.ReturnBullet(someBullet);
    }

    public void AddCoin(int value)
    {
        coin += value;
        SaveCoin();
    }

    public void SaveCoin()
    {
        PlayerPrefs.SetInt("Coin", coin);
        PlayerPrefs.Save();
    }

    public void LoadCoin()
    {
        coin = PlayerPrefs.GetInt("Coin", 0);
    }
}
