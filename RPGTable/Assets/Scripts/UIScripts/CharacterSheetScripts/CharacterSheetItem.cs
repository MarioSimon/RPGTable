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
        int weight;

        if (int.TryParse(itemWeight.text, out weight))
        {
            return weight;
        }
        itemWeight.text = "0";

        return 0;
    }

    public int GetItemAmount()
    {
        int count;

        if (int.TryParse(itemAmount.text, out count))
        {
            return count;
        }
        itemAmount.text = "0";

        return 0;
    }

    public void DestroyItem()
    {
        Destroy(item);
    }
}
