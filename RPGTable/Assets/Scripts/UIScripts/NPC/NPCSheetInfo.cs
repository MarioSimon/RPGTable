using System;
using Unity.Netcode;

[Serializable]
public class NPCSheetInfo : INetworkSerializable
{
    #region Variables

    public int sheetID = -1;
    public int avatarID = 0;
    public int animatorID = -1;

    public string NPCName = "";
    public int NPCSize = 0;
    public int NPCType = 0;
    public int NPCAlignement = 0;

    // NPC stats
    public string armorClass = "";
    public string healthPoints = "";
    public string speed = "";

    public string strScore = "";
    public string dexScore = "";
    public string conScore = "";
    public string intScore = "";
    public string wisScore = "";
    public string chaScore = "";

    public string skills = "";
    public string damageVulnerabilites = "";
    public string damageResistances = "";
    public string damageInmunities = "";
    public string conditionInmunities = "";
    public string senses = "";
    public string languages = "";
    public string challenge = "";

    // NPC combat
    public int traitCount = 0;
    public string[] traitName;
    public string[] traitDescription;

    public int actionCount = 0;
    public string[] actionName;
    public int[] actionType;
    public string[] actionAttackModifier;
    public string[] actionD1NumDices;
    public int[] actionD1DiceType;
    public string[] actionD1FlatDamage;
    public int[] actionD1Type;
    public string[] actionD2NumDices;
    public int[] actionD2DiceType;
    public string[] actionD2FlatDamage;
    public int[] actionD2Type;
    public int[] actionDC;

    #endregion

    public NPCSheetInfo GetCopy()
    {
        NPCSheetInfo copy = new NPCSheetInfo();

        copy.sheetID = -1;
        copy.avatarID = this.avatarID;
        copy.animatorID = this.animatorID;

        copy.NPCName = this.NPCName;
        copy.NPCSize = this.NPCSize;
        copy.NPCType = this.NPCType;
        copy.NPCAlignement = this.NPCAlignement;

        copy.armorClass = this.armorClass;
        copy.healthPoints = this.healthPoints;
        copy.speed = this.speed;

        copy.strScore = this.strScore;
        copy.dexScore = this.dexScore;
        copy.conScore = this.conScore;
        copy.intScore = this.intScore;
        copy.wisScore = this.wisScore;
        copy.chaScore = this.chaScore;

        copy.skills = this.skills;
        copy.damageVulnerabilites = this.damageVulnerabilites;
        copy.damageResistances = this.damageResistances;
        copy.damageInmunities = this.damageInmunities;
        copy.conditionInmunities = this.conditionInmunities;
        copy.senses = this.senses;
        copy.languages = this.languages;
        copy.challenge = this.challenge;

        copy.traitCount = this.traitCount;
        copy.traitName = this.traitName;
        copy.traitDescription = this.traitDescription;

        copy.actionCount = this.actionCount;
        copy.actionName = this.actionName;
        copy.actionType = this.actionType;
        copy.actionAttackModifier = this.actionAttackModifier;
        copy.actionD1NumDices = this.actionD1NumDices;
        copy.actionD1DiceType = this.actionD1DiceType;
        copy.actionD1FlatDamage = this.actionD1FlatDamage;
        copy.actionD1Type = this.actionD1Type;
        copy.actionD2NumDices = this.actionD2NumDices;
        copy.actionD2DiceType = this.actionD2DiceType;
        copy.actionD2FlatDamage = this.actionD2FlatDamage;
        copy.actionD2Type = this.actionD2Type;
        copy.actionDC = this.actionDC;

        return copy;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref sheetID);

        serializer.SerializeValue(ref NPCName);
        serializer.SerializeValue(ref NPCSize);
        serializer.SerializeValue(ref NPCType);
        serializer.SerializeValue(ref NPCAlignement);

        serializer.SerializeValue(ref armorClass);
        serializer.SerializeValue(ref healthPoints);
        serializer.SerializeValue(ref speed);

        serializer.SerializeValue(ref strScore);
        serializer.SerializeValue(ref dexScore);
        serializer.SerializeValue(ref conScore);
        serializer.SerializeValue(ref intScore);
        serializer.SerializeValue(ref wisScore);
        serializer.SerializeValue(ref chaScore);

        serializer.SerializeValue(ref skills);
        serializer.SerializeValue(ref damageVulnerabilites);
        serializer.SerializeValue(ref damageResistances);
        serializer.SerializeValue(ref damageInmunities);
        serializer.SerializeValue(ref conditionInmunities);
        serializer.SerializeValue(ref senses);
        serializer.SerializeValue(ref languages);
        serializer.SerializeValue(ref challenge);

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

        serializer.SerializeValue(ref actionCount);
        if (serializer.IsReader)
        {
            actionName = new string[actionCount];
            actionType = new int[actionCount];

            actionAttackModifier = new string[actionCount];

            actionD1NumDices = new string[actionCount];
            actionD1DiceType = new int[actionCount];
            actionD1FlatDamage = new string[actionCount];
            actionD1Type = new int[actionCount];

            actionD2NumDices = new string[actionCount];
            actionD2DiceType = new int[actionCount];
            actionD2FlatDamage = new string[actionCount];
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

            if (actionAttackModifier[i] == null)
            {
                actionAttackModifier[i] = "";
            }
            serializer.SerializeValue(ref actionAttackModifier[i]);

            if (actionD1NumDices[i] == null)
            {
                actionD1NumDices[i] = "";
            }
            serializer.SerializeValue(ref actionD1NumDices[i]);
            serializer.SerializeValue(ref actionD1DiceType[i]);
            if (actionD1FlatDamage[i] == null)
            {
                actionD1FlatDamage[i] = "";
            }
            serializer.SerializeValue(ref actionD1FlatDamage[i]);
            serializer.SerializeValue(ref actionD1Type[i]);

            if (actionD2NumDices[i] == null)
            {
                actionD2NumDices[i] = "";
            }
            serializer.SerializeValue(ref actionD2NumDices[i]);
            serializer.SerializeValue(ref actionD2DiceType[i]);
            if (actionD2FlatDamage[i] == null)
            {
                actionD2FlatDamage[i] = "";
            }
            serializer.SerializeValue(ref actionD2FlatDamage[i]);
            serializer.SerializeValue(ref actionD2Type[i]);

            serializer.SerializeValue(ref actionDC[i]);
        }

    }
}
