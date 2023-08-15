using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    #region variables

    public string playerName;

    private TokenController selectedToken;

    private UIManager UIManager;
    private GameManager gameManager;

    private LineRenderer lineRenderer;
    private Text rulerDistance;
    private const float METER_TO_FEET_RATIO = 3.281f;
    #endregion

    #region unity event functions

    private void Awake()
    {
        UIManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
        lineRenderer = GetComponent<LineRenderer>();
        rulerDistance = UIManager.GetRulerDistanceText();
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
        InteractWithRuler();
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

    private void InteractWithRuler()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

        if (hasHit)
        {
            if (Input.GetKey(KeyCode.R) && Input.GetMouseButtonDown(0))
            {
                lineRenderer.SetPosition(0, hit.point + Vector3.up * 0.25f);
            }

            if (Input.GetKey(KeyCode.R) && Input.GetMouseButton(0))
            {
                lineRenderer.SetPosition(1, hit.point + Vector3.up * 0.25f);

                float distanceValue = Vector3.Distance(lineRenderer.GetPosition(0) - Vector3.up * 0.25f, lineRenderer.GetPosition(1) - Vector3.up * 0.25f) * METER_TO_FEET_RATIO;
                rulerDistance.text = Mathf.RoundToInt(distanceValue).ToString();
                rulerDistance.GetComponent<RectTransform>().position = Input.mousePosition + new Vector3(0, 30, 0);
            }
        }

        if (!Input.GetKey(KeyCode.R) || !Input.GetMouseButton(0))
        {
            for(int i = 0; i < lineRenderer.positionCount; i++){
                lineRenderer.SetPosition(i, Vector3.zero);
            }
            rulerDistance.text = "";
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
            if (target == null) { continue; }

            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.R))
            {
                SelectToken(target);
            }
        }
    }

    private void InteractWithMovement()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        if (selectedToken == null) { return; }
        if (!IsOwner && !IsHost) { return; }

        RaycastHit hit;
        bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

        if (hasHit)
        {
            WalkableZone terrain = hit.transform.GetComponent<WalkableZone>();

            if (terrain == null) { return; }

            if (Input.GetMouseButtonDown(0))
            {
                if (playerName == selectedToken.ownerName.Value)
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
