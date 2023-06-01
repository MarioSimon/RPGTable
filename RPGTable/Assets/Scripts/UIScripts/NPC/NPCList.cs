using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCList : MonoBehaviour
{
    [SerializeField] UIManager uIManager;
    [SerializeField] GameManager gameManager;

    [SerializeField] GameObject NPCListParent;
    [SerializeField] GameObject NPCSelectorPrefab;

    [SerializeField] Button addNPC;
    

    [SerializeField] InputField searchBar;

    private void Start()
    {
        addNPC.onClick.AddListener(() => AddNPC());
    }

    private void AddNPC()
    {
        NPCSheetInfo newNPCSheet = new NPCSheetInfo();
        newNPCSheet.sheetID = gameManager.GetNewNPCSheetID();
        gameManager.AddNewNPCSheetInfo(newNPCSheet);

        GameObject newNPC = Instantiate(NPCSelectorPrefab);
        newNPC.GetComponent<NPCSelector>().NPC_ID = newNPCSheet.sheetID;
        newNPC.GetComponent<RectTransform>().SetParent(NPCListParent.GetComponent<RectTransform>());
    }
}
