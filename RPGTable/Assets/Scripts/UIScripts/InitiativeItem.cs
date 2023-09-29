using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeItem : MonoBehaviour, IComparable
{
    [SerializeField] GameManager gameManager;

    [SerializeField] Button deleteItem;
    public Text tokenName;
    public InputField tokenInitiative;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        tokenInitiative.onValueChanged.AddListener(delegate { CheckInitiativeInt(); });
        deleteItem.onClick.AddListener(delegate { gameManager.RemoveFromInitiativeTracker(tokenName.text); });
    }

    private void CheckInitiativeInt()
    {
        int value;
        if (!int.TryParse(tokenInitiative.text, out value))
        {
            tokenInitiative.text = "0";
        }
    }

    public int CompareTo(object other)
    {
        if (other == null) return 1;

        InitiativeItem otherInitiativeItem = other as InitiativeItem;

        if (otherInitiativeItem != null)
        {
            int initiative = GetIntInitiative(tokenInitiative);
            int otherInitiative = GetIntInitiative(otherInitiativeItem.tokenInitiative);

            return initiative.CompareTo(otherInitiative);
        }
        else
        {
            throw new ArgumentException("Object is not an Initiative Item");
        }
        
    }

    private int GetIntInitiative(InputField textInitiative)
    {
        int value;
        if (int.TryParse(textInitiative.text, out value))
        {
            return value;
        }
        else
        {
            return 0;
        }

    }
}
