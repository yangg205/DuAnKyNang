using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrashData", menuName = "Trash/TrashData")]
public class TrashData : ScriptableObject
{
    public List<string> collectedTrashTypes = new List<string>(); // Lưu loại rác
    public List<Sprite> collectedTrashSprites = new List<Sprite>(); // Lưu hình ảnh rác
    public List<string> collectedTrashNames = new List<string>(); // Lưu tên rác

    public void ClearData()
    {
        collectedTrashTypes.Clear();
        collectedTrashSprites.Clear();
        collectedTrashNames.Clear();
    }
}
