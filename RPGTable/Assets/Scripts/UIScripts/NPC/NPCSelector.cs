using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSelector : MonoBehaviour
{
    UIManager uIManager;
    GameManager gameManager;
    Canvas canvas;

    [SerializeField] Button openNPCSheet;
    public Text NPCName;

    public int NPC_ID;

    [SerializeField] GameObject NPCSheetPrefab;

    void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
        canvas = FindObjectOfType<Canvas>();

        openNPCSheet.onClick.AddListener(() => OpenNPCSheet());
    }

    void OpenNPCSheet()
    {
        NPCSheetInfo NPCInfo = gameManager.GetNPCSheetInfo(NPC_ID);
        if (NPCInfo == null) { return; }
        GameObject NPCSheet = Instantiate(NPCSheetPrefab);
        NPCSheet.GetComponent<NPCSheetManager>().NPCInfo = NPCInfo;
        NPCSheet.GetComponent<RectTransform>().SetParent(canvas.gameObject.transform, false);
    }

}
