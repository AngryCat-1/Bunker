using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void TelergramLink()
    {
         Application.OpenURL("https://t.me/infinityrequiemstudio");
    }
    public void ToGame()
    {
        SceneManager.LoadScene("Game_Scene");
    }
}
