using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrashSorting : MonoBehaviour
{
    public TrashData trashData; // Tham chiếu tới ScriptableObject
    public Image TrashImage; // UI Image hiển thị rác
    public TextMeshProUGUI ResultText; // Kết quả phân loại
    public Button RecycleButton, MixedButton, ElectronicButton; // Các nút phân loại

    private int currentTrashIndex = 0; // Chỉ số rác hiện tại
    private string currentTrashType; // Loại rác hiện tại

    private void Start()
    {
        if (trashData.collectedTrashSprites.Count == 0)
        {
            Debug.LogError("No trash collected in Scene 1!");
            ResultText.text = "Không có rác để phân loại.";
            return;
        }

        SetupButtons();
        LoadNextTrash();
    }

    private void SetupButtons()
    {
        RecycleButton.onClick.AddListener(() => CheckTrashType("Rác Tái Chế"));
        MixedButton.onClick.AddListener(() => CheckTrashType("Rác Tổng Hợp"));
        ElectronicButton.onClick.AddListener(() => CheckTrashType("Rác Điện Tử"));
    }

    private void LoadNextTrash()
    {
        if (currentTrashIndex >= trashData.collectedTrashSprites.Count)
        {
            ResultText.text = "Hoàn thành phân loại rác!";
            TrashImage.enabled = false;
            return;
        }

        TrashImage.sprite = trashData.collectedTrashSprites[currentTrashIndex];
        currentTrashType = trashData.collectedTrashTypes[currentTrashIndex];
        currentTrashIndex++;
        ResultText.text = ""; // Xóa thông báo cũ
    }

    private void CheckTrashType(string selectedType)
    {
        if (selectedType == currentTrashType)
        {
            ResultText.text = "Đúng!";
            ResultText.color = Color.green;
        }
        else
        {
            ResultText.text = "Sai!";
            ResultText.color = Color.red;
        }

        Invoke(nameof(LoadNextTrash), 1f); // Tải rác tiếp theo sau 1 giây
    }
}
