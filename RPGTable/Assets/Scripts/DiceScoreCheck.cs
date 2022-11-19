using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DiceScoreCheck : NetworkBehaviour
{
    [SerializeField] UIManager UIManager;

    Dice dice;
    bool result = false;
    bool reseting = false;

    private void OnTriggerStay(Collider col)
    {
        if (!IsHost) { return; }
        GetDiceResult(col);
    }

    private void GetDiceResult(Collider col)
    {
        dice = col.gameObject.GetComponentInParent<Dice>();

        if (dice.IsStopped())
        {
            switch (dice.type)
            {
                case diceType.d4:
                    GetD4Result(col);
                    break;
                case diceType.d6:
                    GetD6Result(col);
                    break;
                case diceType.d8:
                    GetD8Result(col);
                    break;
                case diceType.d10:
                    GetD10Result(col);
                    break;
                case diceType.pd:
                    GetDPResult(col);
                    break;
                case diceType.d12:
                    GetD12Result(col);
                    break;
                case diceType.d20:
                    GetD20Result(col);
                    break;
                default:
                    Debug.LogError("Something went wrong when determining dice type");
                    break;
            }
        }
    }

    private void GetD4Result(Collider col)
    {
        if (!result)
        {
            switch (col.gameObject.name)
            {
                case "Side1":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D4: 1 + " + dice.modifier + " = " + (1 + dice.modifier));
                    result = true;
                    break;
                case "Side2":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D4: 2 + " + dice.modifier + " = " + (2 + dice.modifier));
                    result = true;
                    break;
                case "Side3":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D4: 3 + " + dice.modifier + " = " + (3 + dice.modifier));
                    result = true;
                    break;
                case "Side4":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D4: 4 + " + dice.modifier + " = " + (4 + dice.modifier));
                    result = true;
                    break;
            }
        }
        else if (result && !reseting)
        {
            reseting = true;
            StartCoroutine(ResetDice());
        }
    }

    private void GetD6Result(Collider col)
    {
        if (!result)
        {
            switch (col.gameObject.name)
            {
                case "Side1":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D6: 6");
                    result = true;
                    break;
                case "Side2":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D6: 5");
                    result = true;
                    break;
                case "Side3":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D6: 4");
                    result = true;
                    break;
                case "Side4":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D6: 3");
                    result = true;
                    break;
                case "Side5":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D6: 2");
                    result = true;
                    break;
                case "Side6":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D6: 1");
                    result = true;
                    break;
            }
        }
        else if (result && !reseting)
        {
            reseting = true;
            StartCoroutine(ResetDice());
        }
    }

    private void GetD8Result(Collider col)
    {
        if (!result)
        {
            switch (col.gameObject.name)
            {
                case "Side1":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D8: 8");
                    result = true;
                    break;
                case "Side2":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D8: 7");
                    result = true;
                    break;
                case "Side3":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D8: 6");
                    result = true;
                    break;
                case "Side4":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D8: 5");
                    result = true;
                    break;
                case "Side5":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D8: 4");
                    result = true;
                    break;
                case "Side6":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D8: 3");
                    result = true;
                    break;
                case "Side7":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D8: 2");
                    result = true;
                    break;
                case "Side8":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D8: 1");
                    result = true;
                    break;
            }
        }
        else if (result && !reseting)
        {
            reseting = true;
            StartCoroutine(ResetDice());
        }
    }

    private void GetD10Result(Collider col)
    {
        if (!result)
        {
            switch (col.gameObject.name)
            {
                case "Side1":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D10: 10");
                    result = true;
                    break;
                case "Side2":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D10: 9");
                    result = true;
                    break;
                case "Side3":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D10: 8");
                    result = true;
                    break;
                case "Side4":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D10: 7");
                    result = true;
                    break;
                case "Side5":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D10: 6");
                    result = true;
                    break;
                case "Side6":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D10: 5");
                    result = true;
                    break;
                case "Side7":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D10: 4");
                    result = true;
                    break;
                case "Side8":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D10: 3");
                    result = true;
                    break;
                case "Side9":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D10: 2");
                    result = true;
                    break;
                case "Side10":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D10: 1");
                    result = true;
                    break;
            }
        }
        else if (result && !reseting)
        {
            reseting = true;
            StartCoroutine(ResetDice());
        }
    }

    private void GetDPResult(Collider col)
    {
        if (!result)
        {
            switch (col.gameObject.name)
            {
                case "Side10":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a percentile dice: 00");
                    result = true;
                    break;
                case "Side20":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a percentile dice: 90");
                    result = true;
                    break;
                case "Side30":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a percentile dice: 80");
                    result = true;
                    break;
                case "Side40":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a percentile dice: 70");
                    result = true;
                    break;
                case "Side50":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a percentile dice: 60");
                    result = true;
                    break;
                case "Side60":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a percentile dice: 50");
                    result = true;
                    break;
                case "Side70":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a percentile dice: 40");
                    result = true;
                    break;
                case "Side80":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a percentile dice: 30");
                    result = true;
                    break;
                case "Side90":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a percentile dice: 20");
                    result = true;
                    break;
                case "Side00":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a percentile dice: 10");
                    result = true;
                    break;
            }
        }
        else if (result && !reseting)
        {
            reseting = true;
            StartCoroutine(ResetDice());
        }
    }

    private void GetD12Result(Collider col)
    {
        if (!result)
        {
            switch (col.gameObject.name)
            {
                case "Side1":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D12: 12");
                    result = true;
                    break;
                case "Side2":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D12: 11");
                    result = true;
                    break;
                case "Side3":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D12: 10");
                    result = true;
                    break;
                case "Side4":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D12: 9");
                    result = true;
                    break;
                case "Side5":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D12: 8");
                    result = true;
                    break;
                case "Side6":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D12: 7");
                    result = true;
                    break;
                case "Side7":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D12: 6");
                    result = true;
                    break;
                case "Side8":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D12: 5");
                    result = true;
                    break;
                case "Side9":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D12: 4");
                    result = true;
                    break;
                case "Side10":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D12: 3");
                    result = true;
                    break;
                case "Side11":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D12: 2");
                    result = true;
                    break;
                case "Side12":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + " rolls a D12: 1");
                    result = true;
                    break;
            }
        }
        else if (result && !reseting)
        {
            reseting = true;
            StartCoroutine(ResetDice());
        }
    }

    private void GetD20Result(Collider col)
    {
        if (!result)
        {
            switch (col.gameObject.name)
            {
                case "Side1":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (20 + dice.modifier));
                    result = true;
                    break;
                case "Side2":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (19 + dice.modifier));
                    result = true;
                    break;
                case "Side3":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (18 + dice.modifier));
                    result = true;
                    break;
                case "Side4":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (17 + dice.modifier));
                    result = true;
                    break;
                case "Side5":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (16 + dice.modifier));
                    result = true;
                    break;
                case "Side6":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (15 + dice.modifier));
                    result = true;
                    break;
                case "Side7":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (14 + dice.modifier));
                    result = true;
                    break;
                case "Side8":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (13 + dice.modifier));
                    result = true;
                    break;
                case "Side9":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (12 + dice.modifier));
                    result = true;
                    break;
                case "Side10":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (11 + dice.modifier));
                    result = true;
                    break;
                case "Side11":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (10+ dice.modifier));
                    result = true;
                    break;
                case "Side12":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (9 + dice.modifier));
                    result = true;
                    break;
                case "Side13":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (8 + dice.modifier));
                    result = true;
                    break;
                case "Side14":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (7 + dice.modifier));
                    result = true;
                    break;
                case "Side15":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (6 + dice.modifier));
                    result = true;
                    break;
                case "Side16":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (5 + dice.modifier));
                    result = true;
                    break;
                case "Side17":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (4 + dice.modifier));
                    result = true;
                    break;
                case "Side18":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (3 + dice.modifier));
                    result = true;
                    break;
                case "Side19":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (2 + dice.modifier));
                    result = true;
                    break;
                case "Side20":
                    UIManager.NotifyDiceScoreClientRpc(dice.thrownBy + dice.line + (1 + dice.modifier));
                    result = true;
                    break;
            }
        }
        else if (result && !reseting)
        {
            reseting = true;
            StartCoroutine(ResetDice());
        }
    }

    private IEnumerator ResetDice()
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(dice.gameObject);        
        result = false;
        reseting = false;
    }
}
