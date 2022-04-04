using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemPriceText;
    [SerializeField] private Image itemImage;

    [SerializeField] private Button mainItemButton;
    [SerializeField] private Button lockedButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button cancelButton;

    [SerializeField] private GameObject lockedState;
    [SerializeField] private GameObject buyState;
    [SerializeField] private GameObject outlineBg;

    private PlayerController playerController;
    private int itemPrice;
    private string itemMaterialName;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        mainItemButton.onClick.AddListener(OnMainButtonClick);
        lockedButton.onClick.AddListener(OnLockedButtonClick);
        buyButton.onClick.AddListener(OnBuyButtonClick);
        cancelButton.onClick.AddListener(OnCancelButtonClick);
    }

    public void InitItem(Item.ItemType itemType)
    {
        itemNameText.text = Item.GetItemName(itemType);
        itemPrice = Item.GetItemCost(itemType);
        itemPriceText.text = itemPrice.ToString();
        itemImage.sprite = Resources.Load<Sprite>(Item.GetTextureName(itemType));
        Debug.LogError(itemImage.sprite);
        itemMaterialName = Item.GetMaterialTextureName(itemType);
    }

    private void OnLockedButtonClick()
    {
        buyState.SetActive(true);
    }

    private void OnBuyButtonClick()
    {
        //if(user money >= itemPrice)
        buyState.SetActive(false);
        lockedState.SetActive(false);
    }

    private void OnCancelButtonClick()
    {
        buyState.SetActive(false);
        lockedState.SetActive(true);
    }

    private void OnMainButtonClick()
    {
        if(!lockedState.activeSelf && itemMaterialName != null) //Maybe not check this and just set the button.enabled = false;
        {
            Transform content = transform.parent;
            foreach (Transform child in content)
            {
                child.Find("OutlineBg").gameObject.SetActive(false);
            }
            outlineBg.SetActive(true);
            playerController.SetTShirtMaterial(itemMaterialName);
        }
    }
}
