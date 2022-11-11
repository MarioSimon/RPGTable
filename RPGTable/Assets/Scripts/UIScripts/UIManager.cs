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

    //Esto no me gusta, estoy probando cosas y luego veré si puedo arreglarlo
    public Player localPlayer;

    [Header("Main Menu")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] public InputField inputFieldName;
    [SerializeField] InputField inputFieldIP;
    [SerializeField] Button buttonHost;
    [SerializeField] Button buttonClient;

    [Header("In Game HUD")]
    [SerializeField] GameObject inGameHUD;
    [SerializeField] Button buttonSpawnToken;
    [SerializeField] Button buttonSpawnNewCharSheet;
    [SerializeField] Button openCharSelector;

    //Esto probablemente se mueva mas adelante
    [SerializeField] GameObject tokenPrefab;
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

    [Header("Dice Registry")]
    [SerializeField] GameObject diceRegistry;
    [SerializeField] Text diceRegistryText;

    [Header("Character Selector")]
    [SerializeField] GameObject characterSelector;
    [SerializeField] Button closeCharacterSelector;
    [SerializeField] GameObject characterListArea;
    List<GameObject> characterList = new List<GameObject>();
    [SerializeField] float characterSpace;
    [SerializeField] GameObject characterButtonPrefab;


    

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
        buttonSpawnToken.onClick.AddListener(() => SpawnToken());

        buttonSpawnNewCharSheet.onClick.AddListener(() => SpawnNewCharSheet());
        openCharSelector.onClick.AddListener(() => ActivateCharacterSelector());
        closeCharacterSelector.onClick.AddListener(() => DeactivateCharacterSelector());

        buttonThrowD4.onClick.AddListener(() => RollDiceServerRpc(diceType.d4, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD6.onClick.AddListener(() => RollDiceServerRpc(diceType.d6, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD8.onClick.AddListener(() => RollDiceServerRpc(diceType.d8, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD10.onClick.AddListener(() => RollDiceServerRpc(diceType.d10, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD12.onClick.AddListener(() => RollDiceServerRpc(diceType.d12, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD20.onClick.AddListener(() => RollDiceServerRpc(diceType.d20, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD100.onClick.AddListener(() => RollDiceServerRpc(diceType.pd, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
    }

    #endregion

    private void SpawnToken()
    {
        SpawnTokenServerRpc(localPlayer.givenName.Value);
    }    

    private void SpawnNewCharSheet()
    {
        int newID = gameManager.GetNewSheetID();
       
        GameObject charSheet = Instantiate(charSheetPrefab); 
        charSheet.GetComponent<CharacterSheetManager>().CSInfo = new CharacterSheetInfo();
        charSheet.GetComponent<CharacterSheetManager>().CSInfo.playerName = localPlayer.givenName.Value.ToString();
        charSheet.GetComponent<CharacterSheetManager>().CSInfo.sheetID = newID;
        charSheet.GetComponent<CharacterSheetManager>().CSInfo.ownerID = NetworkManager.Singleton.LocalClientId;
        charSheet.GetComponent<RectTransform>().SetParent(canvas.gameObject.transform, false);
        SpawnNewCharSheetServerRpc(charSheet.GetComponent<CharacterSheetManager>().CSInfo);
    }

    public void AddCharacterButton(int characterID, string characterName)
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

        newCharacterButton.GetComponent<RectTransform>().SetParent(characterListArea.transform);
        newCharacterButton.GetComponent<RectTransform>().localPosition = position;

        characterList.Add(newCharacterButton);
    }

    #region ServerRpc

    [ServerRpc(RequireOwnership = false)]
    private void SpawnTokenServerRpc(FixedString64Bytes ownerName)
    {       
        GameObject token = Instantiate(tokenPrefab, Vector3.zero, Quaternion.identity);
        token.GetComponent<TokenController>().ownerName.Value = ownerName;
        token.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RollDiceServerRpc(diceType type, Vector3 position, string thrownBy)
    {
        gameManager.RollDice(type, position, thrownBy, 0);
    }

    //[ServerRpc(RequireOwnership = false)]
    //private void SpawnNewCharSheetServerRpc(ulong clientID, FixedString64Bytes ownerName)
    //{
    //    GameObject charSheet = Instantiate(charSheetPrefab);
    //    charSheet.GetComponent<CharacterSheetManager>().playerName.text = ownerName.Value.ToString();
    //    charSheet.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
    //    charSheet.GetComponent<RectTransform>().SetParent(canvas.gameObject.transform, false);
    //}

    [ServerRpc(RequireOwnership = false)]
    private void SpawnNewCharSheetServerRpc(CharacterSheetInfo CSInfo)
    {
        gameManager.AddNewCharacterSheetInfo(CSInfo);
    }

   //[ServerRpc]
   //private void OpenCharSheetServerRpc(int sheetIndex)
   //{
   //    CharacterSheetInfo CSInfo = gameManager.GetSheetInfo(sheetIndex);
   //    if (CSInfo == null) { return; }
   //    GameObject charSheet = Instantiate(charSheetPrefab);
   //    charSheet.GetComponent<CharacterSheetManager>().CSInfo = CSInfo;
   //    charSheet.GetComponent<NetworkObject>().SpawnWithOwnership(CSInfo.ownerID);
   //    charSheet.GetComponent<RectTransform>().SetParent(canvas.gameObject.transform, false);
   //}
    
    #endregion

    #region ClientRpc

    [ClientRpc]
    public void NotifyDiceScoreClientRpc(string scoreMessage)
    {
        diceRegistryText.text += "\n" + scoreMessage;
    }

    [ClientRpc]
    public void AddCharacterButtonClientRpc(int characterID, string characterName)
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

        newCharacterButton.GetComponent<RectTransform>().SetParent(characterListArea.transform);
        newCharacterButton.GetComponent<RectTransform>().localPosition = position;

        characterList.Add(newCharacterButton);

    }

    [ClientRpc]
    public void UpdateCharacterButtonNameClientRpc(int charID, string newName)
    {
        characterList[charID].GetComponent<CharacterSelector>().characterName.text = newName;
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

    private void ActivateCharacterSelector()
    {
        characterSelector.SetActive(true);
    }

    private void DeactivateCharacterSelector()
    {
        characterSelector.SetActive(false);
    }

    #endregion

    #region Netcode Related Methods

    private void StartHost()
    {
        if (!SetIPAndPort()) { return; }

        NetworkManager.Singleton.StartHost();
        DeactivateMainMenu();
        ActivateInGameHUD();
    }

    private void StartClient()
    {
        if (!SetIPAndPort()) { return; }

        NetworkManager.Singleton.StartClient();      
        DeactivateMainMenu();
        ActivateInGameHUD();
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
