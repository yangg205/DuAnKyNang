using UnityEngine;
using UnityEngine.UI;

public class GarbageTruckManager : MonoBehaviour
{
    public Button goToLandfillButton; // Nút "Đi vào bãi rác"
    public Transform spawnPoint; // Vị trí ngoài bản đồ để tạo xe mới
    public GameObject garbageTruckPrefab; // Prefab của xe rác (để tạo xe mới khi cần)

    private void Start()
    {
        // Gán sự kiện cho nút "Đi vào bãi rác"
        goToLandfillButton.onClick.AddListener(OnGoToLandfillButtonClicked);
        goToLandfillButton.gameObject.SetActive(true); // Đảm bảo nút hiển thị khi bắt đầu
    }

    // Khi nút "Đi vào bãi rác" được bấm
    private void OnGoToLandfillButtonClicked()
    {
        // Ẩn nút khi xe đang di chuyển đến bãi rác
        goToLandfillButton.gameObject.SetActive(false);

        // Tạo một xe mới và bắt đầu di chuyển
        GameObject newTruck = Instantiate(garbageTruckPrefab, spawnPoint.position, spawnPoint.rotation);
        GarbageTruck truckScript = newTruck.GetComponent<GarbageTruck>();
        if (truckScript != null)
        {
            truckScript.StartMovingToDestination();
        }
    }
}
