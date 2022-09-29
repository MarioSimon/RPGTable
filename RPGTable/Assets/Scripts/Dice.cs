using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{

    #region Variables

    [SerializeField] private diceType type;
    [SerializeField] private float throwForce = 1.0f;
    private Rigidbody rb;

    //private Vector3 startPos;
    //private Vector3 prevPos;
    //bool selected;

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
