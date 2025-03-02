using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrashData", menuName = "Trash/TrashData")]
public class TrashData : ScriptableObject
{
    public List<string> collectedTrashTypes = new List<string>(); // Loại rác đã thu gom
    public List<Sprite> collectedTrashSprites = new List<Sprite>(); // Hình ảnh rác đã thu gom

    public void ClearData()
    {
        collectedTrashTypes.Clear();
        collectedTrashSprites.Clear();
    }
}
