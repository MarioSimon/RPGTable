using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System;
using Unity.Collections;

public class UIManager : NetworkBehaviour
{
    #region Variables

    [SerializeField] NetworkManager networkManager;
    UnityTransport transport;
    readonly ushort port = 7777;

    [Header("Main Menu")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] public InputField inputFieldName;
    [SerializeField] InputField inputFieldIP;
    [SerializeField] Button buttonHost;
    [SerializeField] Button buttonClient;

    [Header("In Game HUD")]
    [SerializeField] GameObject inGameHUD;
    [SerializeField] Button buttonSpawnToken;

    //Esto probablemente se mueva mas adelante
    [SerializeField] GameObject tokenPrefab;

    [Header("Dice Box")]
    [SerializeField] GameObject diceBox;
    [SerializeField] Button buttonThrowD4;
    [SerializeField] Button buttonThrowD6;
    [SerializeField] Button buttonThrowD8;
    [SerializeField] Button buttonThrowD10;
    [SerializeField] Button buttonThrowD12;
    [SerializeField] Button buttonThrowD20;
    [SerializeField] Button buttonThrowD100;

    [SerializeField] GameObject d4Prefab;
    [SerializeField] GameObject d6Prefab;
    [SerializeField] GameObject d8Prefab;
    [SerializeField] GameObject d10Prefab;
    [SerializeField] GameObject d12Prefab;
    [SerializeField] GameObject d20Prefab;
    [SerializeField] GameObject pdPrefab;

    [Header("Dice Registry")]
    [SerializeField] GameObject diceRegistry;
    [SerializeField] Text diceRegistryText;


    //Esto no me gusta, estoy probando cosas y luego veré si puedo arreglarlo
    public Player localPlayer;

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
        buttonThrowD4.onClick.AddListener(() => ThrowDiceServerRpc(diceType.d4, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD6.onClick.AddListener(() => ThrowDiceServerRpc(diceType.d6, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD8.onClick.AddListener(() => ThrowDiceServerRpc(diceType.d8, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD10.onClick.AddListener(() => ThrowDiceServerRpc(diceType.d10, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD12.onClick.AddListener(() => ThrowDiceServerRpc(diceType.d12, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD20.onClick.AddListener(() => ThrowDiceServerRpc(diceType.d20, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
        buttonThrowD100.onClick.AddListener(() => ThrowDiceServerRpc(diceType.pd, Camera.main.transform.position, localPlayer.givenName.Value.ToString()));
    }

    #endregion

    private void SpawnToken()
    {
        SpawnTokenServerRpc(localPlayer.givenName.Value);
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
    private void ThrowDiceServerRpc(diceType type, Vector3 position, string thrownBy)
    {
        position.y -= 0.5f;

        switch (type)
        {
            case diceType.d4:
                GameObject d4Dice = Instantiate(d4Prefab, position, Quaternion.identity);
                d4Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d4Dice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.d6:
                GameObject d6Dice = Instantiate(d6Prefab, position, Quaternion.identity);
                d6Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d6Dice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.d8:
                GameObject d8Dice = Instantiate(d8Prefab, position, Quaternion.identity);
                d8Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d8Dice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.d10:
                GameObject d10Dice = Instantiate(d10Prefab, position, Quaternion.identity);
                d10Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d10Dice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.pd:
                GameObject pdDice = Instantiate(pdPrefab, position, Quaternion.identity);
                pdDice.GetComponent<Dice>().thrownBy = thrownBy;
                pdDice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.d12:
                GameObject d12Dice = Instantiate(d12Prefab, position, Quaternion.identity);
                d12Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d12Dice.GetComponent<NetworkObject>().Spawn();
                break;
            case diceType.d20:
                GameObject d20Dice = Instantiate(d20Prefab, position, Quaternion.identity);
                d20Dice.GetComponent<Dice>().thrownBy = thrownBy;
                d20Dice.GetComponent<NetworkObject>().Spawn();
                break;
        }
    }

    #endregion

    #region ClientRpc

    [ClientRpc]
    public void NotifyDiceScoreClientRpc(string scoreMessage)
    {
        diceRegistryText.text += scoreMessage + "\n";
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
