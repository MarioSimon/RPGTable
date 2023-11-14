using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    #region variables

    [SerializeField] UIManager uiManager;
    [SerializeField] LevelEditorManager levelEditorManager;

    List<CharacterSheetInfo> characterSheets;
    [SerializeField] List<GameObject> playerAvatarList;  
    public List<Sprite> playerAvatarPortrait;

    List<NPCSheetInfo> NPCSheets;
    [SerializeField] List<GameObject> NPCAvatarList;

    [SerializeField] GameObject tokenShortcutPrefab;

    Dictionary<string, ulong> playerList;

    public List<GameObject> currentLevel;
    Dictionary<string, List<LevelItemInfo>> savedLevels;

    #endregion

    private void Start()
    {
        characterSheets = new List<CharacterSheetInfo>();
        NPCSheets = new List<NPCSheetInfo>();

        playerList = new Dictionary<string, ulong>();

        currentLevel = new List<GameObject>();
        savedLevels = new Dictionary<string, List<LevelItemInfo>>();
    }

    #region player management

    public void AddPlayerToList(string playerName, ulong playerID)
    {
        if (playerList.ContainsKey(playerName)) { return; }

        playerList.Add(playerName, playerID);
    }

    public ulong GetPlayerId(string playerName)
    {
        if (!playerList.ContainsKey(playerName)) { return 0; }

        return playerList[playerName];
    }

    #endregion

    #region tokens

    public void SpawnPlayerToken(ulong ownerID, string ownerName, int avatarID, CharacterSheetInfo characterSheetInfo)
    {
        if (IsHost)
        {
            GameObject token = Instantiate(playerAvatarList[avatarID], Vector3.zero, Quaternion.identity);

            TokenController tokenController = token.GetComponent<TokenController>();
            tokenController.ownerName.Value = new FixedString64Bytes(ownerName);
            tokenController.tokenType = tokenType.PC;
            tokenController.characterSheetInfo = characterSheetInfo;
            token.GetComponent<NetworkObject>().SpawnWithOwnership(ownerID);

            SpawnTokenShortcutClientRpc(characterSheetInfo);
            AddToInitiativeTrackerClientRpc(characterSheetInfo.characterName);
        }
        else
        {
            SpawnPlayerTokenServerRpc(ownerID, ownerName, avatarID, characterSheetInfo);
        }
    }

    public void SpawnNPCToken(ulong ownerID, string ownerName, int avatarID, NPCSheetInfo _NPCSheetInfo)
    {
        if (IsHost)
        {
            GameObject token = Instantiate(NPCAvatarList[avatarID], Vector3.zero, Quaternion.identity);

            TokenController tokenController = token.GetComponent<TokenController>();
            tokenController.ownerName.Value = new FixedString64Bytes(ownerName);
            _NPCSheetInfo.sheetID = -1;
            tokenController.tokenType = tokenType.NPC;
            tokenController.NPCSheetInfo = _NPCSheetInfo;
            token.transform.localScale = GetTokenScale(_NPCSheetInfo.NPCSize);
            token.GetComponent<NetworkObject>().SpawnWithOwnership(ownerID);

            AddToInitiativeTrackerClientRpc(_NPCSheetInfo.NPCName);
        }
        else
        {
            SpawnNPCTokenServerRpc(ownerID, ownerName, avatarID, _NPCSheetInfo);
        }
    }

    public IEnumerator LoadActiveTokenShortcut()
    {
        yield return new WaitForSeconds(1);

        LoadActiveTokenShortcutsServerRpc();
    }

    public IEnumerator LoadCurrentInitiativeOrder()
    {
        yield return new WaitForSeconds(1);

        LoadCurrentInitiativeOrderServerRpc();
    }

    Vector3 GetTokenScale(int tokenSize)
    {
        Vector3 tokenScale = Vector3.one;

        switch (tokenSize)
        {
            case 0:
                break;
            case 1:
                tokenScale *= 0.5f;
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                tokenScale *= 2f;
                break;
            case 5:
                tokenScale *= 3f;
                break;
            case 6:
                tokenScale *= 4f;
                break;
        }

        return tokenScale;
    }

    public void RemoveFromInitiativeTracker(string characterName)
    {
        RemoveFromInitiativeTrackerClientRpc(characterName);
    }

    public void SetInitiative(string name, int value)
    {
        if (IsHost)
        {
            SetInitiativeClientRpc(name, value);
        }
        else
        {
            SetInitiativeServerRpc(name, value);
        }
    }

    #endregion

    #region NPC sheets

    public NPCSheetInfo GetNPCSheetInfo(int index)
    {
        return NPCSheets[index];
    }

    public void AddNewNPCSheetInfo(NPCSheetInfo NPCInfo)
    {
        if (!IsHost) { return; }

        NPCSheets.Add(NPCInfo);
        SaveNPCsToJSON();
    }

    public int GetNewNPCSheetID()
    {
        return NPCSheets.Count;
    }

    public void SaveNPCSheetChanges(NPCSheetInfo NPCInfo)
    {
        if (NPCInfo.sheetID < 0) { return; }

        NPCSheets[NPCInfo.sheetID] = NPCInfo;
        SaveNPCsToJSON();
        uiManager.UpdateNPCButtonName(NPCInfo.sheetID, NPCInfo.NPCName);
    }

    public void DeleteNPC(int NPC_ID)
    {
        if (NPC_ID > NPCSheets.Count || NPC_ID < 0) { return; }

        NPCSheets.RemoveAt(NPC_ID);
        UpdateNPCIDs();

        SaveNPCsToJSON();
    }

    private void UpdateNPCIDs()
    {
        for (int i = 0; i < NPCSheets.Count; i++)
        {
            NPCSheets[i].sheetID = i;
            uiManager.UpdateNPCButtonID(i);
        }
    }

    public void SaveNPCsToJSON()
    {
        if (NPCSheets.Count < 1) { return; }

        SerializableList<NPCSheetInfo> savedNPCInfo = new SerializableList<NPCSheetInfo>();

        foreach (NPCSheetInfo NPCInfo in NPCSheets)
        {
            savedNPCInfo.list.Add(NPCInfo);
        }

        string json = JsonUtility.ToJson(savedNPCInfo);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/npcs.json", json);
    }

    public IEnumerator LoadNPCsFromJSON()
    {
        yield return new WaitForSeconds(1);


        if (File.Exists(Application.dataPath + "/StreamingAssets/npcs.json")) 
        { 

            string jsonString = File.ReadAllText(Application.dataPath + "/StreamingAssets/npcs.json");
            SerializableList<NPCSheetInfo> savedNPCInfo = JsonUtility.FromJson<SerializableList<NPCSheetInfo>>(jsonString);

            foreach (NPCSheetInfo NPCInfo in savedNPCInfo.list)
            {
                AddNewNPCSheetInfo(NPCInfo);
            }

            uiManager.LoadSavedNPCs(NPCSheets);
        }
    }

    #endregion

    #region character sheets

    public CharacterSheetInfo GetSheetInfo(int index)
    {
        return characterSheets[index];
    }

    public void AddNewCharacterSheetInfo(CharacterSheetInfo charInfo)
    {
        UpdateSheetListClientRpc(charInfo);
        SaveCharactersToJSON();
        uiManager.AddCharacterButtonClientRpc(charInfo.sheetID, charInfo.characterName, charInfo.avatarID);
    }

    public void SaveCharacterSheetChanges(CharacterSheetInfo charInfo)
    {        
        if (!IsServer) 
        {
            SaveCharacterSheetChangesServerRpc(charInfo); 
        } else
        {
            SaveCharacterSheetChangesClientRpc(charInfo);
            SaveCharactersToJSON();
        }            
    }

    public int GetNewSheetID()
    {
        return characterSheets.Count;
    }

    public void DeleteCharacter(int characterID)
    {
        if (!IsHost)
        {
            if (NetworkManager.LocalClientId == characterSheets[characterID].ownerID)
            {
                DeleteCharacterServerRpc(characterID);
            }            
        }
        else
        {
            if (characterID > characterSheets.Count || characterID < 0) { return; }

            characterSheets.RemoveAt(characterID);
            uiManager.RemoveCharacterButton(characterID);
            UpdateCharacterIDs();
            SaveCharactersToJSON();

            DeleteCharacterClientRpc(characterID);
        }
    }

    private void UpdateCharacterIDs()
    {
        for (int i = 0; i < characterSheets.Count; i++)
        {
            characterSheets[i].sheetID = i;
            uiManager.UpdateCharacterButtonID(i);
        }
    }

    public void SaveCharactersToJSON()
    {
        if (characterSheets.Count < 1) { return; }

        SerializableList<CharacterSheetInfo> savedCharactersInfo = new SerializableList<CharacterSheetInfo>();

        foreach (CharacterSheetInfo CSInfo in characterSheets)
        {
            savedCharactersInfo.list.Add(CSInfo);
        }

        string json = JsonUtility.ToJson(savedCharactersInfo);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/player characters.json", json);
    }

    public IEnumerator LoadCharactersFromJSON()
    {
        yield return new WaitForSeconds(1);

        if (File.Exists(Application.dataPath + "/StreamingAssets/player characters.json"))
        {
            string jsonString = File.ReadAllText(Application.dataPath + "/StreamingAssets/player characters.json");
            SerializableList<CharacterSheetInfo> savedCharactersInfo = JsonUtility.FromJson<SerializableList<CharacterSheetInfo>>(jsonString);

            foreach (CharacterSheetInfo CSInfo in savedCharactersInfo.list)
            {
                AddNewCharacterSheetInfo(CSInfo);
            }
        }     
    }

    #endregion

    #region levels

    void SaveItemInfo(string levelName, int id, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        LevelItemInfo itemInfo;
        itemInfo.itemID = id;
        itemInfo.itemPosition = position;
        itemInfo.itemRotation = rotation;
        itemInfo.itemScale = scale;

        savedLevels[levelName].Add(itemInfo);
    }

    public bool SaveLevel(string levelName)
    {
        bool newSave = false;
        if (!savedLevels.ContainsKey(levelName))
        {
            savedLevels.Add(levelName, new List<LevelItemInfo>());
            newSave = true;
        }
        else
        {
            savedLevels[levelName] = new List<LevelItemInfo>();
        }      

        foreach (GameObject item in currentLevel)
        {
            if (item == null) { continue; }

            SaveItemInfo(levelName, item.GetComponent<LevelItemHandler>().id, item.transform.position, item.transform.rotation.eulerAngles, item.transform.localScale);
        }

        return newSave;
    }

    public bool DeleteLevel(string levelName)
    {
        if (!savedLevels.ContainsKey(levelName))
        {
            return false;
        }

        savedLevels.Remove(levelName);
        return true;
    }

    public void LoadLevel(string levelName)
    {
        if (!savedLevels.ContainsKey(levelName))
        {
            Debug.Log("Error: selected level doesn't exist");
            return;
        }

        foreach (GameObject item in currentLevel)
        {
            if (item == null) { continue; }
            item.GetComponent<NetworkObject>().Despawn();
        }
        currentLevel.Clear();

        foreach (LevelItemInfo itemInfo in savedLevels[levelName])
        {
            levelEditorManager.SpawnLevelItem(itemInfo);
        }
    }

    public List<LevelItemInfo> GetLevelState()
    {
        List<LevelItemInfo> state = new List<LevelItemInfo>();

        foreach (GameObject item in currentLevel)
        {
            if (item == null) { continue; }

            LevelItemInfo itemInfo;
            itemInfo.itemID = item.GetComponent<LevelItemHandler>().id;
            itemInfo.itemPosition = item.transform.position;
            itemInfo.itemRotation = item.transform.rotation.eulerAngles;
            itemInfo.itemScale = item.transform.localScale;

            state.Add(itemInfo);
        }

        return state;
    }

    public void LoadLevelState(List<LevelItemInfo> levelState)
    {
        foreach (GameObject item in currentLevel)
        {
            if (item == null) { continue; }
            item.GetComponent<NetworkObject>().Despawn();
        }
        currentLevel.Clear();

        foreach (LevelItemInfo itemInfo in levelState)
        {
            levelEditorManager.SpawnLevelItem(itemInfo);
        }
    }

    public int GetLevelNumber()
    {
        return savedLevels.Count;
    }

    public void SaveLevelsToJSON()
    {
        if (savedLevels.Count < 1) { return; }

        SerializableList<SavedLevelParams> savedLevelsInfo = new SerializableList<SavedLevelParams>();

        foreach (string level in savedLevels.Keys)
        {
            SavedLevelParams levelParams;
            levelParams.levelItems = new SerializableList<LevelItemInfo>();

            levelParams.levelName = level;
            levelParams.levelItems.list = savedLevels[level];

            savedLevelsInfo.list.Add(levelParams);
        }

        string json = JsonUtility.ToJson(savedLevelsInfo);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/levels.json", json);

        //Debug.Log("SAVED LEVELS AT " + Application.dataPath + "/StreamingAssets/levels.json");
    }

    public List<string> LoadLevelsFromJSON()
    {
        string jsonString = File.ReadAllText(Application.dataPath + "/StreamingAssets/levels.json");
        SerializableList<SavedLevelParams> savedLevelsInfo = JsonUtility.FromJson<SerializableList<SavedLevelParams>>(jsonString);

        List<string> newLevels = new List<string>();

        foreach (SavedLevelParams levelInfo in savedLevelsInfo.list)
        {
            string levelName = levelInfo.levelName;
            List<LevelItemInfo> levelItems = levelInfo.levelItems.list;

            if (savedLevels.ContainsKey(levelName))
            {
                savedLevels[levelName] = levelItems;
            }
            else
            {
                savedLevels.Add(levelName, levelItems);
                newLevels.Add(levelName);
            }            
        }
        //Debug.Log("LOADED LEVELS FROM " + Application.dataPath + "/StreamingAssets/levels.json");

        return newLevels;
    }

    #endregion

    #region ServerRpc

    [ServerRpc(RequireOwnership = false)]
    public void AddPlayerToListServerRpc(string playerName, ulong playerID)
    {
        AddPlayerToList(playerName, playerID);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerTokenServerRpc(ulong ownerID, string ownerName, int avatarID, CharacterSheetInfo characterSheetInfo)
    {
        if (!IsHost) { return; }

        SpawnPlayerToken(ownerID, ownerName, avatarID, characterSheetInfo);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnNPCTokenServerRpc(ulong ownerID, string ownerName, int avatarID, NPCSheetInfo NPCSheetInfo)
    {
        if (!IsHost) { return; }

        SpawnNPCToken(ownerID, ownerName, avatarID, NPCSheetInfo);
    }

    [ServerRpc (RequireOwnership = false)]
    public void AddSavedCharactersServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            List<CharacterSheetInfo> charSheets = characterSheets;

            foreach (CharacterSheetInfo charInfo in charSheets)
            {
                AddSavedCharactersClientRpc(charInfo, clientRpcParams);
            }
        }        
    }

    [ServerRpc (RequireOwnership = false)]
    private void SaveCharacterSheetChangesServerRpc(CharacterSheetInfo charInfo)
    {
        characterSheets[charInfo.sheetID] = charInfo;
        SaveCharacterSheetChangesClientRpc(charInfo);
    }

    [ServerRpc (RequireOwnership = false)]
    private void DeleteCharacterServerRpc(int characterID)
    {
        if (characterID > characterSheets.Count || characterID < 0) { return; }

        characterSheets.RemoveAt(characterID);
        uiManager.RemoveCharacterButton(characterID);
        UpdateCharacterIDs();
        SaveCharactersToJSON();

        DeleteCharacterClientRpc(characterID);
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoadActiveTokenShortcutsServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            CharacterShortcut[] tokenShortcuts = FindObjectsOfType<CharacterShortcut>();

            foreach (CharacterShortcut cs in tokenShortcuts)
            {
                SpawnTokenShortcutClientRpc(GetSheetInfo(cs.charID), clientRpcParams);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void LoadCurrentInitiativeOrderServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };
        }

        InitiativeItem[] initiativeItems = FindObjectsOfType<InitiativeItem>();

        foreach (InitiativeItem item in initiativeItems)
        {
            int initiative = 0;
            int.TryParse(item.tokenInitiative.text, out initiative);

            AddToInitiativeTrackerClientRpc(item.tokenName.text, initiative);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetInitiativeServerRpc(string name, int value)
    {
        SetInitiative(name, value);
    }

    #endregion

    #region ClientRpc

    [ClientRpc]
    public void AddSavedCharactersClientRpc(CharacterSheetInfo charInfo, ClientRpcParams clientRpcParams)
    {
        if (IsHost) { return; }
       
        characterSheets.Add(charInfo);
        uiManager.AddCharacterButton(charInfo.sheetID, charInfo.characterName, charInfo.avatarID);     
    }

    [ClientRpc]
    private void UpdateSheetListClientRpc(CharacterSheetInfo charInfo)
    {
        characterSheets.Add(charInfo);
    }

    [ClientRpc]
    private void SaveCharacterSheetChangesClientRpc(CharacterSheetInfo charInfo)
    {
        characterSheets[charInfo.sheetID] = charInfo;
    }

    [ClientRpc]
    private void DeleteCharacterClientRpc(int characterID)
    {
        if (IsHost) { return; }
        if (characterID > characterSheets.Count || characterID < 0) { return; }

        characterSheets.RemoveAt(characterID);
        uiManager.RemoveCharacterButton(characterID);
        UpdateCharacterIDs();
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

    [ClientRpc]
    private void RemoveFromInitiativeTrackerClientRpc(string characterName)
    {
        uiManager.RemoveFromInitiativeTracker(characterName);
    }
    
    [ClientRpc]
    private void SetInitiativeClientRpc(string name, int value)
    {
        uiManager.SetInitiative(name, value);
    }
    
    #endregion
}

[Serializable]
struct SavedLevelParams
{
    public string levelName;
    public SerializableList<LevelItemInfo> levelItems;
}

[Serializable]
public class SerializableList<T>
{
    public List<T> list = new List<T>();
}