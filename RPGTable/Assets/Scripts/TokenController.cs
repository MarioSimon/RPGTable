using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine.AI;
using System;

public class TokenController : NetworkBehaviour
{
    #region variables

    [SerializeField] GameObject tokenModel;
    [SerializeField] GameObject tokenMenu;
    [SerializeField] GameObject characterSheetPrefab;

    GameManager gameManager;
    Canvas canvas;
    NavMeshAgent navMeshAgent;

    public CharacterSheetInfo characterSheetInfo;

    public NetworkVariable<FixedString64Bytes> tokenName;
    public NetworkVariable<FixedString64Bytes> ownerName;

    Color selectedColor = new Color(0, 0.75f, 0);
    [SerializeField] Color standardColor;

    bool selected = false;
    GameObject tokenMenuInstance;
    #endregion

    #region unity event functions

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        gameManager = FindObjectOfType<GameManager>();
        canvas = FindObjectOfType<Canvas>();
    }

    private void Start()
    {
        tokenModel.GetComponent<Renderer>().material.color = standardColor;
    }

    private void Update()
    {
        if (!IsOwner && !IsHost) { return; }
        //TestDamage();
        if (Input.GetMouseButtonDown(1))
        {
            InteractWithTokenMenu();
        }

        
    }

    #endregion
    private void TestDamage()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (IsHost)
            {
                TakeDamage(5);
            }
            else
            {
                TakeDamageServerRpc(5);
            }
        }
        Debug.Log(characterSheetInfo.currHealthPoints);
    }

    #region character sheet

    private int ToInt(string textValue) 
    {
        int value = -1;

        int.TryParse(textValue, out value);

        return value;
    }

    public void TakeDamage(int damage)
    {
        if (!IsHost) { return; }

        int temporaryHP = ToInt(characterSheetInfo.tempHealthPoints);
        int currentHP = ToInt(characterSheetInfo.currHealthPoints);

        if (temporaryHP > 0)
        {
            int aux = damage;
            damage -= temporaryHP;
            temporaryHP -= aux;
        }

        if (damage > 0)
        {
            currentHP -= damage;
        }

        if (temporaryHP < 0)
        {
            temporaryHP = 0;
        }

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        characterSheetInfo.tempHealthPoints = temporaryHP.ToString();
        characterSheetInfo.currHealthPoints = currentHP.ToString();
        
        PropagateHealthPointChangesClientRpc(characterSheetInfo.sheetID, temporaryHP.ToString(), currentHP.ToString());
        FindObjectOfType<GameManager>().SaveCharacterSheetChanges(characterSheetInfo);
    }
  
    #endregion

    #region Selection

    public void ChangeSelection()
    {
        selected = !selected;
        ChangeColor(selected);
    }

    public void ChangeColor(bool selected)
    {
        if (selected)
        {
            tokenModel.GetComponent<Renderer>().material.color = selectedColor;
        }
        else
        {
            tokenModel.GetComponent<Renderer>().material.color = standardColor;
        }
    }

    #endregion

    #region Movement and Rotation

    public void MoveTo(Vector3 destination)
    {
        //navMeshAgent.Warp(destination);
        transform.position = destination;
    }

    #endregion

    #region context menu functions

    private void InteractWithTokenMenu()
    {

        RaycastHit hit;
        bool hasHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);

        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (hasHit) 
        { 
            TokenController target = hit.transform.GetComponent<TokenController>();
            if (target != this)
            {
                if (tokenMenuInstance != null)
                {
                    Destroy(tokenMenuInstance);
                }

                return;
            }

            OpenTokenMenu();

        }
    }


    public void OpenTokenMenu()
    {
        if (tokenMenuInstance != null)
        {
            Destroy(tokenMenuInstance);
        }

        GameObject contextMenu = Instantiate(tokenMenu, Input.mousePosition, Quaternion.identity);
        contextMenu.GetComponent<RectTransform>().SetParent(canvas.transform);

        ContextMenu contextMenuHandler = contextMenu.GetComponent<ContextMenu>();
        contextMenuHandler.buttonList[0].onClick.AddListener(() => OpenCharacterSheet());
        contextMenuHandler.buttonList[1].onClick.AddListener(() => OpenActionMenu());
        contextMenuHandler.buttonList[2].onClick.AddListener(() => OpenGesturesMenu());
        contextMenuHandler.buttonList[3].onClick.AddListener(() => DestroyToken());
        contextMenuHandler.buttonList[4].onClick.AddListener(() => MoveUp());
        contextMenuHandler.buttonList[5].onClick.AddListener(() => MoveDown());


        tokenMenuInstance = contextMenu;
    }

    private void OpenCharacterSheet()
    {
        Destroy(tokenMenuInstance);

        OpenCharacterSheetServerRpc();
    }

    private void OpenActionMenu()
    {
        CloseSecondaryPannels();
        tokenMenuInstance.GetComponent<ContextMenu>().secondaryPannels[0].SetActive(true);
    }

    private void OpenGesturesMenu()
    {
        CloseSecondaryPannels();
        tokenMenuInstance.GetComponent<ContextMenu>().secondaryPannels[1].SetActive(true);
    }

    private void CloseSecondaryPannels()
    {
        tokenMenuInstance.GetComponent<ContextMenu>().secondaryPannels[0].SetActive(false);
        tokenMenuInstance.GetComponent<ContextMenu>().secondaryPannels[1].SetActive(false);
    }

    private void MoveUp()
    {
        MoveUpServerRpc();
    }
    

    private void MoveDown()
    {
        transform.position += Vector3.down * 1.5f;
    }

    private void SizeUp()
    {

    }

    private void SizeDown()
    {

    }

    private void DestroyToken()
    {
        Destroy(tokenMenuInstance);

        DestroyTokenServerRpc();
    }


    #endregion

    #region ServerRpc

    [ServerRpc(RequireOwnership = false)]
    public void MoveToServerRpc(Vector3 destination)
    {
        MoveTo(destination);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int damage)
    {
        TakeDamage(damage);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OpenCharacterSheetServerRpc()
    {
        if (characterSheetInfo == null) { return; }

        GameObject charSheet = Instantiate(characterSheetPrefab);
        charSheet.GetComponent<CharacterSheetManager>().CSInfo = characterSheetInfo;
        charSheet.GetComponent<RectTransform>().SetParent(canvas.gameObject.transform, false);
    }

    [ServerRpc]
    private void DestroyTokenServerRpc()
    {
        this.gameObject.GetComponent<NetworkObject>().Despawn();
    }

    [ServerRpc]
    private void MoveUpServerRpc()
    {
        MoveUpClientRpc();
    }

    #endregion

    #region clientRpc

    [ClientRpc]
    private void PropagateHealthPointChangesClientRpc(int characterID, string tempHP, string currHP)
    {
        characterSheetInfo.tempHealthPoints = tempHP;
        characterSheetInfo.currHealthPoints = currHP;

        CharacterSheetManager[] activeSheets = FindObjectsOfType<CharacterSheetManager>();

        foreach (CharacterSheetManager sheet in activeSheets)
        {
            if (sheet.CSInfo.sheetID == characterID)
            {
                sheet.tempHealthPoints.text = tempHP;
                sheet.currHealthPoints.text = currHP;
            }
        }
    }

    [ClientRpc]
    private void MoveUpClientRpc()
    {
        transform.position += Vector3.up * 1.5f;
    }
    #endregion
}
