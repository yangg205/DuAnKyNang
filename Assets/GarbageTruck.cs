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
    public float maxCollectTime = 30f; // Thay đổi thời gian thu gom

    private int currentTrashCount = 0;
    private Transform currentTarget;
    private bool isMoving = false;
    private float remainingTime;

    private void Start()
    {
        remainingTime = maxCollectTime;
        SetTarget(trashCollectPoint);
        UpdateTrashCountText();
        StartEngineSequence();
        trashData.ClearData();
    }

    private void Update()
    {
        if (!isMoving && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timeSlider.value = remainingTime / maxCollectTime;

            if (remainingTime <= 0 || currentTrashCount >= trashCapacity)
            {
                MoveToLandfill();
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
        if (currentTrashCount >= trashCapacity)
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

        if (currentTrashCount >= trashCapacity)
        {
            MoveToLandfill();
        }
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
        if (engineStartSound != null) engineStartSound.Play();
        if (truckAnimator != null) truckAnimator.SetTrigger("StartEngine");
        ToggleSmokeEffect(true);
    }

    private void StopEngineSequence()
    {
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
