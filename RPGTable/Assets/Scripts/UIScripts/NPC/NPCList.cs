using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCList : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] GameManager gameManager;

    [SerializeField] GameObject NPCListParent;
    [SerializeField] GameObject NPCSearchListParent;
    [SerializeField] GameObject NPCSelectorPrefab;

    [SerializeField] Button addNPC;

    [SerializeField] InputField searchBar;

    string lastSearch = "";

    private void Start()
    {
        addNPC.onClick.AddListener(() => AddNPC());
    }

    private void Update()
    {
        if (searchBar.text == "" && Searching())
        {
            ToggleSearchContent();
        }

        if (searchBar.text != "" && !Searching())
        {
            ToggleSearchContent();
        }

        if (searchBar.text != "" && searchBar.text != lastSearch)
        {
            lastSearch = searchBar.text;
            FilterNPCByName(searchBar.text);
        }
    }

    private void AddNPC()
    {
        NPCSheetInfo newNPCSheet = new NPCSheetInfo();
        newNPCSheet.sheetID = gameManager.GetNewNPCSheetID();
        newNPCSheet.NPCName = "New NPC";
            
        GameObject newNPC = Instantiate(NPCSelectorPrefab);
        newNPC.GetComponent<NPCSelector>().NPC_ID = newNPCSheet.sheetID;
        newNPC.GetComponent<RectTransform>().SetParent(NPCListParent.GetComponent<RectTransform>());

        uiManager.NPCSelectorList.Add(newNPC);
        gameManager.AddNewNPCSheetInfo(newNPCSheet);
    }

    private void ToggleSearchContent()
    {
        bool toggleContent = !NPCListParent.activeInHierarchy;
        bool toggleSearch = !NPCSearchListParent.activeInHierarchy;

        NPCListParent.SetActive(toggleContent);
        NPCSearchListParent.SetActive(toggleSearch);
    }

    private bool Searching()
    {
        return !NPCListParent.activeInHierarchy && NPCSearchListParent.activeInHierarchy;
    }

    public void LoadNPC(int NPC_ID, string NPCName)
    {
        GameObject savedNPC = Instantiate(NPCSelectorPrefab);
        savedNPC.GetComponent<NPCSelector>().NPC_ID = NPC_ID;
        savedNPC.GetComponent<NPCSelector>().NPCName.text = NPCName;
        savedNPC.GetComponent<RectTransform>().SetParent(NPCListParent.GetComponent<RectTransform>());

        uiManager.NPCSelectorList.Add(savedNPC);
    }

    public void FilterNPCByName(string nameFilter)
    {
        foreach (GameObject NPC in uiManager.NPCSearchSelectorList)
        {
            Destroy(NPC);
        }
        uiManager.NPCSearchSelectorList.Clear();

        foreach (GameObject NPC in uiManager.NPCSelectorList)
        {
            NPCSelector _NPCSelector = NPC.GetComponent<NPCSelector>();

            if (_NPCSelector.NPCName.text.ToLower().Contains(nameFilter.ToLower()))
            {
                GameObject searchNPCSelector = Instantiate(NPCSelectorPrefab);
                searchNPCSelector.GetComponent<NPCSelector>().NPC_ID = _NPCSelector.NPC_ID;
                searchNPCSelector.GetComponent<NPCSelector>().NPCName.text = _NPCSelector.NPCName.text;

                searchNPCSelector.GetComponent<RectTransform>().SetParent(NPCSearchListParent.GetComponent<RectTransform>());

                uiManager.NPCSearchSelectorList.Add(searchNPCSelector);
            }      
        }
    }
}
