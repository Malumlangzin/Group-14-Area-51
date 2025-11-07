using UnityEngine;

[System.Serializable]
public class Item
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
