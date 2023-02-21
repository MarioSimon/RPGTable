using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System;
using Unity.Collections;
using System.Collections.Generic;

public class UIManager : NetworkBehaviour
{
    #region Variables

    [SerializeField] Canvas canvas;
    [SerializeField] NetworkManager networkManager;
    [SerializeField] GameManager gameManager;
    UnityTransport transport;
    readonly ushort port = 7777;

    public Player localPlayer;

    [Header("Main Menu")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] public InputField inputFieldName;
    [SerializeField] InputField inputFieldIP;
    [SerializeField] Button buttonHost;
    [SerializeField] Button buttonClient;

    [Header("In Game HUD")]
    [SerializeField] GameObject inGameHUD;
    [SerializeField] Button toggleLibrary;
    [SerializeField] Button buttonSpawnNewCharSheet;
    [SerializeField] Button toggleCharSelector;
    [SerializeField] Button toggleDiceBox;
    [SerializeField] Button toggleDmInventory;
    [SerializeField] GameObject charSheetPrefab;

    [Header("Dice Box")]
    [SerializeField] GameObject diceBox;
    [SerializeField] Button buttonThrowD4;
    [SerializeField] Button buttonThrowD6;
    [SerializeField] Button buttonThrowD8;
    [SerializeField] Button buttonThrowD10;
    [SerializeField] Button buttonThrowD12;
    [SerializeField] Button buttonThrowD20;
    [SerializeField] Button buttonThrowD100;

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

    [Header("Characters")]
    [SerializeField] GameObject characterSelector;
    [SerializeField] Button closeCharacterSelector;
    [SerializeField] GameObject characterListArea;
    List<GameObject> characterList = new List<GameObject>();
    [SerializeField] float characterSpace;
    [SerializeField] GameObject characterButtonPrefab;

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
    #endregion

    #region Unity Event Functions

    private void Awake()
    {
        transport = (UnityTransport)networkManager.NetworkConfig.NetworkTransport;
    }

    private void Start()
    {
        inputFieldIP.text = "127.0.0.1";

        buttonHost.onClick.AddListener(() => StartHost());
        buttonClient.onClick.AddListener(() => StartClient());

        toggleLibrary.onClick.AddListener(() => ToggleLibrary());
        closeLibrary.onClick.AddListener(() => ToggleLibrary());
        buttonSpawnNewCharSheet.onClick.AddListener(() => ToggleCharacterCreator());
        closeCharacterCreator.onClick.AddListener(() => ToggleCharacterCreator());
        toggleCharSelector.onClick.AddListener(() => ToggleCharacterSelector());
        closeCharacterSelector.onClick.AddListener(() => ToggleCharacterSelector());
        toggleDiceBox.onClick.AddListener(() => ToggleDiceBox());

        minimizedBarOpenChat.onClick.AddListener(delegate { ToggleMinimizedBar(); ToggleTextChat(); });
        minimizedBarOpenRegistry.onClick.AddListener(delegate { ToggleMinimizedBar(); ToggleDiceRegistry(); });
        textChatOpenRegistry.onClick.AddListener(delegate { ToggleTextChat(); ToggleDiceRegistry(); });
        textChatCloseChat.onClick.AddListener(delegate { ToggleTextChat(); ToggleMinimizedBar(); });
        diceRegistryOpenChat.onClick.AddListener(delegate { ToggleDiceRegistry(); ToggleTextChat(); });
        diceRegistryCloseRegistry.onClick.AddListener(delegate { ToggleDiceRegistry(); ToggleMinimizedBar(); });


        buttonThrowD4.onClick.AddListener(() => RollDiceServerRpc(diceType.d4, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD6.onClick.AddListener(() => RollDiceServerRpc(diceType.d6, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD8.onClick.AddListener(() => RollDiceServerRpc(diceType.d8, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD10.onClick.AddListener(() => RollDiceServerRpc(diceType.d10, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD12.onClick.AddListener(() => RollDiceServerRpc(diceType.d12, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD20.onClick.AddListener(() => RollDiceServerRpc(diceType.d20, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD100.onClick.AddListener(() => RollDiceServerRpc(diceType.pd, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
    }

    private void LateUpdate()
    {
        if (textChatInput.text.Length > 0 && Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage();
        }
    }

    #endregion

    public void AddCharacterButton(int characterID, string characterName, int portraitID)
    {
        Vector3 position;

        if (characterList.Count % 2 == 0)
        {
            position = new Vector3(characterListArea.GetComponent<RectTransform>().rect.width / 4, -characterSpace / 2 - characterList.Count / 2 * characterSpace, 0);
        }
        else
        {
            position = new Vector3(characterListArea.GetComponent<RectTransform>().rect.width / 4 * 3, -characterSpace / 2 - characterList.Count / 2 * characterSpace, 0);
        }

        GameObject newCharacterButton = Instantiate(characterButtonPrefab);

        newCharacterButton.GetComponent<CharacterSelector>().characterName.text = characterName;
        newCharacterButton.GetComponent<CharacterSelector>().charID = characterID;
        newCharacterButton.GetComponent<CharacterSelector>().characterPortrait.sprite = gameManager.avatarPortrait[portraitID];

        newCharacterButton.GetComponent<RectTransform>().SetParent(characterListArea.transform);
        newCharacterButton.GetComponent<RectTransform>().localPosition = position;

        characterList.Add(newCharacterButton);
    }

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
        bool deleted = gameManager.DeleteLevel(levelList.captionText.text);

        if (deleted)
        {
            // borrar el nivel borrado de la lista
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

    private void SendChatMessage()
    {
        StringContainer username = new StringContainer();
        username.SomeText = localPlayer.givenName.Value.ToString();
        StringContainer msg = new StringContainer();
        msg.SomeText = textChatInput.text;
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

                if (firstWord == "/whisp")
                {
                    string secondWord = msg.SomeText.Split(new char[] { ' ' })[1];
                }

                return;
            }


            PostChatMessageClientRpc(username, msg);
        }
        else
        {
            SendChatMessageServerRpc(username, msg);
        }
    }

    #region ServerRpc

    [ServerRpc(RequireOwnership = false)]
    private void RollDiceServerRpc(diceType type, Vector3 position, string thrownBy)
    {
        gameManager.RollDice(type, position, thrownBy, 0);
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
            }

            return;
        }


        PostChatMessageClientRpc(username, msg);
    }

    
    #endregion

    #region ClientRpc

    [ClientRpc]
    public void NotifyDiceScoreClientRpc(string scoreMessage)
    {
        diceRegistryText.text += "\n" + scoreMessage;
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
        newCharacterButton.GetComponent<CharacterSelector>().characterPortrait.sprite = gameManager.avatarPortrait[portraitID];

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
    #endregion

    #region UI Related Methods

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

    private void ToggleTextChat()
    {
        bool toggle = !textChat.activeInHierarchy;
        textChat.SetActive(toggle);
    }

    private void ToggleDiceRegistry()
    {
        bool toggle = !diceRegistry.activeInHierarchy;
        diceRegistry.SetActive(toggle);
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
    }

    private void StartClient()
    {
        if (!SetIPAndPort()) { return; }

        NetworkManager.Singleton.StartClient();

        Destroy(toggleDmInventory.gameObject);

        DeactivateMainMenu();
        ActivateInGameHUD();

        StartCoroutine(FindObjectOfType<CharacterCreator>().LoadCharacterCreationOptions());
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
