using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DiceScoreCheck : NetworkBehaviour
{
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
                    Debug.Log("1");
                    result = true;
                    break;
                case "Side2":
                    Debug.Log("2");
                    result = true;
                    break;
                case "Side3":
                    Debug.Log("3");
                    result = true;
                    break;
                case "Side4":
                    Debug.Log("4");
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
                    Debug.Log("6");
                    result = true;
                    break;
                case "Side2":
                    Debug.Log("5");
                    result = true;
                    break;
                case "Side3":
                    Debug.Log("4");
                    result = true;
                    break;
                case "Side4":
                    Debug.Log("3");
                    result = true;
                    break;
                case "Side5":
                    Debug.Log("2");
                    result = true;
                    break;
                case "Side6":
                    Debug.Log("1");
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
                    Debug.Log("8");
                    result = true;
                    break;
                case "Side2":
                    Debug.Log("7");
                    result = true;
                    break;
                case "Side3":
                    Debug.Log("6");
                    result = true;
                    break;
                case "Side4":
                    Debug.Log("5");
                    result = true;
                    break;
                case "Side5":
                    Debug.Log("4");
                    result = true;
                    break;
                case "Side6":
                    Debug.Log("3");
                    result = true;
                    break;
                case "Side7":
                    Debug.Log("2");
                    result = true;
                    break;
                case "Side8":
                    Debug.Log("1");
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
                    Debug.Log("10");
                    result = true;
                    break;
                case "Side2":
                    Debug.Log("9");
                    result = true;
                    break;
                case "Side3":
                    Debug.Log("8");
                    result = true;
                    break;
                case "Side4":
                    Debug.Log("7");
                    result = true;
                    break;
                case "Side5":
                    Debug.Log("6");
                    result = true;
                    break;
                case "Side6":
                    Debug.Log("5");
                    result = true;
                    break;
                case "Side7":
                    Debug.Log("4");
                    result = true;
                    break;
                case "Side8":
                    Debug.Log("3");
                    result = true;
                    break;
                case "Side9":
                    Debug.Log("2");
                    result = true;
                    break;
                case "Side10":
                    Debug.Log("1");
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
                    Debug.Log("00");
                    result = true;
                    break;
                case "Side20":
                    Debug.Log("90");
                    result = true;
                    break;
                case "Side30":
                    Debug.Log("80");
                    result = true;
                    break;
                case "Side40":
                    Debug.Log("70");
                    result = true;
                    break;
                case "Side50":
                    Debug.Log("60");
                    result = true;
                    break;
                case "Side60":
                    Debug.Log("50");
                    result = true;
                    break;
                case "Side70":
                    Debug.Log("40");
                    result = true;
                    break;
                case "Side80":
                    Debug.Log("30");
                    result = true;
                    break;
                case "Side90":
                    Debug.Log("20");
                    result = true;
                    break;
                case "Side00":
                    Debug.Log("10");
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
                    Debug.Log("12");
                    result = true;
                    break;
                case "Side2":
                    Debug.Log("11");
                    result = true;
                    break;
                case "Side3":
                    Debug.Log("10");
                    result = true;
                    break;
                case "Side4":
                    Debug.Log("9");
                    result = true;
                    break;
                case "Side5":
                    Debug.Log("8");
                    result = true;
                    break;
                case "Side6":
                    Debug.Log("7");
                    result = true;
                    break;
                case "Side7":
                    Debug.Log("6");
                    result = true;
                    break;
                case "Side8":
                    Debug.Log("5");
                    result = true;
                    break;
                case "Side9":
                    Debug.Log("4");
                    result = true;
                    break;
                case "Side10":
                    Debug.Log("3");
                    result = true;
                    break;
                case "Side11":
                    Debug.Log("2");
                    result = true;
                    break;
                case "Side12":
                    Debug.Log("1");
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
                    Debug.Log("20");
                    result = true;
                    break;
                case "Side2":
                    Debug.Log("19");
                    result = true;
                    break;
                case "Side3":
                    Debug.Log("18");
                    result = true;
                    break;
                case "Side4":
                    Debug.Log("17");
                    result = true;
                    break;
                case "Side5":
                    Debug.Log("16");
                    result = true;
                    break;
                case "Side6":
                    Debug.Log("15");
                    result = true;
                    break;
                case "Side7":
                    Debug.Log("14");
                    result = true;
                    break;
                case "Side8":
                    Debug.Log("13");
                    result = true;
                    break;
                case "Side9":
                    Debug.Log("12");
                    result = true;
                    break;
                case "Side10":
                    Debug.Log("11");
                    result = true;
                    break;
                case "Side11":
                    Debug.Log("10");
                    result = true;
                    break;
                case "Side12":
                    Debug.Log("9");
                    result = true;
                    break;
                case "Side13":
                    Debug.Log("8");
                    result = true;
                    break;
                case "Side14":
                    Debug.Log("7");
                    result = true;
                    break;
                case "Side15":
                    Debug.Log("6");
                    result = true;
                    break;
                case "Side16":
                    Debug.Log("5");
                    result = true;
                    break;
                case "Side17":
                    Debug.Log("4");
                    result = true;
                    break;
                case "Side18":
                    Debug.Log("3");
                    result = true;
                    break;
                case "Side19":
                    Debug.Log("2");
                    result = true;
                    break;
                case "Side20":
                    Debug.Log("1");
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
        //dice.transform.rotation = Quaternion.identity;
        //dice.ResetPosition();          
        result = false;
        reseting = false;
    }
}
