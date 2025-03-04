using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GarbageTruck : MonoBehaviour
{
    public TrashData trashData;
    public Transform trashCollectPoint;
    public Transform landfillPoint;
    public ParticleSystem smokeEffect;
    public Animator truckAnimator;
    public AudioSource engineStartSound;
    public TextMeshProUGUI trashCountText;
    public Slider timeSlider;
    public float moveSpeed = 5f;
    public int trashCapacity = 10;
    public float maxCollectTime = 30f;

    private int currentTrashCount = 0;
    private Transform currentTarget;
    private bool isMoving = false;
    private float remainingTime;
    private bool isTruckStarted = false; // Trạng thái theo dõi nếu xe đã khởi động

    private void Start()
    {
        remainingTime = maxCollectTime;
        SetTarget(trashCollectPoint);
        UpdateTrashCountText();
        trashData.ClearData();

        // Tắt hiệu ứng khói khi bắt đầu trò chơi
        ToggleSmokeEffect(false);
    }

    private void Update()
    {
        if (!isMoving && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timeSlider.value = remainingTime / maxCollectTime;

            if (remainingTime <= 0 || currentTrashCount >= trashCapacity)
            {
                if (!isTruckStarted)
                {
                    StartCoroutine(StartTruckAndMoveToLandfill());
                }
            }
        }

        if (isMoving && currentTarget != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, currentTarget.position) <= 0.1f)
            {
                if (currentTarget == landfillPoint)
                {
                    ArriveAtLandfill();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ngăn không cho thu rác nếu đã hết thời gian hoặc xe đã đầy rác
        if (remainingTime <= 0 || currentTrashCount >= trashCapacity)
        {
            return;
        }

        if (other.CompareTag("Trash"))
        {
            TrashItem trashItem = other.GetComponent<TrashItem>();
            if (trashItem != null)
            {
                AddTrashToTruck(trashItem.trashType, trashItem.GetComponent<SpriteRenderer>().sprite, trashItem.trashName);
                Destroy(other.gameObject);
            }
        }
    }


    private void AddTrashToTruck(string trashType, Sprite trashSprite, string trashName)
    {
        currentTrashCount++;
        trashData.collectedTrashTypes.Add(trashType);
        trashData.collectedTrashSprites.Add(trashSprite);
        trashData.collectedTrashNames.Add(trashName);
        UpdateTrashCountText();

        if (currentTrashCount >= trashCapacity && !isTruckStarted)
        {
            StartCoroutine(StartTruckAndMoveToLandfill());
        }
    }

    private IEnumerator StartTruckAndMoveToLandfill()
    {
        isTruckStarted = true;

        // Kích hoạt hiệu ứng khói và âm thanh ngay lập tức
        StartEngineSequence();

        // Chờ 4 giây trước khi di chuyển
        yield return new WaitForSeconds(4f);

        // Di chuyển đến bãi đổ rác
        MoveToLandfill();
    }

    private void MoveToLandfill()
    {
        isMoving = true;
        SetTarget(landfillPoint);
    }

    private void ArriveAtLandfill()
    {
        isMoving = false;
        StopEngineSequence();
        SceneManager.LoadScene("Scene2");
    }

    private void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    private void StartEngineSequence()
    {
        // Phát âm thanh khởi động
        if (engineStartSound != null) engineStartSound.Play();

        // Kích hoạt hoạt ảnh khởi động
        if (truckAnimator != null) truckAnimator.SetTrigger("StartEngine");

        // Bật hiệu ứng khói
        ToggleSmokeEffect(true);
    }

    private void StopEngineSequence()
    {
        // Tắt hiệu ứng khói khi xe dừng
        ToggleSmokeEffect(false);
    }

    private void ToggleSmokeEffect(bool isActive)
    {
        if (smokeEffect != null)
        {
            if (isActive) smokeEffect.Play();
            else smokeEffect.Stop();
        }
    }

    private void UpdateTrashCountText()
    {
        trashCountText.text = $"Rác: {currentTrashCount}/{trashCapacity}";
    }
}
