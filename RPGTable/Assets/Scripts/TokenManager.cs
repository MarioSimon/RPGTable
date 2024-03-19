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

    public GameObject[] npcAvatarList;

    public RuntimeAnimatorController[] weaponAnimationList;

    private GameObject token;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && token != null)
        {
            if (IsPlayer())
            {
                SpawnPlayerToken();
            }              
            else
            {
                SpawnNPCToken();
            }
        }

        if (Input.GetMouseButtonDown(1) && token != null)
        {
            CancelSpawnToken();
        }
    }
    private void CancelSpawnToken()
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
            gameManager.SpawnPlayerToken(characterSheetInfo.ownerID, characterSheetInfo.publicPageCharacterInfo.playerName, characterSheetInfo.publicPageCharacterInfo.avatarID, worldPosition, characterSheetInfo);          
        }
        else
        {
            SpawnPlayerTokenServerRpc(worldPosition, characterSheetInfo);
        }
    }

    private void SpawnNPCToken()
    {
        if (!IsHost) { return; }

        Vector3 worldPosition = GetTablePoint();
        if (worldPosition == new Vector3(-99, -99, -99)) { return; }

        NPCSheetInfo npcSheetInfo = token.GetComponent<TokenController>().NPCSheetInfo;
        Destroy(GameObject.FindGameObjectWithTag("ItemImage"));

        if (IsHost)
        {
            gameManager.SpawnNPCToken(0, "", npcSheetInfo.avatarID, worldPosition, npcSheetInfo);
        }
        else
        {
            SpawnNPCTokenServerRpc(worldPosition, npcSheetInfo);
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
        token.GetComponent<TokenController>().tokenType = tokenType.PC;
        token.AddComponent<ItemPreviewFollow>();
        token.tag = "ItemImage";
    }

    public void DragNPCToken(string ownerName, int avatarID, NPCSheetInfo npcSheetInfo)
    {
        token = Instantiate(npcAvatarList[avatarID], new Vector3(-99, -99, -99), Quaternion.identity);
        token.GetComponent<TokenController>().ownerName.Value = new FixedString64Bytes(ownerName);
        token.GetComponent<TokenController>().NPCSheetInfo = npcSheetInfo;
        token.GetComponent<TokenController>().enabled = false;
        token.AddComponent<ItemPreviewFollow>();
        token.tag = "ItemImage";
    }

    private bool IsPlayer()
    {
        if (token == null)
        {
            return false;
        }

        TokenController controller = token.GetComponent<TokenController>();

        if (controller != null && controller.tokenType == tokenType.PC)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SwitchWeaponAndAttack(TokenWeaponInfo tokenWeaponInfo)
    {
        if (tokenWeaponInfo.tokenType == tokenType.PC)
        {
            TokenController[] activeTokens = FindObjectsOfType<TokenController>();
            TokenController token = null;
            for (int i = 0; i < activeTokens.Length; i++)
            {
                if (activeTokens[i].characterSheetInfo.sheetID == tokenWeaponInfo.tokenID)
                {
                    token = activeTokens[i];
                    break;
                }
            }
            if (token != null)
            {
                if (tokenWeaponInfo.weaponID == -1)
                {
                    token.TriggerMagicAnimation();
                }
                else
                {
                    token.SwitchWeapon(weaponAnimationList[tokenWeaponInfo.weaponID], tokenWeaponInfo.weaponID);

                    if (tokenWeaponInfo.weaponID == 0)
                    {
                        token.TriggerPunchAnimation();
                    }
                    else
                    {
                        token.TriggerAttackAnimation();
                    }
                }
            }       
        }
    }
    #region ServerRPC

    [ServerRpc (RequireOwnership = false)]
    private void SpawnPlayerTokenServerRpc(Vector3 spawnPosition, CharacterSheetInfo characterSheetInfo)
    {
        gameManager.SpawnPlayerToken(characterSheetInfo.ownerID, characterSheetInfo.publicPageCharacterInfo.playerName, characterSheetInfo.publicPageCharacterInfo.avatarID, spawnPosition, characterSheetInfo);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnNPCTokenServerRpc(Vector3 spawnPosition, NPCSheetInfo npcSheetInfo)
    {
        gameManager.SpawnNPCToken(0, "", npcSheetInfo.avatarID, spawnPosition, npcSheetInfo);
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
        tokenShortcut.GetComponent<CharacterShortcut>().characterName.text = characterSheetInfo.publicPageCharacterInfo.characterName;
        tokenShortcut.GetComponent<CharacterShortcut>().characterPortrait.sprite = playerAvatarPortrait[characterSheetInfo.publicPageCharacterInfo.avatarID];
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

public struct TokenWeaponInfo : INetworkSerializable
{
    public int tokenID;
    public string tokenName;
    public tokenType tokenType;
    public int weaponID;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref tokenID);
        serializer.SerializeValue(ref tokenName);
        serializer.SerializeValue(ref tokenType);
        serializer.SerializeValue(ref weaponID);
    }
}