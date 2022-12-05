using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] LevelEditorManager levelEditorManager;
    List<CharacterSheetInfo> characterSheets;

    [SerializeField] GameObject d4Prefab;
    [SerializeField] GameObject d6Prefab;
    [SerializeField] GameObject d8Prefab;
    [SerializeField] GameObject d10Prefab;
    [SerializeField] GameObject pdPrefab;
    [SerializeField] GameObject d12Prefab;
    [SerializeField] GameObject d20Prefab;

    public List<GameObject> currentLevel;
    Dictionary<string, List<levelItemInfo>> savedLevels;

    private void Start()
    {
        characterSheets = new List<CharacterSheetInfo>();
        currentLevel = new List<GameObject>();
        savedLevels = new Dictionary<string, List<levelItemInfo>>();
    }

    public void RollDice(diceType type, Vector3 position, string thrownBy, int modifier)
    {
        position.y -= 0.5f;

        switch (type)
        {
            case diceType.d4:
                GameObject d4Dice = Instantiate(d4Prefab, position, Quaternion.identity);
                d4Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d4Dice.GetComponent<Dice>().modifier = modifier;
                d4Dice.GetComponent<Dice>().line = "[D4]:";
                d4Dice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.d6:
                GameObject d6Dice = Instantiate(d6Prefab, position, Quaternion.identity);
                d6Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d6Dice.GetComponent<Dice>().modifier = modifier;
                d6Dice.GetComponent<Dice>().line = "[D6]:";
                d6Dice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.d8:
                GameObject d8Dice = Instantiate(d8Prefab, position, Quaternion.identity);
                d8Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d8Dice.GetComponent<Dice>().modifier = modifier;
                d8Dice.GetComponent<Dice>().line = "[D8]:";
                d8Dice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.d10:
                GameObject d10Dice = Instantiate(d10Prefab, position, Quaternion.identity);
                d10Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d10Dice.GetComponent<Dice>().modifier = modifier;
                d10Dice.GetComponent<Dice>().line = "[D10]:";
                d10Dice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.pd:
                GameObject pdDice = Instantiate(pdPrefab, position, Quaternion.identity);
                pdDice.GetComponent<Dice>().thrownBy = thrownBy;
                pdDice.GetComponent<Dice>().modifier = modifier;
                pdDice.GetComponent<Dice>().line = "[Percentile]:";
                pdDice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.d12:
                GameObject d12Dice = Instantiate(d12Prefab, position, Quaternion.identity);
                d12Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d12Dice.GetComponent<Dice>().modifier = modifier;
                d12Dice.GetComponent<Dice>().line = "[D12]:";
                d12Dice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.d20:
                GameObject d20Dice = Instantiate(d20Prefab, position, Quaternion.identity);
                d20Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d20Dice.GetComponent<Dice>().modifier = modifier;
                d20Dice.GetComponent<Dice>().line = "[D20]:";
                d20Dice.GetComponent<NetworkObject>().Spawn();
                break;
        }
    }

    public void RollDice(diceType type, Vector3 position, string thrownBy, int modifier, string line)
    {
        position.y -= 0.5f;

        switch (type)
        {
            case diceType.d4:
                GameObject d4Dice = Instantiate(d4Prefab, position, Quaternion.identity);
                d4Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d4Dice.GetComponent<Dice>().modifier = modifier;
                d4Dice.GetComponent<Dice>().line = line;
                d4Dice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.d6:
                GameObject d6Dice = Instantiate(d6Prefab, position, Quaternion.identity);
                d6Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d6Dice.GetComponent<Dice>().modifier = modifier;
                d6Dice.GetComponent<Dice>().line = line;
                d6Dice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.d8:
                GameObject d8Dice = Instantiate(d8Prefab, position, Quaternion.identity);
                d8Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d8Dice.GetComponent<Dice>().modifier = modifier;
                d8Dice.GetComponent<Dice>().line = line;
                d8Dice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.d10:
                GameObject d10Dice = Instantiate(d10Prefab, position, Quaternion.identity);
                d10Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d10Dice.GetComponent<Dice>().modifier = modifier;
                d10Dice.GetComponent<Dice>().line = line;
                d10Dice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.pd:
                GameObject pdDice = Instantiate(pdPrefab, position, Quaternion.identity);
                pdDice.GetComponent<Dice>().thrownBy = thrownBy;
                pdDice.GetComponent<Dice>().modifier = modifier;
                pdDice.GetComponent<Dice>().line = line;
                pdDice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.d12:
                GameObject d12Dice = Instantiate(d12Prefab, position, Quaternion.identity);
                d12Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d12Dice.GetComponent<Dice>().modifier = modifier;
                d12Dice.GetComponent<Dice>().line = line;
                d12Dice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.d20:
                GameObject d20Dice = Instantiate(d20Prefab, position, Quaternion.identity);
                d20Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d20Dice.GetComponent<Dice>().modifier = modifier;
                d20Dice.GetComponent<Dice>().line = line;
                d20Dice.GetComponent<NetworkObject>().Spawn();
                break;
        }
    }

    public CharacterSheetInfo GetSheetInfo(int index)
    {
        return characterSheets[index];
    }

    public void AddNewCharacterSheetInfo(CharacterSheetInfo charInfo)
    {
        UpdateSheetListClientRpc(charInfo);
        uiManager.AddCharacterButtonClientRpc(charInfo.sheetID, charInfo.characterName);
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

    void SaveItemInfo(string levelName, int id, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        levelItemInfo itemInfo;
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
            savedLevels.Add(levelName, new List<levelItemInfo>());
            newSave = true;
        }
        else
        {
            savedLevels[levelName] = new List<levelItemInfo>();
        }      

        foreach (GameObject item in currentLevel)
        {
            if (item == null) { continue; }

            SaveItemInfo(levelName, item.GetComponent<LevelItemHandler>().id, item.transform.position, item.transform.rotation.eulerAngles, item.transform.localScale);
        }

        return newSave;
    }

    public void LoadLevel(string levelName)
    {
        foreach (GameObject item in currentLevel)
        {
            if (item == null) { continue; }
            item.GetComponent<NetworkObject>().Despawn();
        }
        currentLevel.Clear();

        foreach (levelItemInfo itemInfo in savedLevels[levelName])
        {
            levelEditorManager.SpawnLevelItem(itemInfo);
        }
    }

    public int GetLevelNumber()
    {
        return savedLevels.Count;
    }
    #region ServerRpc

    [ServerRpc (RequireOwnership = false)]
    public void AddSavedCharactersServerRpc(ulong clientID)
    {
        List<CharacterSheetInfo> charSheets = characterSheets;

        foreach (CharacterSheetInfo charInfo in charSheets)
        {
            AddSavedCharactersClientRpc(clientID, charInfo);
        }
    }


    [ServerRpc (RequireOwnership = false)]
    void SaveCharacterSheetChangesServerRpc(CharacterSheetInfo charInfo)
    {
        characterSheets[charInfo.sheetID] = charInfo;
        SaveCharacterSheetChangesClientRpc(charInfo);
    }
    #endregion

    #region ClientRpc

    [ClientRpc]
    public void AddSavedCharactersClientRpc(ulong clientID, CharacterSheetInfo charInfo)
    {
        if (NetworkManager.Singleton.LocalClientId != clientID || IsServer) { return; }
       
        characterSheets.Add(charInfo);
        uiManager.AddCharacterButton(charInfo.sheetID, charInfo.characterName);     
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
