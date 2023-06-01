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
    
    #endregion

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
