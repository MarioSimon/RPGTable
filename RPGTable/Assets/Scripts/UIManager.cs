using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System;

public class UIManager : MonoBehaviour
{
    #region variables
    [SerializeField] NetworkManager networkManager;
    UnityTransport transport;
    readonly ushort port = 7777;

    [Header("Main Menu")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] InputField inputFieldName;
    [SerializeField] InputField inputFieldIP;
    [SerializeField] Button buttonHost;
    [SerializeField] Button buttonClient;

    #endregion

    #region Unity Event Functions

    private void Awake()
    {
        transport = (UnityTransport)networkManager.NetworkConfig.NetworkTransport;
    }

    private void Start()
    {
        buttonHost.onClick.AddListener(() => StartHost());
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

    #endregion

    #region Netcode Related Methods

    private void StartHost()
    {
        if (!SetIPAndPort()) { return; }

        NetworkManager.Singleton.StartHost();
        DeactivateMainMenu();
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
