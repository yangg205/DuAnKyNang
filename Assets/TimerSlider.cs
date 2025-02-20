using UnityEngine;
using UnityEngine.UI;

public class AutoTimerSlider : MonoBehaviour
{
    public Slider timeSlider; // Slider thời gian
    public float totalTime = 30f; // Tổng thời gian (30 giây)
    private float currentTime; // Thời gian còn lại

    private void Start()
    {
        // Khởi tạo thời gian và slider
        currentTime = totalTime;
        if (timeSlider != null)
        {
            timeSlider.maxValue = totalTime; // Đặt giá trị tối đa
            timeSlider.value = totalTime;   // Đặt giá trị ban đầu
        }
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // Giảm thời gian
            UpdateSlider();                // Cập nhật slider
        }
        else
        {
            currentTime = 0; // Đảm bảo thời gian không âm
            HandleTimeUp();  // Gọi hành động khi hết giờ
        }
    }

    private void UpdateSlider()
    {
        if (timeSlider != null)
        {
            timeSlider.value = currentTime; // Cập nhật giá trị slider
        }
    }

    private void HandleTimeUp()
    {
        Debug.Log("Thời gian đã hết!"); // Xử lý khi thời gian hết
        // Thêm logic cần thiết khi hết giờ:
        // Ví dụ: SceneManager.LoadScene("NextScene");
    }
}
