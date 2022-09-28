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
        buttonHost.onClick.AddListener(() => StartHost());
        buttonClient.onClick.AddListener(() => StartClient());
        buttonSpawnToken.onClick.AddListener(() => SpawnToken());
        buttonThrowD4.onClick.AddListener(() => ThrowD4());
        buttonThrowD6.onClick.AddListener(() => ThrowD6());
        buttonThrowD8.onClick.AddListener(() => ThrowD8());
        buttonThrowD10.onClick.AddListener(() => ThrowD10());
        buttonThrowD12.onClick.AddListener(() => ThrowD12());
        buttonThrowD20.onClick.AddListener(() => ThrowD20());
        buttonThrowD100.onClick.AddListener(() => ThrowD100());
    }

    #endregion

    private void SpawnToken()
    {
        SpawnTokenServerRpc(localPlayer.givenName.Value);
    }    

    private void ThrowD4()
    {
        ThrowD4ServerRpc(Camera.main.transform.position);
    }

    private void ThrowD6()
    {
        ThrowD6ServerRpc(Camera.main.transform.position);
    }

    private void ThrowD8()
    {
        ThrowD8ServerRpc(Camera.main.transform.position);
    }

    private void ThrowD10()
    {
        ThrowD10ServerRpc(Camera.main.transform.position);
    }
    private void ThrowD12()
    {
        ThrowD12ServerRpc(Camera.main.transform.position);
    }

    private void ThrowD20()
    {
        ThrowD20ServerRpc(Camera.main.transform.position);
    }

    private void ThrowD100()
    {
        ThrowD100ServerRpc(Camera.main.transform.position);
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
    private void ThrowD4ServerRpc(Vector3 position)
    {
        GameObject token = Instantiate(d4Prefab, position, Quaternion.identity);
        token.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ThrowD6ServerRpc(Vector3 position)
    {
        GameObject token = Instantiate(d6Prefab, position, Quaternion.identity);
        token.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ThrowD8ServerRpc(Vector3 position)
    {
        GameObject token = Instantiate(d8Prefab, position, Quaternion.identity);
        token.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ThrowD10ServerRpc(Vector3 position)
    {
        GameObject token = Instantiate(d10Prefab, position, Quaternion.identity);
        token.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ThrowD12ServerRpc(Vector3 position)
    {
        GameObject token = Instantiate(d12Prefab, position, Quaternion.identity);
        token.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ThrowD20ServerRpc(Vector3 position)
    {
        GameObject token = Instantiate(d20Prefab, position, Quaternion.identity);
        token.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ThrowD100ServerRpc(Vector3 position)
    {
        Vector3 dicePos1 = new Vector3(position.x + 0.25f, position.y, position.z + 0.25f);
        Vector3 dicePos2 = new Vector3(position.x - 0.25f, position.y, position.z - 0.25f);

        GameObject token1 = Instantiate(pdPrefab, dicePos1, Quaternion.identity);
        GameObject token2 = Instantiate(d10Prefab, dicePos2, Quaternion.identity);

        token1.GetComponent<NetworkObject>().Spawn();
        token2.GetComponent<NetworkObject>().Spawn();
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
