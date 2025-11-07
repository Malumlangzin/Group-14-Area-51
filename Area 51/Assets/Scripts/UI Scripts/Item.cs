using UnityEngine;

<<<<<<< Updated upstream
[System.Serializable]
public class Item
=======
<<<<<<< HEAD
public class Item : MonoBehaviour
=======
[System.Serializable]
public class Item
>>>>>>> c8a557d70e1d74ee7e9252c115c05f13bb199478
>>>>>>> Stashed changes
{
    public int id;
    public string itemName;
    public Sprite icon;
    public string description;
    public GameObject prefab;

    public Item(int id, string name, Sprite icon, string description = "", GameObject prefab = null)
    {
        this.id = id;
        this.itemName = name;
        this.icon = icon;
        this.description = description;
        this.prefab = prefab;
    }
}
