using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StoreDialog : MonoBehaviour
{
    [SerializeField] private GameObject storeItem;
    [SerializeField] private GameObject content;
    [SerializeField] private Button nextItemButton;
    [SerializeField] private Button prevItemButton;

    private List<StoreItem> storeItems = new List<StoreItem>();
    private StoreItem defaultStoreItem;

    // Start is called before the first frame update
    void Start()
    {
        nextItemButton.onClick.AddListener(NextItem);
        prevItemButton.onClick.AddListener(PrevItem);

        for (int i = 0; i < GameController.instance.storeItemList.Count; i++)
        {
            InstantiateStoreItem(GameController.instance.storeItemList[i]);
        }

        SetDefaultItem();
    }

    private void InstantiateStoreItem(JSONReader.Item item)
    {
        GameObject s = Instantiate(storeItem, content.transform);
        StoreItem si = s.GetComponent<StoreItem>();
        si.InitItem(item);
        s.gameObject.SetActive(false);
        storeItems.Add(si);
    }
    public void EquipStoreItemById(int id)
    {
        var storeItemList = GameController.instance.storeItemList;
        for (int i = 0; i < storeItemList.Count; i++)
        {
            storeItemList[i].isEquipped = id == storeItemList[i].id ? true : false;
            storeItems[i].InitItem(storeItemList[i]);
        }
        GameController.instance.OverrideItemJson();
    }

    public void SetDefaultItem()
    {
        defaultStoreItem = storeItems.FirstOrDefault(s => s.isEquipped);
        defaultStoreItem.gameObject.SetActive(true);
    }

    public void NextItem()
    {
        for(int i = 0; i < storeItems.Count; i++)
        {
            if (storeItems[i] == defaultStoreItem)
            {
                storeItems[i].gameObject.SetActive(false);
                if (i + 1 > 0 && i + 1 < storeItems.Count)
                {
                    storeItems[i + 1].gameObject.SetActive(true);
                    defaultStoreItem = storeItems[i + 1];
                    break;
                }
                else
                {
                    storeItems[0].gameObject.SetActive(true);
                    defaultStoreItem = storeItems[0];
                    break;
                }
            }
        }
    }

    public void PrevItem()
    {
        for (int i = 0; i < storeItems.Count; i++)
        {
            if (storeItems[i] == defaultStoreItem)
            {
                storeItems[i].gameObject.SetActive(false);
                if (i - 1 >= 0 && i - 1 < storeItems.Count)
                {
                    storeItems[i - 1].gameObject.SetActive(true);
                    defaultStoreItem = storeItems[i - 1];
                    break;
                }
                else
                {
                    storeItems.Last().gameObject.SetActive(true);
                    defaultStoreItem = storeItems.Last();
                    break;
                }
            }
        }
    }
}
