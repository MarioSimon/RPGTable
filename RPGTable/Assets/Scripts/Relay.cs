using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode.Transports.UTP;
using System.Threading.Tasks;
using System;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Netcode;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class Relay : MonoBehaviour
{
    private UnityTransport transport;
    private const int MAX_PLAYERS = 10;

    public string joinCode;

    private async void Awake()
    {
        transport = FindObjectOfType<UnityTransport>();

        await Authenticate();
    }

    private static async Task Authenticate()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void StartGame()
    {
        Allocation a = await RelayService.Instance.CreateAllocationAsync(MAX_PLAYERS);

        transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

        joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

        NetworkManager.Singleton.StartHost();
    }

    public async void JoinGame(string joinCode)
    {
        JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(joinCode);

        transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

        NetworkManager.Singleton.StartClient();
    }
}
