using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreDialog : MonoBehaviour
{
    [SerializeField] private GameObject storeItem;
    [SerializeField] private Button headTopBtn;
    [SerializeField] private Button chestTopBtn;
    [SerializeField] private Button legsTopBtn;
    [SerializeField] private Button shoesTopBtn;
    [SerializeField] private GameObject content;

    private List<GameObject> storeItems = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        headTopBtn.onClick.AddListener(HeadCategoryChosen);

        InstantiateStoreItem(Item.ItemType.SidemenShirt1);
        InstantiateStoreItem(Item.ItemType.TshirtTest);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HeadCategoryChosen()
    {
        ClearContentView();
        InstantiateStoreItem(Item.ItemType.SidemenShirt1);
        InstantiateStoreItem(Item.ItemType.TshirtTest);
        InstantiateStoreItem(Item.ItemType.SidemenShirt3);
        InstantiateStoreItem(Item.ItemType.SidemenShirt4);
    }

    private void InstantiateStoreItem(Item.ItemType itemType)
    {
        GameObject s = Instantiate(storeItem, content.transform);
        s.GetComponent<StoreItem>().InitItem(itemType);
        storeItems.Add(s);
    }

    private void ClearContentView()
    {
        for(int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < storeItems.Count; i++)
        {
            Destroy(storeItems[i]);
        }
    }
}
