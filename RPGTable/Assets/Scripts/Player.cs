using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    #region Variables

    public NetworkVariable<FixedString64Bytes> givenName;

    [SerializeField] private TokenController selectedToken;
    private UIManager UIManager;

    #endregion

    #region Unity Event Functions

    private void Awake()
    {
        UIManager = FindObjectOfType<UIManager>();
        NetworkManager.OnClientConnectedCallback += ConfigurePlayer;

        givenName = new NetworkVariable<FixedString64Bytes>("");
    }

    private void Start()
    {       
        if (IsLocalPlayer)
        {
            UIManager.localPlayer = this;
            UIManager.GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
        }
    }

    private void Update()
    {
        if (!IsLocalPlayer) { return; }
        if (Input.GetMouseButtonDown(2))
        {
            DeselectToken();
        }
        InteractWithSelection();
        InteractWithMovement();
    }

    #endregion

    #region Player Related Methods

    private void ConfigurePlayer(ulong obj)
    {
        if (IsLocalPlayer)
        {
            SetPlayerNameServerRpc(UIManager.inputFieldName.text);
        }
    }

    #endregion

    #region Token Interaction Methods

    public void SelectToken(TokenController token)
    {
        DeselectToken();
        selectedToken = token;
        selectedToken.ChangeSelection();
    }

    public void DeselectToken()
    {
        if (selectedToken != null)
        {
            selectedToken.ChangeSelection();
        }
        selectedToken = null;
    }

    private void InteractWithSelection()
    {

        RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

        foreach (RaycastHit hit in hits)
        {
            TokenController target = hit.transform.GetComponent<TokenController>();
            if (target == null) continue;

            if (Input.GetMouseButtonDown(0))
            {
                SelectToken(target);
            }
        }
    }

    private void InteractWithMovement()
    {       
        if (selectedToken == null) { return; }
        if (givenName.Value != selectedToken.ownerName.Value && !IsHost) { return; }

        RaycastHit hit;
        bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

        if (hasHit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (givenName.Value == selectedToken.ownerName.Value)
                    //MoveTokenServerRpc(hit.point);
                    selectedToken.MoveToServerRpc(hit.point);
                else if (IsHost)
                    selectedToken.MoveTo(hit.point);
            }
        }
    }

    #endregion

    #region Utility

    private Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    #endregion

    #region ServerRpc

    [ServerRpc]
    public void SetPlayerNameServerRpc(string name)
    {
        this.givenName.Value = new FixedString64Bytes(name);
    }

    #endregion
}
