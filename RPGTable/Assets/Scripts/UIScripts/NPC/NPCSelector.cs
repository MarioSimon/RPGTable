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
    [SerializeField] Button spawnNPCToken;
    [SerializeField] Button deleteNPC;
    public Text NPCName;

    public int NPC_ID;

    [SerializeField] GameObject NPCSheetPrefab;
    private int NPCCounter;

    void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
        canvas = FindObjectOfType<Canvas>();

        NPCSheetInfo NPCInfo = gameManager.GetNPCSheetInfo(NPC_ID);

        openNPCSheet.onClick.AddListener(() => OpenNPCSheet());
        spawnNPCToken.onClick.AddListener(() => SpawnToken(0, "", NPCInfo.avatarID, NPCInfo.GetCopy()));
        deleteNPC.onClick.AddListener(() => DeleteNPC());
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

        gameManager.SpawnNPCToken(ownerID, ownerName, avatarID, _NPCSheetInfo);
    }

    void DeleteNPC()
    {
        GameObject selector = uIManager.NPCSelectorList[NPC_ID];
        uIManager.RemoveNPCButtonFromList(NPC_ID);
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
