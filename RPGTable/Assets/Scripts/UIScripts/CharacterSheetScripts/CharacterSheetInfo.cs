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

    // page avaliability variables
    public bool publicBasicInfo = true;
    public bool publicSkills = true;
    public bool publicFeatures = true;
    public bool publicInventory = true;
    public bool publicSpells = true;
    public bool publicActions = true;
    public bool publicPersonality = true;

    public PublicPageCharacterInfo publicPageCharacterInfo;
    public BasicPageCharacterInfo basicPageCharacterInfo;
    public SkillsPageCharacterInfo skillsPageCharacterInfo;
    public FeaturesPageCharacterInfo featuresPageCharacterInfo;
    public InventoryPageCharacterInfo inventoryPageCharacterInfo;
    public SpellsPageCharacterInfo spellsPageCharacterInfo;
    public ActionsPageCharacterInfo actionsPageCharacterInfo;
    public PersonalityPageCharacterInfo personalityPageCharacterInfo;

    #endregion

    public CharacterSheetInfo()
    {
        #region public info default values
        publicPageCharacterInfo.characterName = "";
        publicPageCharacterInfo.playerName = "";
        publicPageCharacterInfo.appearance = "";
        publicPageCharacterInfo.avatarID = 0;
        #endregion

        #region basic info default values
        basicPageCharacterInfo.clasAndLevel = "";
        basicPageCharacterInfo.subclass = "";
        basicPageCharacterInfo.race = "";
        basicPageCharacterInfo.background = "";
        basicPageCharacterInfo.alignement = "";
        basicPageCharacterInfo.experience = "";

        basicPageCharacterInfo.strScore = "10";
        basicPageCharacterInfo.dexScore = "10";
        basicPageCharacterInfo.conScore = "10";
        basicPageCharacterInfo.intScore = "10";
        basicPageCharacterInfo.wisScore = "10";
        basicPageCharacterInfo.chaScore = "10";

        basicPageCharacterInfo.maxHealthPoints = "0";
        basicPageCharacterInfo.currHealthPoints = "0";
        basicPageCharacterInfo.tempHealthPoints = "0";
        basicPageCharacterInfo.initiativeBonus = "0";
        basicPageCharacterInfo.armorClass = "0";
        basicPageCharacterInfo.speed = "0";
        basicPageCharacterInfo.hitDiceType = 0;
        basicPageCharacterInfo.deathSaveSuccesses = 0;
        basicPageCharacterInfo.deathSaveFails = 0;
        #endregion

        #region skills default values
        skillsPageCharacterInfo.proficencyBonus = "2";
        skillsPageCharacterInfo.strProf = false;
        skillsPageCharacterInfo.dexProf = false;
        skillsPageCharacterInfo.conProf = false;
        skillsPageCharacterInfo.intProf = false;
        skillsPageCharacterInfo.wisProf = false;
        skillsPageCharacterInfo.chaProf = false;

        skillsPageCharacterInfo.skillProf = new int[18];
        skillsPageCharacterInfo.skillCharacteristic = new int[18] { 1, 4, 3, 0, 5, 3, 4, 5, 3, 4, 3, 4, 5, 5, 3, 1, 1, 4 };
        skillsPageCharacterInfo.skillBonus = new string[18];
        skillsPageCharacterInfo.skillTotal = new string[18];
        #endregion

        #region features default values
        featuresPageCharacterInfo.featuresAndTraits = "";
        featuresPageCharacterInfo.traitCount = 0;
        featuresPageCharacterInfo.proficencies = "";
        #endregion

        #region inventory default values   
        inventoryPageCharacterInfo.itemCount = 0;

        inventoryPageCharacterInfo.totalWeight = "0";
        inventoryPageCharacterInfo.maxWeight = "0";
        inventoryPageCharacterInfo.copperPieces = "0";
        inventoryPageCharacterInfo.silverPieces = "0";
        inventoryPageCharacterInfo.electrumPieces = "0";
        inventoryPageCharacterInfo.goldPieces = "0";
        inventoryPageCharacterInfo.platinumPieces = "0";
        #endregion

        #region spells default vales
        spellsPageCharacterInfo.spellCastingAbility = 0;
        spellsPageCharacterInfo.spellSaveDC = "10";
        spellsPageCharacterInfo.spellAttackMod = "2";
        spellsPageCharacterInfo.spellLevelList = new SpellLevelInfo[10];
        for (int i = 0; i < 10; i++){
            spellsPageCharacterInfo.spellLevelList[i].spellCount = 0;
        }
        #endregion

        #region action default values
        actionsPageCharacterInfo.actionList.actionCount = 0;
        actionsPageCharacterInfo.bonusActionList.actionCount = 0;
        actionsPageCharacterInfo.reactionList.actionCount = 0;
        #endregion

        #region personality default values
        personalityPageCharacterInfo.traits = "";
        personalityPageCharacterInfo.ideals = "";
        personalityPageCharacterInfo.bonds = "";
        personalityPageCharacterInfo.flaws = "";
        #endregion
}

    #region NetworkSerializable Methods

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ownerID);
        serializer.SerializeValue(ref sheetID);

        serializer.SerializeValue(ref publicBasicInfo);
        serializer.SerializeValue(ref publicSkills);
        serializer.SerializeValue(ref publicFeatures);
        serializer.SerializeValue(ref publicInventory);
        serializer.SerializeValue(ref publicSpells);
        serializer.SerializeValue(ref publicActions);
        serializer.SerializeValue(ref publicPersonality);

        serializer.SerializeValue(ref publicPageCharacterInfo);
        serializer.SerializeValue(ref basicPageCharacterInfo);
        serializer.SerializeValue(ref skillsPageCharacterInfo);
        serializer.SerializeValue(ref featuresPageCharacterInfo);
        serializer.SerializeValue(ref inventoryPageCharacterInfo);
        serializer.SerializeValue(ref spellsPageCharacterInfo);
        serializer.SerializeValue(ref actionsPageCharacterInfo);
        serializer.SerializeValue(ref personalityPageCharacterInfo);
    }

    #endregion
}

[Serializable]
public struct PublicPageCharacterInfo : INetworkSerializable
{
    public string characterName;
    public string playerName;
    public string appearance;
    public int avatarID;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref characterName);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref appearance);
        serializer.SerializeValue(ref avatarID);
    }
}

[Serializable]
public struct BasicPageCharacterInfo : INetworkSerializable
{
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
    public int deathSaveSuccesses;
    public int deathSaveFails;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
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
    }
}

[Serializable]
public struct SkillsPageCharacterInfo :INetworkSerializable
{
    public string proficencyBonus;
    public bool strProf;
    public bool dexProf;
    public bool conProf;
    public bool intProf;
    public bool wisProf;
    public bool chaProf;

    public int[] skillProf;
    public int[] skillCharacteristic;
    public string[] skillBonus;
    public string[] skillTotal;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
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
                skillBonus[i] = "0";
            }
            serializer.SerializeValue(ref skillBonus[i]);
            if (skillTotal[i] == null)
            {
                skillTotal[i] = "0";
            }
            serializer.SerializeValue(ref skillTotal[i]);
        }

    }
}

[Serializable]
public struct FeaturesPageCharacterInfo : INetworkSerializable
{
    public string featuresAndTraits;
    public int traitCount;
    public string[] traitName;
    public string[] traitDescription;
    public string proficencies;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref featuresAndTraits);
        serializer.SerializeValue(ref traitCount);
        if (serializer.IsReader)
        {
            traitName = new string[traitCount];
            traitDescription = new string[traitCount];
        }

        for (int i = 0; i < traitCount; i++)
        {
            if (traitName[i] == null)
            {
                traitName[i] = "";
            }
            serializer.SerializeValue(ref traitName[i]);

            if (traitDescription[i] == null)
            {
                traitDescription[i] = "";
            }
            serializer.SerializeValue(ref traitDescription[i]);
        }

        serializer.SerializeValue(ref proficencies);
    }
}

[Serializable]
public struct InventoryPageCharacterInfo : INetworkSerializable
{
    public int itemCount;

    public string[] itemNames;
    public string[] itemAmounts;
    public string[] itemWeights;

    public string totalWeight;
    public string maxWeight;
    public string copperPieces;
    public string silverPieces;
    public string electrumPieces;
    public string goldPieces;
    public string platinumPieces;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
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
    }
}

[Serializable]
public struct SpellsPageCharacterInfo : INetworkSerializable
{
    public int spellCastingAbility;
    public string spellSaveDC;
    public string spellAttackMod;
    public SpellLevelInfo[] spellLevelList;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref spellCastingAbility);

        if (spellSaveDC == null)
        {
            spellSaveDC = "";
        }
        serializer.SerializeValue(ref spellSaveDC);

        if (spellAttackMod == null)
        {
            spellAttackMod = "";
        }
        serializer.SerializeValue(ref spellAttackMod);

        if (serializer.IsReader)
        {
            spellLevelList = new SpellLevelInfo[10];
        }

        for (int i = 0; i < 10; i++)
        {
            serializer.SerializeValue(ref spellLevelList[i]);
        }
    }
}

[Serializable]
public struct SpellLevelInfo : INetworkSerializable
{
    public int spellCount;
    public string[] spellNames;
    public bool[] preparedSpells;
    public int[] spellLevel;
    public int[] spellSchool;
    public string[] spellCastingTime;
    public string[] spellRange;
    public string[] spellComponents;
    public string[] spellDuration;
    public string[] spellDescription;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref spellCount);

        if (serializer.IsReader)
        {
            spellNames = new string[spellCount];
            preparedSpells = new bool[spellCount];
            spellLevel = new int[spellCount];
            spellSchool = new int[spellCount];
            spellCastingTime = new string[spellCount];
            spellRange = new string[spellCount];
            spellComponents = new string[spellCount];
            spellDuration = new string[spellCount];
            spellDescription = new string[spellCount];
        }

        for (int i = 0; i < spellCount; i++)
        {
            if (spellNames[i] == null)
            {
                spellNames[i] = "";
            }
            serializer.SerializeValue(ref spellNames[i]);
            serializer.SerializeValue(ref preparedSpells[i]);
            serializer.SerializeValue(ref spellSchool[i]);
            if (spellCastingTime[i] == null)
            {
                spellCastingTime[i] = "";
            }
            serializer.SerializeValue(ref spellCastingTime[i]);
            if (spellRange[i] == null)
            {
                spellRange[i] = "";
            }
            serializer.SerializeValue(ref spellRange[i]);
            if (spellComponents[i] == null)
            {
                spellComponents[i] = "";
            }
            serializer.SerializeValue(ref spellComponents[i]);
            if (spellDuration[i] == null)
            {
                spellDuration[i] = "";
            }
            serializer.SerializeValue(ref spellDuration[i]);
            if (spellDescription[i] == null)
            {
                spellDescription[i] = "";
            }
            serializer.SerializeValue(ref spellDescription[i]);
        }
    }
}

[Serializable]
public struct ActionsPageCharacterInfo : INetworkSerializable
{
    public ActionTypeInfo actionList;
    public ActionTypeInfo bonusActionList;
    public ActionTypeInfo reactionList;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref actionList);
        serializer.SerializeValue(ref bonusActionList);
        serializer.SerializeValue(ref reactionList);
    }
}

[Serializable]
public struct ActionTypeInfo : INetworkSerializable
{
    public int actionCount;
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

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
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
    }
}

[Serializable]
public struct PersonalityPageCharacterInfo : INetworkSerializable
{
    public string traits;
    public string ideals;
    public string bonds;
    public string flaws;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref traits);
        serializer.SerializeValue(ref ideals);
        serializer.SerializeValue(ref bonds);
        serializer.SerializeValue(ref flaws);
    }
}