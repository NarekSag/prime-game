using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemPriceText;
    [SerializeField] private Image itemImage;

    [SerializeField] private Button buyButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private GameObject equippedState;

    [HideInInspector] public bool isEquipped;

    private PlayerController playerController;
    private StoreDialog store;
    private int itemId;
    private int itemPrice;
    private string itemMaterialName;

    private void Start()
    {
        playerController = GameController.instance.playerController;
        store = GameObject.Find("Store").GetComponent<StoreDialog>();

        buyButton.onClick.AddListener(OnBuyButtonClick);
        equipButton.onClick.AddListener(OnEquipButtonClick);
    }

    public void InitItem(JSONReader.Item item)
    {
        itemId = item.id;
        itemNameText.text = item.name;
        itemPrice = item.price;
        itemPriceText.text = itemPrice.ToString();
        itemImage.sprite = Resources.Load<Sprite>(item.textureName);
        itemMaterialName = item.materialTextureName;
        isEquipped = item.isEquipped;
        SetButtonsState(item);
    }

    private void OnBuyButtonClick()
    {
        if(GameController.instance.currencyCounter >= itemPrice)
        {
            GameController.instance.UpdateCurrency(-itemPrice);
            buyButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);
            GameController.instance.storeItemList.FirstOrDefault(s => s.id == itemId).isBought = true;
            GameController.instance.OverrideItemJson();
        }
    }

    private void OnEquipButtonClick()
    {
        equipButton.gameObject.SetActive(false);
        equippedState.SetActive(true);
        playerController.SetTShirtMaterial(itemMaterialName);
        store.EquipStoreItemById(itemId);
    }

    private void SetButtonsState(JSONReader.Item item)
    {
        buyButton.gameObject.SetActive(!item.isBought);
        equipButton.gameObject.SetActive(!item.isEquipped && item.isBought);
        equippedState.SetActive(item.isBought && item.isEquipped);
    }

    public StoreItem GetItemById(int id)
    {
        if(id == itemId)
        {
            return this;
        }
        return null;
    }

    public int GetItemId()
    {
        return itemId;
    }
}
