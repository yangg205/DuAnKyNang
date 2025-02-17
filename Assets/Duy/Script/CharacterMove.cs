using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMove : MonoBehaviour
{
    public Vector2 targetPosition;  // Vị trí mà nhân vật di chuyển tới
    public float speed = 3f;        // Tốc độ di chuyển của nhân vật
    private Vector2 initialPosition;  // Vị trí ban đầu
    private bool isMovingBack = false;  // Biến kiểm tra xem có quay lại vị trí ban đầu không
    public GameObject bagPrefab;  // Prefab túi
    public DoorController doorController;  // Reference đến DoorController

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
            PlaceBag();  // Đặt túi khi nhân vật đến vị trí đích
            doorController.OpenDoor();  // Mở cửa khi nhân vật đến vị trí đích
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
            doorController.StartCoroutine(ReappearAfterDelay(2f));
        }
    }

    void PlaceBag()
    {
        if (bagPrefab != null)
        {
            Instantiate(bagPrefab, targetPosition, Quaternion.identity);
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
        yield return new WaitForSeconds(delay);  // Wait for the specified delay
        gameObject.SetActive(true);  // Make the character appear again
        canMove = true;  // Allow the character to start moving again
        hasFlipped = false;  // Reset flip state for new movement
        StartMovement();  // Start the movement process again
    }
}