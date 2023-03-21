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

    public void AddRoll(string rollKey, string thrownBy, int numberOfDices, string rollMessage)
    {
        if (activeRolls.ContainsKey(rollKey)) { return; }

        DiceRollInfo newRoll;
        newRoll.playerName = thrownBy;
        newRoll.numberOfDices = numberOfDices;
        newRoll.rolledDices = 0;
        newRoll.diceScores = new int[numberOfDices];
        newRoll.message = rollMessage;

        activeRolls.Add(rollKey, newRoll);
    }

    public DiceRollInfo GetRollInfo(string rollKey)
    {
        return activeRolls[rollKey];
    }

    private void UpdateRoll(string rollKey, int result, int modifier, Action<string, int> resultFunction)
    {
        if (!activeRolls.ContainsKey(rollKey)) { return; }

        DiceRollInfo roll = activeRolls[rollKey];

        roll.rolledDices += 1;

        for (int i = 0; i < roll.numberOfDices; i++)
        {
            if (roll.diceScores[i] < 1)
            {
                roll.diceScores[i] = result;
                break;
            }
        }

        if (roll.numberOfDices == roll.rolledDices)
        {
            resultFunction(rollKey, modifier);
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

    public IEnumerator RollDice(string rollKey, diceType type, int modifier, Action<string, int> resultFunction)
    {
        yield return new WaitForSeconds(0.25f);

        GameObject dice = new GameObject();
        int result = 0;

        switch (type)
        {
            case diceType.d4:
                dice = Instantiate(d4Prefab, throwPoint.position, Quaternion.identity);
                dice.GetComponent<NetworkObject>().Spawn();

                Dice d4 = dice.GetComponent<Dice>();

                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(d4.IsStopped);

                result = d4.GetDiceScore();
                break;

            case diceType.d6:
                dice = Instantiate(d6Prefab, throwPoint.position, Quaternion.identity);
                dice.GetComponent<NetworkObject>().Spawn();

                Dice d6 = dice.GetComponent<Dice>();

                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(d6.IsStopped);

                result = d6.GetDiceScore();
                break;
            case diceType.d8:
                dice = Instantiate(d8Prefab, throwPoint.position, Quaternion.identity);
                dice.GetComponent<NetworkObject>().Spawn();

                Dice d8 = dice.GetComponent<Dice>();

                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(d8.IsStopped);

                result = d8.GetDiceScore();
                break;
            case diceType.d10:
                dice = Instantiate(d10Prefab, throwPoint.position, Quaternion.identity);
                dice.GetComponent<NetworkObject>().Spawn();

                Dice d10 = dice.GetComponent<Dice>();

                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(d10.IsStopped);

                result = d10.GetDiceScore();
                break;
            case diceType.pd:
                dice = Instantiate(pdPrefab, throwPoint.position, Quaternion.identity);
                dice.GetComponent<NetworkObject>().Spawn();

                Dice pd = dice.GetComponent<Dice>();

                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(pd.IsStopped);

                result = pd.GetDiceScore();
                break;
            case diceType.d12:
                dice = Instantiate(d12Prefab, throwPoint.position, Quaternion.identity);
                dice.GetComponent<NetworkObject>().Spawn();

                Dice d12 = dice.GetComponent<Dice>();

                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(d12.IsStopped);

                result = d12.GetDiceScore();
                break;
            case diceType.d20:
                dice = Instantiate(d20Prefab, throwPoint.position, Quaternion.identity);
                dice.GetComponent<NetworkObject>().Spawn();

                Dice d20 = dice.GetComponent<Dice>();

                yield return new WaitForSeconds(1f);

                yield return new WaitUntil(d20.IsStopped);

                result = d20.GetDiceScore();
                break;
        }

        UpdateRoll(rollKey, result, modifier, resultFunction);

        yield return new WaitForSeconds(1f);

        dice.GetComponent<NetworkObject>().Despawn();
        Destroy(dice.gameObject);
    }

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

    #endregion
}
public struct DiceRollInfo
{
    public string playerName;
    public int numberOfDices;
    public int rolledDices;
    public int[] diceScores;
    public string message;
}