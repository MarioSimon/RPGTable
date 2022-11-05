using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class CharacterSheetInfo : INetworkSerializable
{
    #region Variables

    public ulong ownerID;
    public int sheetID = -1;

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

    public string strScore;
    public string dexScore;
    public string conScore;
    public string intScore;
    public string wisScore;
    public string chaScore;

    public string maxHealthPoints;
    public string currHealthPoints;
    public string tempHealthPoints;
    public string initiativeBonus;
    public string armorClass;
    public string speed;
    public int hitDiceType;
    public int deathSaveSuccesses = 0;
    public int deathSaveFails = 0;

    // skills variables
    public string proficencyBonus;
    public bool strProf;
    public bool dexProf;
    public bool conProf;
    public bool intProf;
    public bool wisProf;
    public bool chaProf;

    public List<Skill> skillList;
    public int[] skillProf;
    public int[] skillCharacteristic;
    public string[] skillBonus;
    public string[] skillTotal;

    // features
    public string featuresAndTraits;
    public string proficencies;

    // inventory
    public List<GameObject> itemList;

    public string totalWeight;
    public string maxWeight;
    public string copperPieces;
    public string silverPieces;
    public string electrumPieces;
    public string goldPieces;
    public string platinumPieces;



    #endregion


    #region NetworkSerializable Methods

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ownerID);
        serializer.SerializeValue(ref sheetID);

        serializer.SerializeValue(ref characterName);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref appearance);

        serializer.SerializeValue(ref clasAndLevel);
        serializer.SerializeValue(ref subclass);
        serializer.SerializeValue(ref race);
        serializer.SerializeValue(ref background);
        serializer.SerializeValue(ref alignement);
        serializer.SerializeValue(ref experience);

        serializer.SerializeValue(ref strScore);
        serializer.SerializeValue(ref dexScore);
        serializer.SerializeValue(ref conScore);
        serializer.SerializeValue(ref intScore);
        serializer.SerializeValue(ref wisScore);
        serializer.SerializeValue(ref chaScore);

        serializer.SerializeValue(ref maxHealthPoints);
        serializer.SerializeValue(ref currHealthPoints);
        serializer.SerializeValue(ref tempHealthPoints);
        serializer.SerializeValue(ref initiativeBonus);
        serializer.SerializeValue(ref armorClass);
        serializer.SerializeValue(ref speed);
        serializer.SerializeValue(ref hitDiceType);
        serializer.SerializeValue(ref deathSaveSuccesses);
        serializer.SerializeValue(ref deathSaveFails);

        serializer.SerializeValue(ref proficencyBonus);
        serializer.SerializeValue(ref strProf);
        serializer.SerializeValue(ref dexProf);
        serializer.SerializeValue(ref conProf);
        serializer.SerializeValue(ref intProf);
        serializer.SerializeValue(ref wisProf);
        serializer.SerializeValue(ref chaProf);

        int skillLength = 0;

        if (!serializer.IsReader)
        {
            skillLength = skillProf.Length;
        }

        serializer.SerializeValue(ref skillLength);

        if (serializer.IsReader)
        {
            skillProf = new int[skillLength];
            skillCharacteristic = new int[skillLength];
            skillBonus = new string[skillLength];
            skillTotal = new string[skillLength];
        }

        for (int i = 0; i < skillLength; i++)
        {
            serializer.SerializeValue(ref skillProf[i]);
            serializer.SerializeValue(ref skillCharacteristic[i]);
            serializer.SerializeValue(ref skillBonus[i]);
            serializer.SerializeValue(ref skillTotal[i]);
        }

        serializer.SerializeValue(ref featuresAndTraits);
        serializer.SerializeValue(ref proficencies);

        //serializer.SerializeValue(ref itemList);

        serializer.SerializeValue(ref totalWeight);
        serializer.SerializeValue(ref maxWeight);
        serializer.SerializeValue(ref copperPieces);
        serializer.SerializeValue(ref silverPieces);
        serializer.SerializeValue(ref electrumPieces);
        serializer.SerializeValue(ref goldPieces);
        serializer.SerializeValue(ref platinumPieces);
    }

    #endregion
}
