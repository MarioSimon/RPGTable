using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public NetworkVariable<FixedString64Bytes> givenName;

    [SerializeField] private TokenController selectedToken;
    private UIManager UIManager;

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
        }
    }

    private void ConfigurePlayer(ulong obj)
    {
        if (IsLocalPlayer)
        {
            SetPlayerNameServerRpc(UIManager.inputFieldName.text);
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
                    selectedToken.gameObject.GetComponent<Mover>().MoveTo(hit.point);
            }
        }
    }

    private Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    #region ServerRpc

    [ServerRpc]
    private void SelectTokenServerRpc()
    {

    }

    [ServerRpc]
    private void MoveTokenServerRpc(Vector3 destination)
    {
        selectedToken.gameObject.GetComponent<Mover>().MoveTo(destination);
    }

    [ServerRpc]
    public void SetPlayerNameServerRpc(string name)
    {
        this.givenName.Value = new FixedString64Bytes(name);
    }

    #endregion
}
