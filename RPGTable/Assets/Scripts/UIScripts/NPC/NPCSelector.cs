using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSelector : MonoBehaviour
{
    UIManager uiManager;
    GameManager gameManager;
    TokenManager tokenManager;
    Canvas canvas;

    [SerializeField] Button openNPCSheet;
    [SerializeField] Button spawnNPCToken;
    [SerializeField] Button deleteNPC;
    public Text NPCName;

    public int NPC_ID;

    [SerializeField] GameObject NPCSheetPrefab;
    private int NPCCounter;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
        tokenManager = FindObjectOfType<TokenManager>();
        canvas = FindObjectOfType<Canvas>();

        NPCSheetInfo NPCInfo = gameManager.GetNPCSheetInfo(NPC_ID);

        openNPCSheet.onClick.AddListener(() => OpenNPCSheet());
        spawnNPCToken.onClick.AddListener(() => SpawnToken(0, "", NPCInfo.avatarID, NPCInfo.GetCopy()));
        if (gameManager.GetNPCSheetInfo(NPC_ID).avatarID > 0)
        {
            deleteNPC.enabled = false;
        }
        else
        {
            deleteNPC.onClick.AddListener(() => DeleteNPC());
        }   
    }

    void OpenNPCSheet()
    {
        NPCSheetInfo NPCInfo = gameManager.GetNPCSheetInfo(NPC_ID);
        if (NPCInfo == null) { return; }
        GameObject NPCSheet = Instantiate(NPCSheetPrefab);
        NPCSheet.GetComponent<NPCSheetManager>().NPCInfo = NPCInfo;
        NPCSheet.GetComponent<RectTransform>().SetParent(canvas.gameObject.transform, false);
    }

    void SpawnToken(ulong ownerID, string ownerName, int avatarID, NPCSheetInfo _NPCSheetInfo)
    {
        NPCCounter++;
        _NPCSheetInfo.NPCName += " " + NPCCounter;

        tokenManager.DragNPCToken(ownerName, avatarID, _NPCSheetInfo);
    }

    void DeleteNPC()
    {
        GameObject selector = uiManager.NPCSelectorList[NPC_ID];
        uiManager.RemoveNPCButtonFromList(NPC_ID);
        gameManager.DeleteNPC(NPC_ID);

        if (selector != this.gameObject)
        {
            Destroy(selector);
        }
        Destroy(this.gameObject);
    }

    public void ReduceNPCCounter()
    {
        if (NPCCounter <= 0) { return; }
        NPCCounter--;
    }
}
