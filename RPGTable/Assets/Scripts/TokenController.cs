using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine.AI;
using UnityEngine.UI;

public class TokenController : NetworkBehaviour
{
    #region variables

    [SerializeField] GameObject tokenModel;
    [SerializeField] GameObject tokenBase;
    [SerializeField] GameObject tokenMenu;
    [SerializeField] GameObject characterSheetPrefab;   

    [SerializeField] Animator animator;
    GameManager gameManager;
    Canvas canvas;

    public CharacterSheetInfo characterSheetInfo;

    public NetworkVariable<FixedString64Bytes> tokenName;
    public NetworkVariable<FixedString64Bytes> ownerName;

    Color selectedColor = new Color(0, 0.75f, 0);
    [SerializeField] Color standardColor;

    bool selected = false;
    GameObject tokenMenuInstance;
    InputField tokenHP;

    bool prone = false;
    #endregion

    #region unity event functions

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        canvas = FindObjectOfType<Canvas>();
    }

    private void Start()
    {
        tokenBase.GetComponent<Renderer>().material.color = standardColor;
    }

    private void Update()
    {
        if (!IsOwner && !IsHost) { return; }

        Test();

        if (Input.GetMouseButtonDown(1))
        {
            InteractWithTokenMenu();
        }

        //if (tokenHP != null && Input.GetKeyDown(KeyCode.Return))
        //{
        //    UpdateTokenHP();
        //}
    }

    #endregion
    private void Test()
    {
        //test damage
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (IsHost)
            {
                TakeDamage(5);
            }
            else
            {
                TakeDamageServerRpc(5);
            }
            Debug.Log(characterSheetInfo.currHealthPoints);
        }
        //test heal
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (IsHost)
            {
                Heal(5, 0);
            }
            else
            {
                HealServerRpc(5, 0);
            }
            Debug.Log(characterSheetInfo.currHealthPoints);
        }
        //test magic
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TriggerMagicAnimation();
        }
        //test CaC
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TriggerCaCAnimation();
        }
        //test attack
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TriggerAttackAnimation();
        }
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

        currentHP -= damage;

        if (temporaryHP < 0)
        {
            temporaryHP = 0;
        }

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        if (currentHP == 0)
        {
            FallProne();
        }
        else
        {
            TriggerDamagedAnimation();
        }

        characterSheetInfo.tempHealthPoints = temporaryHP.ToString();
        characterSheetInfo.currHealthPoints = currentHP.ToString();

        if (tokenHP != null)
        {
            tokenHP.text = currentHP.ToString();
        }

        PropagateHealthPointChangesClientRpc(characterSheetInfo.sheetID, temporaryHP.ToString(), currentHP.ToString());
        FindObjectOfType<GameManager>().SaveCharacterSheetChanges(characterSheetInfo);
    }

    public void Heal(int restoredHP, int newTemporaryHP)
    {
        if (!IsHost) { return; }

        int maxHP = ToInt(characterSheetInfo.maxHealthPoints);
        int temporaryHP = newTemporaryHP;
        int currentHP = ToInt(characterSheetInfo.currHealthPoints);

        currentHP += restoredHP;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        characterSheetInfo.tempHealthPoints = temporaryHP.ToString();
        characterSheetInfo.currHealthPoints = currentHP.ToString();

        if (tokenHP != null)
        {
            tokenHP.text = currentHP.ToString();
        }

        PropagateHealthPointChangesClientRpc(characterSheetInfo.sheetID, temporaryHP.ToString(), currentHP.ToString());
        FindObjectOfType<GameManager>().SaveCharacterSheetChanges(characterSheetInfo);
    }

    private void TriggerMagicAnimation()
    {
        if (IsHost)
        {
            int animation = Random.Range(0, 5);

            switch (animation)
            {
                case 0:
                    animator.SetTrigger("magic1H");
                    break;
                case 1:
                    animator.SetTrigger("magic2H0");
                    break;
                case 2:
                    animator.SetTrigger("magic2H1");
                    break;
                case 3:
                    animator.SetTrigger("magic2H2");
                    break;
                case 4:
                    animator.SetTrigger("magic2H3");
                    break;
            }

            TriggerMagicAnimationClientRpc(animation);
        }
        else
        {
            TriggerMagicAnimationServerRpc();
        }
    }

    private void TriggerDamagedAnimation()
    {
        if (IsHost)
        {
            int animation = Random.Range(0, 3);

            switch (animation)
            {
                case 0:
                    animator.SetTrigger("bodyHit");
                    break;
                case 1:
                    animator.SetTrigger("headHit");
                    break;
                case 2:
                    animator.SetTrigger("ribHit");
                    break;
            }

            TriggerDamagedAnimationClientRpc(animation);
        }
        else
        {
            TriggerDamagedAnimationServerRpc();
        }
    }

    private void TriggerAttackAnimation()
    {
        if (IsHost)
        {
            int animation = Random.Range(0, 3);

            switch (animation)
            {
                case 0:
                    animator.SetTrigger("attack0");
                    break;
                case 1:
                    animator.SetTrigger("attack1");
                    break;
                case 2:
                    animator.SetTrigger("attack2");
                    break;
            }

            TriggerAttackAnimationClientRpc(animation);
        }
        else
        {
            TriggerAttackAnimationServerRpc();
        }
    }

    private void TriggerCaCAnimation()
    {
        if (IsHost)
        {
            TriggerCaCAnimationClientRpc();
        }
        else
        {
            TriggerCaCAnimationServerRpc();
        }
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
            tokenBase.GetComponent<Renderer>().material.color = selectedColor;
        }
        else
        {
            tokenBase.GetComponent<Renderer>().material.color = standardColor;
        }
    }

    #endregion

    #region Movement and Rotation

    public void MoveTo(Vector3 destination)
    {
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
                    tokenHP = null;
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
        //contextMenuHandler.buttonList[6].onClick.AddListener(() => );
        //contextMenuHandler.buttonList[7].onClick.AddListener(() => );
        contextMenuHandler.buttonList[8].onClick.AddListener(() => FallProne());
        contextMenuHandler.buttonList[9].onClick.AddListener(() => GetUp());
        contextMenuHandler.buttonList[10].onClick.AddListener(() => TauntGesture());
        contextMenuHandler.buttonList[11].onClick.AddListener(() => LaughGesture());
        contextMenuHandler.buttonList[12].onClick.AddListener(() => BattlecryGesture());
        contextMenuHandler.buttonList[13].onClick.AddListener(() => ShrugGesture());

        tokenHP = tokenMenu.GetComponentInChildren<InputField>();
        tokenHP = tokenMenu.transform.GetChild(0).GetComponent<InputField>(); ;
        tokenHP.text = characterSheetInfo.currHealthPoints;
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
        MoveDownServerRpc();
    }

    private void SizeUp()
    {

    }

    private void SizeDown()
    {

    }

    private void FallProne()
    {
        FallProneServerRpc();
    }

    private void GetUp()
    {
        GetUpServerRpc();
    }

    private void LaughGesture()
    {
        if (IsHost)
        {
            LaughGestureClientRpc();
        }
        else
        {
            LaughGestureServerRpc();
        }        
    }

    private void ShrugGesture()
    {
        if (IsHost)
        {
            ShrugGestureClientRpc();
        }
        else
        {
            ShrugGestureServerRpc();
        }
    }

    private void BattlecryGesture()
    {
        if (IsHost)
        {
            BattlecryGestureClientRpc();
        }
        else
        {
            BattlecryGestureServerRpc();
        }
    }

    private void DestroyToken()
    {
        tokenHP = null;
        Destroy(tokenMenuInstance);

        DestroyTokenServerRpc();
    }

    private void TauntGesture()
    {
        if (IsHost)
        {
            TauntGestureClientRpc();
        }
        else
        {
            TauntGestureServerRpc();
        }
    }

    private void UpdateTokenHP()
    {
        string HPtext = tokenHP.text;

        if (HPtext.StartsWith("+"))
        {
            HPtext = HPtext.Substring(1);

            int healValue;

            if (!int.TryParse(HPtext, out healValue)) 
            {
                tokenHP.text = characterSheetInfo.currHealthPoints;
                return;
            }
            Heal(healValue, 0);
        }
        else if (HPtext.StartsWith("-"))
        {
            HPtext = HPtext.Substring(1);

            int damageValue;

            if (!int.TryParse(HPtext, out damageValue))
            {
                tokenHP.text = characterSheetInfo.currHealthPoints;
                return;
            }
            TakeDamage(damageValue);
        }
        else
        {
            int HPValue;
            if (!int.TryParse(HPtext, out HPValue))
            {
                tokenHP.text = characterSheetInfo.currHealthPoints;
                return;
            }

            characterSheetInfo.currHealthPoints = HPValue.ToString();

            PropagateHealthPointChangesClientRpc(characterSheetInfo.sheetID, characterSheetInfo.tempHealthPoints, HPValue.ToString());
            FindObjectOfType<GameManager>().SaveCharacterSheetChanges(characterSheetInfo);
        }
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
    public void HealServerRpc(int restoredHP, int newTemporaryHP)
    {
        Heal(restoredHP, newTemporaryHP);
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
    private void TriggerDamagedAnimationServerRpc()
    {
        TriggerDamagedAnimation();
    }

    [ServerRpc]
    private void TriggerAttackAnimationServerRpc()
    {
        TriggerAttackAnimation();
    }

    [ServerRpc]
    private void TriggerMagicAnimationServerRpc()
    {
        TriggerMagicAnimation();
    }

    [ServerRpc]
    private void TriggerCaCAnimationServerRpc()
    {
        TriggerCaCAnimationClientRpc();
    }

    [ServerRpc]
    private void DestroyTokenServerRpc()
    {
        this.gameObject.GetComponent<NetworkObject>().Despawn();
    }

    [ServerRpc]
    private void MoveUpServerRpc()
    {
        transform.position += Vector3.up * 1.5f;
    }

    [ServerRpc]
    private void MoveDownServerRpc()
    {
        transform.position += Vector3.down * 1.5f;
    }

    [ServerRpc]
    private void FallProneServerRpc()
    {
        FallProneClientRpc();
    }

    [ServerRpc]
    private void GetUpServerRpc()
    {
        GetUpClientRpc();
    }

    [ServerRpc]
    private void LaughGestureServerRpc()
    {
        LaughGestureClientRpc();
    }

    [ServerRpc]
    private void ShrugGestureServerRpc()
    {
        ShrugGestureClientRpc();
    }

    [ServerRpc]
    private void BattlecryGestureServerRpc()
    {
        BattlecryGestureClientRpc();
    }

    [ServerRpc]
    private void TauntGestureServerRpc()
    {
        TauntGestureClientRpc();
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
    private void TriggerDamagedAnimationClientRpc(int animation)
    {
        switch (animation)
        {
            case 0:
                animator.SetTrigger("bodyHit");
                break;
            case 1:
                animator.SetTrigger("headHit");
                break;
            case 2:
                animator.SetTrigger("ribHit");
                break;
        }
    }

    [ClientRpc]
    private void TriggerAttackAnimationClientRpc(int animation)
    {
        switch (animation)
        {
            case 0:
                animator.SetTrigger("attack0");
                break;
            case 1:
                animator.SetTrigger("attack1");
                break;
            case 2:
                animator.SetTrigger("attack2");
                break;
        }
    }

    [ClientRpc]
    private void TriggerMagicAnimationClientRpc(int animation)
    {
        switch (animation)
        {
            case 0:
                animator.SetTrigger("magic1H");
                break;
            case 1:
                animator.SetTrigger("magic2H0");
                break;
            case 2:
                animator.SetTrigger("magic2H1");
                break;
            case 3:
                animator.SetTrigger("magic2H2");
                break;
            case 4:
                animator.SetTrigger("magic2H3");
                break;
        }
    }

    [ClientRpc]
    private void TriggerCaCAnimationClientRpc()
    {
        animator.SetTrigger("cac");
    }

    [ClientRpc]
    private void FallProneClientRpc()
    {
        if (prone) { return; }
        prone = true;
        animator.SetTrigger("prone");
    }

    [ClientRpc]
    private void GetUpClientRpc()
    {
        prone = false;
        animator.SetTrigger("getUp");
    }

    [ClientRpc]
    private void LaughGestureClientRpc()
    {
        animator.SetTrigger("laugh");
    }

    [ClientRpc]
    private void ShrugGestureClientRpc()
    {
        animator.SetTrigger("shrug");
    }

    [ClientRpc]
    private void BattlecryGestureClientRpc()
    {
        animator.SetTrigger("battlecry");
    }

    [ClientRpc]
    private void TauntGestureClientRpc()
    {
        animator.SetTrigger("taunt");
    }

    #endregion
}
