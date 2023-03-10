//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{

    #region Variables

    public diceType type;

    private Rigidbody rb;
    [SerializeField] private GameObject sides;

    public string thrownBy;
    public int modifier;
    public string line;

    [SerializeField] private float throwForce = 2.0f; 

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 throwDir = GetThrowDirection(Vector3.zero);

        TorqueDice();
        ThrowDice(throwDir);
    }

    void TorqueDice()
    {
        float dirX = Random.Range(0, 500);
        float dirY = Random.Range(0, 500);
        float dirZ = Random.Range(0, 500);

        rb.AddTorque(dirX, dirY, dirZ);
    }

    void ThrowDice(Vector3 throwDir)
    {
        Vector3 velocity = new Vector3(throwDir.x, 0, throwDir.z) * throwForce;

        rb.AddForce(velocity);
    }

    Vector3 GetThrowDirection(Vector3 tablePos)
    {
        return tablePos - transform.position;
    }

    public bool IsStopped()
    {
        return (rb.velocity.x == 0 && rb.velocity.y == 0 && rb.velocity.z == 0);
    }

    public int GetDiceScore()
    {
        int result = 0;
        Transform lowerSide = transform;
        lowerSide.position = Vector3.zero;

        foreach(Transform side in sides.transform)
        {
            if (side.position.y < lowerSide.position.y)
            {
                lowerSide = side;
            }
        }

        switch (type)
        {
            case diceType.d4:
                result = GetD4Result(lowerSide);
                break;
            case diceType.d6:
                result = GetD6Result(lowerSide);
                break;
            case diceType.d8:
                 result = GetD8Result(lowerSide);
                break;
            case diceType.d10:
                result = GetD10Result(lowerSide);
                break;
            case diceType.pd:
                result = GetDPResult(lowerSide);
                break;
            case diceType.d12:
                result = GetD12Result(lowerSide);
                break;
            case diceType.d20:
                result = GetD20Result(lowerSide);
                break;
            default:
                Debug.LogError("Something went wrong when determining dice type");
                break;
        }
        
        return result;
    }

    private int GetD4Result(Transform side)
    {
        switch (side.name)
        {
            case "Side1":
                return 1;
            case "Side2":
                return 2;
            case "Side3":
                return 3;
            case "Side4":
                return 4;
        }
        return -1;
    }

    private int GetD6Result(Transform side)
    {
        switch (side.name)
        {
            case "Side1":
                return 6;
            case "Side2":
                return 5;
            case "Side3":
                return 4;
            case "Side4":
                return 3;
            case "Side5":
                return 2;
            case "Side6":
                return 1;
        }
        return -1;
    }

    private int GetD8Result(Transform side)
    {
        switch (side.name)
        {
            case "Side1":
                return 8;
            case "Side2":
                return 7;
            case "Side3":
                return 6;
            case "Side4":
                return 5;
            case "Side5":
                return 4;
            case "Side6":
                return 3;
            case "Side7":
                return 2;
            case "Side8":
                return 1;
        }
        return -1;
    }

    private int GetD10Result(Transform side)
    {
        switch (side.name)
        {
            case "Side1":
                return 0;
            case "Side2":
                return 9;
            case "Side3":
                return 8;
            case "Side4":
                return 7;
            case "Side5":
                return 6;
            case "Side6":
                return 5;
            case "Side7":
                return 4;
            case "Side8":
                return 3;
            case "Side9":
                return 2;
            case "Side10":
                return 1;
        }
        return -1;
    }

    private int GetDPResult(Transform side)
    {
        switch (side.name)
        {
            case "Side10":
                return 00;
            case "Side20":
                return 90;
            case "Side30":
                return 80;
            case "Side40":
                return 70;
            case "Side50":
                return 60;
            case "Side60":
                return 50;
            case "Side70":
                return 40;
            case "Side80":
                return 30;
            case "Side90":
                return 20;
            case "Side00":
                return 10;
        }
        return -1;
    }

    private int GetD12Result(Transform side)
    {
        switch (side.name)
        {
            case "Side1":
                return 12;
            case "Side2":
                return 11;
            case "Side3":
                return 10;
            case "Side4":
                return 9;
            case "Side5":
                return 8;
            case "Side6":
                return 7;
            case "Side7":
                return 6;
            case "Side8":
                return 5;
            case "Side9":
                return 4;
            case "Side10":
                return 3;
            case "Side11":
                return 2;
            case "Side12":
                return 1;
        }
        return -1;
    }

    private int GetD20Result(Transform side)
    {
        switch (side.name)
        {
            case "Side1":
                return 20;
            case "Side2":
                return 19;
            case "Side3":
                return 18;
            case "Side4":
                return 17;
            case "Side5":
                return 16;
            case "Side6":
                return 15;
            case "Side7":
                return 14;
            case "Side8":
                return 13;
            case "Side9":
                return 12;
            case "Side10":
                return 11;
            case "Side11":
                return 10;
            case "Side12":
                return 9;
            case "Side13":
                return 8;
            case "Side14":
                return 7;
            case "Side15":
                return 6;
            case "Side16":
                return 5;
            case "Side17":
                return 4;
            case "Side18":
                return 3;
            case "Side19":
                return 2;
            case "Side20":
                return 1;
        }
        return -1;
    }

}


public enum diceType
{
    d4 = 0,
    d6 = 1,
    d8 = 2,
    d10 = 3,
    pd = 4,
    d12 = 5,
    d20 = 6
}
