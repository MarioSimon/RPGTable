using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DiceHandler : NetworkBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject d4Prefab;
    [SerializeField] private GameObject d6Prefab;
    [SerializeField] private GameObject d8Prefab;
    [SerializeField] private GameObject d10Prefab;
    [SerializeField] private GameObject pdPrefab;
    [SerializeField] private GameObject d12Prefab;
    [SerializeField] private GameObject d20Prefab;

    private Dictionary<string, DiceRollInfo> activeRolls;

    [SerializeField] private GameObject diceCam;
    [SerializeField] private Transform throwPoint;

    private void Start()
    {
        activeRolls = new Dictionary<string, DiceRollInfo>();
    }

    private void Update()
    {
        if (!IsHost) { return; }

        if (activeRolls.Count > 0 && !diceCam.activeInHierarchy)
        {
            diceCam.SetActive(true);
            ShowDiceBoxClientRpc();
        }
        else if (activeRolls.Count == 0 && diceCam.activeInHierarchy)
        {
            diceCam.SetActive(false);
            HideDiceBoxClientRpc();
        }
    }

    public string GetNewRollKey(string keyPrefix)
    {
        string rollKey = "";
        do
        {
            rollKey = keyPrefix + UnityEngine.Random.Range(int.MinValue, int.MaxValue).ToString();
        } while (activeRolls.ContainsKey(rollKey));

        return rollKey;
    }

    public void AddRoll(string rollKey, string thrownBy, int numberOfDices, string rollMessage, DiceType[] diceTypes = default)
    {
        if (activeRolls.ContainsKey(rollKey)) { return; }

        DiceRollInfo newRoll;
        newRoll.playerName = thrownBy;
        newRoll.numberOfDices = numberOfDices;
        newRoll.rolledDices = 0;
        newRoll.diceScores = new DiceResult[numberOfDices];
        newRoll.diceTypes = diceTypes;
        newRoll.message = rollMessage;

        activeRolls.Add(rollKey, newRoll);
    }

    public DiceRollInfo GetRollInfo(string rollKey)
    {
        return activeRolls[rollKey];
    }

    private void UpdateRoll(string rollKey, DiceResult diceResult, int modifier, ClientRpcParams clientRpcParams, Action<string, int, ClientRpcParams, int> resultFunction, int sheetID)
    {
        if (!activeRolls.ContainsKey(rollKey)) { return; }

        DiceRollInfo roll = activeRolls[rollKey];

        roll.rolledDices += 1;

        for (int i = 0; i < roll.numberOfDices; i++)
        {
            if (roll.diceScores[i].result < 1)
            {
                roll.diceScores[i] = diceResult;
                break;
            }
        }

        if (roll.numberOfDices == roll.rolledDices)
        {
            resultFunction(rollKey, modifier, clientRpcParams, sheetID);
        }
        else
        {
            activeRolls[rollKey] = roll;
        }
    }

    public void DeleteRoll(string rollKey)
    {
        if (activeRolls.ContainsKey(rollKey))
        {
            activeRolls.Remove(rollKey);
        }
    }

    public IEnumerator RollDice(string rollKey, DiceType type, int modifier, ClientRpcParams clientRpcParams, Action<string, int, ClientRpcParams, int> resultFunction, int sheetID = -1)
    {
        yield return new WaitForSeconds(0.25f);

        GameObject dice = new GameObject();
        DiceResult result;
        result.result = -1;
        result.diceType = DiceType.pd;

        switch (type)
        {
            case DiceType.d4:
                dice = Instantiate(d4Prefab, throwPoint.position, Quaternion.identity);
                dice.GetComponent<NetworkObject>().Spawn();

                Dice d4 = dice.GetComponent<Dice>();

                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(d4.IsStopped);

                result.result = d4.GetDiceScore();
                result.diceType = DiceType.d4;
                break;

            case DiceType.d6:
                dice = Instantiate(d6Prefab, throwPoint.position, Quaternion.identity);
                dice.GetComponent<NetworkObject>().Spawn();

                Dice d6 = dice.GetComponent<Dice>();

                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(d6.IsStopped);

                result.result = d6.GetDiceScore();
                result.diceType = DiceType.d6;
                break;
            case DiceType.d8:
                dice = Instantiate(d8Prefab, throwPoint.position, Quaternion.identity);
                dice.GetComponent<NetworkObject>().Spawn();

                Dice d8 = dice.GetComponent<Dice>();

                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(d8.IsStopped);

                result.result = d8.GetDiceScore();
                result.diceType = DiceType.d8;
                break;
            case DiceType.d10:
                dice = Instantiate(d10Prefab, throwPoint.position, Quaternion.identity);
                dice.GetComponent<NetworkObject>().Spawn();

                Dice d10 = dice.GetComponent<Dice>();

                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(d10.IsStopped);

                result.result = d10.GetDiceScore();
                result.diceType = DiceType.d10;
                break;
            case DiceType.pd:
                dice = Instantiate(pdPrefab, throwPoint.position, Quaternion.identity);
                dice.GetComponent<NetworkObject>().Spawn();

                Dice pd = dice.GetComponent<Dice>();

                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(pd.IsStopped);

                result.result = pd.GetDiceScore();
                result.diceType = DiceType.pd;
                break;
            case DiceType.d12:
                dice = Instantiate(d12Prefab, throwPoint.position, Quaternion.identity);
                dice.GetComponent<NetworkObject>().Spawn();

                Dice d12 = dice.GetComponent<Dice>();

                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(d12.IsStopped);

                result.result = d12.GetDiceScore();
                result.diceType = DiceType.d12;
                break;
            case DiceType.d20:
                dice = Instantiate(d20Prefab, throwPoint.position, Quaternion.identity);
                dice.GetComponent<NetworkObject>().Spawn();

                Dice d20 = dice.GetComponent<Dice>();

                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(d20.IsStopped);

                result.result = d20.GetDiceScore();
                result.diceType = DiceType.d20;
                break;
        }

        UpdateRoll(rollKey, result, modifier, clientRpcParams, resultFunction, sheetID);

        yield return new WaitForSeconds(1f);

        dice.GetComponent<NetworkObject>().Despawn();
        Destroy(dice.gameObject);
    }

    #region character sheet rolls

    public void RollCheck(string characterName, string abilitySkill, int modifier)
    {
        RollCheckServerRpc(characterName, abilitySkill, modifier);
    }

    public void RollAbilitySave(string characterName, string ability, int modifier)
    {
        RollSaveServerRpc(characterName, ability, modifier);
    }

    public void RollHitDice(string characterName, DiceType hitDie, int sheetID, int modifier)
    {
        RollHitDiceServerRpc(characterName, hitDie, sheetID, modifier);
    }

    public void RollDeathSavingThrow(string characterName, int sheetID)
    {
        RollDeathSavingThrowServerRpc(characterName, sheetID);
    }

    public void RollAttackAction(AttackRollInfo attackRollInfo)
    {
        RollAttackActionServerRpc(attackRollInfo);
    }

    public void RollActionDamage(AttackRollInfo attackRollInfo)
    {
        RollActionDamageServerRpc(attackRollInfo);
    }

    [ServerRpc]
    private void RollAttackActionServerRpc(AttackRollInfo attackRollInfo, ServerRpcParams serverRpcParams = default)
    {
        string rollKey = GetNewRollKey(attackRollInfo.characterName + "-");
        string message = "";

        if (attackRollInfo.toHitModifier >= 0)
        {
            message = " [" + attackRollInfo.actionName + " (+" + attackRollInfo.toHitModifier + ")]: ";
        }
        else
        {
            message = " [" + attackRollInfo.actionName + " (" + attackRollInfo.toHitModifier + ")]: ";
        }

        AddRoll(rollKey, attackRollInfo.characterName, 1, message);

        var clientId = serverRpcParams.Receive.SenderClientId;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        StartCoroutine(RollDice(rollKey, DiceType.d20, attackRollInfo.toHitModifier, clientRpcParams, ResolveCheckOrSave));
    }

    [ServerRpc]
    private void RollActionDamageServerRpc(AttackRollInfo attackRollInfo, ServerRpcParams serverRpcParams = default)
    {
        string rollKey = GetNewRollKey(attackRollInfo.characterName + "-");
        string message = "";

        if (attackRollInfo.damage2NumberOfDices == 0 && attackRollInfo.damage2Modifier == 0)
        {
            message = " [" + attackRollInfo.actionName + " attack]: {0} " + attackRollInfo.damage1Type;
        }
        else
        {
            message = " [" + attackRollInfo.actionName + " damage]: {0} " + attackRollInfo.damage1Type + " +  {1} " + attackRollInfo.damage2Type;
        }

        int numDices = attackRollInfo.damage1NumberOfDices + attackRollInfo.damage2NumberOfDices;

        DiceType[] dicetypes = { attackRollInfo.damage1Dice, attackRollInfo.damage2Dice };

        if (numDices > 0)
            AddRoll(rollKey, attackRollInfo.characterName, numDices, message, dicetypes);

        var clientId = serverRpcParams.Receive.SenderClientId;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        for (int i = 0; i < attackRollInfo.damage1NumberOfDices; i++)
        {
            StartCoroutine(RollDice(rollKey, attackRollInfo.damage1Dice, attackRollInfo.damage1Modifier, clientRpcParams, ResolveDamageRoll));
        }

        for (int i = 0; i < attackRollInfo.damage2NumberOfDices; i++)
        {
            StartCoroutine(RollDice(rollKey, attackRollInfo.damage2Dice, attackRollInfo.damage1Modifier, clientRpcParams, ResolveDamageRoll));
        }
    }


    void ResolveCheckOrSave(string rollKey, int modifier, ClientRpcParams clientRpcParams = default, int sheetID = -1)
    {
        DiceRollInfo roll = GetRollInfo(rollKey);
        int result = 0;

        foreach (DiceResult diceScore in roll.diceScores)
        {
            result += diceScore.result;
        }

        result += modifier;

        uiManager.NotifyDiceScoreClientRpc(roll.playerName + roll.message + result.ToString());

        DeleteRoll(rollKey);
    }

    void ResolveDamageRoll(string rollKey, int modifier, ClientRpcParams clientRpcParams = default, int sheetID = -1)
    {
        DiceRollInfo roll = GetRollInfo(rollKey);
        int result1 = 0;
        int result2 = 0;

        foreach (DiceResult diceScore in roll.diceScores)
        {
            if (diceScore.diceType == roll.diceTypes[0])
            {
                result1 += diceScore.result;
            }
            else
            {
                result2 += diceScore.result;
            }
        }
        result1 += modifier;

        string message = string.Format(roll.message, result1, result2);

        uiManager.NotifyDiceScoreClientRpc(roll.playerName + message);

        DeleteRoll(rollKey);

    }

    void ResolveHitDiceRoll(string rollKey, int modifier, ClientRpcParams clientRpcParams = default, int sheetID = -1)
    {
        DiceRollInfo roll = GetRollInfo(rollKey);
        int result = 0;

        foreach (DiceResult diceScore in roll.diceScores)
        {
            result += diceScore.result;
        }

        result += modifier;

        if (result <= 0)
        {
            result = 1;
        }

        HitDiceHealClientRpc(result, sheetID);

        uiManager.NotifyDiceScoreClientRpc(roll.playerName + roll.message + result.ToString());

        DeleteRoll(rollKey);
    }

    void ResolveDeathSavingThrow(string rollKey, int modifier, ClientRpcParams clientRpcParams = default, int sheetID = -1)
    {
        DiceRollInfo roll = GetRollInfo(rollKey);
        int result = 0;

        foreach (DiceResult diceScore in roll.diceScores)
        {
            result += diceScore.result;
        }

        ProcessDeathSavingThrowClientRpc(result, sheetID);
        uiManager.NotifyDiceScoreClientRpc(roll.playerName + roll.message + result.ToString());

        DeleteRoll(rollKey);
    }

    #endregion

    #region serverRpc

    [ServerRpc (RequireOwnership = false)]
    void RollCheckServerRpc(string characterName, string abilitySkill, int modifier, ServerRpcParams serverRpcParams = default)
    {
        string rollKey = GetNewRollKey(characterName + "-");
        string message = "";


        if (modifier >= 0)
        {
            message  = " [" + abilitySkill + " (+" + modifier + ")]: ";
        }
        else
        {
            message = " [" + abilitySkill + " (" + modifier + ")]: ";
        }        
        
        AddRoll(rollKey, characterName, 1, message);

        var clientId = serverRpcParams.Receive.SenderClientId;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId}
            }
        };

        StartCoroutine(RollDice(rollKey, DiceType.d20, modifier, clientRpcParams, ResolveCheckOrSave));
    }

    [ServerRpc(RequireOwnership = false)]
    void RollSaveServerRpc(string characterName, string abilitySkill, int modifier, ServerRpcParams serverRpcParams = default)
    {
        string rollKey = GetNewRollKey(characterName + "-");
        string message = "";


        if (modifier >= 0)
        {
            message = " [" + abilitySkill + " saving throw (+" + modifier + ")]: ";
        }
        else
        {
            message = " [" + abilitySkill + " saving throw (" + modifier + ")]: ";
        }

        AddRoll(rollKey, characterName, 1, message);

        var clientId = serverRpcParams.Receive.SenderClientId;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        StartCoroutine(RollDice(rollKey, DiceType.d20, modifier, clientRpcParams, ResolveCheckOrSave));
    }

    [ServerRpc(RequireOwnership = false)]
    void RollHitDiceServerRpc(string characterName, DiceType hitDie, int modifier, int sheetID, ServerRpcParams serverRpcParams = default)
    {
        string rollKey = GetNewRollKey(characterName + "-");
        string message = "";


        if (modifier >= 0)
        {
            message = " [Hit die (+" + modifier + ")]: ";
        }
        else
        {
            message = " [Hit die (+" + modifier + ")]: ";
        }

        AddRoll(rollKey, characterName, 1, message);

        ClientRpcParams clientRpcParams = new ClientRpcParams();

        StartCoroutine(RollDice(rollKey, hitDie, modifier, clientRpcParams, ResolveHitDiceRoll, sheetID));
    }

    [ServerRpc(RequireOwnership = false)]
    void RollDeathSavingThrowServerRpc(string characterName, int sheetID, ServerRpcParams serverRpcParams = default)
    {
        string rollKey = GetNewRollKey(characterName + "-");
        string message = " [Death saving throw]: ";

        AddRoll(rollKey, characterName, 1, message);

        ClientRpcParams clientRpcParams = new ClientRpcParams();

        StartCoroutine(RollDice(rollKey, DiceType.d20, 0, clientRpcParams, ResolveDeathSavingThrow, sheetID));
    }

    #endregion

    #region clientRpc

    [ClientRpc]
    private void ShowDiceBoxClientRpc()
    {
        if (IsOwner) { return; }

        diceCam.SetActive(true);
    }

    [ClientRpc]
    private void HideDiceBoxClientRpc()
    {
        if (IsOwner) { return; }

        diceCam.SetActive(false);
    }

    [ClientRpc]
    private void HitDiceHealClientRpc(int result, int sheetID)
    {
        CharacterSheetManager[] activeSheets = FindObjectsOfType<CharacterSheetManager>();

        foreach (CharacterSheetManager sheet in activeSheets)
        {
            if (sheetID == sheet.CSInfo.sheetID)
            {
                sheet.HitDiceHeal(result);
            }
        }
    }

    [ClientRpc]
    private void ProcessDeathSavingThrowClientRpc(int result, int sheetID)
    {
        CharacterSheetManager[] activeSheets = FindObjectsOfType<CharacterSheetManager>();

        foreach (CharacterSheetManager sheet in activeSheets)
        {
            if (sheetID == sheet.CSInfo.sheetID)
            {
                sheet.ProcessDeathSavingThrow(result);
            }
        }
    }

    #endregion
}

public struct AttackRollInfo : INetworkSerializable
{
    public int sheetID;

    public string characterName;
    public string actionName;

    public int toHitModifier;

    public int damage1NumberOfDices;
    public DiceType damage1Dice;
    public int damage1Modifier;
    public string damage1Type;

    public int damage2NumberOfDices;
    public DiceType damage2Dice;
    public int damage2Modifier;
    public string damage2Type;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref sheetID);

        serializer.SerializeValue(ref characterName);
        serializer.SerializeValue(ref actionName);

        serializer.SerializeValue(ref toHitModifier);

        serializer.SerializeValue(ref damage1NumberOfDices);
        serializer.SerializeValue(ref damage1Dice);
        serializer.SerializeValue(ref damage1Modifier);
        serializer.SerializeValue(ref damage1Type);

        serializer.SerializeValue(ref damage2NumberOfDices);
        serializer.SerializeValue(ref damage2Dice);
        serializer.SerializeValue(ref damage2Modifier);
        serializer.SerializeValue(ref damage2Type);
    }
}

public struct DiceRollInfo
{
    public string playerName;
    public int numberOfDices;
    public int rolledDices;
    public DiceResult[] diceScores;
    public DiceType[] diceTypes;
    public string message;
}

public struct DiceResult
{
    public DiceType diceType;
    public int result;
}