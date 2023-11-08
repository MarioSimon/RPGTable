using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeTracker : MonoBehaviour
{
    [SerializeField] GameObject orderListParent;
    public List<InitiativeItem> initiativeList = new List<InitiativeItem>();

    [SerializeField] GameObject initiativeItemPrefab;

    public void AddToTracker(string name, bool isHost)
    {
        if (Tracking(name)) { return; }

        GameObject newItem = Instantiate(initiativeItemPrefab);

        newItem.GetComponent<InitiativeItem>().tokenName.text = name;
        newItem.GetComponent<RectTransform>().SetParent(orderListParent.GetComponent<RectTransform>());

        if (!isHost)
        {
            newItem.GetComponent<InitiativeItem>().tokenInitiative.interactable = false;
        }

        initiativeList.Add(newItem.GetComponent<InitiativeItem>());
    }

    public void SortInitiative()
    {
        initiativeList.Sort();

        foreach (InitiativeItem item in initiativeList)
        {
            item.gameObject.transform.SetAsLastSibling();
        }
    }

    public void SetInitiative(string name, int newInitiative)
    {
        foreach (InitiativeItem item in initiativeList)
        {
            if (item.tokenName.text == name)
            {
                item.tokenInitiative.text = newInitiative.ToString();
                break;
            }
        }

        SortInitiative();
    }

    private bool Tracking(string characterName)
    {
        bool tracking = false;

        foreach (InitiativeItem item in initiativeList)
        {
            if (item.tokenName.text == characterName)
            {
                tracking = true;
                break;
            }
        }

        return tracking;
    }

    public void RemoveFromTracker(string characterName)
    {
        foreach (InitiativeItem item in initiativeList)
        {
            if (item.tokenName.text == characterName)
            {
                initiativeList.Remove(item);
                Destroy(item.gameObject);
                break;
            }
        }
    }
}
