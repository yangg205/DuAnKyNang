using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AutoTimerSlider : MonoBehaviour
{
    public Slider timeSlider; // Thanh thời gian
    public float totalTime = 30f; // Thời gian tổng (30 giây)
    private float currentTime; // Thời gian còn lại
    private bool isGameStopped = false; // Trạng thái dừng game
    public GameObject timeOutPanel; // Panel hiện khi hết thời gian

    private void Start()
    {
        currentTime = totalTime;
        if (timeSlider != null)
        {
            timeSlider.maxValue = totalTime;
            timeSlider.value = totalTime;
        }

        if (timeOutPanel != null)
        {
            timeOutPanel.SetActive(false); // Ẩn panel ban đầu
        }
    }

    private void Update()
    {
        if (!isGameStopped && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateSlider();
        }
        else if (!isGameStopped && currentTime <= 0)
        {
            currentTime = 0;
            EndGame();
        }
    }

    private void UpdateSlider()
    {
        if (timeSlider != null)
        {
            timeSlider.value = currentTime;
        }
    }

    private void EndGame()
    {
        Debug.Log("Thời gian đã hết!");
        isGameStopped = true;
        if (timeOutPanel != null)
        {
            timeOutPanel.SetActive(true); // Hiển thị panel
        }
    }

    public void GoToSortingScene()
    {
        SceneManager.LoadScene("TrashSortingScene"); // Chuyển sang scene phân loại rác
    }
}
