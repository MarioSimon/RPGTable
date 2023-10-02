using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System;
using Unity.Collections;
using System.Collections.Generic;
using System.Collections;
using static UnityEngine.UI.Dropdown;

public class UIManager : NetworkBehaviour
{
    #region Variables

    [SerializeField] Canvas canvas;
    [SerializeField] NetworkManager networkManager;
    [SerializeField] GameManager gameManager;
    [SerializeField] DiceHandler diceHandler;
    UnityTransport transport;
    readonly ushort port = 7777;

    public Player localPlayer;
    [SerializeField] Text rulerDistanceText;

    [Header("Main Menu")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] public InputField inputFieldName;
    [SerializeField] InputField inputFieldIP;
    [SerializeField] Button buttonHost;
    [SerializeField] Button buttonClient;

    [Header("In Game HUD")]
    [SerializeField] GameObject inGameHUD;
    [SerializeField] Button toggleLibrary;
    [SerializeField] Button toggleCharSelector;
    [SerializeField] Button toggleDiceBox;
    [SerializeField] Button toggleDmInventory;
    [SerializeField] Button toggleNPCList;
    [SerializeField] Button toggleInitiativeTracker;

    [SerializeField] GameObject activeTokensParent;

    [Header("Dice Box")]
    [SerializeField] GameObject diceBox;
    [SerializeField] Button buttonThrowD4;
    [SerializeField] Button buttonThrowD6;
    [SerializeField] Button buttonThrowD8;
    [SerializeField] Button buttonThrowD10;
    [SerializeField] Button buttonThrowD12;
    [SerializeField] Button buttonThrowD20;
    [SerializeField] Button buttonThrowD100;

    [SerializeField] InputField diceNumber;
    [SerializeField] Toggle hideRoll;

    [SerializeField] Button minimizeDiceCam;
    [SerializeField] GameObject diceCamRender;

    [Header("Dice Registry/Chat")]
    [SerializeField] GameObject minimizedBar;
    [SerializeField] Button minimizedBarOpenChat;
    [SerializeField] Button minimizedBarOpenRegistry;
    [SerializeField] GameObject minimizedBarChatNotification;
    [SerializeField] GameObject minimizedBarRegistryNotification;

    [SerializeField] GameObject textChat;
    [SerializeField] Button textChatOpenRegistry;
    [SerializeField] Button textChatCloseChat;
    [SerializeField] GameObject textChatRegistryNotification;
    [SerializeField] GameObject textChatContent;
    [SerializeField] InputField textChatInput;
    [SerializeField] GameObject messagePrefab;

    [SerializeField] GameObject diceRegistry;
    [SerializeField] Button diceRegistryOpenChat;
    [SerializeField] Button diceRegistryCloseRegistry;
    [SerializeField] GameObject diceRegistryChatNotification;
    [SerializeField] Text diceRegistryText;
    [SerializeField] GameObject diceRegistryContent;

    [Header("Characters")]
    [SerializeField] GameObject characterSelector;
    [SerializeField] Button closeCharacterSelector;
    [SerializeField] GameObject characterListArea;
    List<GameObject> characterList = new List<GameObject>();
    [SerializeField] float characterSpace;
    [SerializeField] GameObject characterButtonPrefab;
    [SerializeField] Button toggleCharacterCreator;

    [SerializeField] GameObject characterCreator;
    [SerializeField] Button closeCharacterCreator;

    [Header("DM Inventory")]
    [SerializeField] GameObject dmInventory;
    [SerializeField] Button closeDmInventory;
    [SerializeField] Dropdown itemType;
    [SerializeField] GameObject structureItems;
    [SerializeField] GameObject floorTileItems;
    [SerializeField] GameObject decorItems;
    [SerializeField] Button openSaveLevelPanel;
    [SerializeField] Button openLoadLevelPanel;
    [SerializeField] Button saveToFile;
    [SerializeField] Button loadFromFile;

    [SerializeField] GameObject saveLevelPanel;
    [SerializeField] Button closeSaveLevelPanel;
    [SerializeField] InputField levelName;
    [SerializeField] Button saveLevel;

    [SerializeField] GameObject loadLevelPanel;
    [SerializeField] Button closeLoadLevelPanel;
    [SerializeField] Dropdown levelList;
    [SerializeField] Button loadLevel;
    [SerializeField] Button deleteLevel;

    [Header("Library")]
    [SerializeField] GameObject library;
    [SerializeField] Button closeLibrary;


    [Header("NPC List")]
    [SerializeField] GameObject NPCListWindow;
    public List<GameObject> NPCSelectorList = new List<GameObject>();
    public List<GameObject> NPCSearchSelectorList = new List<GameObject>();
    [SerializeField] Button closeNPCList;

    [Header("Initiative Tracker")]
    [SerializeField] InitiativeTracker initiativeTracker;
    [SerializeField] GameObject initiativeTrackerWindow;
    public List<GameObject> initiativeList = new List<GameObject>();
    [SerializeField] Button closeInitiativeTracker;

    #endregion

    #region Unity Event Functions

    private void Awake()
    {
        transport = (UnityTransport)networkManager.NetworkConfig.NetworkTransport;
    }

    private void Start()
    {
        inputFieldIP.text = "127.0.0.1";
        diceNumber.text = "1";

        buttonHost.onClick.AddListener(() => StartHost());
        buttonClient.onClick.AddListener(() => StartClient());

        toggleLibrary.onClick.AddListener(() => ToggleLibrary());
        closeLibrary.onClick.AddListener(() => ToggleLibrary());
        toggleCharacterCreator.onClick.AddListener(() => ToggleCharacterCreator());
        closeCharacterCreator.onClick.AddListener(() => ToggleCharacterCreator());
        toggleCharSelector.onClick.AddListener(() => ToggleCharacterSelector());
        closeCharacterSelector.onClick.AddListener(() => ToggleCharacterSelector());
        toggleDiceBox.onClick.AddListener(() => ToggleDiceBox());
        toggleNPCList.onClick.AddListener(() => ToggleNPCList());
        closeNPCList.onClick.AddListener(() => ToggleNPCList());

        toggleInitiativeTracker.onClick.AddListener(() => ToggleInitiativeTracker());
        closeInitiativeTracker.onClick.AddListener(() => ToggleInitiativeTracker());

        minimizedBarOpenChat.onClick.AddListener(delegate { ToggleMinimizedBar(); ToggleTextChat(); });
        minimizedBarOpenRegistry.onClick.AddListener(delegate { ToggleMinimizedBar(); ToggleDiceRegistry(); });
        textChatOpenRegistry.onClick.AddListener(delegate { ToggleTextChat(); ToggleDiceRegistry(); });
        textChatCloseChat.onClick.AddListener(delegate { ToggleTextChat(); ToggleMinimizedBar(); });
        diceRegistryOpenChat.onClick.AddListener(delegate { ToggleDiceRegistry(); ToggleTextChat(); });
        diceRegistryCloseRegistry.onClick.AddListener(delegate { ToggleDiceRegistry(); ToggleMinimizedBar(); });

        buttonThrowD4.onClick.AddListener(() => RollDiceServerRpc(DiceType.d4, localPlayer.playerName));
        buttonThrowD6.onClick.AddListener(() => RollDiceServerRpc(DiceType.d6, localPlayer.playerName));
        buttonThrowD8.onClick.AddListener(() => RollDiceServerRpc(DiceType.d8, localPlayer.playerName));
        buttonThrowD10.onClick.AddListener(() => RollDiceServerRpc(DiceType.d10, localPlayer.playerName));
        buttonThrowD12.onClick.AddListener(() => RollDiceServerRpc(DiceType.d12, localPlayer.playerName));
        buttonThrowD20.onClick.AddListener(() => RollDiceServerRpc(DiceType.d20, localPlayer.playerName));
        buttonThrowD100.onClick.AddListener(() => RollDiceServerRpc(DiceType.pd, localPlayer.playerName));
        diceNumber.onValueChanged.AddListener(delegate { CheckInt(diceNumber); });
        minimizeDiceCam.onClick.AddListener(() => ToggleDiceCam());
    }

    private void LateUpdate()
    {
        if (textChatInput.text.Length > 0 && Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage();
        }
    }

    #endregion

    #region player characters

    public void AddCharacterButton(int characterID, string characterName, int portraitID)
    {
        GameObject newCharacterButton = Instantiate(characterButtonPrefab);

        newCharacterButton.GetComponent<CharacterSelector>().characterName.text = characterName;
        newCharacterButton.GetComponent<CharacterSelector>().charID = characterID;
        newCharacterButton.GetComponent<CharacterSelector>().characterPortrait.sprite = gameManager.playerAvatarPortrait[portraitID];

        newCharacterButton.GetComponent<RectTransform>().SetParent(characterListArea.transform);

        characterList.Add(newCharacterButton);
    }

    public void RemoveCharacterButton(int characterID)
    {
        GameObject characterButton = characterList[characterID];

        characterList.RemoveAt(characterID);
        Destroy(characterButton);
    }

    public void UpdateCharacterButtonID(int newID)
    {
        characterList[newID].GetComponent<CharacterSelector>().charID = newID;
    }

    #endregion

    #region npc

    public void UpdateNPCButtonName(int NPC_ID, string newName)
    {
        NPCSelectorList[NPC_ID].GetComponent<NPCSelector>().NPCName.text = newName;
    }

    public void LoadSavedNPCs(List<NPCSheetInfo> savedNPCs)
    {
        NPCList _NPCList = FindObjectOfType<NPCList>();

        foreach (NPCSheetInfo NPCInfo in savedNPCs)
        {
            _NPCList.LoadNPC(NPCInfo.sheetID, NPCInfo.NPCName);
        }
    }

    public void UpdateNPCButtonID(int newID)
    {
        NPCSelectorList[newID].GetComponent<NPCSelector>().NPC_ID = newID;
    }

    public void RemoveNPCButtonFromList(int NPC_ID)
    {
        NPCSelectorList.RemoveAt(NPC_ID);
    }

    #endregion

    public GameObject GetActiveTokensParent()
    {
        return activeTokensParent;
    }

    public void AddToInitiativeTracker(string name)
    {
        initiativeTracker.AddToTracker(name);
    }

    public void SetInitiative(string characterName, int initiative)
    {
        initiativeTracker.SetInitiative(characterName, initiative);
    }

    public void RemoveFromInitiativeTracker(string characterName)
    {
        initiativeTracker.RemoveFromTracker(characterName);
    }

    public void SortInitiativeTracker()
    {
        initiativeTracker.SortInitiative();
    }

    #region Levels

    private void SaveLevel()
    {
        if (levelName.text == null || levelName.text == "")
        {
            levelName.text = "level_" + gameManager.GetLevelNumber().ToString();
        }
        bool newSave = gameManager.SaveLevel(levelName.text);

        if (newSave)
        {
            List<string> newLevel = new List<string>();
            newLevel.Add(levelName.text);

            levelList.AddOptions(newLevel);
        }

        levelName.text = "";
        ToggleSaveLevelPanel();
    }

    private void LoadLevel()
    {
        gameManager.LoadLevel(levelList.captionText.text);
        ToggleLoadLevelPanel();
    }

    private void DeleteLevel()
    {
        string levelName = levelList.captionText.text;

        bool deleted = gameManager.DeleteLevel(levelName);

        if (deleted)
        {
            int index = 0;

            for(int i = 0; i < levelList.options.Count; i++)
            {
                if (levelList.options[i].text == levelName)
                {
                    index = i;
                    break;
                }
            }

            if (index > 0)
            {
                levelList.options.RemoveAt(index);
                levelList.value = 0;
            }
        }
    }

    private void SaveLevelsToFile()
    {
        gameManager.SaveLevelsToJSON();
    }

    private void LoadLevelsFromFile()
    {
        if (!System.IO.File.Exists(Application.dataPath + "/StreamingAssets/levels.json")) { return; }

            List<string> newLevels = gameManager.LoadLevelsFromJSON();

        if (newLevels.Count > 0)
        {
            levelList.AddOptions(newLevels);
        }
    }

    #endregion

    #region Messages

    private void SendChatMessage()
    {
        StringContainer username = new StringContainer(localPlayer.playerName);
        StringContainer msg = new StringContainer(textChatInput.text);
        textChatInput.text = "";

        textChatInput.Select();
        textChatInput.ActivateInputField();

        if (IsHost)
        {
            if (msg.SomeText.StartsWith("/"))
            {
                string firstWord = msg.SomeText.Split(new char[] { ' ' })[0];

                if (firstWord == "/clear")
                {
                    foreach (Transform message in textChatContent.transform)
                    {
                        Destroy(message.gameObject);
                    }
                    return;
                }
                else if (firstWord == "/whisp")
                {
                    string secondWord = msg.SomeText.Split(new char[] { ' ' })[1];

                    ulong receiverId = gameManager.GetPlayerId(secondWord);
                    ClientRpcParams clientRpcParams;

                    if (receiverId == 0 && secondWord != localPlayer.playerName)
                    {

                        clientRpcParams = new ClientRpcParams
                        {
                            Send = new ClientRpcSendParams
                            {
                                TargetClientIds = new ulong[] { networkManager.LocalClientId }
                            }
                        };

                        StringContainer systemName = new StringContainer("SYSTEM");
                        StringContainer errorMessage = new StringContainer("ERROR: The player you tried to whisper to does not exist!");

                        PostWhisperMessageClientRpc(systemName, new StringContainer(), errorMessage, true, clientRpcParams);
                        return;
                    }

                    clientRpcParams = new ClientRpcParams
                    {
                        Send = new ClientRpcSendParams
                        {
                            TargetClientIds = new ulong[] { networkManager.LocalClientId, receiverId }
                        }
                    };

                    int wordIndex = msg.SomeText.IndexOf(" ") + 1;
                    string message = msg.SomeText.Substring(wordIndex);

                    wordIndex = message.IndexOf(" ") + 1;
                    message = message.Substring(wordIndex);

                    msg.SomeText = message;

                    StringContainer receiverName = new StringContainer(secondWord);

                    PostWhisperMessageClientRpc(username, receiverName, msg, false, clientRpcParams);
                    return;
                }
                else
                {
                    ClientRpcParams clientRpcParams = new ClientRpcParams
                    {
                        Send = new ClientRpcSendParams
                        {
                            TargetClientIds = new ulong[] { networkManager.LocalClientId }
                        }
                    };

                    StringContainer systemName = new StringContainer("SYSTEM");
                    StringContainer errorMessage = new StringContainer("ERROR: The command you tried to use does not exist!");

                    PostWhisperMessageClientRpc(systemName, new StringContainer(), errorMessage, true, clientRpcParams);
                    return;
                }
            }


            PostChatMessageClientRpc(username, msg);
        }
        else
        {
            SendChatMessageServerRpc(username, msg);
        }
    }

    private void NotifyCommandError(ServerRpcParams serverRpcParams)
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

            StringContainer systemName = new StringContainer("SYSTEM");
            StringContainer errorMessage = new StringContainer("ERROR: The command you tried to use does not exist!");

            PostWhisperMessageClientRpc(systemName, new StringContainer(), errorMessage, true, clientRpcParams);
        }
    }

    private void NotifyWhispError(ServerRpcParams serverRpcParams)
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

            StringContainer systemName = new StringContainer("SYSTEM");
            StringContainer errorMessage = new StringContainer("ERROR: The player you tried to whisper to does not exist!");

            PostWhisperMessageClientRpc(systemName, new StringContainer(), errorMessage, true, clientRpcParams);
        }
    }

    #endregion

    #region Dice rolls

    private void RollDice(DiceType type, string thrownBy, ClientRpcParams clientRpcParams)
    {
        string rollKey = diceHandler.GetNewRollKey(thrownBy + "-");
        string message = "";
        int numberOfDices = int.Parse(diceNumber.text);

        switch (type)
        {
            case DiceType.d4:
                message = " [" + numberOfDices + "D4]: ";
                break;
            case DiceType.d6:
                message = " [" + numberOfDices + "D6]: ";
                break;
            case DiceType.d8:
                message = " [" + numberOfDices + "D8]: ";
                break;
            case DiceType.d10:
                message = " [" + numberOfDices + "D10]: ";
                break;
            case DiceType.d12:
                message = " [" + numberOfDices + "D12]: ";
                break;
            case DiceType.d20:
                message = " [" + numberOfDices + "D20]: ";
                break;
        }

        diceHandler.AddRoll(rollKey, thrownBy, int.Parse(diceNumber.text), message);

        for (int i = 0; i < int.Parse(diceNumber.text); i ++)
        {
            StartCoroutine(diceHandler.RollDice(rollKey, type, 0, clientRpcParams, ResolveSimpleRoll));
        }  
    }

    private void RollD100(string thrownBy, ClientRpcParams clientRpcParams)
    {
        string rollKey = diceHandler.GetNewRollKey(thrownBy + "-");
        string message = " [D100]: ";

        diceHandler.AddRoll(rollKey, thrownBy, 2, message);

        StartCoroutine(diceHandler.RollDice(rollKey, DiceType.pd, 0, clientRpcParams, ResolveSimpleRoll));
        StartCoroutine(diceHandler.RollDice(rollKey, DiceType.d10, 0, clientRpcParams, ResolveSimpleRoll));
    }

    public void ResolveSimpleRoll(string rollKey, int modifier, ClientRpcParams clientRpcParams = default, int sheetID = -1)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        foreach (DiceResult diceScore in roll.diceScores)
        {
            result += diceScore.result;
        }

        NotifyDiceScoreClientRpc(roll.playerName + roll.message + result.ToString());

        diceHandler.DeleteRoll(rollKey);
    }

    #endregion

    void CheckInt(InputField inputField)
    {
        int value;
        if (!int.TryParse(inputField.text, out value))
        {
            inputField.text = "1";
        }
        if(value < 1)
        {
            inputField.text = "1";
        }
        else if (value > 99)
        {
            inputField.text = "99";
        }
    }

    public Text GetRulerDistanceText()
    {
        return rulerDistanceText;
    }

    #region ServerRpc

    [ServerRpc(RequireOwnership = false)]
    private void RollDiceServerRpc(DiceType type, string thrownBy, ServerRpcParams serverRpcParams = default)
    {

        var clientId = serverRpcParams.Receive.SenderClientId;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };


        if (type != DiceType.pd)
        {
            RollDice(type,thrownBy, clientRpcParams);
        }
        else
        {
            RollD100(thrownBy, clientRpcParams);
        }
        
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SendChatMessageServerRpc(StringContainer username, StringContainer msg, ServerRpcParams serverRpcParams = default)
    {
        if (msg.SomeText.StartsWith("/"))
        {
            string firstWord = msg.SomeText.Split(new char[] { ' ' })[0];

            if (firstWord == "/clear")
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

                    ClearChatClientRpc(clientRpcParams);
                }
            }
            if (firstWord == "/whisp")
            {
                string secondWord = msg.SomeText.Split(new char[] { ' ' })[1];

                ulong receiverId = gameManager.GetPlayerId(secondWord);

                if (receiverId == 0 && secondWord != localPlayer.playerName)
                {
                    NotifyWhispError(serverRpcParams);
                    return;
                }

                var clientId = serverRpcParams.Receive.SenderClientId;

                if (NetworkManager.ConnectedClients.ContainsKey(clientId))
                {
                    ClientRpcParams clientRpcParams = new ClientRpcParams
                    {
                        Send = new ClientRpcSendParams
                        {
                            TargetClientIds = new ulong[] { clientId, receiverId, 0 }
                        }
                    };

                    int wordIndex = msg.SomeText.IndexOf(" ") + 1;
                    string message = msg.SomeText.Substring(wordIndex);

                    wordIndex = message.IndexOf(" ") + 1;
                    message = message.Substring(wordIndex);

                    msg.SomeText = message;

                    StringContainer receiverName = new StringContainer(secondWord);

                    PostWhisperMessageClientRpc(username, receiverName, msg, false, clientRpcParams);
                    return;
                }
            }

            NotifyCommandError(serverRpcParams);
            return;
        }
        PostChatMessageClientRpc(username, msg);

    }

    #endregion

    #region ClientRpc

    [ClientRpc]
    public void NotifyDiceScoreClientRpc(string scoreMessage)
    {
        GameObject message = Instantiate(messagePrefab);

        message.GetComponent<Text>().text = scoreMessage;

        message.GetComponent<RectTransform>().SetParent(diceRegistryContent.GetComponent<RectTransform>());
        message.GetComponent<RectTransform>().SetAsLastSibling();

        if (!diceRegistry.activeInHierarchy)
        {
            minimizedBarRegistryNotification.SetActive(true);
            textChatRegistryNotification.SetActive(true);
        }
    }

    [ClientRpc]
    public void AddCharacterButtonClientRpc(int characterID, string characterName, int portraitID)
    {
        Vector3 position;

        if (characterList.Count % 2 == 0)
        {
            position = new Vector3(characterListArea.GetComponent<RectTransform>().rect.width / 4, -characterSpace/2 - characterList.Count / 2 * characterSpace, 0);
        }
        else
        {
            position = new Vector3(characterListArea.GetComponent<RectTransform>().rect.width / 4 * 3, -characterSpace/2 - characterList.Count / 2 * characterSpace, 0);
        }
        
        GameObject newCharacterButton = Instantiate(characterButtonPrefab);

        newCharacterButton.GetComponent<CharacterSelector>().characterName.text = characterName;
        newCharacterButton.GetComponent<CharacterSelector>().charID = characterID;
        newCharacterButton.GetComponent<CharacterSelector>().characterPortrait.sprite = gameManager.playerAvatarPortrait[portraitID];

        newCharacterButton.GetComponent<RectTransform>().SetParent(characterListArea.transform);
        newCharacterButton.GetComponent<RectTransform>().localPosition = position;

        characterList.Add(newCharacterButton);

    }

    [ClientRpc]
    public void UpdateCharacterButtonNameClientRpc(int charID, string newName)
    {
        characterList[charID].GetComponent<CharacterSelector>().characterName.text = newName;
    }

    [ClientRpc]
    private void PostChatMessageClientRpc(StringContainer username, StringContainer msg)
    {
        GameObject message = Instantiate(messagePrefab);

        message.GetComponent<Text>().text = username.SomeText + ": " + msg.SomeText;

        message.GetComponent<RectTransform>().SetParent(textChatContent.GetComponent<RectTransform>());
        message.GetComponent<RectTransform>().SetAsLastSibling();

        if (!textChat.activeInHierarchy)
        {
            diceRegistryChatNotification.SetActive(true);
            minimizedBarChatNotification.SetActive(true);
        }
    }

    [ClientRpc]
    private void ClearChatClientRpc(ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        foreach (Transform message in textChatContent.transform)
        {
            Destroy(message.gameObject);
        }
    }

    [ClientRpc]
    private void PostWhisperMessageClientRpc(StringContainer sender, StringContainer receiver, StringContainer msg, bool error, ClientRpcParams clientRpcParams)
    {
        GameObject message = Instantiate(messagePrefab);

        if (error)
        {
            message.GetComponent<Text>().color = Color.red;
        }
        else
        {
            message.GetComponent<Text>().color = Color.magenta;
            if (IsHost)
            {
                sender.SomeText += " (to " + receiver.SomeText + ")";
            }
        }      

        message.GetComponent<Text>().text = sender.SomeText + ": " + msg.SomeText;

        message.GetComponent<RectTransform>().SetParent(textChatContent.GetComponent<RectTransform>());
        message.GetComponent<RectTransform>().SetAsLastSibling();

        if (!textChat.activeInHierarchy)
        {
            diceRegistryChatNotification.SetActive(true);
            minimizedBarChatNotification.SetActive(true);
        }
    }
    #endregion

    #region UI Navigation Methods

    private void ActivateMainMenu()
    {
        mainMenu.SetActive(true);
    }

    private void DeactivateMainMenu()
    {
        mainMenu.SetActive(false);
    }

    private void ActivateInGameHUD()
    {
        inGameHUD.SetActive(true);
    }

    private void DeactivateInGameHUD()
    {
        inGameHUD.SetActive(false);
    }

    public void ToggleCharacterSelector()
    {
        bool toggle = !characterSelector.activeInHierarchy;
        characterSelector.SetActive(toggle);
    }

    private void ToggleDiceBox()
    {
        bool toggle = !diceBox.activeInHierarchy;
        diceBox.SetActive(toggle);
    }

    private void ToggleNPCList()
    {
        bool toggle = !NPCListWindow.activeInHierarchy;
        NPCListWindow.SetActive(toggle);
    }

    private void ToggleTextChat()
    {
        bool toggle = !textChat.activeInHierarchy;
        textChat.SetActive(toggle);

        if (textChat.activeInHierarchy)
        {
            minimizedBarChatNotification.SetActive(false);
            diceRegistryChatNotification.SetActive(false);
        }
    }

    private void ToggleDiceRegistry()
    {
        bool toggle = !diceRegistry.activeInHierarchy;
        diceRegistry.SetActive(toggle);

        if (diceRegistry.activeInHierarchy)
        {
            minimizedBarRegistryNotification.SetActive(false);
            textChatRegistryNotification.SetActive(false);
        }
    }

    private void ToggleMinimizedBar()
    {
        bool toggle = !minimizedBar.activeInHierarchy;
        minimizedBar.SetActive(toggle);
    }

    private void ToggleDmInventory()
    {
        bool toggle = !dmInventory.activeInHierarchy;
        dmInventory.SetActive(toggle);
    }

    private void ToggleSaveLevelPanel()
    {
        bool toggle = !saveLevelPanel.activeInHierarchy;
        saveLevelPanel.SetActive(toggle);
    }

    private void ToggleLoadLevelPanel()
    {
        bool toggle = !loadLevelPanel.activeInHierarchy;
        loadLevelPanel.SetActive(toggle);
    }

    private void SwitchItemType(int newType)
    {
        switch (newType)
        {
            case 0:
                structureItems.SetActive(true);
                floorTileItems.SetActive(false);
                decorItems.SetActive(false);
                break;
            case 1:
                structureItems.SetActive(false);
                floorTileItems.SetActive(true);
                decorItems.SetActive(false);
                break;
            case 2:
                structureItems.SetActive(false);
                floorTileItems.SetActive(false);
                decorItems.SetActive(true);
                break;
        }
    }

    private void ToggleLibrary()
    {
        bool toggle = !library.activeInHierarchy;
        library.SetActive(toggle);
    }

    private void ToggleCharacterCreator()
    {
        bool toggle = !characterCreator.activeInHierarchy;
        characterCreator.SetActive(toggle);
    }

    private void ToggleDiceCam()
    {
        bool toggle = !diceCamRender.activeInHierarchy;
        diceCamRender.SetActive(toggle);
    }

    private void ToggleInitiativeTracker()
    {
        bool toggle = !initiativeTrackerWindow.activeInHierarchy;
        initiativeTrackerWindow.SetActive(toggle);
    }

    #endregion

    #region Netcode Related Methods

    private void StartHost()
    {
        if (!SetIPAndPort()) { return; }

        NetworkManager.Singleton.StartHost();

        toggleDmInventory.onClick.AddListener(() => ToggleDmInventory());
        closeDmInventory.onClick.AddListener(() => ToggleDmInventory());
        itemType.onValueChanged.AddListener(delegate { SwitchItemType(itemType.value); });
        openSaveLevelPanel.onClick.AddListener(() => ToggleSaveLevelPanel());
        openLoadLevelPanel.onClick.AddListener(() => ToggleLoadLevelPanel());
        saveToFile.onClick.AddListener(() => SaveLevelsToFile());
        loadFromFile.onClick.AddListener(() => LoadLevelsFromFile());
        deleteLevel.onClick.AddListener(() => DeleteLevel());

        closeSaveLevelPanel.onClick.AddListener(() => ToggleSaveLevelPanel());
        saveLevel.onClick.AddListener(() => SaveLevel());

        closeLoadLevelPanel.onClick.AddListener(() => ToggleLoadLevelPanel());
        loadLevel.onClick.AddListener(() => LoadLevel());

        LoadLevelsFromFile();

        DeactivateMainMenu();
        ActivateInGameHUD();

        StartCoroutine(FindObjectOfType<CharacterCreator>().LoadCharacterCreationOptions());
        gameManager.LoadCharactersFromJSON();
        gameManager.LoadNPCsFromJSON();
    }

    private void StartClient()
    {
        if (!SetIPAndPort()) { return; }

        NetworkManager.Singleton.StartClient();

        Destroy(toggleDmInventory.transform.parent.gameObject);
        Destroy(toggleNPCList.transform.parent.gameObject);

        DeactivateMainMenu();
        ActivateInGameHUD();

        StartCoroutine(FindObjectOfType<CharacterCreator>().LoadCharacterCreationOptions());
        StartCoroutine(gameManager.LoadActiveTokenShortcut());
        StartCoroutine(gameManager.LoadCurrentInitiativeOrder());
    }

    private bool SetIPAndPort()
    {
        bool success = false;
        var ip = inputFieldIP.text;

        if (!string.IsNullOrEmpty(ip))
        {
            transport.SetConnectionData(ip, port);
            success = true;
        }

        return success;
    }

    #endregion
}
