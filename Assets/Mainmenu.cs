using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Scene1");
        //SceneManager.LoadScene("Scene2");  
        ////SceneManager.LoadScene("Yang2"); 
                                        
    }

    public void OpenOptions()
    {
        Debug.Log("Mở cài đặt - Hiển thị UI Options hoặc chuyển scene.");
   
    }

    public void QuitGame()
    {
        Debug.Log("Thoát game!");
        Application.Quit(); 
    }
}
