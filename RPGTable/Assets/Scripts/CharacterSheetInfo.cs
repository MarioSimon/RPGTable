using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class CharacterSheetInfo : INetworkSerializable
{
    #region Variables
    //GameManager gameManager = GameObject.FindObjectOfType<GameManager>();

    // public info variables
    public string characterName;
    public string playerName; 
    public string appearance;

    // basic info variables
    public string clasAndLevel;
    public string subclass;
    public string race;
    public string background;
    public string alignement;
    public string experience;

    public int strScore;
    public int dexScore;
    public int conScore;
    public int intScore;
    public int wisScore;
    public int chaScore;

    public int maxHealthPoints;
    public int currHealthPoints;
    public int tempHealthPoints;
    public int initiativeBonus;
    public int armorClass;
    public int speed;
    public diceType hitDice;
    public int deathSaveSuccesses;
    public int deathSaveFails;

    // skills variables




    #endregion

    #region Methods


    #endregion

    #region NetworkSerializable Methods

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref characterName);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref appearance);
    }

    #endregion
}
