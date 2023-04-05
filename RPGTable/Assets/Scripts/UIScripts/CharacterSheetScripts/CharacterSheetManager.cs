using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Unity.Collections;
using System;

public class CharacterSheetManager : MonoBehaviour
{
    #region Variables
    public CharacterSheetInfo CSInfo;
    
    private GameManager gameManager;
    private UIManager uiManager;
    private DiceHandler diceHandler;

    private GameObject currentPage;
    private bool permisson;

    [SerializeField] GameObject characterSheet;

    #region Navigation bar
    [Header("NavBar")]
    [SerializeField] Button buttonClose;
    [SerializeField] Button buttonSpawnToken;
    [SerializeField] Button buttonPublicInfo;
    [SerializeField] Button buttonBasicInfo;
    [SerializeField] Button buttonSkills;
    [SerializeField] Button buttonFeatures;
    [SerializeField] Button buttonInventory;
    [SerializeField] Button buttonSpells;
    [SerializeField] Button buttonActions;
    [SerializeField] Button buttonPersonality;
    #endregion

    #region Public Info
    [Header("Public Info")]
    [SerializeField] GameObject publicInfo;
    [SerializeField] InputField characterName;
    [SerializeField] public InputField playerName;
    [SerializeField] InputField appearance;

    [SerializeField] Image publicInfoBlocker;
    #endregion

    #region Basic Info
    [Header("Basic Info")]
    // Info
    [SerializeField] GameObject basicInfo;
    [SerializeField] InputField clasAndLevel;
    [SerializeField] InputField subclass;
    [SerializeField] InputField race;
    [SerializeField] InputField background;
    [SerializeField] InputField alignement;
    [SerializeField] InputField experience;
    // AbilityScores
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
    // Stats
    [SerializeField] public InputField maxHealthPoints;
    [SerializeField] public InputField currHealthPoints;
    [SerializeField] public InputField tempHealthPoints;
    [SerializeField] InputField initiativeBonus;
    [SerializeField] Button rollInitiative;
    [SerializeField] Dropdown hitDice;
    [SerializeField] Button rollHitDice;
    [SerializeField] InputField armorClass;
    [SerializeField] InputField speed;
    [SerializeField] Toggle deathSaveSuccess1;
    [SerializeField] Toggle deathSaveSuccess2;
    [SerializeField] Toggle deathSaveSuccess3;
    [SerializeField] Toggle deathSaveFail1;
    [SerializeField] Toggle deathSaveFail2;
    [SerializeField] Toggle deathSaveFail3;
    [SerializeField] Button rollDeathSave;
    [SerializeField] Button resetDeathSaves;
    // Other
    [SerializeField] Image basicInfoBlocker;
    [SerializeField] Toggle basicInfoPublisher;
    #endregion

    #region Skills   

    [Header("Skills")]
    [SerializeField] GameObject skills;
    [SerializeField] InputField proficencyBonus;
    // Saving Proficencies
    [SerializeField] Toggle strProficency;
    [SerializeField] Toggle dexProficency;
    [SerializeField] Toggle conProficency;
    [SerializeField] Toggle intProficency;
    [SerializeField] Toggle wisProficency;
    [SerializeField] Toggle chaProficency;
    // Skill Proficencies
    [SerializeField] List<Skill> skillList;
    // Other
    [SerializeField] Image skillsBlocker;
    [SerializeField] Toggle skillsPublisher;

    #endregion

    #region Features

    [Header("Features")]
    [SerializeField] GameObject features;
    [SerializeField] InputField featuresAndTraits;
    [SerializeField] InputField proficencies;

    [SerializeField] Image featuresBlocker;
    [SerializeField] Toggle featuresPublisher;
    #endregion

    #region Inventory

    [Header("Inventory")]
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject inventoryArea;
    [SerializeField] Button addItem;
    [SerializeField] GameObject itemPrefab;

    [SerializeField] List<GameObject> itemList;
    [SerializeField] float itemSpace;

    [SerializeField] InputField totalWeight;
    [SerializeField] InputField maxWeight;
    [SerializeField] InputField copperPieces;
    [SerializeField] InputField silverPieces;
    [SerializeField] InputField electrumPieces;
    [SerializeField] InputField goldPieces;
    [SerializeField] InputField platinumPieces;

    [SerializeField] Image inventoryBlocker;
    [SerializeField] Toggle inventoryPublisher;
    #endregion

    #region Spells

    [Header("Spells")]
    [SerializeField] GameObject spells;

    [SerializeField] Dropdown spellModifier;
    [SerializeField] InputField spellSaveDc;
    [SerializeField] InputField spellAttackMod;

    [SerializeField] Dropdown spellLevelSelector;
    [SerializeField] Button addSpell;
    [SerializeField] GameObject spellPrefab;
    [SerializeField] float spellSpace;
    [SerializeField] GameObject currSpellLevel;

    [SerializeField] GameObject level0Spells;
    [SerializeField] GameObject level0Area;
    List<GameObject> level0SpellsList = new List<GameObject>();

    [SerializeField] GameObject level1Spells;
    [SerializeField] GameObject level1Area;
    List<GameObject> level1SpellsList = new List<GameObject>();

    [SerializeField] GameObject level2Spells;
    [SerializeField] GameObject level2Area;
    List<GameObject> level2SpellsList = new List<GameObject>();

    [SerializeField] GameObject level3Spells;
    [SerializeField] GameObject level3Area;
    List<GameObject> level3SpellsList = new List<GameObject>();

    [SerializeField] GameObject level4Spells;
    [SerializeField] GameObject level4Area;
    List<GameObject> level4SpellsList = new List<GameObject>();

    [SerializeField] GameObject level5Spells;
    [SerializeField] GameObject level5Area;
    List<GameObject> level5SpellsList = new List<GameObject>();

    [SerializeField] GameObject level6Spells;
    [SerializeField] GameObject level6Area;
    List<GameObject> level6SpellsList = new List<GameObject>();

    [SerializeField] GameObject level7Spells;
    [SerializeField] GameObject level7Area;
    List<GameObject> level7SpellsList = new List<GameObject>();

    [SerializeField] GameObject level8Spells;
    [SerializeField] GameObject level8Area;
    List<GameObject> level8SpellsList = new List<GameObject>();

    [SerializeField] GameObject level9Spells;
    [SerializeField] GameObject level9Area;
    List<GameObject> level9SpellsList = new List<GameObject>();

    [SerializeField] Image spellsBlocker1;
    [SerializeField] Image spellsBlocker2;
    [SerializeField] Image spellsBlocker3;
    [SerializeField] Toggle spellsPublisher;

    #endregion

    #region Actions

    [Header("Actions")]
    [SerializeField] GameObject actions;
    [SerializeField] InputField actionsText;
    [SerializeField] InputField bonusActions;
    [SerializeField] InputField reactions;

    [SerializeField] Image actionsBlocker;
    [SerializeField] Toggle actionsPublisher;

    #endregion

    #region Personality

    [Header("Personality")]
    [SerializeField] GameObject personailty;
    [SerializeField] InputField traits;
    [SerializeField] InputField ideals;
    [SerializeField] InputField bonds;
    [SerializeField] InputField flaws;

    [SerializeField] Image personalityBlocker;
    [SerializeField] Toggle personalityPublisher;

    #endregion

    #endregion

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
        diceHandler = FindObjectOfType<DiceHandler>();

        // navigation related events
        buttonClose.onClick.AddListener(() => CloseSheet());
        buttonSpawnToken.onClick.AddListener(() => SpawnToken(NetworkManager.Singleton.LocalClientId, playerName.text, CSInfo.avatarID, CSInfo));
        buttonPublicInfo.onClick.AddListener(() => OpenPublicInfoPage());
        buttonBasicInfo.onClick.AddListener(() => OpenBasicInfoPage());
        buttonSkills.onClick.AddListener(() => OpenSkillsPage());
        buttonFeatures.onClick.AddListener(() => OpenFeaturesPage());
        buttonInventory.onClick.AddListener(() => OpenInventoryPage());
        buttonSpells.onClick.AddListener(() => OpenSpellsPage());
        buttonActions.onClick.AddListener(() => OpenActionsPage());
        buttonPersonality.onClick.AddListener(() => OpenPersonalityPage());

        bool isServer = NetworkManager.Singleton.IsServer;
        permisson = isServer || NetworkManager.Singleton.LocalClientId == CSInfo.ownerID;

        if (isServer)
        {
            playerName.interactable = true;
        }

        CheckPermisson();

        // basic info related events
        characterName.onValueChanged.AddListener(delegate { uiManager.UpdateCharacterButtonNameClientRpc(CSInfo.sheetID, characterName.text); });

        // ability score related logic events
        strScore.onValueChanged.AddListener(delegate { CheckStrScore(); CalculateMaxWeight(); });
        dexScore.onValueChanged.AddListener(delegate { CheckDexScore(); });
        conScore.onValueChanged.AddListener(delegate { CheckConScore(); });
        intScore.onValueChanged.AddListener(delegate { CheckIntScore(); });
        wisScore.onValueChanged.AddListener(delegate { CheckWisScore(); });
        chaScore.onValueChanged.AddListener(delegate { CheckChaScore(); });

        // ability related roll events
        strCheck.onClick.AddListener(() => RollStrenghtCheck());
        dexCheck.onClick.AddListener(() => RollDexterityCheck());
        conCheck.onClick.AddListener(() => RollConstitutionCheck());
        intCheck.onClick.AddListener(() => RollIntelligenceCheck());
        wisCheck.onClick.AddListener(() => RollWisdomCheck());
        chaCheck.onClick.AddListener(() => RollCharismaCheck());

        strSave.onClick.AddListener(() => RollStrengthSave());
        dexSave.onClick.AddListener(() => RollDexteritySave());
        conSave.onClick.AddListener(() => RollConstitutionSave());
        intSave.onClick.AddListener(() => RollIntelligenceSave());
        wisSave.onClick.AddListener(() => RollWisdomSave());
        chaSave.onClick.AddListener(() => RollCharismaSave());

        // character stats related events
        maxHealthPoints.onValueChanged.AddListener(delegate { CheckInt(maxHealthPoints); });
        currHealthPoints.onValueChanged.AddListener(delegate { CheckInt(currHealthPoints); UpdateTokenHealthPoints(tempHealthPoints.text, currHealthPoints.text); });
        tempHealthPoints.onValueChanged.AddListener(delegate { CheckInt(tempHealthPoints); UpdateTokenHealthPoints(tempHealthPoints.text, currHealthPoints.text); });

        armorClass.onValueChanged.AddListener(delegate { CheckInt(armorClass); });
        initiativeBonus.onValueChanged.AddListener(delegate { CheckInt(initiativeBonus); });

        rollHitDice.onClick.AddListener(() => RollHitDice());
        rollInitiative.onClick.AddListener(() => RollInitiativeCheck());
        rollDeathSave.onClick.AddListener(() => RollDeathSavingThrow());
        resetDeathSaves.onClick.AddListener(() => ResetDeathSavingThrows());

        // skill related logic events
        proficencyBonus.onValueChanged.AddListener(delegate { CheckProficencyBonus(); });
        foreach (Skill skill in skillList)
        {
            proficencyBonus.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });
            skill.skillProficency.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });
            skill.skillCharacteristic.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });
            skill.skillExtraBonus.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });

            CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus);

            // skill related roll events
            skill.skillCheck.onClick.AddListener(() => RollSkillCheck(skill.skillName, skill.skillTotalBonus.text));
        }

        // inventory related events
        addItem.onClick.AddListener(() => AddItemToInventory());
        maxWeight.onValueChanged.AddListener(delegate { CheckInt(maxWeight); });
        totalWeight.onValueChanged.AddListener(delegate { CheckInt(totalWeight); });

        copperPieces.onValueChanged.AddListener(delegate { CheckInt(copperPieces); });
        silverPieces.onValueChanged.AddListener(delegate { CheckInt(silverPieces); });
        electrumPieces.onValueChanged.AddListener(delegate { CheckInt(electrumPieces); });
        goldPieces.onValueChanged.AddListener(delegate { CheckInt(goldPieces); });
        platinumPieces.onValueChanged.AddListener(delegate { CheckInt(platinumPieces); });

        // spell related events
        spellModifier.onValueChanged.AddListener(delegate { UpdateSpellAbilityMod(); });
        spellSaveDc.onValueChanged.AddListener(delegate { CheckInt(spellSaveDc); });
        spellAttackMod.onValueChanged.AddListener(delegate { CheckInt(spellAttackMod); });

        spellLevelSelector.onValueChanged.AddListener(delegate { SwitchSpellLevelList(); });
        addSpell.onClick.AddListener(() => AddSpell());


        if (CSInfo != null)
        {
            GetCharacterInfo();
        }
        else
        {
            CSInfo = new CharacterSheetInfo();
        }

        CheckPublicPages(permisson);

        currentPage = publicInfo;
        currSpellLevel = level0Spells;
    }   

    #region Navigation Methods

    void OpenPublicInfoPage()
    {
        RefreshSheetData();
        currentPage.SetActive(false);
        publicInfo.SetActive(true);
        currentPage = publicInfo;
    }

    void OpenBasicInfoPage()
    {
        RefreshSheetData();
        currentPage.SetActive(false);
        basicInfo.SetActive(true);
        currentPage = basicInfo;
    }

    void OpenSkillsPage()
    {
        RefreshSheetData();
        currentPage.SetActive(false);
        skills.SetActive(true);
        currentPage = skills;
    }

    void OpenFeaturesPage()
    {
        RefreshSheetData();
        currentPage.SetActive(false);
        features.SetActive(true);
        currentPage = features;
    }

    void OpenInventoryPage()
    {
        RefreshSheetData();
        currentPage.SetActive(false);
        inventory.SetActive(true);
        currentPage = inventory;
    }

    void OpenSpellsPage()
    {
        RefreshSheetData();
        currentPage.SetActive(false);
        spells.SetActive(true);
        currentPage = spells;
    }

    void OpenActionsPage()
    {
        RefreshSheetData();
        currentPage.SetActive(false);
        actions.SetActive(true);
        currentPage = actions;
    }

    void OpenPersonalityPage()
    {
        RefreshSheetData();
        currentPage.SetActive(false);
        personailty.SetActive(true);
        currentPage = personailty;
    }

    void CloseSheet()
    {
        if (permisson)
        {
            SetCharacterInfo();
            gameManager.SaveCharacterSheetChanges(CSInfo);
        }
        Destroy(characterSheet);
       
    }
    #endregion

    #region Logic Methods

    void GetCharacterInfo()
    {
        characterName.text = CSInfo.characterName;
        playerName.text = CSInfo.playerName;
        appearance.text = CSInfo.appearance;

        basicInfoPublisher.isOn = CSInfo.publicBasicInfo;
        skillsPublisher.isOn = CSInfo.publicSkills;
        featuresPublisher.isOn = CSInfo.publicFeatures;
        inventoryPublisher.isOn = CSInfo.publicInventory;
        spellsPublisher.isOn = CSInfo.publicSpells;
        actionsPublisher.isOn = CSInfo.publicActions;
        personalityPublisher.isOn = CSInfo.publicPersonality;

        clasAndLevel.text = CSInfo.clasAndLevel;
        subclass.text = CSInfo.subclass;
        race.text = CSInfo.race;
        background.text = CSInfo.background;
        alignement.text = CSInfo.alignement;
        experience.text = CSInfo.experience;

        strScore.text = CSInfo.strScore;
        dexScore.text = CSInfo.dexScore;
        conScore.text = CSInfo.conScore;
        intScore.text = CSInfo.intScore;
        wisScore.text = CSInfo.wisScore;
        chaScore.text = CSInfo.chaScore;

        maxHealthPoints.text = CSInfo.maxHealthPoints;
        currHealthPoints.text = CSInfo.currHealthPoints;
        tempHealthPoints.text = CSInfo.tempHealthPoints;
        initiativeBonus.text = CSInfo.initiativeBonus;
        armorClass.text = CSInfo.armorClass;
        speed.text = CSInfo.speed;
        hitDice.value = CSInfo.hitDiceType;
        switch (CSInfo.deathSaveSuccesses)
        {
            case 1:
                deathSaveSuccess1.isOn = true;
                break;
            case 2:
                deathSaveSuccess1.isOn = true;
                deathSaveSuccess2.isOn = true;
                break;
            case 3:
                deathSaveSuccess1.isOn = true;
                deathSaveSuccess2.isOn = true;
                deathSaveSuccess3.isOn = true;
                break;
        }
        switch (CSInfo.deathSaveFails)
        {
            case 1:
                deathSaveFail1.isOn = true;
                break;
            case 2:
                deathSaveFail1.isOn = true;
                deathSaveFail2.isOn = true;
                break;
            case 3:
                deathSaveFail1.isOn = true;
                deathSaveFail2.isOn = true;
                deathSaveFail3.isOn = true;
                break;
        }

        proficencyBonus.text = CSInfo.proficencyBonus;
        strProficency.isOn = CSInfo.strProf;
        dexProficency.isOn = CSInfo.dexProf;
        conProficency.isOn = CSInfo.conProf;
        intProficency.isOn = CSInfo.intProf;
        wisProficency.isOn = CSInfo.wisProf;
        chaProficency.isOn = CSInfo.chaProf;

        for (int i = 0; i < skillList.Count; i++)
        {
            skillList[i].skillProficency.value = CSInfo.skillProf[i];
            skillList[i].skillCharacteristic.value = CSInfo.skillCharacteristic[i];
            skillList[i].skillExtraBonus.text = CSInfo.skillBonus[i];
            skillList[i].skillTotalBonus.text = CSInfo.skillTotal[i];
        }

        featuresAndTraits.text = CSInfo.featuresAndTraits;
        proficencies.text = CSInfo.proficencies;

        for (int i = 0; i < CSInfo.itemCount; i++)
        {
            AddItemToInventory();
            CharacterSheetItem item = itemList[i].GetComponent<CharacterSheetItem>();
            item.itemName.text = CSInfo.itemNames[i];
            item.itemAmount.text = CSInfo.itemAmounts[i];
            item.itemWeight.text = CSInfo.itemWeights[i];
        }      

        copperPieces.text = CSInfo.copperPieces;
        silverPieces.text = CSInfo.silverPieces;
        electrumPieces.text = CSInfo.electrumPieces;
        goldPieces.text = CSInfo.goldPieces;
        platinumPieces.text = CSInfo.platinumPieces;

        spellModifier.value = CSInfo.spellCastingAbility;

        for (int i = 0; i < CSInfo.level0SpellCount; i++)
        {
            AddNewSpellByLevel(level0SpellsList, level0Area);
            SpellInfo spell = level0SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.level0SpellNames[i];
            spell.prepared.isOn = CSInfo.level0PreparedSpells[i];
            spell.spellLevel.value = 0;
            spell.spellSchool.value = CSInfo.level0SpellSchool[i];
            spell.spellCastingTime.text = CSInfo.level0SpellCastingTime[i];
            spell.spellRange.text = CSInfo.level0SpellCastingTime[i];
            spell.spellComponents.text = CSInfo.level0SpellComponents[i];
            spell.spellDuration.text = CSInfo.level0SpellDuration[i];
            spell.spellDescription.text = CSInfo.level0SpellDescription[i];
        }

        for (int i = 0; i < CSInfo.level1SpellCount; i++)
        {
            AddNewSpellByLevel(level1SpellsList, level1Area);
            SpellInfo spell = level1SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.level1SpellNames[i];
            spell.prepared.isOn = CSInfo.level1PreparedSpells[i];
            spell.spellLevel.value = 1;
            spell.spellSchool.value = CSInfo.level1SpellSchool[i];
            spell.spellCastingTime.text = CSInfo.level1SpellCastingTime[i];
            spell.spellRange.text = CSInfo.level1SpellCastingTime[i];
            spell.spellComponents.text = CSInfo.level1SpellComponents[i];
            spell.spellDuration.text = CSInfo.level1SpellDuration[i];
            spell.spellDescription.text = CSInfo.level1SpellDescription[i];
        }

        for (int i = 0; i < CSInfo.level2SpellCount; i++)
        {
            AddNewSpellByLevel(level2SpellsList, level2Area);
            SpellInfo spell = level2SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.level2SpellNames[i];
            spell.prepared.isOn = CSInfo.level2PreparedSpells[i];
            spell.spellLevel.value = 2;
            spell.spellSchool.value = CSInfo.level2SpellSchool[i];
            spell.spellCastingTime.text = CSInfo.level2SpellCastingTime[i];
            spell.spellRange.text = CSInfo.level2SpellCastingTime[i];
            spell.spellComponents.text = CSInfo.level2SpellComponents[i];
            spell.spellDuration.text = CSInfo.level2SpellDuration[i];
            spell.spellDescription.text = CSInfo.level2SpellDescription[i];
        }

        for (int i = 0; i < CSInfo.level3SpellCount; i++)
        {
            AddNewSpellByLevel(level3SpellsList, level0Area);
            SpellInfo spell = level3SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.level3SpellNames[i];
            spell.prepared.isOn = CSInfo.level3PreparedSpells[i];
            spell.spellLevel.value = 3;
            spell.spellSchool.value = CSInfo.level3SpellSchool[i];
            spell.spellCastingTime.text = CSInfo.level3SpellCastingTime[i];
            spell.spellRange.text = CSInfo.level3SpellCastingTime[i];
            spell.spellComponents.text = CSInfo.level3SpellComponents[i];
            spell.spellDuration.text = CSInfo.level3SpellDuration[i];
            spell.spellDescription.text = CSInfo.level3SpellDescription[i];
        }

        for (int i = 0; i < CSInfo.level4SpellCount; i++)
        {
            AddNewSpellByLevel(level4SpellsList, level0Area);
            SpellInfo spell = level4SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.level4SpellNames[i];
            spell.prepared.isOn = CSInfo.level4PreparedSpells[i];
            spell.spellLevel.value = 4;
            spell.spellSchool.value = CSInfo.level4SpellSchool[i];
            spell.spellCastingTime.text = CSInfo.level4SpellCastingTime[i];
            spell.spellRange.text = CSInfo.level4SpellCastingTime[i];
            spell.spellComponents.text = CSInfo.level4SpellComponents[i];
            spell.spellDuration.text = CSInfo.level4SpellDuration[i];
            spell.spellDescription.text = CSInfo.level4SpellDescription[i];
        }

        for (int i = 0; i < CSInfo.level5SpellCount; i++)
        {
            AddNewSpellByLevel(level5SpellsList, level5Area);
            SpellInfo spell = level5SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.level5SpellNames[i];
            spell.prepared.isOn = CSInfo.level5PreparedSpells[i];
            spell.spellLevel.value = 5;
            spell.spellSchool.value = CSInfo.level5SpellSchool[i];
            spell.spellCastingTime.text = CSInfo.level5SpellCastingTime[i];
            spell.spellRange.text = CSInfo.level5SpellCastingTime[i];
            spell.spellComponents.text = CSInfo.level5SpellComponents[i];
            spell.spellDuration.text = CSInfo.level5SpellDuration[i];
            spell.spellDescription.text = CSInfo.level5SpellDescription[i];
        }

        for (int i = 0; i < CSInfo.level6SpellCount; i++)
        {
            AddNewSpellByLevel(level6SpellsList, level6Area);
            SpellInfo spell = level6SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.level6SpellNames[i];
            spell.prepared.isOn = CSInfo.level6PreparedSpells[i];
            spell.spellLevel.value = 6;
            spell.spellSchool.value = CSInfo.level6SpellSchool[i];
            spell.spellCastingTime.text = CSInfo.level6SpellCastingTime[i];
            spell.spellRange.text = CSInfo.level6SpellCastingTime[i];
            spell.spellComponents.text = CSInfo.level6SpellComponents[i];
            spell.spellDuration.text = CSInfo.level6SpellDuration[i];
            spell.spellDescription.text = CSInfo.level6SpellDescription[i];
        }

        for (int i = 0; i < CSInfo.level7SpellCount; i++)
        {
            AddNewSpellByLevel(level7SpellsList, level7Area);
            SpellInfo spell = level7SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.level7SpellNames[i];
            spell.prepared.isOn = CSInfo.level7PreparedSpells[i];
            spell.spellLevel.value = 7;
            spell.spellSchool.value = CSInfo.level7SpellSchool[i];
            spell.spellCastingTime.text = CSInfo.level7SpellCastingTime[i];
            spell.spellRange.text = CSInfo.level7SpellCastingTime[i];
            spell.spellComponents.text = CSInfo.level7SpellComponents[i];
            spell.spellDuration.text = CSInfo.level7SpellDuration[i];
            spell.spellDescription.text = CSInfo.level7SpellDescription[i];
        }

        for (int i = 0; i < CSInfo.level8SpellCount; i++)
        {
            AddNewSpellByLevel(level8SpellsList, level8Area);
            SpellInfo spell = level8SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.level8SpellNames[i];
            spell.prepared.isOn = CSInfo.level8PreparedSpells[i];
            spell.spellLevel.value = 8;
            spell.spellSchool.value = CSInfo.level8SpellSchool[i];
            spell.spellCastingTime.text = CSInfo.level8SpellCastingTime[i];
            spell.spellRange.text = CSInfo.level8SpellCastingTime[i];
            spell.spellComponents.text = CSInfo.level8SpellComponents[i];
            spell.spellDuration.text = CSInfo.level8SpellDuration[i];
            spell.spellDescription.text = CSInfo.level8SpellDescription[i];
        }

        for (int i = 0; i < CSInfo.level9SpellCount; i++)
        {
            AddNewSpellByLevel(level9SpellsList, level0Area);
            SpellInfo spell = level9SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.level9SpellNames[i];
            spell.prepared.isOn = CSInfo.level9PreparedSpells[i];
            spell.spellLevel.value = 9;
            spell.spellSchool.value = CSInfo.level9SpellSchool[i];
            spell.spellCastingTime.text = CSInfo.level9SpellCastingTime[i];
            spell.spellRange.text = CSInfo.level9SpellCastingTime[i];
            spell.spellComponents.text = CSInfo.level9SpellComponents[i];
            spell.spellDuration.text = CSInfo.level9SpellDuration[i];
            spell.spellDescription.text = CSInfo.level9SpellDescription[i];
        }

        actionsText.text = CSInfo.actions;
        bonusActions.text = CSInfo.bonusActions;
        reactions.text = CSInfo.reactions;


        traits.text = CSInfo.traits;
        ideals.text = CSInfo.ideals;
        bonds.text = CSInfo.bonds;
        flaws.text = CSInfo.flaws; 
    }

    void SetCharacterInfo()
    {
        CSInfo.characterName = characterName.text;
        CSInfo.playerName = playerName.text;
        CSInfo.appearance = appearance.text;

        CSInfo.publicBasicInfo = basicInfoPublisher.isOn;
        CSInfo.publicSkills = skillsPublisher.isOn;
        CSInfo.publicFeatures = featuresPublisher.isOn;
        CSInfo.publicInventory = inventoryPublisher.isOn;
        CSInfo.publicSpells = spellsPublisher.isOn;
        CSInfo.publicActions = actionsPublisher.isOn;
        CSInfo.publicPersonality = personalityPublisher.isOn;

        CSInfo.clasAndLevel = clasAndLevel.text;
        CSInfo.subclass = subclass.text;
        CSInfo.race = race.text;
        CSInfo.background = background.text;
        CSInfo.alignement = alignement.text;
        CSInfo.experience = experience.text;

        CSInfo.strScore = strScore.text;
        CSInfo.dexScore = dexScore.text;
        CSInfo.conScore = conScore.text;
        CSInfo.intScore = intScore.text;
        CSInfo.wisScore = wisScore.text;
        CSInfo.chaScore = chaScore.text;

        CSInfo.maxHealthPoints = maxHealthPoints.text;
        CSInfo.currHealthPoints = currHealthPoints.text;
        CSInfo.tempHealthPoints = tempHealthPoints.text;
        CSInfo.initiativeBonus = initiativeBonus.text;
        CSInfo.armorClass = armorClass.text;
        CSInfo.speed = speed.text;
        CSInfo.hitDiceType = hitDice.value;
        if (deathSaveFail1.isOn)
            CSInfo.deathSaveFails += 1;
        if (deathSaveFail2.isOn)
            CSInfo.deathSaveFails += 1;
        if (deathSaveFail3.isOn)
            CSInfo.deathSaveFails += 1;
        if (deathSaveSuccess1.isOn)
            CSInfo.deathSaveSuccesses += 1;
        if (deathSaveSuccess2.isOn)
            CSInfo.deathSaveSuccesses += 1;
        if (deathSaveSuccess3.isOn)
            CSInfo.deathSaveSuccesses += 1;

        CSInfo.proficencyBonus = proficencyBonus.text;
        CSInfo.strProf = strProficency.isOn;
        CSInfo.dexProf = dexProficency.isOn;
        CSInfo.conProf = conProficency.isOn;
        CSInfo.intProf = intProficency.isOn;
        CSInfo.wisProf = wisProficency.isOn;
        CSInfo.chaProf = chaProficency.isOn;

        CSInfo.skillProf = new int[skillList.Count];
        CSInfo.skillCharacteristic = new int[skillList.Count];
        CSInfo.skillBonus = new string[skillList.Count];
        CSInfo.skillTotal = new string[skillList.Count];

        for (int i = 0; i < skillList.Count; i++)
        {
            CSInfo.skillProf[i] = skillList[i].skillProficency.value;
            CSInfo.skillCharacteristic[i] = skillList[i].skillCharacteristic.value;
            CSInfo.skillBonus[i] = skillList[i].skillExtraBonus.text;
            CSInfo.skillTotal[i] = skillList[i].skillTotalBonus.text;
        }

        CSInfo.featuresAndTraits = featuresAndTraits.text;
        CSInfo.proficencies = proficencies.text;

        CSInfo.itemCount = itemList.Count;
        CSInfo.itemNames = new string[CSInfo.itemCount];
        CSInfo.itemAmounts = new string[CSInfo.itemCount];
        CSInfo.itemWeights = new string[CSInfo.itemCount];

        for (int i = 0; i < itemList.Count; i++)
        {
            CharacterSheetItem item = itemList[i].GetComponent<CharacterSheetItem>();
            CSInfo.itemNames[i] = item.itemName.text;
            CSInfo.itemAmounts[i] = item.itemAmount.text;
            CSInfo.itemWeights[i] = item.itemWeight.text;
        }

        CSInfo.copperPieces = copperPieces.text;
        CSInfo.silverPieces = silverPieces.text;
        CSInfo.electrumPieces = electrumPieces.text;
        CSInfo.goldPieces = goldPieces.text;
        CSInfo.platinumPieces = platinumPieces.text;

        CSInfo.spellCastingAbility = spellModifier.value;

        CSInfo.level0SpellCount = level0SpellsList.Count;
        CSInfo.level0SpellNames = new string[CSInfo.level0SpellCount];
        CSInfo.level0PreparedSpells = new bool[CSInfo.level0SpellCount];
        CSInfo.level0SpellSchool = new int[CSInfo.level0SpellCount];
        CSInfo.level0SpellCastingTime = new string[CSInfo.level0SpellCount];
        CSInfo.level0SpellRange = new string[CSInfo.level0SpellCount];
        CSInfo.level0SpellComponents = new string[CSInfo.level0SpellCount];
        CSInfo.level0SpellDuration = new string[CSInfo.level0SpellCount];
        CSInfo.level0SpellDescription = new string[CSInfo.level0SpellCount];

        for (int i = 0; i < CSInfo.level0SpellCount; i++)
        {
            SpellInfo spell = level0SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.level0SpellNames[i] = spell.spellName.text;
            CSInfo.level0PreparedSpells[i] = spell.prepared.isOn;
            CSInfo.level0SpellSchool[i] = spell.spellSchool.value;
            CSInfo.level0SpellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.level0SpellRange[i] = spell.spellRange.text;
            CSInfo.level0SpellComponents[i] = spell.spellComponents.text;
            CSInfo.level0SpellDuration[i] = spell.spellDuration.text;
            CSInfo.level0SpellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.level1SpellCount = level1SpellsList.Count;
        CSInfo.level1SpellNames = new string[CSInfo.level1SpellCount];
        CSInfo.level1PreparedSpells = new bool[CSInfo.level1SpellCount];
        CSInfo.level1SpellSchool = new int[CSInfo.level1SpellCount];
        CSInfo.level1SpellCastingTime = new string[CSInfo.level1SpellCount];
        CSInfo.level1SpellRange = new string[CSInfo.level1SpellCount];
        CSInfo.level1SpellComponents = new string[CSInfo.level1SpellCount];
        CSInfo.level1SpellDuration = new string[CSInfo.level1SpellCount];
        CSInfo.level1SpellDescription = new string[CSInfo.level1SpellCount];

        for (int i = 0; i < CSInfo.level1SpellCount; i++)
        {
            SpellInfo spell = level1SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.level1SpellNames[i] = spell.spellName.text;
            CSInfo.level1PreparedSpells[i] = spell.prepared.isOn;
            CSInfo.level1SpellSchool[i] = spell.spellSchool.value;
            CSInfo.level1SpellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.level1SpellRange[i] = spell.spellRange.text;
            CSInfo.level1SpellComponents[i] = spell.spellComponents.text;
            CSInfo.level1SpellDuration[i] = spell.spellDuration.text;
            CSInfo.level1SpellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.level2SpellCount = level2SpellsList.Count;
        CSInfo.level2SpellNames = new string[CSInfo.level2SpellCount];
        CSInfo.level2PreparedSpells = new bool[CSInfo.level2SpellCount];
        CSInfo.level2SpellSchool = new int[CSInfo.level2SpellCount];
        CSInfo.level2SpellCastingTime = new string[CSInfo.level2SpellCount];
        CSInfo.level2SpellRange = new string[CSInfo.level2SpellCount];
        CSInfo.level2SpellComponents = new string[CSInfo.level2SpellCount];
        CSInfo.level2SpellDuration = new string[CSInfo.level2SpellCount];
        CSInfo.level2SpellDescription = new string[CSInfo.level2SpellCount];

        for (int i = 0; i < CSInfo.level2SpellCount; i++)
        {
            SpellInfo spell = level2SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.level2SpellNames[i] = spell.spellName.text;
            CSInfo.level2PreparedSpells[i] = spell.prepared.isOn;
            CSInfo.level2SpellSchool[i] = spell.spellSchool.value;
            CSInfo.level2SpellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.level2SpellRange[i] = spell.spellRange.text;
            CSInfo.level2SpellComponents[i] = spell.spellComponents.text;
            CSInfo.level2SpellDuration[i] = spell.spellDuration.text;
            CSInfo.level2SpellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.level3SpellCount = level3SpellsList.Count;
        CSInfo.level3SpellNames = new string[CSInfo.level3SpellCount];
        CSInfo.level3PreparedSpells = new bool[CSInfo.level3SpellCount];
        CSInfo.level3SpellSchool = new int[CSInfo.level3SpellCount];
        CSInfo.level3SpellCastingTime = new string[CSInfo.level3SpellCount];
        CSInfo.level3SpellRange = new string[CSInfo.level3SpellCount];
        CSInfo.level3SpellComponents = new string[CSInfo.level3SpellCount];
        CSInfo.level3SpellDuration = new string[CSInfo.level3SpellCount];
        CSInfo.level3SpellDescription = new string[CSInfo.level3SpellCount];

        for (int i = 0; i < CSInfo.level3SpellCount; i++)
        {
            SpellInfo spell = level3SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.level3SpellNames[i] = spell.spellName.text;
            CSInfo.level3PreparedSpells[i] = spell.prepared.isOn;
            CSInfo.level3SpellSchool[i] = spell.spellSchool.value;
            CSInfo.level3SpellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.level3SpellRange[i] = spell.spellRange.text;
            CSInfo.level3SpellComponents[i] = spell.spellComponents.text;
            CSInfo.level3SpellDuration[i] = spell.spellDuration.text;
            CSInfo.level3SpellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.level4SpellCount = level4SpellsList.Count;
        CSInfo.level4SpellNames = new string[CSInfo.level4SpellCount];
        CSInfo.level4PreparedSpells = new bool[CSInfo.level4SpellCount];
        CSInfo.level4SpellSchool = new int[CSInfo.level4SpellCount];
        CSInfo.level4SpellCastingTime = new string[CSInfo.level4SpellCount];
        CSInfo.level4SpellRange = new string[CSInfo.level4SpellCount];
        CSInfo.level4SpellComponents = new string[CSInfo.level4SpellCount];
        CSInfo.level4SpellDuration = new string[CSInfo.level4SpellCount];
        CSInfo.level4SpellDescription = new string[CSInfo.level4SpellCount];

        for (int i = 0; i < CSInfo.level4SpellCount; i++)
        {
            SpellInfo spell = level4SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.level4SpellNames[i] = spell.spellName.text;
            CSInfo.level4PreparedSpells[i] = spell.prepared.isOn;
            CSInfo.level4SpellSchool[i] = spell.spellSchool.value;
            CSInfo.level4SpellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.level4SpellRange[i] = spell.spellRange.text;
            CSInfo.level4SpellComponents[i] = spell.spellComponents.text;
            CSInfo.level4SpellDuration[i] = spell.spellDuration.text;
            CSInfo.level4SpellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.level5SpellCount = level5SpellsList.Count;
        CSInfo.level5SpellNames = new string[CSInfo.level5SpellCount];
        CSInfo.level5PreparedSpells = new bool[CSInfo.level5SpellCount];
        CSInfo.level5SpellSchool = new int[CSInfo.level5SpellCount];
        CSInfo.level5SpellCastingTime = new string[CSInfo.level5SpellCount];
        CSInfo.level5SpellRange = new string[CSInfo.level5SpellCount];
        CSInfo.level5SpellComponents = new string[CSInfo.level5SpellCount];
        CSInfo.level5SpellDuration = new string[CSInfo.level5SpellCount];
        CSInfo.level5SpellDescription = new string[CSInfo.level5SpellCount];

        for (int i = 0; i < CSInfo.level5SpellCount; i++)
        {
            SpellInfo spell = level5SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.level5SpellNames[i] = spell.spellName.text;
            CSInfo.level5PreparedSpells[i] = spell.prepared.isOn;
            CSInfo.level5SpellSchool[i] = spell.spellSchool.value;
            CSInfo.level5SpellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.level5SpellRange[i] = spell.spellRange.text;
            CSInfo.level5SpellComponents[i] = spell.spellComponents.text;
            CSInfo.level5SpellDuration[i] = spell.spellDuration.text;
            CSInfo.level5SpellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.level6SpellCount = level6SpellsList.Count;
        CSInfo.level6SpellNames = new string[CSInfo.level6SpellCount];
        CSInfo.level6PreparedSpells = new bool[CSInfo.level6SpellCount];
        CSInfo.level6SpellSchool = new int[CSInfo.level6SpellCount];
        CSInfo.level6SpellCastingTime = new string[CSInfo.level6SpellCount];
        CSInfo.level6SpellRange = new string[CSInfo.level6SpellCount];
        CSInfo.level6SpellComponents = new string[CSInfo.level6SpellCount];
        CSInfo.level6SpellDuration = new string[CSInfo.level6SpellCount];
        CSInfo.level6SpellDescription = new string[CSInfo.level6SpellCount];

        for (int i = 0; i < CSInfo.level6SpellCount; i++)
        {
            SpellInfo spell = level6SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.level6SpellNames[i] = spell.spellName.text;
            CSInfo.level6PreparedSpells[i] = spell.prepared.isOn;
            CSInfo.level6SpellSchool[i] = spell.spellSchool.value;
            CSInfo.level6SpellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.level6SpellRange[i] = spell.spellRange.text;
            CSInfo.level6SpellComponents[i] = spell.spellComponents.text;
            CSInfo.level6SpellDuration[i] = spell.spellDuration.text;
            CSInfo.level6SpellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.level7SpellCount = level7SpellsList.Count;
        CSInfo.level7SpellNames = new string[CSInfo.level7SpellCount];
        CSInfo.level7PreparedSpells = new bool[CSInfo.level7SpellCount];
        CSInfo.level7SpellSchool = new int[CSInfo.level7SpellCount];
        CSInfo.level7SpellCastingTime = new string[CSInfo.level7SpellCount];
        CSInfo.level7SpellRange = new string[CSInfo.level7SpellCount];
        CSInfo.level7SpellComponents = new string[CSInfo.level7SpellCount];
        CSInfo.level7SpellDuration = new string[CSInfo.level7SpellCount];
        CSInfo.level7SpellDescription = new string[CSInfo.level7SpellCount];

        for (int i = 0; i < CSInfo.level7SpellCount; i++)
        {
            SpellInfo spell = level7SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.level7SpellNames[i] = spell.spellName.text;
            CSInfo.level7PreparedSpells[i] = spell.prepared.isOn;
            CSInfo.level7SpellSchool[i] = spell.spellSchool.value;
            CSInfo.level7SpellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.level7SpellRange[i] = spell.spellRange.text;
            CSInfo.level7SpellComponents[i] = spell.spellComponents.text;
            CSInfo.level7SpellDuration[i] = spell.spellDuration.text;
            CSInfo.level7SpellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.level8SpellCount = level8SpellsList.Count;
        CSInfo.level8SpellNames = new string[CSInfo.level8SpellCount];
        CSInfo.level8PreparedSpells = new bool[CSInfo.level8SpellCount];
        CSInfo.level8SpellSchool = new int[CSInfo.level8SpellCount];
        CSInfo.level8SpellCastingTime = new string[CSInfo.level8SpellCount];
        CSInfo.level8SpellRange = new string[CSInfo.level8SpellCount];
        CSInfo.level8SpellComponents = new string[CSInfo.level8SpellCount];
        CSInfo.level8SpellDuration = new string[CSInfo.level8SpellCount];
        CSInfo.level8SpellDescription = new string[CSInfo.level8SpellCount];

        for (int i = 0; i < CSInfo.level8SpellCount; i++)
        {
            SpellInfo spell = level8SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.level8SpellNames[i] = spell.spellName.text;
            CSInfo.level8PreparedSpells[i] = spell.prepared.isOn;
            CSInfo.level8SpellSchool[i] = spell.spellSchool.value;
            CSInfo.level8SpellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.level8SpellRange[i] = spell.spellRange.text;
            CSInfo.level8SpellComponents[i] = spell.spellComponents.text;
            CSInfo.level8SpellDuration[i] = spell.spellDuration.text;
            CSInfo.level8SpellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.level9SpellCount = level9SpellsList.Count;
        CSInfo.level9SpellNames = new string[CSInfo.level9SpellCount];
        CSInfo.level9PreparedSpells = new bool[CSInfo.level9SpellCount];
        CSInfo.level9SpellSchool = new int[CSInfo.level9SpellCount];
        CSInfo.level9SpellCastingTime = new string[CSInfo.level9SpellCount];
        CSInfo.level9SpellRange = new string[CSInfo.level9SpellCount];
        CSInfo.level9SpellComponents = new string[CSInfo.level9SpellCount];
        CSInfo.level9SpellDuration = new string[CSInfo.level9SpellCount];
        CSInfo.level9SpellDescription = new string[CSInfo.level9SpellCount];

        for (int i = 0; i < CSInfo.level9SpellCount; i++)
        {
            SpellInfo spell = level9SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.level9SpellNames[i] = spell.spellName.text;
            CSInfo.level9PreparedSpells[i] = spell.prepared.isOn;
            CSInfo.level9SpellSchool[i] = spell.spellSchool.value;
            CSInfo.level9SpellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.level9SpellRange[i] = spell.spellRange.text;
            CSInfo.level9SpellComponents[i] = spell.spellComponents.text;
            CSInfo.level9SpellDuration[i] = spell.spellDuration.text;
            CSInfo.level9SpellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.actions = actionsText.text;
        CSInfo.bonusActions = bonusActions.text;
        CSInfo.reactions = reactions.text;

        CSInfo.traits = traits.text;
        CSInfo.ideals = ideals.text;
        CSInfo.bonds = bonds.text;
        CSInfo.flaws = flaws.text;
    }

    // ability score methods

    void CheckStrScore()
    {
        int score;
        if (!int.TryParse(strScore.text, out score))
        {
            strScore.text = "";
        } 
        else if (score < 1)
        {
            strScore.text = "1";
        }
        else if (score > 30)
        {
            strScore.text = "30";
        }
        UpdateAbilityModifier(int.Parse(strScore.text), strModifier);
    }

    void CheckDexScore()
    {
        int score;
        if (!int.TryParse(dexScore.text, out score))
        {
            dexScore.text = "";
        }
        else if (score < 1)
        {
            dexScore.text = "1";
        }
        else if (score > 30)
        {
            dexScore.text = "30";
        }
        UpdateAbilityModifier(int.Parse(dexScore.text), dexModifier);
    }

    void CheckConScore()
    {
        int score;
        if (!int.TryParse(conScore.text, out score))
        {
            conScore.text = "";
        }
        else if (score < 1)
        {
            conScore.text = "1";
        }
        else if (score > 30)
        {
            conScore.text = "30";
        }
        UpdateAbilityModifier(int.Parse(conScore.text), conModifier);
    }

    void CheckIntScore()
    {
        int score;
        if (!int.TryParse(intScore.text, out score))
        {
            intScore.text = "";
        }
        else if (score < 1)
        {
            intScore.text = "1";
        }
        else if (score > 30)
        {
            intScore.text = "30";
        }
        UpdateAbilityModifier(int.Parse(intScore.text), intModifier);
    }

    void CheckWisScore()
    {
        int score;
        if (!int.TryParse(wisScore.text, out score))
        {
            wisScore.text = "";
        }
        else if (score < 1)
        {
            wisScore.text = "1";
        }
        else if (score > 30)
        {
            wisScore.text = "30";
        }
        UpdateAbilityModifier(int.Parse(wisScore.text), wisModifier);
    }

    void CheckChaScore()
    {
        int score;
        if (!int.TryParse(chaScore.text, out score))
        {
            chaScore.text = "";
        }
        else if (score < 1)
        {
            chaScore.text = "1";
        }
        else if (score > 30)
        {
            chaScore.text = "30";
        }
        UpdateAbilityModifier(int.Parse(chaScore.text), chaModifier);
    }

    // dice roll methods

    public void ResolveCheckOrSave(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        foreach (int diceScore in roll.diceScores)
        {
            result += diceScore;
        }

        result += modifier;

        uiManager.NotifyDiceScoreClientRpc(roll.playerName + roll.message + result.ToString());

        diceHandler.DeleteRoll(rollKey);
    }

    public void HitDiceHeal(int result)
    {
        int hp = int.Parse(currHealthPoints.text) + result;
        int maxHP = int.Parse(maxHealthPoints.text);

        if (hp > maxHP)
        {
            hp = maxHP;
        }

        currHealthPoints.text = hp.ToString();
    }

    public void ProcessDeathSavingThrow(int result)
    { 
        if (result == 1)
        {
            DeathSavingThrowFail();
            DeathSavingThrowFail();
        }
        else if (result < 10)
        {
            DeathSavingThrowFail();
        }
        else if (result == 20)
        {
            currHealthPoints.text = "1";
            ResetDeathSavingThrows();
        }
        else
        {
            DeathSavingThrowSuccess();
        }
    }

    // skill methods

    void CheckProficencyBonus()
    {
        int bonus;
        if (!int.TryParse(proficencyBonus.text, out bonus))
        {
            proficencyBonus.text = "";
        }
        else if (bonus < 2)
        {
            proficencyBonus.text = "2";
        }
        else if (bonus > 6)
        {
            proficencyBonus.text = "6";
        }
    }

    void CalculateSkillTotalBonus(Dropdown prof, Dropdown skill, InputField bonus, Text total)
    {
        int bonusMod;
        int profMod = GetSkillProficencyBonus(prof);
        int charMod = GetSkillCharacteristicBonus(skill);
        int totalBonus = 0;
        if (int.TryParse(bonus.text, out bonusMod))
        {
            totalBonus = profMod + charMod + bonusMod;
        }
        else
        {
            bonus.text = "";
            totalBonus = profMod + charMod;
        }
        total.text = totalBonus.ToString();
    }

    // inventory methods

    void AddItemToInventory()
    {
        if (itemList.Count >= 30) return;

        Vector3 position = new Vector3(215f, - 20 + itemList.Count * itemSpace, 0);
        
        GameObject item = Instantiate(itemPrefab);                     
        item.GetComponent<RectTransform>().SetParent(inventoryArea.transform);
        item.GetComponent<RectTransform>().localPosition = position;
        itemList.Add(item);

    }

    void CalculateMaxWeight()
    {
        int newMaxWeight = int.Parse(strScore.text) * 15;
        maxWeight.text = newMaxWeight.ToString();
    }

    // spell methods

    void UpdateSpellAbilityMod()
    {
        int prof = int.Parse(proficencyBonus.text);
        int mod;

        switch (spellModifier.value)
        {
            case 0:
                mod = int.Parse(intModifier.text);
                spellSaveDc.text = (8 + prof + mod).ToString();
                spellAttackMod.text = (prof + mod).ToString();
                break;
            case 1:
                mod = int.Parse(wisModifier.text);
                spellSaveDc.text = (8 + prof + mod).ToString();
                spellAttackMod.text = (prof + mod).ToString();
                break;
            case 2:
                mod = int.Parse(chaModifier.text);
                spellSaveDc.text = (8 + prof + mod).ToString();
                spellAttackMod.text = (prof + mod).ToString();
                break;
        }
    }
    
    void SwitchSpellLevelList()
    {
        switch (spellLevelSelector.value)
        {
            case 0:
                currSpellLevel.SetActive(false);
                level0Spells.SetActive(true);
                currSpellLevel = level0Spells;
                break;
            case 1:
                currSpellLevel.SetActive(false);
                level1Spells.SetActive(true);
                currSpellLevel = level1Spells;
                break;
            case 2:
                currSpellLevel.SetActive(false);
                level2Spells.SetActive(true);
                currSpellLevel = level2Spells;
                break;
            case 3:
                currSpellLevel.SetActive(false);
                level3Spells.SetActive(true);
                currSpellLevel = level3Spells;
                break;
            case 4:
                currSpellLevel.SetActive(false);
                level4Spells.SetActive(true);
                currSpellLevel = level4Spells;
                break;
            case 5:
                currSpellLevel.SetActive(false);
                level5Spells.SetActive(true);
                currSpellLevel = level5Spells;
                break;
            case 6:
                currSpellLevel.SetActive(false);
                level6Spells.SetActive(true);
                currSpellLevel = level6Spells;
                break;
            case 7:
                currSpellLevel.SetActive(false);
                level7Spells.SetActive(true);
                currSpellLevel = level7Spells;
                break;
            case 8:
                currSpellLevel.SetActive(false);
                level8Spells.SetActive(true);
                currSpellLevel = level8Spells;
                break;
            case 9:
                currSpellLevel.SetActive(false);
                level9Spells.SetActive(true);
                currSpellLevel = level9Spells;
                break;
        }
    }

    void AddSpell()
    {
        switch (spellLevelSelector.value)
        {
            case 0:
                AddNewSpellByLevel(level0SpellsList, level0Area);
                break;
            case 1:
                AddNewSpellByLevel(level1SpellsList, level1Area);
                break;
            case 2:
                AddNewSpellByLevel(level2SpellsList, level2Area);
                break;
            case 3:
                AddNewSpellByLevel(level3SpellsList, level3Area);
                break;
            case 4:
                AddNewSpellByLevel(level4SpellsList, level4Area);
                break;
            case 5:
                AddNewSpellByLevel(level5SpellsList, level5Area);
                break;
            case 6:
                AddNewSpellByLevel(level6SpellsList, level6Area);
                break;
            case 7:
                AddNewSpellByLevel(level7SpellsList, level7Area);
                break;
            case 8:
                AddNewSpellByLevel(level8SpellsList, level8Area);
                break;
            case 9:
                AddNewSpellByLevel(level9SpellsList, level9Area);
                break;
        }
    }

    private void AddNewSpellByLevel(List<GameObject> spellList, GameObject listArea)
    {
        Vector3 position = new Vector3(75f, -20 + spellList.Count * spellSpace, 0);

        GameObject spell = Instantiate(spellPrefab);
        GameObject spellItem = spell.GetComponent<SpellInfo>().spellItem;
        spellItem.GetComponent<RectTransform>().SetParent(listArea.transform);
        spellItem.GetComponent<RectTransform>().localPosition = position;
        spellList.Add(spell);
    }

    // token related methods

    void SpawnToken(ulong ownerID, string ownerName, int avatarID, CharacterSheetInfo characterSheetInfo)
    {
        gameManager.SpawnToken(ownerID, ownerName, avatarID, characterSheetInfo);
    }

    public void UpdateTokenHealthPoints(string tempHP, string currHP)
    {
        TokenController[] tokens = FindObjectsOfType<TokenController>();

        foreach (TokenController token in tokens)
        {
            if (token.characterSheetInfo.sheetID == CSInfo.sheetID)
            {
                token.characterSheetInfo.tempHealthPoints = tempHP;
                token.characterSheetInfo.currHealthPoints = currHP;
            }
        }
    }

    // auxiliary methods

    private void CheckPermisson()
    {
        if (permisson)
        {
            publicInfoBlocker.enabled = false;
            basicInfoBlocker.enabled = false;
            skillsBlocker.enabled = false;
            featuresBlocker.enabled = false;
            inventoryBlocker.enabled = false;
            spellsBlocker1.enabled = false;
            spellsBlocker2.enabled = false;
            spellsBlocker3.enabled = false;
            actionsBlocker.enabled = false;
            personalityBlocker.enabled = false;

            basicInfoPublisher.gameObject.SetActive(true);
            skillsPublisher.gameObject.SetActive(true);
            featuresPublisher.gameObject.SetActive(true);
            inventoryPublisher.gameObject.SetActive(true);
            spellsPublisher.gameObject.SetActive(true);
            actionsPublisher.gameObject.SetActive(true);
            personalityPublisher.gameObject.SetActive(true);

            buttonSpawnToken.enabled = true;
        }
    }

    void CheckInt(InputField inputField)
    {
        int value;
        if (!int.TryParse(inputField.text, out value))
        {
            inputField.text = "";
        }
    }

    DiceType GetHitDice(int diceSelector)
    {
        switch (diceSelector)
        {
            case 0:
                return DiceType.d6;
            case 1:
                return DiceType.d8;
            case 2:
                return DiceType.d10;
            case 3:
                return DiceType.d12;
        }
        return DiceType.pd; // error check
    }

    void ResetDeathSavingThrows()
    {
        deathSaveFail1.isOn = false;
        deathSaveFail2.isOn = false;
        deathSaveFail3.isOn = false;
        deathSaveSuccess1.isOn = false;
        deathSaveSuccess2.isOn = false;
        deathSaveSuccess3.isOn = false;
    }

    void UpdateAbilityModifier(int score, Text modifier)
    {
        modifier.text = CalculateAbilityModifier(score).ToString();
    }

    int CalculateAbilityModifier(int abilityScore)
    {
        int mod = -5; // modifier value for a score of 1

        mod += abilityScore / 2;

        return mod;
    }

    int GetSkillProficencyBonus(Dropdown profType)
    {
        switch (profType.value)
        {
            case 0: 
                return 0;
            case 1:
                return int.Parse(proficencyBonus.text);
            case 2:
                return int.Parse(proficencyBonus.text) / 2;
            case 3:
                return int.Parse(proficencyBonus.text) * 2;
        }
        return -1;
    }

    int GetSkillCharacteristicBonus(Dropdown characteristic)
    {
        switch (characteristic.value)
        {
            case 0:
                return int.Parse(strModifier.text);
            case 1:
                return int.Parse(dexModifier.text);
            case 2:
                return int.Parse(conModifier.text);
            case 3:
                return int.Parse(intModifier.text);
            case 4:
                return int.Parse(wisModifier.text);
            case 5:
                return int.Parse(chaModifier.text);
        }
        return 0;
    }

    void RefreshSheetData()
    {
        if (!permisson) { return; }
        SetCharacterInfo();
        gameManager.SaveCharacterSheetChanges(CSInfo);
        //GetCharacterInfo();
    }

    void CheckPublicPages(bool permisson)
    {
        if (permisson) { return; }

        buttonBasicInfo.interactable = basicInfoPublisher.isOn;
        buttonSkills.interactable = skillsPublisher.isOn;
        buttonFeatures.interactable = featuresPublisher.isOn;
        buttonInventory.interactable = inventoryPublisher.isOn;
        buttonSpells.interactable = spellsPublisher.isOn;
        buttonActions.interactable = actionsPublisher.isOn;
        buttonPersonality.interactable = personalityPublisher.isOn;
    }

    void DeathSavingThrowFail()
    {
        if (!deathSaveFail1.isOn)
        {
            deathSaveFail1.isOn = true;
            return;
        }
        if (!deathSaveFail2.isOn)
        {
            deathSaveFail2.isOn = true;
            return;
        }
        if (!deathSaveFail3.isOn)
        {
            deathSaveFail3.isOn = true;
            return;
        }
    }

    void DeathSavingThrowSuccess()
    {
        if (!deathSaveSuccess1.isOn)
        {
            deathSaveSuccess1.isOn = true;
            return;
        }
        if (!deathSaveSuccess2.isOn)
        {
            deathSaveSuccess2.isOn = true;
            return;
        }
        if (!deathSaveSuccess3.isOn)
        {
            deathSaveSuccess3.isOn = true;
            return;
        }
    }

    #endregion

    #region rolling methods

    // ability and skill checks

    void RollStrenghtCheck()
    {
        diceHandler.RollCheck(characterName.text, "Strenght", int.Parse(strModifier.text));
    }

    void RollDexterityCheck()
    {
        diceHandler.RollCheck(characterName.text, "Dexterity", int.Parse(dexModifier.text));
    }

    void RollConstitutionCheck()
    {
        diceHandler.RollCheck(characterName.text, "Constitution", int.Parse(conModifier.text));
    }

    void RollIntelligenceCheck()
    {
        diceHandler.RollCheck(characterName.text, "Intelligence", int.Parse(intModifier.text));
    }

    void RollWisdomCheck()
    {
        diceHandler.RollCheck(characterName.text, "Wisdom", int.Parse(wisModifier.text));
    }

    void RollCharismaCheck()
    {
        diceHandler.RollCheck(characterName.text, "Charisma", int.Parse(chaModifier.text));
    }

    void RollSkillCheck(string skillName, string skillTotalBonus)
    {
        diceHandler.RollCheck(characterName.text, skillName, int.Parse(skillTotalBonus));
    }

    void RollInitiativeCheck()
    {
        if (initiativeBonus.text == "")
        {
            initiativeBonus.text = "0";
        }

        diceHandler.RollCheck(characterName.text, "Initiative", int.Parse(initiativeBonus.text));
    }

    // ability saving throws

    void RollStrengthSave()
    {
        diceHandler.RollAbilitySave(characterName.text, "Strength", int.Parse(strModifier.text));
    }

    void RollDexteritySave()
    {
        diceHandler.RollAbilitySave(characterName.text, "Dexterity", int.Parse(dexModifier.text));
    }

    void RollConstitutionSave()
    {
        diceHandler.RollAbilitySave(characterName.text, "Constitution", int.Parse(conModifier.text));
    }

    void RollIntelligenceSave()
    {
        diceHandler.RollAbilitySave(characterName.text, "Intelligence", int.Parse(intModifier.text));
    }

    void RollWisdomSave()
    {
        diceHandler.RollAbilitySave(characterName.text, "Wisdom", int.Parse(wisModifier.text));
    }

    void RollCharismaSave()
    {
        diceHandler.RollAbilitySave(characterName.text, "Charisma", int.Parse(chaModifier.text));
    }

    // other rolls

    void RollHitDice()
    {
        diceHandler.RollHitDice(characterName.text, GetHitDice(hitDice.value), int.Parse(conModifier.text), CSInfo.sheetID);
    }

    void RollDeathSavingThrow()
    {
        diceHandler.RollDeathSavingThrow(characterName.text, CSInfo.sheetID);
    }

    #endregion

}

[Serializable]
public struct Skill
{
    public string skillName;
    public Button skillCheck;
    public Dropdown skillProficency;
    public Dropdown skillCharacteristic;
    public InputField skillExtraBonus;
    public Text skillTotalBonus;
}
