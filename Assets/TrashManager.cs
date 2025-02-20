using System.Collections.Generic;
using UnityEngine;

public class TrashManager : MonoBehaviour
{
    public static TrashManager Instance;

    private List<TrashItem> trashItems = new List<TrashItem>();

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Lưu rác vào TrashManager
    public void AddTrash(TrashItem trashItem)
    {
        if (trashItem != null && !trashItems.Contains(trashItem))
        {
            trashItems.Add(trashItem);
            Debug.Log("Rác đã được lưu vào TrashManager: " + trashItem.trashType);
        }
    }

    // Có thể thêm các phương thức để xử lý rác, ví dụ lấy số lượng rác
    public int GetTrashCount()
    {
        return trashItems.Count;
    }
}
