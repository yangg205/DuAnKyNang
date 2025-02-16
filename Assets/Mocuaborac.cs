using UnityEngine;

public class Mocuaborac : MonoBehaviour
{
    public Animator animator; // Animator của nhà
    public GameObject trashPrefab; // Prefab của rác
    public Transform spawnPoint; // Vị trí phía trước nhà để tạo rác

    public float trashSpawnDelay = 1f; // Độ trễ khi tạo rác sau khi mở cửa
    public float closeDoorDelay = 3f; // Độ trễ trước khi đóng cửa
    public float openDoorChance = 0.5f; // Tỉ lệ mở cửa (50% mặc định)
    public float routineInterval = 5f; // Khoảng thời gian giữa các lần kiểm tra

    private bool isDoorOpened = false; // Trạng thái kiểm soát cửa

    private void Start()
    {
        // Tự động lặp lại quy trình mỗi routineInterval giây
        InvokeRepeating(nameof(PerformTrashRoutine), 0f, routineInterval);
    }

    // Thực hiện quy trình: random mở cửa, tạo rác, đóng cửa
    public void PerformTrashRoutine()
    {
        if (!isDoorOpened && ShouldOpenDoor())
        {
            OpenDoor(); // Mở cửa nếu random cho phép
            Invoke(nameof(SpawnTrash), trashSpawnDelay); // Tạo rác sau một khoảng thời gian
            Invoke(nameof(CloseDoor), trashSpawnDelay + closeDoorDelay); // Đóng cửa sau khi tạo rác
        }
        else
        {
            Debug.Log("Cửa không mở lần này.");
        }
    }

    // Kiểm tra xem có nên mở cửa không (dựa vào random)
    private bool ShouldOpenDoor()
    {
        float randomValue = Random.Range(0f, 1f); // Random giá trị từ 0 đến 1
        Debug.Log($"Giá trị random: {randomValue}, cần nhỏ hơn {openDoorChance} để mở cửa.");
        return randomValue <= openDoorChance; // So sánh với tỉ lệ mở cửa
    }

    // Kích hoạt trigger để mở cửa
    private void OpenDoor()
    {
        if (animator != null)
        {
            animator.SetTrigger("mo"); // Kích hoạt trigger "mo"
            isDoorOpened = true; // Đánh dấu trạng thái cửa đã mở
            Debug.Log("Cửa đang mở...");
        }
        else
        {
            Debug.LogWarning("Animator chưa được gán!");
        }
    }

    // Tạo túi rác tại vị trí phía trước nhà
    private void SpawnTrash()
    {
        if (trashPrefab != null && spawnPoint != null)
        {
            Instantiate(trashPrefab, spawnPoint.position, spawnPoint.rotation); // Tạo rác
            Debug.Log("Đã tạo rác trước nhà!");
        }
        else
        {
            Debug.LogWarning("Trash Prefab hoặc Spawn Point chưa được thiết lập!");
        }
    }

    // Kích hoạt trigger để đóng cửa
    private void CloseDoor()
    {
        if (animator != null)
        {
            animator.SetTrigger("dong"); // Kích hoạt trigger "dong"
            isDoorOpened = false; // Đặt trạng thái về ban đầu
            Debug.Log("Cửa đã đóng.");
        }
        else
        {
            Debug.LogWarning("Animator chưa được gán!");
        }
    }
}
