using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{

    public enum ItemType
    {
        SidemenShirt1,
        TshirtTest,
        SidemenShirt3,
        SidemenShirt4
    }

/*    public enum Items
    {
        Bandana1,
        Bandana2,
        Shirt1,
        Shirt2,
        Shorts1,
        Shorts2
    }*/

    public static int GetItemCost(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.SidemenShirt1: return 10;
            case ItemType.TshirtTest: return 20;
            case ItemType.SidemenShirt3: return 30;
            case ItemType.SidemenShirt4: return 40;
        }
    }

    public static string GetItemName(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.SidemenShirt1: return "SidemenShirt1";
            case ItemType.TshirtTest: return "TshirtTest";
            case ItemType.SidemenShirt3: return "SidemenShirt3";
            case ItemType.SidemenShirt4: return "SidemenShirt4";
        }
    }

    public static string GetTextureName(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.SidemenShirt1: return "SidemenShirt1";
            case ItemType.TshirtTest: return "TshirtTest";
            case ItemType.SidemenShirt3: return "Legs";
            case ItemType.SidemenShirt4: return "Shoes";
        }
    }

    public static string GetMaterialTextureName(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.SidemenShirt1: return "SidemenShirt1";
            case ItemType.TshirtTest: return "TshirtTest";
            case ItemType.SidemenShirt3: return "Legs";
            case ItemType.SidemenShirt4: return "Shoes";
        }
    }

    public static int GetItemId(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.SidemenShirt1: return 0;
            case ItemType.TshirtTest: return 1;
            case ItemType.SidemenShirt3: return 2;
            case ItemType.SidemenShirt4: return 3;
        }
    }
}
