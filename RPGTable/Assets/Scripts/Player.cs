using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    #region variables

    public string playerName;

    private TokenController selectedToken;

    private UIManager UIManager;
    private GameManager gameManager;

    #endregion

    #region unity event functions

    private void Awake()
    {
        UIManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
        NetworkManager.OnClientConnectedCallback += ConfigurePlayer;
    }

    private void Start()
    {

        if (IsLocalPlayer)
        {
            UIManager.localPlayer = this;
            FindObjectOfType<CameraController>().enabled = true;
            AddPlayerToList(UIManager.inputFieldName.text, NetworkManager.Singleton.LocalClientId);
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
            gameManager.AddSavedCharactersServerRpc();
            //AddPlayerToList(playerName, obj);
        }
    }

    private void AddPlayerToList(string name, ulong id)
    {
        if (IsHost)
        {
            gameManager.AddPlayerToList(name, id);
        }
        else
        {
            gameManager.AddPlayerToListServerRpc(name, id);
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
        if (playerName != selectedToken.ownerName.Value && !IsHost) { return; }

        RaycastHit hit;
        bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

        if (hasHit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (playerName == selectedToken.ownerName.Value)
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
        SetPlayerNameClientRpc(name);
    }
    #endregion

    [ClientRpc]
    public void SetPlayerNameClientRpc(string name)
    {
        playerName = name;
    }
}
