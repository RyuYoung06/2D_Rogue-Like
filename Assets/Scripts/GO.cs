using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneManager.LoadScene("Level_1");
    }
}