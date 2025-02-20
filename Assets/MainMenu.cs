using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // Tải Scene tiếp theo (Level_0 → Level_1)
    }

    //public void GoToSettingsMenu()
    //{
    //    SceneManager.LoadScene("SettingsMenu");
    //    // Chuyển đến menu cài đặt
    //}

    //public void GoToMainMenu()
    //{
    //    SceneManager.LoadScene("MainMenu");
    //    // Chuyển về màn hình chính
    //}

    public void QuitGame()
    {
        Application.Quit();
        // Thoát game (Chỉ hoạt động khi build game, không hoạt động trong Editor)
    }
}