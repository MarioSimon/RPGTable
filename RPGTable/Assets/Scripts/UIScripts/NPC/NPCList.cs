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
        newNPCSheet.NPCName = "New NPC";
            
        GameObject newNPC = Instantiate(NPCSelectorPrefab);
        newNPC.GetComponent<NPCSelector>().NPC_ID = newNPCSheet.sheetID;
        newNPC.GetComponent<RectTransform>().SetParent(NPCListParent.GetComponent<RectTransform>());

        uIManager.NPCList.Add(newNPC);
        gameManager.AddNewNPCSheetInfo(newNPCSheet);
    }

    public void LoadNPC(int NPC_ID, string NPCName)
    {
        GameObject savedNPC = Instantiate(NPCSelectorPrefab);
        savedNPC.GetComponent<NPCSelector>().NPC_ID = NPC_ID;
        savedNPC.GetComponent<NPCSelector>().NPCName.text = NPCName;
        savedNPC.GetComponent<RectTransform>().SetParent(NPCListParent.GetComponent<RectTransform>());

        uIManager.NPCList.Add(savedNPC);
    }
}
