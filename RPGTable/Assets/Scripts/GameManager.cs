using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region ServerRPCs

    [ServerRpc]
    private void NotifyNewConnectionServerRpc(string playerName)
    {
        NotifyNewConnectionClientRpc(playerName);
    }

    #endregion

    #region ClientRPCs

    [ClientRpc]
    private void NotifyNewConnectionClientRpc(string playerName)
    {
        Debug.Log(playerName + "  se ha conectado.");
    }

    #endregion


    private void OnConnectedToServer()
    {
        
    }
}
