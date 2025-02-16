using UnityEngine;

public class GarbageTruck : MonoBehaviour
{
    public Transform trashStoragePoint; // Vị trí lưu trữ rác trên xe
    public Transform destinationPoint; // Điểm đến khi xe chạy
    public float moveSpeed = 5f; // Tốc độ xe chạy
    public int trashCapacity = 10; // Số lượng rác tối đa

    private int currentTrashCount = 0;
    private bool isMoving = false;

    public void CollectTrash(GameObject trash)
    {
        if (currentTrashCount < trashCapacity)
        {
            trash.transform.position = trashStoragePoint.position + new Vector3(0, 0.5f * currentTrashCount, 0);
            trash.transform.parent = trashStoragePoint;
            currentTrashCount++;
            Destroy(trash.GetComponent<TrashItem>()); // Xóa script kéo thả
        }

        if (currentTrashCount >= trashCapacity)
        {
            StartMoving();
        }
    }

    public bool IsNearTruck(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) < 2f; // Kiểm tra nếu rác gần xe
    }

    private void StartMoving()
    {
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinationPoint.position, moveSpeed * Time.deltaTime);

            // Kiểm tra nếu xe đã đến điểm đích
            if (Vector3.Distance(transform.position, destinationPoint.position) < 0.1f)
            {
                Debug.Log("Xe đã đến điểm đích. Đang xả rác...");
                isMoving = false;
                currentTrashCount = 0;

                // Reset vị trí xe về ban đầu nếu cần
                // transform.position = initialPosition;
            }
        }
    }
}
