using System.Collections;
using TMPro; // Sử dụng TextMeshPro
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
    public AudioSource engineStartSound; // Tham chiếu đến âm thanh khởi động
    public TextMeshProUGUI trashCountText; // TMP Text hiển thị số lượng rác
    public float moveSpeed = 5f; // Tốc độ xe chạy
    public int trashCapacity = 10; // Số lượng rác tối đa

    private int currentTrashCount = 0; // Số lượng rác hiện tại
    private bool isMoving = false; // Trạng thái di chuyển
    private bool isEngineRunning = false; // Trạng thái động cơ
    private Transform currentTarget; // Mục tiêu hiện tại
    private bool isWaiting = false; // Trạng thái chờ

    private void Start()
    {
        goToLandfillButton.onClick.AddListener(MoveToLandfill); // Gắn sự kiện cho nút

        StopEngineSequence(); // Tắt khói và động cơ ban đầu
        SetTarget(trashCollectPoint); // Mục tiêu ban đầu là vị trí lấy rác
        UpdateTrashCountText(); // Cập nhật hiển thị số rác
    }

    private void Update()
    {
        if (isMoving && currentTarget != null)
        {
            // Di chuyển xe tới mục tiêu
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

            // Khi đến mục tiêu
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
        goToLandfillButton.gameObject.SetActive(true); // Hiển thị nút "Đi vào bãi rác"
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
        StartEngineSequence();
    }

    public void MoveToLandfill()
    {
        goToLandfillButton.gameObject.SetActive(false); // Ẩn nút
        SetTarget(landfillPoint);
        isMoving = true;
        Debug.Log("Xe đang di chuyển vào bãi rác...");
    }

    private void StartEngineSequence()
    {
        if (!isEngineRunning)
        {
            isEngineRunning = true;

            if (engineStartSound != null) engineStartSound.Play();
            if (truckAnimator != null) truckAnimator.SetTrigger("StartEngine");
            ToggleSmokeEffect(true);
        }
    }

    private void StopEngineSequence()
    {
        if (truckAnimator != null) truckAnimator.SetTrigger("StopEngine");
        ToggleSmokeEffect(false);
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
            if (isActive) smokeEffect.Play();
            else smokeEffect.Stop();
        }
    }

    private void SpawnNewTruck()
    {
        GameObject newTruck = Instantiate(gameObject, spawnPoint.position, spawnPoint.rotation);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentTrashCount >= trashCapacity)
        {
            Debug.Log("Xe đã đầy rác, không xử lý va chạm thêm nữa.");
            return;
        }

        if (other.CompareTag("Trash"))
        {
            Debug.Log("Xe đã va chạm với rác!");

            TrashItem trashItem = other.GetComponent<TrashItem>();
            if (trashItem != null)
            {
                AddTrashToTruck();
                Destroy(other.gameObject);
            }
        }
    }

    public void AddTrashToTruck()
    {
        if (currentTrashCount < trashCapacity)
        {
            currentTrashCount++;
            Debug.Log($"Rác đã được thêm vào xe. Số lượng hiện tại: {currentTrashCount}/{trashCapacity}");
            UpdateTrashCountText();

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

    private IEnumerator WaitBeforeMoving()
    {
        isWaiting = true;
        StartEngineSequence();
        yield return new WaitForSeconds(4f);
        isWaiting = false;
        StartMovingToDestination();
    }

    private void UpdateTrashCountText()
    {
        if (trashCountText != null)
        {
            trashCountText.text = $"Trash: {currentTrashCount}/{trashCapacity}";
        }
    }
}
