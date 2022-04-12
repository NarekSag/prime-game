using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    private TextAsset textJson;

    [System.Serializable]
    public class Item
    {
        public int id;
        public string name;
        public int price;
        public string textureName;
        public string materialTextureName;
        public bool isBought;
        public bool isEquipped;
    }

    [System.Serializable]
    public class ItemList
    {
        public List<Item> item;
    }

    public ItemList myItemList = new ItemList();

    private void Awake()
    {
        if(!File.Exists(Application.persistentDataPath + "/items.txt"))
        {
            AddStartingitems();
            File.WriteAllText(Application.persistentDataPath + "/items.txt", JsonUtility.ToJson(myItemList));
        }
        else
        {
            string path = File.ReadAllText(Application.persistentDataPath + "/items.txt");
            myItemList = JsonUtility.FromJson<ItemList>(path);
        }
    }

    public void Override(ItemList item)
    {
        string strOutput = JsonUtility.ToJson(item);

        File.WriteAllText(Application.persistentDataPath + "/items.txt", strOutput);
    }

    private void CreateStartingItem(int id, string name, int price, string textureName, string materialTextureName, bool isBought, bool isEquipped)
    {
        Item newItem = new Item();
        newItem.id = id;
        newItem.name = name;
        newItem.price = price;
        newItem.textureName = textureName;
        newItem.materialTextureName = materialTextureName;
        newItem.isBought = isBought;
        newItem.isEquipped = isEquipped;
        myItemList.item.Add(newItem);
    }

    private void AddStartingitems()
    {
        CreateStartingItem(0, "SidemenShirt1", 10, "SidemenShirt1", "SidemenShirt1", true, true);
        CreateStartingItem(1, "TshirtTest", 20, "TshirtTest", "TshirtTest", false, false);
    }
}
