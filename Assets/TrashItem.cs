using UnityEngine;

public class TrashItem : MonoBehaviour
{
    public string trashType; // Loại rác (ví dụ: "Tái Chế", "Điện Tử", "Tổng Hợp")
    private bool isDragging = false; // Kiểm tra xem có đang kéo thả không

    private void OnMouseDown()
    {
        // Bắt đầu kéo thả rác
        isDragging = true;
    }

    private void OnMouseUp()
    {
        // Kết thúc kéo thả rác
        isDragging = false;
    }

    private void Update()
    {
        if (isDragging)
        {
            // Di chuyển rác theo chuột (nếu đang kéo thả)
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }
}
