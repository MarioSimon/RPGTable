using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class TokenManager : NetworkBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager uiManager;

    public GameObject[] playerAvatarList;
    public List<Sprite> playerAvatarPortrait;
    public GameObject tokenShortcutPrefab;

    private GameObject token;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && token != null)
        {
            SpawnPlayerToken();
        }

        if (Input.GetMouseButtonDown(1) && token != null)
        {
            CancelSpawnPlayerToken();
        }
    }
    private void CancelSpawnPlayerToken()
    {
        Destroy(token);
        token = null;
    }

    private void SpawnPlayerToken()
    {
        Vector3 worldPosition = GetTablePoint();
        if (worldPosition == new Vector3(-99, -99, -99)) { return; }

        CharacterSheetInfo characterSheetInfo = token.GetComponent<TokenController>().characterSheetInfo;
        Destroy(GameObject.FindGameObjectWithTag("ItemImage"));

        if (IsHost)
        {


            //CharacterSheetInfo characterSheetInfo = token.GetComponent<TokenController>().characterSheetInfo;
            //tokenController.enabled = true;
            //token.GetComponent<NetworkObject>().SpawnWithOwnership(tokenController.characterSheetInfo.ownerID);
            //SetTokenInfoClientRpc(tokenController.ownerName.Value.ToString(), tokenController.characterSheetInfo);
            //
            //SpawnTokenShortcutClientRpc(tokenController.characterSheetInfo);
            //AddToInitiativeTrackerClientRpc(tokenController.characterSheetInfo.characterName);
            

            gameManager.SpawnPlayerToken(characterSheetInfo.ownerID, characterSheetInfo.playerName, characterSheetInfo.avatarID, worldPosition, characterSheetInfo);


            
        }
        else
        {
            SpawnPlayerTokenServerRpc(worldPosition, characterSheetInfo);
        }
        
    }

    private Vector3 GetTablePoint()
    {
        Ray worldPositionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPosition = new Vector3(-99, -99, -99);

        RaycastHit[] hits = Physics.RaycastAll(worldPositionRay);

        foreach (RaycastHit hit in hits)
        {
            WalkableZone target = hit.transform.GetComponent<WalkableZone>();
            if (target == null) continue;

            worldPosition = hit.point;
            break;
        }
        return worldPosition;
    }

    public void DragPlayerToken(string ownerName, int avatarID, CharacterSheetInfo characterSheetInfo)
    {
        token = Instantiate(playerAvatarList[avatarID], new Vector3(-99, -99, -99), Quaternion.identity);
        token.GetComponent<TokenController>().ownerName.Value = new FixedString64Bytes(ownerName);
        token.GetComponent<TokenController>().characterSheetInfo = characterSheetInfo;
        token.GetComponent<TokenController>().enabled = false;
        token.AddComponent<ItemPreviewFollow>();
        token.tag = "ItemImage";
    }

    #region ServerRPC

    [ServerRpc (RequireOwnership = false)]
    private void SpawnPlayerTokenServerRpc(Vector3 spawnPosition, CharacterSheetInfo characterSheetInfo)
    {
        //SpawnPlayerToken();
        //Destroy(GameObject.FindGameObjectWithTag("ItemImage"));

        gameManager.SpawnPlayerToken(characterSheetInfo.ownerID, characterSheetInfo.playerName, characterSheetInfo.avatarID, spawnPosition, characterSheetInfo);
    }

    #endregion

    #region ClientRPC

    [ClientRpc]
    private void SetTokenInfoClientRpc(string ownerName, CharacterSheetInfo characterSheetInfo)
    {

        int tokenIndex = 0;
        TokenController[] tokenControllers = FindObjectsOfType<TokenController>();
        while (tokenControllers[tokenIndex].characterSheetInfo.sheetID < -1)
        {
            tokenIndex++;
        }

        tokenControllers[tokenIndex].tokenType = tokenType.PC;
        tokenControllers[tokenIndex].characterSheetInfo = characterSheetInfo;
    }

    [ClientRpc]
    private void SpawnTokenShortcutClientRpc(CharacterSheetInfo characterSheetInfo, ClientRpcParams clientRpcParams = default)
    {
        GameObject tokenShortcut = Instantiate(tokenShortcutPrefab);
        tokenShortcut.GetComponent<CharacterShortcut>().characterName.text = characterSheetInfo.characterName;
        tokenShortcut.GetComponent<CharacterShortcut>().characterPortrait.sprite = playerAvatarPortrait[characterSheetInfo.avatarID];
        tokenShortcut.GetComponent<CharacterShortcut>().charID = characterSheetInfo.sheetID;
        tokenShortcut.GetComponent<RectTransform>().SetParent(uiManager.GetActiveTokensParent().GetComponent<RectTransform>());
    }

    [ClientRpc]
    public void AddToInitiativeTrackerClientRpc(string name, int initiative = 0, ClientRpcParams clientRpcParams = default)
    {
        uiManager.AddToInitiativeTracker(name);

        if (initiative > 0)
        {
            uiManager.SetInitiative(name, initiative);
        }
    }




    #endregion

}
