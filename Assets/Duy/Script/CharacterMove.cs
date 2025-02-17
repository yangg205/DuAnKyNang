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

    void Start()
    {
        initialPosition = transform.position;  // Ghi nhớ vị trí ban đầu của nhân vật
        gameObject.SetActive(true);  // Đảm bảo nhân vật được hiển thị khi game bắt đầu
    }

    void Update()
    {
        if (!isMovingBack)
        {
            MoveToTarget();  // Di chuyển đến vị trí đích
        }
        else
        {
            MoveBackToInitialPosition();  // Quay lại vị trí ban đầu
        }
    }

    public void StartMovement()
    {
        // Gọi hàm này khi muốn nhân vật bắt đầu di chuyển
        isMovingBack = false;
    }

    private void MoveToTarget()
    {
        float distance = Vector2.Distance(transform.position, targetPosition);

        if (distance > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            // Lưu lại giá trị scaleX khi đến vị trí đích lần đầu
            if (initialScaleX == 0)
            {
                initialScaleX = transform.localScale.x;  // Lưu giá trị scaleX ban đầu
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
        // Flip mặt về vị trí ban đầu (liên tục cập nhật trong suốt quá trình quay lại)
        if (initialScaleX != 0)
        {
            FlipBack();  // Flip mặt khi di chuyển về
        }

        float distance = Vector2.Distance(transform.position, initialPosition);

        if (distance > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);
        }
        else
        {
            transform.position = initialPosition;
            gameObject.SetActive(false);  // Khi về vị trí ban đầu, ẩn nhân vật
            doorController.CloseDoor();  // Đóng cửa khi nhân vật quay lại
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
    private void FlipBack()
    {
        Vector3 localScale = transform.localScale;
        localScale.x = -initialScaleX;  // Lật mặt về vị trí ban đầu (đảo ngược giá trị scaleX)
        transform.localScale = localScale;
    }
}
