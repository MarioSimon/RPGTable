//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Dice : MonoBehaviour
{

    #region Variables

    public DiceType type;

    private Rigidbody rb;
    [SerializeField] private GameObject sides;

    [SerializeField] private float throwForce = 20.0f; 

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 throwDir = GetThrowDirection();

        RandomRotation();
        TorqueDice();
        ThrowDice(throwDir);
    }

    private void TorqueDice()
    {
        float dirX = Random.Range(200, 1000);
        float dirY = Random.Range(200, 1000);
        float dirZ = Random.Range(200, 1000);

        rb.AddTorque(dirX, dirY, dirZ);
    }

    private void RandomRotation()
    {
        int a = Random.Range(0, 360);
        int b = Random.Range(0, 360);
        int g = Random.Range(0, 360);

        transform.eulerAngles = new Vector3(a, b, g);
    }

    private void ThrowDice(Vector3 throwDir)
    {
        Vector3 velocity = throwDir * throwForce;

        rb.AddForce(velocity);
    }

    private Vector3 GetThrowDirection()
    {
        float dirX = Random.Range(-1f, 1f);
        float dirZ = Random.Range(0f, 1f);

        return new Vector3(dirX, 0, dirZ);
    }

    public bool IsStopped()
    {
        return (rb.velocity.x == 0 && rb.velocity.y == 0 && rb.velocity.z == 0);
    }

    public int GetDiceScore()
    {
        int result = 0;
        Transform lowerSide = transform;

        foreach(Transform side in sides.transform)
        {
            if (side.position.y < lowerSide.position.y)
            {
                lowerSide = side;
            }
        }

        switch (type)
        {
            case DiceType.d4:
                result = GetD4Result(lowerSide);
                break;
            case DiceType.d6:
                result = GetD6Result(lowerSide);
                break;
            case DiceType.d8:
                 result = GetD8Result(lowerSide);
                break;
            case DiceType.d10:
                result = GetD10Result(lowerSide);
                break;
            case DiceType.pd:
                result = GetDPResult(lowerSide);
                break;
            case DiceType.d12:
                result = GetD12Result(lowerSide);
                break;
            case DiceType.d20:
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


public enum DiceType
{
    d4 = 0,
    d6 = 1,
    d8 = 2,
    d10 = 3,
    pd = 4,
    d12 = 5,
    d20 = 6
}
