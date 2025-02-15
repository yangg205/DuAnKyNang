using UnityEngine;

public class ClickChuot : MonoBehaviour
{
    // Hàm này được gọi khi người chơi click vào collider của vật phẩm
    private void OnMouseDown()
    {
        // Hủy vật phẩm (xóa khỏi scene)
        Destroy(gameObject);
    }

}
