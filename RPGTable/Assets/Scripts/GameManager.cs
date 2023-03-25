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
    [SerializeField] List<CharacterSheetInfo> characterSheets;

    [SerializeField] List<GameObject> avatarList;
    public List<Sprite> avatarPortrait;

    Dictionary<string, ulong> playerList;

    public List<GameObject> currentLevel;
    Dictionary<string, List<LevelItemInfo>> savedLevels;

    #endregion

    private void Start()
    {
        characterSheets = new List<CharacterSheetInfo>();
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

    public void SpawnToken(ulong ownerID, string ownerName, int avatarID, CharacterSheetInfo characterSheetInfo)
    {
        if (IsHost)
        {
            GameObject token = Instantiate(avatarList[avatarID], Vector3.zero, Quaternion.identity);
            //token.transform.position += Vector3.down * 1.5f; //prevents spawning in the air

            TokenController tokenController = token.GetComponent<TokenController>();
            tokenController.ownerName.Value = new FixedString64Bytes(ownerName);
            tokenController.characterSheetInfo = characterSheetInfo;
            token.GetComponent<NetworkObject>().SpawnWithOwnership(ownerID);
        }
        else
        {
            SpawnTokenServerRpc(ownerID, ownerName, avatarID, characterSheetInfo);
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
        }            
    }

    public int GetNewSheetID()
    {
        return characterSheets.Count;
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

        Debug.Log("SAVED LEVELS AT " + Application.dataPath + "/StreamingAssets/levels.json");
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
        Debug.Log("LOADED LEVELS FROM " + Application.dataPath + "/StreamingAssets/levels.json");

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
    public void SpawnTokenServerRpc(ulong ownerID, string ownerName, int avatarID, CharacterSheetInfo characterSheetInfo)
    {
        if (!IsHost) { return; }

        SpawnToken(ownerID, ownerName, avatarID, characterSheetInfo);
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
    void UpdateSheetListClientRpc(CharacterSheetInfo charInfo)
    {
        characterSheets.Add(charInfo);
    }

    [ClientRpc]
    void SaveCharacterSheetChangesClientRpc(CharacterSheetInfo charInfo)
    {
        characterSheets[charInfo.sheetID] = charInfo;
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