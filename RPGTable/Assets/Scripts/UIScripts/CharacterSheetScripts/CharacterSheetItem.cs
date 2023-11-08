using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSheetItem : MonoBehaviour
{
    [SerializeField] GameObject item;

    public InputField itemName;
    public InputField itemAmount;
    public InputField itemWeight;

    public int GetItemWeight()
    {
        if (itemWeight.text == null)
        {
            itemWeight.text = "0";
            return 0;
        }
        else
        {
            return int.Parse(itemWeight.text);
        }
    }

    public int GetItemAmount()
    {
        if (itemAmount.text == null)
        {
            itemAmount.text = "0";
            return 0;
        }
        else
        {
            return int.Parse(itemAmount.text);
        }
    }

    public void DestroyItem()
    {
        Destroy(item);
    }
}
