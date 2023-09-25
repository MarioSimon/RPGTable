using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSheetManager : MonoBehaviour
{
    #region Variables

    private GameManager gameManager;
    private UIManager uiManager;
    private DiceHandler diceHandler;

    [SerializeField] GameObject NPCSheet;
    public TokenController NPCToken;

    public NPCSheetInfo NPCInfo;

    [SerializeField] Button closeNPCSheet;

    [Header("Stats")]
    [SerializeField] GameObject statsPage;
    [SerializeField] InputField NPCName;
    [SerializeField] Dropdown NPCSize;
    [SerializeField] Dropdown NPCType;
    [SerializeField] Dropdown NPCAlignement;

    [SerializeField] InputField armorClass;
    [SerializeField] InputField healthPoints;
    [SerializeField] InputField speed;

    [SerializeField] InputField strScore;
    [SerializeField] Text strModifier;
    [SerializeField] Button strCheck;
    [SerializeField] Button strSave;
    [SerializeField] InputField dexScore;
    [SerializeField] Text dexModifier;
    [SerializeField] Button dexCheck;
    [SerializeField] Button dexSave;
    [SerializeField] InputField conScore;
    [SerializeField] Text conModifier;
    [SerializeField] Button conCheck;
    [SerializeField] Button conSave;
    [SerializeField] InputField intScore;
    [SerializeField] Text intModifier;
    [SerializeField] Button intCheck;
    [SerializeField] Button intSave;
    [SerializeField] InputField wisScore;
    [SerializeField] Text wisModifier;
    [SerializeField] Button wisCheck;
    [SerializeField] Button wisSave;
    [SerializeField] InputField chaScore;
    [SerializeField] Text chaModifier;
    [SerializeField] Button chaCheck;
    [SerializeField] Button chaSave;

    [SerializeField] InputField skills;
    [SerializeField] InputField damageVulnerabilites;
    [SerializeField] InputField damageResistances;
    [SerializeField] InputField damageInmunities;
    [SerializeField] InputField conditionInmunities;
    [SerializeField] InputField senses;
    [SerializeField] InputField languages;
    [SerializeField] InputField challenge;

    [SerializeField] Button openCombatPage;

    [Header("Combat")]
    [SerializeField] GameObject combatPage;
    [SerializeField] GameObject traitsParent;
    [SerializeField] GameObject traitsPrefab;
    [SerializeField] Button addTrait;
    List<GameObject> traitList = new List<GameObject>();

    [SerializeField] GameObject actionsParent;
    [SerializeField] GameObject actionsPrefab;
    [SerializeField] Button addAction;
    List<GameObject> actionList = new List<GameObject>();

    [SerializeField] Button openStatsPage;
 
    #endregion
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
        diceHandler = FindObjectOfType<DiceHandler>();

        closeNPCSheet.onClick.AddListener(() => CloseSheet());

        openCombatPage.onClick.AddListener(() => OpenCombatPage());
        openStatsPage.onClick.AddListener(() => OpenStatsPage());

        strScore.onValueChanged.AddListener(delegate { CheckInt(strScore); CalculateAbilityModifier(strScore, strModifier); });
        dexScore.onValueChanged.AddListener(delegate { CheckInt(dexScore); CalculateAbilityModifier(dexScore, dexModifier); });
        conScore.onValueChanged.AddListener(delegate { CheckInt(conScore); CalculateAbilityModifier(conScore, conModifier); });
        intScore.onValueChanged.AddListener(delegate { CheckInt(intScore); CalculateAbilityModifier(intScore, intModifier); });
        wisScore.onValueChanged.AddListener(delegate { CheckInt(wisScore); CalculateAbilityModifier(wisScore, wisModifier); });
        chaScore.onValueChanged.AddListener(delegate { CheckInt(chaScore); CalculateAbilityModifier(chaScore, chaModifier); });

        strCheck.onClick.AddListener(() => RollStrenghtCheck());
        strSave.onClick.AddListener(() => RollStrengthSave());
        dexCheck.onClick.AddListener(() => RollDexterityCheck());
        dexSave.onClick.AddListener(() => RollDexteritySave());
        conCheck.onClick.AddListener(() => RollConstitutionCheck());
        conSave.onClick.AddListener(() => RollConstitutionSave());
        intCheck.onClick.AddListener(() => RollIntelligenceCheck());
        intSave.onClick.AddListener(() => RollIntelligenceSave());
        wisCheck.onClick.AddListener(() => RollWisdomCheck());
        wisSave.onClick.AddListener(() => RollWisdomSave());
        chaCheck.onClick.AddListener(() => RollCharismaCheck());
        chaSave.onClick.AddListener(() => RollCharismaSave());

        LoadNPCInfo();
    }

    #region Logic methods

    void LoadNPCInfo()
    {
        NPCName.text = NPCInfo.NPCName;
        NPCSize.value = NPCInfo.NPCSize;
        NPCType.value = NPCInfo.NPCType;
        NPCAlignement.value = NPCInfo.NPCAlignement;

        armorClass.text = NPCInfo.armorClass;
        healthPoints.text = NPCInfo.healthPoints;
        speed.text = NPCInfo.speed;

        strScore.text = NPCInfo.strScore;
        dexScore.text = NPCInfo.dexScore;
        conScore.text = NPCInfo.conScore;
        intScore.text = NPCInfo.intScore;
        wisScore.text = NPCInfo.wisScore;
        chaScore.text = NPCInfo.chaScore;

        skills.text = NPCInfo.skills;
        damageVulnerabilites.text = NPCInfo.damageVulnerabilites;
        damageResistances.text = NPCInfo.damageResistances;
        damageInmunities.text = NPCInfo.damageInmunities;
        conditionInmunities.text = NPCInfo.conditionInmunities;
        senses.text = NPCInfo.senses;
        languages.text = NPCInfo.languages;
        challenge.text = NPCInfo.challenge;

        for (int i = 0; i < NPCInfo.actionCount; i++)
        {
            AddNewAction(actionList, actionsParent);
            ActionInfo action = actionList[i].GetComponent<ActionInfo>();

            action.actionName.text = NPCInfo.actionName[i];
            action.actionType.value = NPCInfo.actionType[i];
            action.weaponTemplate.value = NPCInfo.actionWeapon[i];

            action.wpnAttackAbility.value = NPCInfo.actionAttackAbility[i];
            action.wpnOtherAttackBonus.text = NPCInfo.actionAttackOtherBonus[i];
            action.wpnAttackProficency.isOn = NPCInfo.actionAttackProficency[i];

            action.wpnDamage1NumberOfDices.text = NPCInfo.actionD1NumDices[i];
            action.wpnDamage1DiceType.value = NPCInfo.actionD1DiceType[i];
            action.wpnDamage1Ability.value = NPCInfo.actionD1Ability[i];
            action.wpnDamage1OtherBonus.text = NPCInfo.actionD1OtherBonus[i];
            action.wpnDamage1DamageType.value = NPCInfo.actionD1Type[i];

            action.wpnDamage2NumberOfDices.text = NPCInfo.actionD2NumDices[i];
            action.wpnDamage2DiceType.value = NPCInfo.actionD2DiceType[i];
            action.wpnDamage2Ability.value = NPCInfo.actionD2Ability[i];
            action.wpnDamage2OtherBonus.text = NPCInfo.actionD2OtherBonus[i];
            action.wpnDamage2DamageType.value = NPCInfo.actionD2Type[i];

            action.saveDC = NPCInfo.actionDC[i];

            action.SetActionConfig();
        }
    } 

    void SaveNPCInfo()
    {
        NPCInfo.NPCName = NPCName.text;
        NPCInfo.NPCSize = NPCSize.value;
        NPCInfo.NPCType = NPCType.value;
        NPCInfo.NPCAlignement = NPCAlignement.value;

        NPCInfo.armorClass = armorClass.text;
        NPCInfo.healthPoints = healthPoints.text;
        NPCInfo.speed = speed.text;

        NPCInfo.strScore = strScore.text;
        NPCInfo.dexScore = dexScore.text;
        NPCInfo.conScore = conScore.text;
        NPCInfo.intScore = intScore.text;
        NPCInfo.wisScore = wisScore.text;
        NPCInfo.chaScore = chaScore.text;

        NPCInfo.skills = skills.text;
        NPCInfo.damageVulnerabilites = damageVulnerabilites.text;
        NPCInfo.damageResistances = damageResistances.text;
        NPCInfo.damageInmunities = damageInmunities.text;
        NPCInfo.conditionInmunities = conditionInmunities.text;
        NPCInfo.senses = senses.text;
        NPCInfo.languages = languages.text;
        NPCInfo.challenge = challenge.text;

        NPCInfo.actionCount = actionList.Count;
        NPCInfo.actionName = new string[NPCInfo.actionCount];
        NPCInfo.actionType = new int[NPCInfo.actionCount];
        NPCInfo.actionWeapon = new int[NPCInfo.actionCount];

        NPCInfo.actionAttackAbility = new int[NPCInfo.actionCount];
        NPCInfo.actionAttackOtherBonus = new string[NPCInfo.actionCount];
        NPCInfo.actionAttackProficency = new bool[NPCInfo.actionCount];

        NPCInfo.actionD1NumDices = new string[NPCInfo.actionCount];
        NPCInfo.actionD1DiceType = new int[NPCInfo.actionCount];
        NPCInfo.actionD1Ability = new int[NPCInfo.actionCount];
        NPCInfo.actionD1OtherBonus = new string[NPCInfo.actionCount];
        NPCInfo.actionD1Type = new int[NPCInfo.actionCount];

        NPCInfo.actionD2NumDices = new string[NPCInfo.actionCount];
        NPCInfo.actionD2DiceType = new int[NPCInfo.actionCount];
        NPCInfo.actionD2Ability = new int[NPCInfo.actionCount];
        NPCInfo.actionD2OtherBonus = new string[NPCInfo.actionCount];
        NPCInfo.actionD2Type = new int[NPCInfo.actionCount];

        NPCInfo.actionDC = new int[NPCInfo.actionCount];

        for (int i = 0; i < NPCInfo.actionCount; i++)
        {
            ActionInfo action = actionList[i].GetComponent<ActionInfo>();

            NPCInfo.actionName[i] = action.actionName.text;
            NPCInfo.actionType[i] = action.actionType.value;
            NPCInfo.actionWeapon[i] = action.weaponTemplate.value;

            NPCInfo.actionAttackAbility[i] = action.wpnAttackAbility.value;
            NPCInfo.actionAttackOtherBonus[i] = action.wpnOtherAttackBonus.text;
            NPCInfo.actionAttackProficency[i] = action.wpnAttackProficency.isOn;

            NPCInfo.actionD1NumDices[i] = action.wpnDamage1NumberOfDices.text;
            NPCInfo.actionD1DiceType[i] = action.wpnDamage1DiceType.value;
            NPCInfo.actionD1Ability[i] = action.wpnDamage1Ability.value;
            NPCInfo.actionD1OtherBonus[i] = action.wpnDamage1OtherBonus.text;
            NPCInfo.actionD1Type[i] = action.wpnDamage1DamageType.value;

            NPCInfo.actionD2NumDices[i] = action.wpnDamage2NumberOfDices.text;
            NPCInfo.actionD2DiceType[i] = action.wpnDamage2DiceType.value;
            NPCInfo.actionD2Ability[i] = action.wpnDamage2Ability.value;
            NPCInfo.actionD2OtherBonus[i] = action.wpnDamage2OtherBonus.text;
            NPCInfo.actionD2Type[i] = action.wpnDamage2DamageType.value;

            NPCInfo.actionDC[i] = action.saveDC;
        }
    }

    private void CheckInt(InputField inputField)
    {
        int value;
        if (!int.TryParse(inputField.text, out value))
        {
            inputField.text = "0";
        }
    }

    private void CalculateAbilityModifier(InputField score, Text modifier)
    {
        int abilityScore = int.Parse(score.text);
        int abilityModifier = CalculateModifier(abilityScore);

        modifier.text = abilityModifier.ToString();
    }

    private int CalculateModifier(int abilityScore)
    {
        int mod = -5; // modifier value for a score of 1

        mod += abilityScore / 2;

        return mod;
    }

    private void AddNewAction(List<GameObject> listOfActions, GameObject actionsParent)
    {
        GameObject action = Instantiate(actionsPrefab);
        GameObject actionItem = action.GetComponent<ActionInfo>().actionItem;
        //action.GetComponent<ActionInfo>().sheetManager = this;
        actionItem.GetComponent<RectTransform>().SetParent(actionsParent.transform);
        listOfActions.Add(action);
    }

    #endregion

    #region Rolling methods

    void RollStrenghtCheck()
    {
        diceHandler.RollCheck(NPCName.text, "Strenght", int.Parse(strModifier.text));
    }

    void RollDexterityCheck()
    {
        diceHandler.RollCheck(NPCName.text, "Dexterity", int.Parse(dexModifier.text));
    }

    void RollConstitutionCheck()
    {
        diceHandler.RollCheck(NPCName.text, "Constitution", int.Parse(conModifier.text));
    }

    void RollIntelligenceCheck()
    {
        diceHandler.RollCheck(NPCName.text, "Intelligence", int.Parse(intModifier.text));
    }

    void RollWisdomCheck()
    {
        diceHandler.RollCheck(NPCName.text, "Wisdom", int.Parse(wisModifier.text));
    }

    void RollCharismaCheck()
    {
        diceHandler.RollCheck(NPCName.text, "Charisma", int.Parse(chaModifier.text));
    }

    void RollInitiativeCheck()
    {
        if (dexModifier.text == "")
        {
            dexModifier.text = "0";
        }

        diceHandler.RollInitiative(NPCName.text, int.Parse(dexModifier.text));
    }

    void RollStrengthSave()
    {
        diceHandler.RollAbilitySave(NPCName.text, "Strength", int.Parse(strModifier.text));
    }

    void RollDexteritySave()
    {
        diceHandler.RollAbilitySave(NPCName.text, "Dexterity", int.Parse(dexModifier.text));
    }

    void RollConstitutionSave()
    {
        diceHandler.RollAbilitySave(NPCName.text, "Constitution", int.Parse(conModifier.text));
    }

    void RollIntelligenceSave()
    {
        diceHandler.RollAbilitySave(NPCName.text, "Intelligence", int.Parse(intModifier.text));
    }

    void RollWisdomSave()
    {
        diceHandler.RollAbilitySave(NPCName.text, "Wisdom", int.Parse(wisModifier.text));
    }

    void RollCharismaSave()
    {
        diceHandler.RollAbilitySave(NPCName.text, "Charisma", int.Parse(chaModifier.text));
    }

    #endregion   

    #region Navigation methods

    void OpenStatsPage()
    {
        statsPage.SetActive(true);
        combatPage.SetActive(false);
    }

    void OpenCombatPage()
    {
        statsPage.SetActive(false);
        combatPage.SetActive(true);
    }

    void CloseSheet()
    {
        SaveNPCInfo();
        if (NPCInfo.sheetID >= 0)
        {
            gameManager.SaveNPCSheetChanges(NPCInfo);
        }
        else
        {
            NPCToken.NPCSheetInfo = NPCInfo;
        }   
        Destroy(NPCSheet);
    }

    #endregion
}
