using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System;

[Serializable]
public class CharacterSheetInfo : INetworkSerializable
{
    #region Variables

    public ulong ownerID;
    public int sheetID = -1;

    // public info variables
    public string characterName = "";
    public string playerName = ""; 
    public string appearance = "";
    public int avatarID = 0;

    // page avaliability variables
    public bool publicBasicInfo = true;
    public bool publicSkills = true;
    public bool publicFeatures = true;
    public bool publicInventory = true;
    public bool publicSpells = true;
    public bool publicActions = true;
    public bool publicPersonality = true;

    // basic info variables
    public string clasAndLevel = "";
    public string subclass = "";
    public string race = "";
    public string background = "";
    public string alignement = "";
    public string experience = "";

    public string strScore = "10";
    public string dexScore = "10";
    public string conScore = "10";
    public string intScore = "10";
    public string wisScore = "10";
    public string chaScore = "10";

    public string maxHealthPoints = "0";
    public string currHealthPoints = "0";
    public string tempHealthPoints = "0";
    public string initiativeBonus = "0";
    public string armorClass = "0";
    public string speed = "0";
    public int hitDiceType = 0;
    public int deathSaveSuccesses = 0;
    public int deathSaveFails = 0;

    // skills variables
    public string proficencyBonus = "2";
    public bool strProf = false;
    public bool dexProf = false;
    public bool conProf = false;
    public bool intProf = false;
    public bool wisProf = false;
    public bool chaProf = false;

    public int[] skillProf = new int[18];
    public int[] skillCharacteristic = new int[18] {1, 4, 3, 0, 5, 3, 4, 5, 3, 4, 3, 4, 5, 5, 3, 1, 1, 4 };
    public string[] skillBonus = new string[18];
    public string[] skillTotal = new string[18];

    // features
    public string featuresAndTraits = "";
    public string proficencies = "";

    // inventory
    public int itemCount = 0;

    public string[] itemNames;
    public string[] itemAmounts;
    public string[] itemWeights;

    public string totalWeight = "0";
    public string maxWeight = "0";
    public string copperPieces = "0";
    public string silverPieces = "0";
    public string electrumPieces = "0";
    public string goldPieces = "0";
    public string platinumPieces = "0";

    public int spellCastingAbility = 0;
    public string spellSaveDC = "10";
    public string spellAttackMod = "2";

    public int level0SpellCount = 0;
    public string[] level0SpellNames;
    public bool[] level0PreparedSpells;
    public int[] level0SpellLevel;
    public int[] level0SpellSchool;
    public string[] level0SpellCastingTime;
    public string[] level0SpellRange;
    public string[] level0SpellComponents;
    public string[] level0SpellDuration;
    public string[] level0SpellDescription;

    public int level1SpellCount = 0;
    public string[] level1SpellNames;
    public bool[] level1PreparedSpells;
    public int[] level1SpellLevel;
    public int[] level1SpellSchool;
    public string[] level1SpellCastingTime;
    public string[] level1SpellRange;
    public string[] level1SpellComponents;
    public string[] level1SpellDuration;
    public string[] level1SpellDescription;

    public int level2SpellCount = 0;
    public string[] level2SpellNames;
    public bool[] level2PreparedSpells;
    public int[] level2SpellLevel;
    public int[] level2SpellSchool;
    public string[] level2SpellCastingTime;
    public string[] level2SpellRange;
    public string[] level2SpellComponents;
    public string[] level2SpellDuration;
    public string[] level2SpellDescription;

    public int level3SpellCount = 0;
    public string[] level3SpellNames;
    public bool[] level3PreparedSpells;
    public int[] level3SpellLevel;
    public int[] level3SpellSchool;
    public string[] level3SpellCastingTime;
    public string[] level3SpellRange;
    public string[] level3SpellComponents;
    public string[] level3SpellDuration;
    public string[] level3SpellDescription;

    public int level4SpellCount = 0;
    public string[] level4SpellNames;
    public bool[] level4PreparedSpells;
    public int[] level4SpellLevel;
    public int[] level4SpellSchool;
    public string[] level4SpellCastingTime;
    public string[] level4SpellRange;
    public string[] level4SpellComponents;
    public string[] level4SpellDuration;
    public string[] level4SpellDescription;

    public int level5SpellCount = 0;
    public string[] level5SpellNames;
    public bool[] level5PreparedSpells;
    public int[] level5SpellLevel;
    public int[] level5SpellSchool;
    public string[] level5SpellCastingTime;
    public string[] level5SpellRange;
    public string[] level5SpellComponents;
    public string[] level5SpellDuration;
    public string[] level5SpellDescription;

    public int level6SpellCount = 0;
    public string[] level6SpellNames;
    public bool[] level6PreparedSpells;
    public int[] level6SpellLevel;
    public int[] level6SpellSchool;
    public string[] level6SpellCastingTime;
    public string[] level6SpellRange;
    public string[] level6SpellComponents;
    public string[] level6SpellDuration;
    public string[] level6SpellDescription;

    public int level7SpellCount = 0;
    public string[] level7SpellNames;
    public bool[] level7PreparedSpells;
    public int[] level7SpellLevel;
    public int[] level7SpellSchool;
    public string[] level7SpellCastingTime;
    public string[] level7SpellRange;
    public string[] level7SpellComponents;
    public string[] level7SpellDuration;
    public string[] level7SpellDescription;

    public int level8SpellCount = 0;
    public string[] level8SpellNames;
    public bool[] level8PreparedSpells;
    public int[] level8SpellLevel;
    public int[] level8SpellSchool;
    public string[] level8SpellCastingTime;
    public string[] level8SpellRange;
    public string[] level8SpellComponents;
    public string[] level8SpellDuration;
    public string[] level8SpellDescription;

    public int level9SpellCount = 0;
    public string[] level9SpellNames;
    public bool[] level9PreparedSpells;
    public int[] level9SpellLevel;
    public int[] level9SpellSchool;
    public string[] level9SpellCastingTime;
    public string[] level9SpellRange;
    public string[] level9SpellComponents;
    public string[] level9SpellDuration;
    public string[] level9SpellDescription;

    public int actionCount = 0;
    public string[] actionName;
    public int[] actionType;
    public int[] actionWeapon;
    public int[] actionAttackAbility;
    public string[] actionAttackOtherBonus;
    public bool[] actionAttackProficency;
    public string[] actionD1NumDices;
    public int[] actionD1DiceType;
    public int[] actionD1Ability;
    public string[] actionD1OtherBonus;
    public int[] actionD1Type;
    public string[] actionD2NumDices;
    public int[] actionD2DiceType;
    public int[] actionD2Ability;
    public string[] actionD2OtherBonus;
    public int[] actionD2Type;
    public int[] actionDC;

    public int bonusActionCount = 0;
    public string[] bonusActionName;
    public int[] bonusActionType;
    public int[] bonusActionWeapon;
    public int[] bonusActionAttackAbility;
    public string[] bonusActionAttackOtherBonus;
    public bool[] bonusActionAttackProficency;
    public string[] bonusActionD1NumDices;
    public int[] bonusActionD1DiceType;
    public int[] bonusActionD1Ability;
    public string[] bonusActionD1OtherBonus;
    public int[] bonusActionD1Type;
    public string[] bonusActionD2NumDices;
    public int[] bonusActionD2DiceType;
    public int[] bonusActionD2Ability;
    public string[] bonusActionD2OtherBonus;
    public int[] bonusActionD2Type;
    public int[] bonusActionDC;

    public int reactionCount = 0;
    public string[] reactionName;
    public int[] reactionType;
    public int[] reactionWeapon;
    public int[] reactionAttackAbility;
    public string[] reactionAttackOtherBonus;
    public bool[] reactionAttackProficency;
    public string[] reactionD1NumDices;
    public int[] reactionD1DiceType;
    public int[] reactionD1Ability;
    public string[] reactionD1OtherBonus;
    public int[] reactionD1Type;
    public string[] reactionD2NumDices;
    public int[] reactionD2DiceType;
    public int[] reactionD2Ability;
    public string[] reactionD2OtherBonus;
    public int[] reactionD2Type;
    public int[] reactionDC;

    public string traits = "";
    public string ideals = "";
    public string bonds = "";
    public string flaws = "";

    #endregion

    #region NetworkSerializable Methods

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ownerID);
        serializer.SerializeValue(ref sheetID);

        serializer.SerializeValue(ref characterName);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref appearance);
        serializer.SerializeValue(ref avatarID);

        serializer.SerializeValue(ref publicBasicInfo);
        serializer.SerializeValue(ref publicSkills);
        serializer.SerializeValue(ref publicFeatures);
        serializer.SerializeValue(ref publicInventory);
        serializer.SerializeValue(ref publicSpells);
        serializer.SerializeValue(ref publicActions);
        serializer.SerializeValue(ref publicPersonality);

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
            if (skillBonus[i] == null)
            {
                skillBonus[i] = "";
            }
            serializer.SerializeValue(ref skillBonus[i]);
            if (skillTotal[i] == null)
            {
                skillTotal[i] = "0";
            }
            serializer.SerializeValue(ref skillTotal[i]);
        }

        serializer.SerializeValue(ref featuresAndTraits);
        serializer.SerializeValue(ref proficencies);

        serializer.SerializeValue(ref itemCount);

        if (serializer.IsReader)
        {
            itemNames = new string[itemCount];
            itemAmounts = new string[itemCount];
            itemWeights = new string[itemCount];
        }

        for (int i = 0; i < itemCount; i++)
        {
            if (itemNames[i] == null)
            {
                itemNames[i] = "";
            }
            serializer.SerializeValue(ref itemNames[i]);
            if (itemAmounts[i] == null)
            {
                itemAmounts[i] = "0";
            }
            serializer.SerializeValue(ref itemAmounts[i]);
            if (itemWeights[i] == null)
            {
                itemWeights[i] = "0";
            }
            serializer.SerializeValue(ref itemWeights[i]);
        }

        serializer.SerializeValue(ref totalWeight);
        serializer.SerializeValue(ref maxWeight);
        serializer.SerializeValue(ref copperPieces);
        serializer.SerializeValue(ref silverPieces);
        serializer.SerializeValue(ref electrumPieces);
        serializer.SerializeValue(ref goldPieces);
        serializer.SerializeValue(ref platinumPieces);

        serializer.SerializeValue(ref spellCastingAbility);
        //serializer.SerializeValue(ref spellSavingDC);
        //serializer.SerializeValue(ref spellCastingAbility);

        serializer.SerializeValue(ref level0SpellCount);
        serializer.SerializeValue(ref level1SpellCount);
        serializer.SerializeValue(ref level2SpellCount);
        serializer.SerializeValue(ref level3SpellCount);
        serializer.SerializeValue(ref level4SpellCount);
        serializer.SerializeValue(ref level5SpellCount);
        serializer.SerializeValue(ref level6SpellCount);
        serializer.SerializeValue(ref level7SpellCount);
        serializer.SerializeValue(ref level8SpellCount);
        serializer.SerializeValue(ref level9SpellCount);

        if (serializer.IsReader)
        {
            level0SpellNames = new string[level0SpellCount];
            level0PreparedSpells = new bool[level0SpellCount];
            level0SpellLevel = new int[level0SpellCount];
            level0SpellSchool = new int[level0SpellCount];
            level0SpellCastingTime = new string[level0SpellCount];
            level0SpellRange = new string[level0SpellCount];
            level0SpellComponents = new string[level0SpellCount] ;
            level0SpellDuration = new string[level0SpellCount];
            level0SpellDescription = new string[level0SpellCount];

            level1SpellNames = new string[level1SpellCount];
            level1PreparedSpells = new bool[level1SpellCount];
            level1SpellLevel = new int[level1SpellCount];
            level1SpellSchool = new int[level1SpellCount];
            level1SpellCastingTime = new string[level1SpellCount];
            level1SpellRange = new string[level1SpellCount];
            level1SpellComponents = new string[level1SpellCount];
            level1SpellDuration = new string[level1SpellCount];
            level1SpellDescription = new string[level1SpellCount];

            level2SpellNames = new string[level2SpellCount];
            level2PreparedSpells = new bool[level2SpellCount];
            level2SpellLevel = new int[level2SpellCount];
            level2SpellSchool = new int[level2SpellCount];
            level2SpellCastingTime = new string[level2SpellCount];
            level2SpellRange = new string[level2SpellCount];
            level2SpellComponents = new string[level2SpellCount];
            level2SpellDuration = new string[level2SpellCount];
            level2SpellDescription = new string[level2SpellCount];

            level3SpellNames = new string[level3SpellCount];
            level3PreparedSpells = new bool[level3SpellCount];
            level3SpellLevel = new int[level3SpellCount];
            level3SpellSchool = new int[level3SpellCount];
            level3SpellCastingTime = new string[level3SpellCount];
            level3SpellRange = new string[level3SpellCount];
            level3SpellComponents = new string[level3SpellCount];
            level3SpellDuration = new string[level3SpellCount];
            level3SpellDescription = new string[level3SpellCount];

            level4SpellNames = new string[level4SpellCount];
            level4PreparedSpells = new bool[level4SpellCount];
            level4SpellLevel = new int[level4SpellCount];
            level4SpellSchool = new int[level4SpellCount];
            level4SpellCastingTime = new string[level4SpellCount];
            level4SpellRange = new string[level4SpellCount];
            level4SpellComponents = new string[level4SpellCount];
            level4SpellDuration = new string[level4SpellCount];
            level4SpellDescription = new string[level4SpellCount];

            level5SpellNames = new string[level5SpellCount];
            level5PreparedSpells = new bool[level5SpellCount];
            level5SpellLevel = new int[level5SpellCount];
            level5SpellSchool = new int[level5SpellCount];
            level5SpellCastingTime = new string[level5SpellCount];
            level5SpellRange = new string[level5SpellCount];
            level5SpellComponents = new string[level5SpellCount];
            level5SpellDuration = new string[level5SpellCount];
            level5SpellDescription = new string[level5SpellCount];

            level6SpellNames = new string[level6SpellCount];
            level6PreparedSpells = new bool[level6SpellCount];
            level6SpellLevel = new int[level6SpellCount];
            level6SpellSchool = new int[level6SpellCount];
            level6SpellCastingTime = new string[level6SpellCount];
            level6SpellRange = new string[level6SpellCount];
            level6SpellComponents = new string[level6SpellCount];
            level6SpellDuration = new string[level6SpellCount];
            level6SpellDescription = new string[level6SpellCount];

            level7SpellNames = new string[level7SpellCount];
            level7PreparedSpells = new bool[level7SpellCount];
            level7SpellLevel = new int[level7SpellCount];
            level7SpellSchool = new int[level7SpellCount];
            level7SpellCastingTime = new string[level7SpellCount];
            level7SpellRange = new string[level7SpellCount];
            level7SpellComponents = new string[level7SpellCount];
            level7SpellDuration = new string[level7SpellCount];
            level7SpellDescription = new string[level7SpellCount];

            level8SpellNames = new string[level8SpellCount];
            level8PreparedSpells = new bool[level8SpellCount];
            level8SpellLevel = new int[level8SpellCount];
            level8SpellSchool = new int[level8SpellCount];
            level8SpellCastingTime = new string[level8SpellCount];
            level8SpellRange = new string[level8SpellCount];
            level8SpellComponents = new string[level8SpellCount];
            level8SpellDuration = new string[level8SpellCount];
            level8SpellDescription = new string[level8SpellCount];

            level9SpellNames = new string[level9SpellCount];
            level9PreparedSpells = new bool[level9SpellCount];
            level9SpellLevel = new int[level9SpellCount];
            level9SpellSchool = new int[level9SpellCount];
            level9SpellCastingTime = new string[level9SpellCount];
            level9SpellRange = new string[level9SpellCount];
            level9SpellComponents = new string[level9SpellCount];
            level9SpellDuration = new string[level9SpellCount];
            level9SpellDescription = new string[level9SpellCount];
        }

        for (int i = 0; i < level0SpellCount; i++)
        {
            if (level0SpellNames[i] == null)
            {
                level0SpellNames[i] = "";
            }
            serializer.SerializeValue(ref level0SpellNames[i]);
            serializer.SerializeValue(ref level0PreparedSpells[i]);
            //serializer.SerializeValue(ref level0SpellLevel[i]);
            serializer.SerializeValue(ref level0SpellSchool[i]);
            if (level0SpellCastingTime[i] == null)
            {
                level0SpellCastingTime[i] = "";
            }
            serializer.SerializeValue(ref level0SpellCastingTime[i]);
            if (level0SpellRange[i] == null)
            {
                level0SpellRange[i] = "";
            }
            serializer.SerializeValue(ref level0SpellRange[i]);
            if (level0SpellComponents[i] == null)
            {
                level0SpellComponents[i] = "";
            }
            serializer.SerializeValue(ref level0SpellComponents[i]);
            if (level0SpellDuration[i] == null)
            {
                level0SpellDuration[i] = "";
            }
            serializer.SerializeValue(ref level0SpellDuration[i]);
            if (level0SpellDescription[i] == null)
            {
                level0SpellDescription[i] = "";
            }
            serializer.SerializeValue(ref level0SpellDescription[i]);
        }

        for (int i = 0; i < level1SpellCount; i++)
        {
            if (level1SpellNames[i] == null)
            {
                level1SpellNames[i] = "";
            }
            serializer.SerializeValue(ref level1SpellNames[i]);
            serializer.SerializeValue(ref level1PreparedSpells[i]);
            //serializer.SerializeValue(ref level1SpellLevel[i]);
            serializer.SerializeValue(ref level1SpellSchool[i]);
            if (level1SpellCastingTime[i] == null)
            {
                level1SpellCastingTime[i] = "";
            }
            serializer.SerializeValue(ref level1SpellCastingTime[i]);
            if (level1SpellRange[i] == null)
            {
                level1SpellRange[i] = "";
            }
            serializer.SerializeValue(ref level1SpellRange[i]);
            if (level1SpellComponents[i] == null)
            {
                level1SpellComponents[i] = "";
            }
            serializer.SerializeValue(ref level1SpellComponents[i]);
            if (level1SpellDuration[i] == null)
            {
                level1SpellDuration[i] = "";
            }
            serializer.SerializeValue(ref level1SpellDuration[i]);
            if (level1SpellDescription[i] == null)
            {
                level1SpellDescription[i] = "";
            }
            serializer.SerializeValue(ref level1SpellDescription[i]);
        }

        for (int i = 0; i < level2SpellCount; i++)
        {
            if (level2SpellNames[i] == null)
            {
                level2SpellNames[i] = "";
            }
            serializer.SerializeValue(ref level2SpellNames[i]);
            serializer.SerializeValue(ref level2PreparedSpells[i]);
            //serializer.SerializeValue(ref level2SpellLevel[i]);
            serializer.SerializeValue(ref level2SpellSchool[i]);
            if (level2SpellCastingTime[i] == null)
            {
                level2SpellCastingTime[i] = "";
            }
            serializer.SerializeValue(ref level2SpellCastingTime[i]);
            if (level2SpellRange[i] == null)
            {
                level2SpellRange[i] = "";
            }
            serializer.SerializeValue(ref level2SpellRange[i]);
            if (level2SpellComponents[i] == null)
            {
                level2SpellComponents[i] = "";
            }
            serializer.SerializeValue(ref level2SpellComponents[i]);
            if (level2SpellDuration[i] == null)
            {
                level2SpellDuration[i] = "";
            }
            serializer.SerializeValue(ref level2SpellDuration[i]);
            if (level2SpellDescription[i] == null)
            {
                level2SpellDescription[i] = "";
            }
            serializer.SerializeValue(ref level2SpellDescription[i]);
        }

        for (int i = 0; i < level3SpellCount; i++)
        {
            if (level3SpellNames[i] == null)
            {
                level3SpellNames[i] = "";
            }
            serializer.SerializeValue(ref level3SpellNames[i]);
            serializer.SerializeValue(ref level3PreparedSpells[i]);
            //serializer.SerializeValue(ref level3SpellLevel[i]);
            serializer.SerializeValue(ref level3SpellSchool[i]);
            if (level3SpellCastingTime[i] == null)
            {
                level3SpellCastingTime[i] = "";
            }
            serializer.SerializeValue(ref level3SpellCastingTime[i]);
            if (level3SpellRange[i] == null)
            {
                level3SpellRange[i] = "";
            }
            serializer.SerializeValue(ref level3SpellRange[i]);
            if (level3SpellComponents[i] == null)
            {
                level3SpellComponents[i] = "";
            }
            serializer.SerializeValue(ref level3SpellComponents[i]);
            if (level3SpellDuration[i] == null)
            {
                level3SpellDuration[i] = "";
            }
            serializer.SerializeValue(ref level3SpellDuration[i]);
            if (level3SpellDescription[i] == null)
            {
                level3SpellDescription[i] = "";
            }
            serializer.SerializeValue(ref level3SpellDescription[i]);
        }

        for (int i = 0; i < level4SpellCount; i++)
        {
            if (level4SpellNames[i] == null)
            {
                level4SpellNames[i] = "";
            }
            serializer.SerializeValue(ref level4SpellNames[i]);
            serializer.SerializeValue(ref level4PreparedSpells[i]);
            //serializer.SerializeValue(ref level4SpellLevel[i]);
            serializer.SerializeValue(ref level4SpellSchool[i]);
            if (level4SpellCastingTime[i] == null)
            {
                level4SpellCastingTime[i] = "";
            }
            serializer.SerializeValue(ref level4SpellCastingTime[i]);
            if (level4SpellRange[i] == null)
            {
                level4SpellRange[i] = "";
            }
            serializer.SerializeValue(ref level4SpellRange[i]);
            if (level4SpellComponents[i] == null)
            {
                level4SpellComponents[i] = "";
            }
            serializer.SerializeValue(ref level4SpellComponents[i]);
            if (level4SpellDuration[i] == null)
            {
                level4SpellDuration[i] = "";
            }
            serializer.SerializeValue(ref level4SpellDuration[i]);
            if (level4SpellDescription[i] == null)
            {
                level4SpellDescription[i] = "";
            }
            serializer.SerializeValue(ref level4SpellDescription[i]);
        }

        for (int i = 0; i < level5SpellCount; i++)
        {
            if (level5SpellNames[i] == null)
            {
                level5SpellNames[i] = "";
            }
            serializer.SerializeValue(ref level5SpellNames[i]);
            serializer.SerializeValue(ref level5PreparedSpells[i]);
            //serializer.SerializeValue(ref level5SpellLevel[i]);
            serializer.SerializeValue(ref level5SpellSchool[i]);
            if (level5SpellCastingTime[i] == null)
            {
                level5SpellCastingTime[i] = "";
            }
            serializer.SerializeValue(ref level5SpellCastingTime[i]);
            if (level5SpellRange[i] == null)
            {
                level5SpellRange[i] = "";
            }
            serializer.SerializeValue(ref level5SpellRange[i]);
            if (level5SpellComponents[i] == null)
            {
                level5SpellComponents[i] = "";
            }
            serializer.SerializeValue(ref level5SpellComponents[i]);
            if (level5SpellDuration[i] == null)
            {
                level5SpellDuration[i] = "";
            }
            serializer.SerializeValue(ref level5SpellDuration[i]);
            if (level5SpellDescription[i] == null)
            {
                level5SpellDescription[i] = "";
            }
            serializer.SerializeValue(ref level5SpellDescription[i]);
        }

        for (int i = 0; i < level6SpellCount; i++)
        {
            if (level6SpellNames[i] == null)
            {
                level6SpellNames[i] = "";
            }
            serializer.SerializeValue(ref level6SpellNames[i]);
            serializer.SerializeValue(ref level6PreparedSpells[i]);
            //serializer.SerializeValue(ref level6SpellLevel[i]);
            serializer.SerializeValue(ref level6SpellSchool[i]);
            if (level6SpellCastingTime[i] == null)
            {
                level6SpellCastingTime[i] = "";
            }
            serializer.SerializeValue(ref level6SpellCastingTime[i]);
            if (level6SpellRange[i] == null)
            {
                level6SpellRange[i] = "";
            }
            serializer.SerializeValue(ref level6SpellRange[i]);
            if (level6SpellComponents[i] == null)
            {
                level6SpellComponents[i] = "";
            }
            serializer.SerializeValue(ref level6SpellComponents[i]);
            if (level6SpellDuration[i] == null)
            {
                level6SpellDuration[i] = "";
            }
            serializer.SerializeValue(ref level6SpellDuration[i]);
            if (level6SpellDescription[i] == null)
            {
                level6SpellDescription[i] = "";
            }
            serializer.SerializeValue(ref level6SpellDescription[i]);
        }

        for (int i = 0; i < level7SpellCount; i++)
        {
            if (level7SpellNames[i] == null)
            {
                level7SpellNames[i] = "";
            }
            serializer.SerializeValue(ref level7SpellNames[i]);
            serializer.SerializeValue(ref level7PreparedSpells[i]);
            //serializer.SerializeValue(ref level7SpellLevel[i]);
            serializer.SerializeValue(ref level7SpellSchool[i]);
            if (level7SpellCastingTime[i] == null)
            {
                level7SpellCastingTime[i] = "";
            }
            serializer.SerializeValue(ref level7SpellCastingTime[i]);
            if (level7SpellRange[i] == null)
            {
                level7SpellRange[i] = "";
            }
            serializer.SerializeValue(ref level7SpellRange[i]);
            if (level7SpellComponents[i] == null)
            {
                level7SpellComponents[i] = "";
            }
            serializer.SerializeValue(ref level7SpellComponents[i]);
            if (level7SpellDuration[i] == null)
            {
                level7SpellDuration[i] = "";
            }
            serializer.SerializeValue(ref level7SpellDuration[i]);
            if (level7SpellDescription[i] == null)
            {
                level7SpellDescription[i] = "";
            }
            serializer.SerializeValue(ref level7SpellDescription[i]);
        }

        for (int i = 0; i < level8SpellCount; i++)
        {
            if (level8SpellNames[i] == null)
            {
                level8SpellNames[i] = "";
            }
            serializer.SerializeValue(ref level8SpellNames[i]);
            serializer.SerializeValue(ref level8PreparedSpells[i]);
            //serializer.SerializeValue(ref level8SpellLevel[i]);
            serializer.SerializeValue(ref level8SpellSchool[i]);
            if (level8SpellCastingTime[i] == null)
            {
                level8SpellCastingTime[i] = "";
            }
            serializer.SerializeValue(ref level8SpellCastingTime[i]);
            if (level8SpellRange[i] == null)
            {
                level8SpellRange[i] = "";
            }
            serializer.SerializeValue(ref level8SpellRange[i]);
            if (level8SpellComponents[i] == null)
            {
                level8SpellComponents[i] = "";
            }
            serializer.SerializeValue(ref level8SpellComponents[i]);
            if (level8SpellDuration[i] == null)
            {
                level8SpellDuration[i] = "";
            }
            serializer.SerializeValue(ref level8SpellDuration[i]);
            if (level8SpellDescription[i] == null)
            {
                level8SpellDescription[i] = "";
            }
            serializer.SerializeValue(ref level8SpellDescription[i]);
        }

        for (int i = 0; i < level9SpellCount; i++)
        {
            if (level9SpellNames[i] == null)
            {
                level9SpellNames[i] = "";
            }
            serializer.SerializeValue(ref level9SpellNames[i]);
            serializer.SerializeValue(ref level9PreparedSpells[i]);
            //serializer.SerializeValue(ref level9SpellLevel[i]);
            serializer.SerializeValue(ref level9SpellSchool[i]);
            if (level9SpellCastingTime[i] == null)
            {
                level9SpellCastingTime[i] = "";
            }
            serializer.SerializeValue(ref level9SpellCastingTime[i]);
            if (level9SpellRange[i] == null)
            {
                level9SpellRange[i] = "";
            }
            serializer.SerializeValue(ref level9SpellRange[i]);
            if (level9SpellComponents[i] == null)
            {
                level9SpellComponents[i] = "";
            }
            serializer.SerializeValue(ref level9SpellComponents[i]);
            if (level9SpellDuration[i] == null)
            {
                level9SpellDuration[i] = "";
            }
            serializer.SerializeValue(ref level9SpellDuration[i]);
            if (level9SpellDescription[i] == null)
            {
                level9SpellDescription[i] = "";
            }
            serializer.SerializeValue(ref level9SpellDescription[i]);
        }

        serializer.SerializeValue(ref actionCount);
        if (serializer.IsReader)
        {
            actionName = new string[actionCount];
            actionType = new int[actionCount];
            actionWeapon = new int[actionCount];

            actionAttackAbility = new int[actionCount];
            actionAttackOtherBonus = new string[actionCount];
            actionAttackProficency = new bool[actionCount];

            actionD1NumDices = new string[actionCount];
            actionD1DiceType = new int[actionCount];
            actionD1Ability = new int[actionCount];
            actionD1OtherBonus = new string[actionCount];
            actionD1Type = new int[actionCount];

            actionD2NumDices = new string[actionCount];
            actionD2DiceType = new int[actionCount];
            actionD2Ability = new int[actionCount];
            actionD2OtherBonus = new string[actionCount];
            actionD2Type = new int[actionCount];

            actionDC = new int[actionCount];
        }

        for (int i = 0; i < actionCount; i++)
        {
            if (actionName[i] == null)
            {
                actionName[i] = "";
            }
            serializer.SerializeValue(ref actionName[i]);
            serializer.SerializeValue(ref actionType[i]);
            serializer.SerializeValue(ref actionWeapon[i]);

            serializer.SerializeValue(ref actionAttackAbility[i]);
            if (actionAttackOtherBonus[i] == null)
            {
                actionAttackOtherBonus[i] = "";
            }
            serializer.SerializeValue(ref actionAttackOtherBonus[i]);
            serializer.SerializeValue(ref actionAttackProficency[i]);

            if (actionD1NumDices[i] == null)
            {
                actionD1NumDices[i] = "";
            }
            serializer.SerializeValue(ref actionD1NumDices[i]);
            serializer.SerializeValue(ref actionD1DiceType[i]);
            serializer.SerializeValue(ref actionD1Ability[i]);
            if (actionD1OtherBonus[i] == null)
            {
                actionD1OtherBonus[i] = "";
            }
            serializer.SerializeValue(ref actionD1OtherBonus[i]);
            serializer.SerializeValue(ref actionD1Type[i]);

            if (actionD2NumDices[i] == null)
            {
                actionD2NumDices[i] = "";
            }
            serializer.SerializeValue(ref actionD2NumDices[i]);
            serializer.SerializeValue(ref actionD2DiceType[i]);
            serializer.SerializeValue(ref actionD2Ability[i]);
            if (actionD2OtherBonus[i] == null)
            {
                actionD2OtherBonus[i] = "";
            }
            serializer.SerializeValue(ref actionD2OtherBonus[i]);
            serializer.SerializeValue(ref actionD2Type[i]);

            serializer.SerializeValue(ref actionDC[i]);
        }

        serializer.SerializeValue(ref bonusActionCount);
        if (serializer.IsReader)
        {
            bonusActionName = new string[bonusActionCount];
            bonusActionType = new int[bonusActionCount];
            bonusActionWeapon = new int[bonusActionCount];

            bonusActionAttackAbility = new int[bonusActionCount];
            bonusActionAttackOtherBonus = new string[bonusActionCount];
            bonusActionAttackProficency = new bool[bonusActionCount];

            bonusActionD1NumDices = new string[bonusActionCount];
            bonusActionD1DiceType = new int[bonusActionCount];
            bonusActionD1Ability = new int[bonusActionCount];
            bonusActionD1OtherBonus = new string[bonusActionCount];
            bonusActionD1Type = new int[bonusActionCount];

            bonusActionD2NumDices = new string[bonusActionCount];
            bonusActionD2DiceType = new int[bonusActionCount];
            bonusActionD2Ability = new int[bonusActionCount];
            bonusActionD2OtherBonus = new string[bonusActionCount];
            bonusActionD2Type = new int[bonusActionCount];

            bonusActionDC = new int[bonusActionCount];
        }

        for (int i = 0; i < bonusActionCount; i++)
        {
            if (bonusActionName[i] == null)
            {
                bonusActionName[i] = "";
            }
            serializer.SerializeValue(ref bonusActionName[i]);
            serializer.SerializeValue(ref bonusActionType[i]);
            serializer.SerializeValue(ref bonusActionWeapon[i]);

            serializer.SerializeValue(ref bonusActionAttackAbility[i]);
            if (bonusActionAttackOtherBonus[i] == null)
            {
                bonusActionAttackOtherBonus[i] = "";
            }
            serializer.SerializeValue(ref bonusActionAttackOtherBonus[i]);
            serializer.SerializeValue(ref bonusActionAttackProficency[i]);

            if (bonusActionD1NumDices[i] == null)
            {
                bonusActionD1NumDices[i] = "";
            }
            serializer.SerializeValue(ref bonusActionD1NumDices[i]);
            serializer.SerializeValue(ref bonusActionD1DiceType[i]);
            serializer.SerializeValue(ref bonusActionD1Ability[i]);
            if (bonusActionD1OtherBonus[i] == null)
            {
                bonusActionD1OtherBonus[i] = "";
            }
            serializer.SerializeValue(ref bonusActionD1OtherBonus[i]);
            serializer.SerializeValue(ref bonusActionD1Type[i]);

            if (bonusActionD2NumDices[i] == null)
            {
                bonusActionD2NumDices[i] = "";
            }
            serializer.SerializeValue(ref bonusActionD2NumDices[i]);
            serializer.SerializeValue(ref bonusActionD2DiceType[i]);
            serializer.SerializeValue(ref bonusActionD2Ability[i]);
            if (bonusActionD2OtherBonus[i] == null)
            {
                bonusActionD2OtherBonus[i] = "";
            }
            serializer.SerializeValue(ref bonusActionD2OtherBonus[i]);
            serializer.SerializeValue(ref bonusActionD2Type[i]);

            serializer.SerializeValue(ref bonusActionDC[i]);
        }

        serializer.SerializeValue(ref reactionCount);
        if (serializer.IsReader)
        {
            reactionName = new string[reactionCount];
            reactionType = new int[reactionCount];
            reactionWeapon = new int[reactionCount];

            reactionAttackAbility = new int[reactionCount];
            reactionAttackOtherBonus = new string[reactionCount];
            reactionAttackProficency = new bool[reactionCount];

            reactionD1NumDices = new string[reactionCount];
            reactionD1DiceType = new int[reactionCount];
            reactionD1Ability = new int[reactionCount];
            reactionD1OtherBonus = new string[reactionCount];
            reactionD1Type = new int[reactionCount];

            reactionD2NumDices = new string[reactionCount];
            reactionD2DiceType = new int[reactionCount];
            reactionD2Ability = new int[reactionCount];
            reactionD2OtherBonus = new string[reactionCount];
            reactionD2Type = new int[reactionCount];

            reactionDC = new int[reactionCount];
        }

        for (int i = 0; i < reactionCount; i++)
        {
            if (reactionName[i] == null)
            {
                reactionName[i] = "";
            }
            serializer.SerializeValue(ref reactionName[i]);
            serializer.SerializeValue(ref reactionType[i]);
            serializer.SerializeValue(ref reactionWeapon[i]);

            serializer.SerializeValue(ref reactionAttackAbility[i]);
            if (reactionAttackOtherBonus[i] == null)
            {
                reactionAttackOtherBonus[i] = "";
            }
            serializer.SerializeValue(ref reactionAttackOtherBonus[i]);
            serializer.SerializeValue(ref reactionAttackProficency[i]);

            if (reactionD1NumDices[i] == null)
            {
                reactionD1NumDices[i] = "";
            }
            serializer.SerializeValue(ref reactionD1NumDices[i]);
            serializer.SerializeValue(ref reactionD1DiceType[i]);
            serializer.SerializeValue(ref reactionD1Ability[i]);
            if (reactionD1OtherBonus[i] == null)
            {
                reactionD1OtherBonus[i] = "";
            }
            serializer.SerializeValue(ref reactionD1OtherBonus[i]);
            serializer.SerializeValue(ref reactionD1Type[i]);

            if (reactionD2NumDices[i] == null)
            {
                reactionD2NumDices[i] = "";
            }
            serializer.SerializeValue(ref reactionD2NumDices[i]);
            serializer.SerializeValue(ref reactionD2DiceType[i]);
            serializer.SerializeValue(ref reactionD2Ability[i]);
            if (reactionD2OtherBonus[i] == null)
            {
                reactionD2OtherBonus[i] = "";
            }
            serializer.SerializeValue(ref reactionD2OtherBonus[i]);
            serializer.SerializeValue(ref reactionD2Type[i]);

            serializer.SerializeValue(ref reactionDC[i]);
        }

        serializer.SerializeValue(ref traits);
        serializer.SerializeValue(ref ideals);
        serializer.SerializeValue(ref bonds);
        serializer.SerializeValue(ref flaws);
    }

    #endregion
}
