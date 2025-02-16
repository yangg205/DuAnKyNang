using UnityEngine;

public class TrashItem : MonoBehaviour
{
    public GarbageTruck garbageTruck; // Tham chiếu tới xe chở rác (Gán trong Inspector)
    private bool isDragging = false;

    private void OnMouseDown()
    {
        Debug.Log("Bắt đầu kéo rác!");
        isDragging = true;
    }

    private void OnMouseUp()
    {
        Debug.Log("Dừng kéo rác!");
        isDragging = false;

        // Kiểm tra nếu rác gần xe chở rác
        if (garbageTruck != null && garbageTruck.IsNearTruck(transform.position))
        {
            Debug.Log("Rác được đưa vào xe!");
            garbageTruck.CollectTrash(gameObject);
        }
    }

    private void Update()
    {
        if (isDragging)
        {
            // Lấy vị trí chuột trên màn hình
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.z); // Khoảng cách từ camera đến rác

            // Cập nhật vị trí của rác theo chuột
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("Chuột đang chạm vào rác!");
    }

    private void OnMouseExit()
    {
        Debug.Log("Chuột rời khỏi rác!");
    }
}
