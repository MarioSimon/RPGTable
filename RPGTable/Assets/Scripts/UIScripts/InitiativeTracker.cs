using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeTracker : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager uIManager;

    [SerializeField] GameObject orderListParent;
    public List<GameObject> initiativeList = new List<GameObject>();

    [SerializeField] GameObject initiativeItemPrefab;

    public void AddToTracker(int ID, string name)
    {
        GameObject newItem = Instantiate(initiativeItemPrefab);

        newItem.GetComponent<InitiativeItem>().tokenID = ID;
        newItem.GetComponent<InitiativeItem>().tokenName.text = name;
        newItem.GetComponent<RectTransform>().SetParent(orderListParent.GetComponent<RectTransform>());

        initiativeList.Add(newItem);
    }

    public void SortInitiative()
    {
        initiativeList.Sort();
    }

    public void SetInitiative(string name, int newInitiative)
    {
        foreach (GameObject item in initiativeList)
        {
            if (item.GetComponent<InitiativeItem>().tokenName.text == name)
            {
                item.GetComponent<InitiativeItem>().tokenInitiative.text = newInitiative.ToString();
            }
        }
    }
}
