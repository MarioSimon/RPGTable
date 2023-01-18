using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreator : NetworkBehaviour
{
    
    [SerializeField] GameManager gameManager;
    [SerializeField] Button raceMenuNext;
    [SerializeField] Button classMenuBack;
    [SerializeField] Button classMenuNext;
    [SerializeField] Button backgroundMenuBack;
    [SerializeField] Button backgroundMenuNext;
    [SerializeField] Button scoresMenuBack;
    [SerializeField] Button scoresMenuNext;

    private CharacterSheetInfo newCharacterSheet;

    void Start()
    {
        
    }


}
