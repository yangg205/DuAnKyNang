using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public Vector2 targetPosition;  // Vị trí mà nhân vật di chuyển tới
    public float speed = 3f;        // Tốc độ di chuyển của nhân vật
    private Vector2 initialPosition;  // Vị trí ban đầu
    private bool isMovingBack = false;  // Biến kiểm tra xem có quay lại vị trí ban đầu không
    public List<GameObject> bagPrefabs;  // Danh sách các prefab túi rác
    public DoorController doorController;  // Reference đến DoorController
    public float reappearDelay = 2f; // Thời gian chờ trước khi xuất hiện lại, có thể chỉnh trong Inspector
    private GameObject bagInstance; // Túi được tạo ra khi di chuyển

    private float initialScaleX;  // Lưu giá trị scaleX ban đầu khi đi tới vị trí đích
    private bool canMove = true;  // Kiểm tra xem nhân vật có thể di chuyển không
    private bool hasFlipped = false;  // Kiểm tra xem nhân vật đã lật mặt chưa

    void Start()
    {
        initialPosition = transform.position;  // Ghi nhớ vị trí ban đầu của nhân vật
        gameObject.SetActive(true);  // Đảm bảo nhân vật được hiển thị khi game bắt đầu
        initialScaleX = transform.localScale.x;  // Lưu giá trị scaleX ban đầu khi game bắt đầu
    }

    void Update()
    {
        if (!isMovingBack && canMove)
        {
            MoveToTarget();  // Di chuyển đến vị trí đích
        }
        else if (isMovingBack && canMove)
        {
            MoveBackToInitialPosition();  // Quay lại vị trí ban đầu
        }
    }

    public void StartMovement()
    {
        // Gọi hàm này khi muốn nhân vật bắt đầu di chuyển
        isMovingBack = false;
        canMove = true;  // Ensure movement is allowed
        hasFlipped = false;  // Reset flip state for new movement cycle
    }

    private void MoveToTarget()
    {
        float distance = Vector2.Distance(transform.position, targetPosition);

        if (distance > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (bagInstance == null && bagPrefabs != null && bagPrefabs.Count > 0)
            {
                // Chọn ngẫu nhiên một prefab từ danh sách
                GameObject randomBagPrefab = bagPrefabs[Random.Range(0, bagPrefabs.Count)];
                bagInstance = Instantiate(randomBagPrefab, transform.position, Quaternion.identity);
                bagInstance.transform.parent = transform;  // Gắn vào nhân vật
            }

            // Flip the character to face the direction of movement if not already flipped
            if (!hasFlipped)
            {
                FlipCharacter(true);  // Flip character when moving to target
                hasFlipped = true;  // Mark as flipped
            }
        }
        else
        {
            isMovingBack = true;  // Sau khi tới đích, bắt đầu quay lại vị trí ban đầu
            ReleaseBag();  // Thả túi ra khỏi nhân vật
            doorController.OpenDoor();
        }
    }

    private void MoveBackToInitialPosition()
    {
        float distance = Vector2.Distance(transform.position, initialPosition);

        if (distance > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);

            // Flip character to face the initial position when moving back
            if (hasFlipped)
            {
                FlipCharacter(false);  // Flip back to initial position
                hasFlipped = false;  // Mark as flipped back to the original state
            }
        }
        else
        {
            transform.position = initialPosition;
            gameObject.SetActive(false);  // Khi về vị trí ban đầu, ẩn nhân vật
            doorController.CloseDoor();  // Đóng cửa khi nhân vật quay lại

            // Start coroutine from doorController to make character reappear after 2 seconds
            doorController.StartCoroutine(ReappearAfterDelay(reappearDelay)); // Dùng biến reappearDelay
        }
    }

    void ReleaseBag()
    {
        if (bagInstance != null)
        {
            bagInstance.transform.parent = null;  // Thả túi ra khỏi nhân vật
            bagInstance = null;  // Xóa reference để lần sau tạo lại
        }
    }

    // Hàm flip lại trục X khi quay về vị trí ban đầu
    private void FlipCharacter(bool toTarget)
    {
        Vector3 localScale = transform.localScale;

        if (toTarget)
        {
            localScale.x = Mathf.Abs(localScale.x);  // Flip to face target
        }
        else
        {
            localScale.x = -Mathf.Abs(localScale.x);  // Flip back to original position
        }

        transform.localScale = localScale;
    }

    // Coroutine to make the character reappear after a delay
    private IEnumerator ReappearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(true);
        canMove = true;
        hasFlipped = false;
        StartMovement();
    }
}
