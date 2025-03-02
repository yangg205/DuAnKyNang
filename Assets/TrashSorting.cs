using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TrashSorting : MonoBehaviour
{
    public TrashData trashData;
    public Image TrashImage;
    public TextMeshProUGUI ResultText;
    public TextMeshProUGUI TrashNameText; // Hiển thị tên rác
    public Button RecycleButton, MixedButton, ElectronicButton;

    private int currentTrashIndex = 0;
    private string currentTrashType;

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
            TrashNameText.enabled = false;
            SceneManager.LoadScene("Scene1");
            return;
        }

        TrashImage.sprite = trashData.collectedTrashSprites[currentTrashIndex];
        currentTrashType = trashData.collectedTrashTypes[currentTrashIndex];
        TrashNameText.text = trashData.collectedTrashNames[currentTrashIndex]; // Hiển thị tên rác
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

        Invoke(nameof(LoadNextTrash), 1f);
    }
}
