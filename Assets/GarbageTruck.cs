using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GarbageTruck : MonoBehaviour
{
    public Transform trashCollectPoint; // Vị trí lấy rác
    public Transform destinationPoint; // Điểm đến khi xe chạy
    public Transform landfillPoint; // Điểm bãi rác
    public Transform spawnPoint; // Vị trí ngoài bản đồ để tạo xe mới
    public Button goToLandfillButton; // Nút "Đi vào bãi rác"
    public ParticleSystem smokeEffect; // Particle System cho khói
    public Animator truckAnimator; // Animator của xe
    public float moveSpeed = 5f; // Tốc độ xe chạy
    public int trashCapacity = 10; // Số lượng rác tối đa

    private int currentTrashCount = 0;
    private bool isMoving = false;
    private bool isEngineRunning = false;
    private Transform currentTarget;
    private bool isWaiting = false;

    private void Start()
    {
        // Gán hành động cho button trực tiếp từ mã
        goToLandfillButton.onClick.AddListener(MoveToLandfill);

        // Tắt tất cả animation và particle khi bắt đầu game
        StopEngineSequence();
        SetTarget(trashCollectPoint);
    }

    private void Update()
    {
        if (isMoving && currentTarget != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, currentTarget.position) <= 0.1f)
            {
                if (currentTarget == trashCollectPoint)
                {
                    StopAtTrashCollectPoint();
                }
                else if (currentTarget == destinationPoint)
                {
                    StopAtDestination();
                }
                else if (currentTarget == landfillPoint)
                {
                    HandleLandfillArrival();
                }
            }
        }
    }

    private void StopAtTrashCollectPoint()
    {
        isMoving = false;
        Debug.Log("Xe đã đến vị trí lấy rác.");
    }

    private void StopAtDestination()
    {
        isMoving = false;
        Debug.Log("Xe đã đến điểm đích.");

        // Hiện nút đi vào bãi rác
        goToLandfillButton.gameObject.SetActive(true);
    }

    private void HandleLandfillArrival()
    {
        isMoving = false;
        Debug.Log("Xe đã đến bãi rác.");

        // Tạo xe mới
        SpawnNewTruck();

        // Xóa xe hiện tại sau 1 giây
        Destroy(gameObject, 1f);
    }

    public void StartMovingToDestination()
    {
        SetTarget(destinationPoint);
        isMoving = true;
        Debug.Log("Xe bắt đầu di chuyển đến đích.");
        StartEngineSequence(); // Bắt đầu engine và particle khi di chuyển
    }

    // Gọi MoveToLandfill khi bấm button
    public void MoveToLandfill()
    {
        // Ẩn nút đi vào bãi rác
        goToLandfillButton.gameObject.SetActive(false);

        // Tiến hành di chuyển đến bãi rác mỗi lần bấm
        SetTarget(landfillPoint);
        isMoving = true;
        Debug.Log("Xe đang di chuyển vào bãi rác...");
    }

    private void StartEngineSequence()
    {
        if (!isEngineRunning)
        {
            isEngineRunning = true;
            if (truckAnimator != null)
            {
                truckAnimator.SetTrigger("StartEngine"); // Gọi trigger "StartEngine"
            }

            ToggleSmokeEffect(true); // Bật particle khi bắt đầu di chuyển
        }
    }

    private void StopEngineSequence()
    {
        if (truckAnimator != null)
        {
            truckAnimator.SetTrigger("StopEngine"); // Gọi trigger "StopEngine"
        }

        ToggleSmokeEffect(false); // Tắt particle khi xe dừng
        isEngineRunning = false;
    }

    private void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    private void ToggleSmokeEffect(bool isActive)
    {
        if (smokeEffect != null)
        {
            if (isActive)
                smokeEffect.Play();
            else
                smokeEffect.Stop();
        }
    }

    private void SpawnNewTruck()
    {
        GameObject newTruck = Instantiate(gameObject, spawnPoint.position, spawnPoint.rotation); // Tạo xe mới ngoài map
        GarbageTruck newTruckScript = newTruck.GetComponent<GarbageTruck>();
        if (newTruckScript != null)
        {
            newTruckScript.StartMovingFromOutside();
        }
    }

    private void StartMovingFromOutside()
    {
        SetTarget(trashCollectPoint);
        isMoving = true;
    }

    // Xử lý va chạm với rác
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trash")) // Kiểm tra nếu là rác
        {
            Debug.Log("Xe đã va chạm với rác!");

            TrashItem trashItem = other.GetComponent<TrashItem>();
            if (trashItem != null)
            {
                // Thêm rác vào xe
                AddTrashToTruck(other.gameObject);

                // Xóa rác khỏi màn hình ngay lập tức
                Destroy(other.gameObject);
            }
        }
    }

    // Thêm rác vào xe
    public void AddTrashToTruck(GameObject trashItem)
    {
        if (currentTrashCount < trashCapacity)
        {
            currentTrashCount++;
            Debug.Log("Rác đã được thả vào xe. Số lượng rác hiện tại: " + currentTrashCount);

            // Nếu xe đã đủ rác, bắt đầu di chuyển đến đích
            if (currentTrashCount >= trashCapacity && !isMoving)
            {
                StartCoroutine(WaitBeforeMoving());
            }
        }
        else
        {
            Debug.Log("Xe đã đầy rác!");
        }
    }

    // Chờ 4 giây sau khi thu gom đủ rác
    private IEnumerator WaitBeforeMoving()
    {
        isWaiting = true;
        StartEngineSequence(); // Bật engine và particle
        yield return new WaitForSeconds(4f); // Chờ 4 giây

        isWaiting = false;
        StartMovingToDestination(); // Bắt đầu di chuyển sau khi chờ
    }
}
