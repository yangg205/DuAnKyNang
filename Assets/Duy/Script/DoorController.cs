using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private bool isDoorOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Mở cửa
    public void OpenDoor()
    {
        if (!isDoorOpen)
        {
            animator.SetTrigger("Open");
            isDoorOpen = true;
        }
    }

    // Đóng cửa
    public void CloseDoor()
    {
        if (isDoorOpen)
        {
            animator.SetTrigger("Close");
            isDoorOpen = false;
        }
    }
}
