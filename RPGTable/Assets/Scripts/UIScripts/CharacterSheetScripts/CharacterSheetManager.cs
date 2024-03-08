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
    private TokenManager tokenManager;

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

    [SerializeField] Button addFeature;
    [SerializeField] GameObject featuresParent;
    [SerializeField] GameObject traitsPrefab;
    private List<GameObject> traitList = new List<GameObject>();

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
    [SerializeField] GameObject actionsPrefab;

    [SerializeField] GameObject actionsParent;
    [SerializeField] Button addAction;
    List<GameObject> actionList = new List<GameObject>();

    [SerializeField] GameObject bonusActionsParent;
    [SerializeField] Button addBonusAction;
    List<GameObject> bonusActionList = new List<GameObject>();

    [SerializeField] GameObject reactionsParent;
    [SerializeField] Button addReaction;
    List<GameObject> reactionList = new List<GameObject>();

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
        tokenManager = FindObjectOfType<TokenManager>();

        // navigation related events
        buttonClose.onClick.AddListener(() => CloseSheet());
        buttonSpawnToken.onClick.AddListener(() => SpawnToken(CSInfo.ownerID, playerName.text, CSInfo.publicPageCharacterInfo.avatarID, CSInfo));
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
        strScore.onValueChanged.AddListener(delegate { CheckAbilityScore(strScore, strModifier); CalculateMaxWeight(); });
        dexScore.onValueChanged.AddListener(delegate { CheckAbilityScore(dexScore, dexModifier); });
        conScore.onValueChanged.AddListener(delegate { CheckAbilityScore(conScore, conModifier); });
        intScore.onValueChanged.AddListener(delegate { CheckAbilityScore(intScore, intModifier); });
        wisScore.onValueChanged.AddListener(delegate { CheckAbilityScore(wisScore, wisModifier); });
        chaScore.onValueChanged.AddListener(delegate { CheckAbilityScore(chaScore, chaModifier); });

        // ability related roll events
        strCheck.onClick.AddListener(() => RollCheck("Strenght", strModifier.text));
        dexCheck.onClick.AddListener(() => RollCheck("Dexterity", dexModifier.text));
        conCheck.onClick.AddListener(() => RollCheck("Constitution", conModifier.text));
        intCheck.onClick.AddListener(() => RollCheck("Intelligence", intModifier.text));
        wisCheck.onClick.AddListener(() => RollCheck("Wisdom", wisModifier.text));
        chaCheck.onClick.AddListener(() => RollCheck("Charisma", chaModifier.text));

        strSave.onClick.AddListener(() => RollSavingThrow("Strenght", strModifier.text));
        dexSave.onClick.AddListener(() => RollSavingThrow("Dexterity", dexModifier.text));
        conSave.onClick.AddListener(() => RollSavingThrow("Constitution", conModifier.text));
        intSave.onClick.AddListener(() => RollSavingThrow("Intelligence", intModifier.text));
        wisSave.onClick.AddListener(() => RollSavingThrow("Wisdom", wisModifier.text));
        chaSave.onClick.AddListener(() => RollSavingThrow("Charisma", chaModifier.text));

        // character stats related events
        currHealthPoints.onValueChanged.AddListener(delegate { UpdateTokenHealthPoints(tempHealthPoints.text, currHealthPoints.text); });
        tempHealthPoints.onValueChanged.AddListener(delegate { UpdateTokenHealthPoints(tempHealthPoints.text, currHealthPoints.text); });

        rollHitDice.onClick.AddListener(() => RollHitDice());
        rollInitiative.onClick.AddListener(() => RollInitiative());
        rollDeathSave.onClick.AddListener(() => RollDeathSavingThrow());
        resetDeathSaves.onClick.AddListener(() => ResetDeathSavingThrows());

        // skill related logic events
        proficencyBonus.onValueChanged.AddListener(delegate { CheckProficencyBonus(); });
        foreach (Skill skill in skillList)
        {
            proficencyBonus.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });

            strScore.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus);  });
            dexScore.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });
            conScore.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });
            intScore.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });
            wisScore.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });
            chaScore.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });

            skill.skillProficency.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });
            skill.skillCharacteristic.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });
            skill.skillExtraBonus.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });

            CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus);

            // skill related roll events
            skill.skillCheck.onClick.AddListener(() => RollCheck(skill.skillName, skill.skillTotalBonus.text));
        }

        // features related events
        addFeature.onClick.AddListener(() => AddNewTrait(traitList, featuresParent));

        // inventory related events
        addItem.onClick.AddListener(() => AddItemToInventory());

        // spell related events
        spellModifier.onValueChanged.AddListener(delegate { UpdateSpellAbilityMod(); });

        spellLevelSelector.onValueChanged.AddListener(delegate { SwitchSpellLevelList(); });
        addSpell.onClick.AddListener(() => AddSpell());

        // action related events
        addAction.onClick.AddListener(() => AddNewAction(actionList, actionsParent));
        addBonusAction.onClick.AddListener(() => AddNewAction(bonusActionList, bonusActionsParent));
        addReaction.onClick.AddListener(() => AddNewAction(reactionList, reactionsParent));

        if (CSInfo != null)
        {
            LoadCharacterInfo();
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
            SaveCharacterInfo();
            gameManager.SaveCharacterSheetChanges(CSInfo);
        }
        Destroy(characterSheet);
       
    }
    #endregion

    #region Logic Methods

    void LoadCharacterInfo()
    {
        characterName.text = CSInfo.publicPageCharacterInfo.characterName;
        playerName.text = CSInfo.publicPageCharacterInfo.playerName;
        appearance.text = CSInfo.publicPageCharacterInfo.appearance;

        basicInfoPublisher.isOn = CSInfo.publicBasicInfo;
        skillsPublisher.isOn = CSInfo.publicSkills;
        featuresPublisher.isOn = CSInfo.publicFeatures;
        inventoryPublisher.isOn = CSInfo.publicInventory;
        spellsPublisher.isOn = CSInfo.publicSpells;
        actionsPublisher.isOn = CSInfo.publicActions;
        personalityPublisher.isOn = CSInfo.publicPersonality;

        #region basic info 
        clasAndLevel.text = CSInfo.basicPageCharacterInfo.clasAndLevel;
        subclass.text = CSInfo.basicPageCharacterInfo.subclass;
        race.text = CSInfo.basicPageCharacterInfo.race;
        background.text = CSInfo.basicPageCharacterInfo.background;
        alignement.text = CSInfo.basicPageCharacterInfo.alignement;
        experience.text = CSInfo.basicPageCharacterInfo.experience;

        strScore.text = CSInfo.basicPageCharacterInfo.strScore;
        dexScore.text = CSInfo.basicPageCharacterInfo.dexScore;
        conScore.text = CSInfo.basicPageCharacterInfo.conScore;
        intScore.text = CSInfo.basicPageCharacterInfo.intScore;
        wisScore.text = CSInfo.basicPageCharacterInfo.wisScore;
        chaScore.text = CSInfo.basicPageCharacterInfo.chaScore;

        maxHealthPoints.text = CSInfo.basicPageCharacterInfo.maxHealthPoints;
        currHealthPoints.text = CSInfo.basicPageCharacterInfo.currHealthPoints;
        tempHealthPoints.text = CSInfo.basicPageCharacterInfo.tempHealthPoints;
        initiativeBonus.text = CSInfo.basicPageCharacterInfo.initiativeBonus;
        armorClass.text = CSInfo.basicPageCharacterInfo.armorClass;
        speed.text = CSInfo.basicPageCharacterInfo.speed;
        hitDice.value = CSInfo.basicPageCharacterInfo.hitDiceType;
        switch (CSInfo.basicPageCharacterInfo.deathSaveSuccesses)
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
        switch (CSInfo.basicPageCharacterInfo.deathSaveFails)
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
        #endregion

        #region skills info

        strProficency.isOn = CSInfo.skillsPageCharacterInfo.strProf;
        dexProficency.isOn = CSInfo.skillsPageCharacterInfo.dexProf;
        conProficency.isOn = CSInfo.skillsPageCharacterInfo.conProf;
        intProficency.isOn = CSInfo.skillsPageCharacterInfo.intProf;
        wisProficency.isOn = CSInfo.skillsPageCharacterInfo.wisProf;
        chaProficency.isOn = CSInfo.skillsPageCharacterInfo.chaProf;

        for (int i = 0; i < skillList.Count; i++)
        {
            skillList[i].skillProficency.value = CSInfo.skillsPageCharacterInfo.skillProf[i];
            skillList[i].skillCharacteristic.value = CSInfo.skillsPageCharacterInfo.skillCharacteristic[i];
            skillList[i].skillExtraBonus.text = CSInfo.skillsPageCharacterInfo.skillBonus[i];
            skillList[i].skillTotalBonus.text = CSInfo.skillsPageCharacterInfo.skillTotal[i];
        }

        proficencyBonus.text = CSInfo.skillsPageCharacterInfo.proficencyBonus;

        #endregion

        #region features and traits info
        featuresAndTraits.text = CSInfo.featuresPageCharacterInfo.featuresAndTraits;
        for (int i = 0; i < CSInfo.featuresPageCharacterInfo.traitCount; i++)
        {
            AddNewTrait(traitList, featuresParent);
            CharacterTrait trait = traitList[i].GetComponent<CharacterTrait>();

            trait.traitName.text = CSInfo.featuresPageCharacterInfo.traitName[i];
            trait.traitDescriptionInput.text = CSInfo.featuresPageCharacterInfo.traitDescription[i];
            trait.UpdateDescriptionAndSize();
        }
        proficencies.text = CSInfo.featuresPageCharacterInfo.proficencies;
        #endregion

        #region inventory
        for (int i = 0; i < CSInfo.inventoryPageCharacterInfo.itemCount; i++)
        {
            AddItemToInventory();
            CharacterSheetItem item = itemList[i].GetComponent<CharacterSheetItem>();
            item.itemName.text = CSInfo.inventoryPageCharacterInfo.itemNames[i];
            item.itemAmount.text = CSInfo.inventoryPageCharacterInfo.itemAmounts[i];
            item.itemWeight.text = CSInfo.inventoryPageCharacterInfo.itemWeights[i];
        }      

        copperPieces.text = CSInfo.inventoryPageCharacterInfo.copperPieces;
        silverPieces.text = CSInfo.inventoryPageCharacterInfo.silverPieces;
        electrumPieces.text = CSInfo.inventoryPageCharacterInfo.electrumPieces;
        goldPieces.text = CSInfo.inventoryPageCharacterInfo.goldPieces;
        platinumPieces.text = CSInfo.inventoryPageCharacterInfo.platinumPieces;
        #endregion

        #region spells
        spellModifier.value = CSInfo.spellsPageCharacterInfo.spellCastingAbility;
        spellSaveDc.text = CSInfo.spellsPageCharacterInfo.spellSaveDC;
        spellAttackMod.text = CSInfo.spellsPageCharacterInfo.spellAttackMod;

        for (int i = 0; i < CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellCount; i++)
        {
            AddNewSpellByLevel(level0SpellsList, level0Area);
            SpellInfo spell = level0SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellNames[i];
            spell.prepared.isOn = CSInfo.spellsPageCharacterInfo.spellLevelList[0].preparedSpells[i];
            spell.spellLevel.value = 0;
            spell.spellSchool.value = CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellSchool[i];
            spell.spellCastingTime.text = CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellCastingTime[i];
            spell.spellRange.text = CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellCastingTime[i];
            spell.spellComponents.text = CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellComponents[i];
            spell.spellDuration.text = CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellDuration[i];
            spell.spellDescription.text = CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellDescription[i];
        }

        for (int i = 0; i < CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellCount; i++)
        {
            AddNewSpellByLevel(level1SpellsList, level1Area);
            SpellInfo spell = level1SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellNames[i];
            spell.prepared.isOn = CSInfo.spellsPageCharacterInfo.spellLevelList[1].preparedSpells[i];
            spell.spellLevel.value = 1;
            spell.spellSchool.value = CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellSchool[i];
            spell.spellCastingTime.text = CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellCastingTime[i];
            spell.spellRange.text = CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellCastingTime[i];
            spell.spellComponents.text = CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellComponents[i];
            spell.spellDuration.text = CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellDuration[i];
            spell.spellDescription.text = CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellDescription[i];
        }

        for (int i = 0; i < CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellCount; i++)
        {
            AddNewSpellByLevel(level2SpellsList, level2Area);
            SpellInfo spell = level2SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellNames[i];
            spell.prepared.isOn = CSInfo.spellsPageCharacterInfo.spellLevelList[2].preparedSpells[i];
            spell.spellLevel.value = 2;
            spell.spellSchool.value = CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellSchool[i];
            spell.spellCastingTime.text = CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellCastingTime[i];
            spell.spellRange.text = CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellCastingTime[i];
            spell.spellComponents.text = CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellComponents[i];
            spell.spellDuration.text = CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellDuration[i];
            spell.spellDescription.text = CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellDescription[i];
        }

        for (int i = 0; i < CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellCount; i++)
        {
            AddNewSpellByLevel(level3SpellsList, level0Area);
            SpellInfo spell = level3SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellNames[i];
            spell.prepared.isOn = CSInfo.spellsPageCharacterInfo.spellLevelList[3].preparedSpells[i];
            spell.spellLevel.value = 3;
            spell.spellSchool.value = CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellSchool[i];
            spell.spellCastingTime.text = CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellCastingTime[i];
            spell.spellRange.text = CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellCastingTime[i];
            spell.spellComponents.text = CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellComponents[i];
            spell.spellDuration.text = CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellDuration[i];
            spell.spellDescription.text = CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellDescription[i];
        }

        for (int i = 0; i < CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellCount; i++)
        {
            AddNewSpellByLevel(level4SpellsList, level0Area);
            SpellInfo spell = level4SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellNames[i];
            spell.prepared.isOn = CSInfo.spellsPageCharacterInfo.spellLevelList[4].preparedSpells[i];
            spell.spellLevel.value = 4;
            spell.spellSchool.value = CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellSchool[i];
            spell.spellCastingTime.text = CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellCastingTime[i];
            spell.spellRange.text = CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellCastingTime[i];
            spell.spellComponents.text = CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellComponents[i];
            spell.spellDuration.text = CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellDuration[i];
            spell.spellDescription.text = CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellDescription[i];
        }

        for (int i = 0; i < CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellCount; i++)
        {
            AddNewSpellByLevel(level5SpellsList, level5Area);
            SpellInfo spell = level5SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellNames[i];
            spell.prepared.isOn = CSInfo.spellsPageCharacterInfo.spellLevelList[5].preparedSpells[i];
            spell.spellLevel.value = 5;
            spell.spellSchool.value = CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellSchool[i];
            spell.spellCastingTime.text = CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellCastingTime[i];
            spell.spellRange.text = CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellCastingTime[i];
            spell.spellComponents.text = CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellComponents[i];
            spell.spellDuration.text = CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellDuration[i];
            spell.spellDescription.text = CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellDescription[i];
        }

        for (int i = 0; i < CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellCount; i++)
        {
            AddNewSpellByLevel(level6SpellsList, level6Area);
            SpellInfo spell = level6SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellNames[i];
            spell.prepared.isOn = CSInfo.spellsPageCharacterInfo.spellLevelList[6].preparedSpells[i];
            spell.spellLevel.value = 6;
            spell.spellSchool.value = CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellSchool[i];
            spell.spellCastingTime.text = CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellCastingTime[i];
            spell.spellRange.text = CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellCastingTime[i];
            spell.spellComponents.text = CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellComponents[i];
            spell.spellDuration.text = CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellDuration[i];
            spell.spellDescription.text = CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellDescription[i];
        }

        for (int i = 0; i < CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellCount; i++)
        {
            AddNewSpellByLevel(level7SpellsList, level7Area);
            SpellInfo spell = level7SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellNames[i];
            spell.prepared.isOn = CSInfo.spellsPageCharacterInfo.spellLevelList[7].preparedSpells[i];
            spell.spellLevel.value = 7;
            spell.spellSchool.value = CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellSchool[i];
            spell.spellCastingTime.text = CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellCastingTime[i];
            spell.spellRange.text = CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellCastingTime[i];
            spell.spellComponents.text = CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellComponents[i];
            spell.spellDuration.text = CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellDuration[i];
            spell.spellDescription.text = CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellDescription[i];
        }

        for (int i = 0; i < CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellCount; i++)
        {
            AddNewSpellByLevel(level8SpellsList, level8Area);
            SpellInfo spell = level8SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellNames[i];
            spell.prepared.isOn = CSInfo.spellsPageCharacterInfo.spellLevelList[8].preparedSpells[i];
            spell.spellLevel.value = 8;
            spell.spellSchool.value = CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellSchool[i];
            spell.spellCastingTime.text = CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellCastingTime[i];
            spell.spellRange.text = CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellCastingTime[i];
            spell.spellComponents.text = CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellComponents[i];
            spell.spellDuration.text = CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellDuration[i];
            spell.spellDescription.text = CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellDescription[i];
        }

        for (int i = 0; i < CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellCount; i++)
        {
            AddNewSpellByLevel(level9SpellsList, level0Area);
            SpellInfo spell = level9SpellsList[i].GetComponent<SpellInfo>();
            spell.spellName.text = CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellNames[i];
            spell.prepared.isOn = CSInfo.spellsPageCharacterInfo.spellLevelList[9].preparedSpells[i];
            spell.spellLevel.value = 9;
            spell.spellSchool.value = CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellSchool[i];
            spell.spellCastingTime.text = CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellCastingTime[i];
            spell.spellRange.text = CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellCastingTime[i];
            spell.spellComponents.text = CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellComponents[i];
            spell.spellDuration.text = CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellDuration[i];
            spell.spellDescription.text = CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellDescription[i];
        }
        #endregion

        #region actions
        for (int i = 0; i < CSInfo.actionsPageCharacterInfo.actionList.actionCount; i++)
        {
            AddNewAction(actionList, actionsParent);
            PCActionInfo action = actionList[i].GetComponent<PCActionInfo>();

            action.actionName.text = CSInfo.actionsPageCharacterInfo.actionList.actionName[i];
            action.actionType.value = CSInfo.actionsPageCharacterInfo.actionList.actionType[i];
            action.weaponTemplate.value = CSInfo.actionsPageCharacterInfo.actionList.actionWeapon[i];

            action.wpnAttackAbility.value = CSInfo.actionsPageCharacterInfo.actionList.actionAttackAbility[i];
            action.wpnOtherAttackBonus.text = CSInfo.actionsPageCharacterInfo.actionList.actionAttackOtherBonus[i];
            action.wpnAttackProficency.isOn = CSInfo.actionsPageCharacterInfo.actionList.actionAttackProficency[i];

            action.wpnDamage1NumberOfDices.text = CSInfo.actionsPageCharacterInfo.actionList.actionD1NumDices[i];
            action.wpnDamage1DiceType.value = CSInfo.actionsPageCharacterInfo.actionList.actionD1DiceType[i];
            action.wpnDamage1Ability.value = CSInfo.actionsPageCharacterInfo.actionList.actionD1Ability[i];
            action.wpnDamage1OtherBonus.text = CSInfo.actionsPageCharacterInfo.actionList.actionD1OtherBonus[i];
            action.wpnDamage1DamageType.value = CSInfo.actionsPageCharacterInfo.actionList.actionD1Type[i];

            action.wpnDamage2NumberOfDices.text = CSInfo.actionsPageCharacterInfo.actionList.actionD2NumDices[i];
            action.wpnDamage2DiceType.value = CSInfo.actionsPageCharacterInfo.actionList.actionD2DiceType[i];
            action.wpnDamage2Ability.value = CSInfo.actionsPageCharacterInfo.actionList.actionD2Ability[i];
            action.wpnDamage2OtherBonus.text = CSInfo.actionsPageCharacterInfo.actionList.actionD2OtherBonus[i];
            action.wpnDamage2DamageType.value = CSInfo.actionsPageCharacterInfo.actionList.actionD2Type[i];

            action.saveDC = CSInfo.actionsPageCharacterInfo.actionList.actionDC[i];

            action.SetActionConfig();
        }

        for (int i = 0; i < CSInfo.actionsPageCharacterInfo.bonusActionList.actionCount; i++)
        {
            AddNewAction(bonusActionList, bonusActionsParent);
            PCActionInfo action = bonusActionList[i].GetComponent<PCActionInfo>();

            action.actionName.text = CSInfo.actionsPageCharacterInfo.bonusActionList.actionName[i];
            action.actionType.value = CSInfo.actionsPageCharacterInfo.bonusActionList.actionType[i];
            action.weaponTemplate.value = CSInfo.actionsPageCharacterInfo.bonusActionList.actionWeapon[i];

            action.wpnAttackAbility.value = CSInfo.actionsPageCharacterInfo.bonusActionList.actionAttackAbility[i];
            action.wpnOtherAttackBonus.text = CSInfo.actionsPageCharacterInfo.bonusActionList.actionAttackOtherBonus[i];
            action.wpnAttackProficency.isOn = CSInfo.actionsPageCharacterInfo.bonusActionList.actionAttackProficency[i];

            action.wpnDamage1NumberOfDices.text = CSInfo.actionsPageCharacterInfo.bonusActionList.actionD1NumDices[i];
            action.wpnDamage1DiceType.value = CSInfo.actionsPageCharacterInfo.bonusActionList.actionD1DiceType[i];
            action.wpnDamage1Ability.value = CSInfo.actionsPageCharacterInfo.bonusActionList.actionD1Ability[i];
            action.wpnDamage1OtherBonus.text = CSInfo.actionsPageCharacterInfo.bonusActionList.actionD1OtherBonus[i];
            action.wpnDamage1DamageType.value = CSInfo.actionsPageCharacterInfo.bonusActionList.actionD1Type[i];

            action.wpnDamage2NumberOfDices.text = CSInfo.actionsPageCharacterInfo.bonusActionList.actionD2NumDices[i];
            action.wpnDamage2DiceType.value = CSInfo.actionsPageCharacterInfo.bonusActionList.actionD2DiceType[i];
            action.wpnDamage2Ability.value = CSInfo.actionsPageCharacterInfo.bonusActionList.actionD2Ability[i];
            action.wpnDamage2OtherBonus.text = CSInfo.actionsPageCharacterInfo.bonusActionList.actionD2OtherBonus[i];
            action.wpnDamage2DamageType.value = CSInfo.actionsPageCharacterInfo.bonusActionList.actionD2Type[i];

            action.saveDC = CSInfo.actionsPageCharacterInfo.bonusActionList.actionDC[i];

            action.SetActionConfig();
        }

        for (int i = 0; i < CSInfo.actionsPageCharacterInfo.reactionList.actionCount; i++)
        {
            AddNewAction(reactionList, reactionsParent);
            PCActionInfo action = reactionList[i].GetComponent<PCActionInfo>();

            action.actionName.text = CSInfo.actionsPageCharacterInfo.reactionList.actionName[i];
            action.actionType.value = CSInfo.actionsPageCharacterInfo.reactionList.actionType[i];
            action.weaponTemplate.value = CSInfo.actionsPageCharacterInfo.reactionList.actionWeapon[i];

            action.wpnAttackAbility.value = CSInfo.actionsPageCharacterInfo.reactionList.actionAttackAbility[i];
            action.wpnOtherAttackBonus.text = CSInfo.actionsPageCharacterInfo.reactionList.actionAttackOtherBonus[i];
            action.wpnAttackProficency.isOn = CSInfo.actionsPageCharacterInfo.reactionList.actionAttackProficency[i];

            action.wpnDamage1NumberOfDices.text = CSInfo.actionsPageCharacterInfo.reactionList.actionD1NumDices[i];
            action.wpnDamage1DiceType.value = CSInfo.actionsPageCharacterInfo.reactionList.actionD1DiceType[i];
            action.wpnDamage1Ability.value = CSInfo.actionsPageCharacterInfo.reactionList.actionD1Ability[i];
            action.wpnDamage1OtherBonus.text = CSInfo.actionsPageCharacterInfo.reactionList.actionD1OtherBonus[i];
            action.wpnDamage1DamageType.value = CSInfo.actionsPageCharacterInfo.reactionList.actionD1Type[i];

            action.wpnDamage2NumberOfDices.text = CSInfo.actionsPageCharacterInfo.reactionList.actionD2NumDices[i];
            action.wpnDamage2DiceType.value = CSInfo.actionsPageCharacterInfo.reactionList.actionD2DiceType[i];
            action.wpnDamage2Ability.value = CSInfo.actionsPageCharacterInfo.reactionList.actionD2Ability[i];
            action.wpnDamage2OtherBonus.text = CSInfo.actionsPageCharacterInfo.reactionList.actionD2OtherBonus[i];
            action.wpnDamage2DamageType.value = CSInfo.actionsPageCharacterInfo.reactionList.actionD2Type[i];

            action.saveDC = CSInfo.actionsPageCharacterInfo.reactionList.actionDC[i];

            action.SetActionConfig();
        }
        #endregion

        #region personality
        traits.text = CSInfo.personalityPageCharacterInfo.traits;
        ideals.text = CSInfo.personalityPageCharacterInfo.ideals;
        bonds.text = CSInfo.personalityPageCharacterInfo.bonds;
        flaws.text = CSInfo.personalityPageCharacterInfo.flaws;
        #endregion
    }

    void SaveCharacterInfo()
    {
        CSInfo.publicPageCharacterInfo.characterName = characterName.text;
        CSInfo.publicPageCharacterInfo.playerName = playerName.text;
        CSInfo.publicPageCharacterInfo.appearance = appearance.text;

        CSInfo.publicBasicInfo = basicInfoPublisher.isOn;
        CSInfo.publicSkills = skillsPublisher.isOn;
        CSInfo.publicFeatures = featuresPublisher.isOn;
        CSInfo.publicInventory = inventoryPublisher.isOn;
        CSInfo.publicSpells = spellsPublisher.isOn;
        CSInfo.publicActions = actionsPublisher.isOn;
        CSInfo.publicPersonality = personalityPublisher.isOn;

        #region basic info
        CSInfo.basicPageCharacterInfo.clasAndLevel = clasAndLevel.text;
        CSInfo.basicPageCharacterInfo.subclass = subclass.text;
        CSInfo.basicPageCharacterInfo.race = race.text;
        CSInfo.basicPageCharacterInfo.background = background.text;
        CSInfo.basicPageCharacterInfo.alignement = alignement.text;
        CSInfo.basicPageCharacterInfo.experience = experience.text;

        CSInfo.basicPageCharacterInfo.strScore = strScore.text;
        CSInfo.basicPageCharacterInfo.dexScore = dexScore.text;
        CSInfo.basicPageCharacterInfo.conScore = conScore.text;
        CSInfo.basicPageCharacterInfo.intScore = intScore.text;
        CSInfo.basicPageCharacterInfo.wisScore = wisScore.text;
        CSInfo.basicPageCharacterInfo.chaScore = chaScore.text;

        CSInfo.basicPageCharacterInfo.maxHealthPoints = maxHealthPoints.text;
        CSInfo.basicPageCharacterInfo.currHealthPoints = currHealthPoints.text;
        CSInfo.basicPageCharacterInfo.tempHealthPoints = tempHealthPoints.text;
        CSInfo.basicPageCharacterInfo.initiativeBonus = initiativeBonus.text;
        CSInfo.basicPageCharacterInfo.armorClass = armorClass.text;
        CSInfo.basicPageCharacterInfo.speed = speed.text;
        CSInfo.basicPageCharacterInfo.hitDiceType = hitDice.value;
        if (deathSaveFail1.isOn)
            CSInfo.basicPageCharacterInfo.deathSaveFails += 1;
        if (deathSaveFail2.isOn)
            CSInfo.basicPageCharacterInfo.deathSaveFails += 1;
        if (deathSaveFail3.isOn)
            CSInfo.basicPageCharacterInfo.deathSaveFails += 1;
        if (deathSaveSuccess1.isOn)
            CSInfo.basicPageCharacterInfo.deathSaveSuccesses += 1;
        if (deathSaveSuccess2.isOn)
            CSInfo.basicPageCharacterInfo.deathSaveSuccesses += 1;
        if (deathSaveSuccess3.isOn)
            CSInfo.basicPageCharacterInfo.deathSaveSuccesses += 1;
        #endregion

        #region skills info

        CSInfo.skillsPageCharacterInfo.proficencyBonus = proficencyBonus.text;
        CSInfo.skillsPageCharacterInfo.strProf = strProficency.isOn;
        CSInfo.skillsPageCharacterInfo.dexProf = dexProficency.isOn;
        CSInfo.skillsPageCharacterInfo.conProf = conProficency.isOn;
        CSInfo.skillsPageCharacterInfo.intProf = intProficency.isOn;
        CSInfo.skillsPageCharacterInfo.wisProf = wisProficency.isOn;
        CSInfo.skillsPageCharacterInfo.chaProf = chaProficency.isOn;

        CSInfo.skillsPageCharacterInfo.skillProf = new int[skillList.Count];
        CSInfo.skillsPageCharacterInfo.skillCharacteristic = new int[skillList.Count];
        CSInfo.skillsPageCharacterInfo.skillBonus = new string[skillList.Count];
        CSInfo.skillsPageCharacterInfo.skillTotal = new string[skillList.Count];

        for (int i = 0; i < skillList.Count; i++)
        {
            CSInfo.skillsPageCharacterInfo.skillProf[i] = skillList[i].skillProficency.value;
            CSInfo.skillsPageCharacterInfo.skillCharacteristic[i] = skillList[i].skillCharacteristic.value;
            CSInfo.skillsPageCharacterInfo.skillBonus[i] = skillList[i].skillExtraBonus.text;
            CSInfo.skillsPageCharacterInfo.skillTotal[i] = skillList[i].skillTotalBonus.text;
        }

        #endregion

        #region features and traits info
        CSInfo.featuresPageCharacterInfo.featuresAndTraits = featuresAndTraits.text;

        CSInfo.featuresPageCharacterInfo.traitCount = traitList.Count;
        CSInfo.featuresPageCharacterInfo.traitName = new string[CSInfo.featuresPageCharacterInfo.traitCount];
        CSInfo.featuresPageCharacterInfo.traitDescription = new string[CSInfo.featuresPageCharacterInfo.traitCount];
        for (int i = 0; i < CSInfo.featuresPageCharacterInfo.traitCount; i++)
        {
            CharacterTrait trait = traitList[i].GetComponent<CharacterTrait>();

            CSInfo.featuresPageCharacterInfo.traitName[i] = trait.traitName.text;
            CSInfo.featuresPageCharacterInfo.traitDescription[i] = trait.traitDescriptionInput.text;
        }
        CSInfo.featuresPageCharacterInfo.proficencies = proficencies.text;
        #endregion

        #region inventory
        CSInfo.inventoryPageCharacterInfo.itemCount = itemList.Count;
        CSInfo.inventoryPageCharacterInfo.itemNames = new string[CSInfo.inventoryPageCharacterInfo.itemCount];
        CSInfo.inventoryPageCharacterInfo.itemAmounts = new string[CSInfo.inventoryPageCharacterInfo.itemCount];
        CSInfo.inventoryPageCharacterInfo.itemWeights = new string[CSInfo.inventoryPageCharacterInfo.itemCount];

        for (int i = 0; i < itemList.Count; i++)
        {
            CharacterSheetItem item = itemList[i].GetComponent<CharacterSheetItem>();
            CSInfo.inventoryPageCharacterInfo.itemNames[i] = item.itemName.text;
            CSInfo.inventoryPageCharacterInfo.itemAmounts[i] = item.itemAmount.text;
            CSInfo.inventoryPageCharacterInfo.itemWeights[i] = item.itemWeight.text;
        }

        CSInfo.inventoryPageCharacterInfo.copperPieces = copperPieces.text;
        CSInfo.inventoryPageCharacterInfo.silverPieces = silverPieces.text;
        CSInfo.inventoryPageCharacterInfo.electrumPieces = electrumPieces.text;
        CSInfo.inventoryPageCharacterInfo.goldPieces = goldPieces.text;
        CSInfo.inventoryPageCharacterInfo.platinumPieces = platinumPieces.text;
        #endregion

        #region spells
        CSInfo.spellsPageCharacterInfo.spellCastingAbility = spellModifier.value;
        CSInfo.spellsPageCharacterInfo.spellSaveDC = spellSaveDc.text;
        CSInfo.spellsPageCharacterInfo.spellAttackMod = spellAttackMod.text;

        CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellCount = level0SpellsList.Count;
        CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellNames = new string[level0SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[0].preparedSpells = new bool[level0SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellSchool = new int[level0SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellCastingTime = new string[level0SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellRange = new string[level0SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellComponents = new string[level0SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellDuration = new string[level0SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellDescription = new string[level0SpellsList.Count];

        for (int i = 0; i < level0SpellsList.Count; i++)
        {
            SpellInfo spell = level0SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellNames[i] = spell.spellName.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[0].preparedSpells[i] = spell.prepared.isOn;
            CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellSchool[i] = spell.spellSchool.value;
            CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellRange[i] = spell.spellRange.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellComponents[i] = spell.spellComponents.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellDuration[i] = spell.spellDuration.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[0].spellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellCount = level1SpellsList.Count;
        CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellNames = new string[level1SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[1].preparedSpells = new bool[level1SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellSchool = new int[level1SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellCastingTime = new string[level1SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellRange = new string[level1SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellComponents = new string[level1SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellDuration = new string[level1SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellDescription = new string[level1SpellsList.Count];

        for (int i = 0; i < level1SpellsList.Count; i++)
        {
            SpellInfo spell = level1SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellNames[i] = spell.spellName.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[1].preparedSpells[i] = spell.prepared.isOn;
            CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellSchool[i] = spell.spellSchool.value;
            CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellRange[i] = spell.spellRange.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellComponents[i] = spell.spellComponents.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellDuration[i] = spell.spellDuration.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[1].spellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellCount = level2SpellsList.Count;
        CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellNames = new string[level2SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[2].preparedSpells = new bool[level2SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellSchool = new int[level2SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellCastingTime = new string[level2SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellRange = new string[level2SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellComponents = new string[level2SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellDuration = new string[level2SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[2].spellDescription = new string[level2SpellsList.Count];

        for (int i = 0; i < level2SpellsList.Count; i++)
        {
            SpellInfo spell = level3SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellNames[i] = spell.spellName.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[3].preparedSpells[i] = spell.prepared.isOn;
            CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellSchool[i] = spell.spellSchool.value;
            CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellRange[i] = spell.spellRange.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellComponents[i] = spell.spellComponents.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellDuration[i] = spell.spellDuration.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellCount = level3SpellsList.Count;
        CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellNames = new string[level3SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[3].preparedSpells = new bool[level3SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellSchool = new int[level3SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellCastingTime = new string[level3SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellRange = new string[level3SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellComponents = new string[level3SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellDuration = new string[level3SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellDescription = new string[level3SpellsList.Count];

        for (int i = 0; i < level3SpellsList.Count; i++)
        {
            SpellInfo spell = level3SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellNames[i] = spell.spellName.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[3].preparedSpells[i] = spell.prepared.isOn;
            CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellSchool[i] = spell.spellSchool.value;
            CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellRange[i] = spell.spellRange.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellComponents[i] = spell.spellComponents.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellDuration[i] = spell.spellDuration.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[3].spellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellCount = level4SpellsList.Count;
        CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellNames = new string[level4SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[4].preparedSpells = new bool[level4SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellSchool = new int[level4SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellCastingTime = new string[level4SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellRange = new string[level4SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellComponents = new string[level4SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellDuration = new string[level4SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellDescription = new string[level4SpellsList.Count];

        for (int i = 0; i < level4SpellsList.Count; i++)
        {
            SpellInfo spell = level4SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellNames[i] = spell.spellName.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[4].preparedSpells[i] = spell.prepared.isOn;
            CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellSchool[i] = spell.spellSchool.value;
            CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellRange[i] = spell.spellRange.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellComponents[i] = spell.spellComponents.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellDuration[i] = spell.spellDuration.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[4].spellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellCount = level5SpellsList.Count;
        CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellNames = new string[level5SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[5].preparedSpells = new bool[level5SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellSchool = new int[level5SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellCastingTime = new string[level5SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellRange = new string[level5SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellComponents = new string[level5SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellDuration = new string[level5SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellDescription = new string[level5SpellsList.Count];

        for (int i = 0; i < level5SpellsList.Count; i++)
        {
            SpellInfo spell = level5SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellNames[i] = spell.spellName.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[5].preparedSpells[i] = spell.prepared.isOn;
            CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellSchool[i] = spell.spellSchool.value;
            CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellRange[i] = spell.spellRange.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellComponents[i] = spell.spellComponents.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellDuration[i] = spell.spellDuration.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[5].spellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellCount = level6SpellsList.Count;
        CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellNames = new string[level6SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[6].preparedSpells = new bool[level6SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellSchool = new int[level6SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellCastingTime = new string[level6SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellRange = new string[level6SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellComponents = new string[level6SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellDuration = new string[level6SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellDescription = new string[level6SpellsList.Count];

        for (int i = 0; i < level6SpellsList.Count; i++)
        {
            SpellInfo spell = level6SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellNames[i] = spell.spellName.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[6].preparedSpells[i] = spell.prepared.isOn;
            CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellSchool[i] = spell.spellSchool.value;
            CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellRange[i] = spell.spellRange.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellComponents[i] = spell.spellComponents.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellDuration[i] = spell.spellDuration.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[6].spellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellCount = level7SpellsList.Count;
        CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellNames = new string[level7SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[7].preparedSpells = new bool[level7SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellSchool = new int[level7SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellCastingTime = new string[level7SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellRange = new string[level7SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellComponents = new string[level7SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellDuration = new string[level7SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellDescription = new string[level7SpellsList.Count];

        for (int i = 0; i < level7SpellsList.Count; i++)
        {
            SpellInfo spell = level7SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellNames[i] = spell.spellName.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[7].preparedSpells[i] = spell.prepared.isOn;
            CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellSchool[i] = spell.spellSchool.value;
            CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellRange[i] = spell.spellRange.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellComponents[i] = spell.spellComponents.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellDuration[i] = spell.spellDuration.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[7].spellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellCount = level8SpellsList.Count;
        CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellNames = new string[level8SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[8].preparedSpells = new bool[level8SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellSchool = new int[level8SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellCastingTime = new string[level8SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellRange = new string[level8SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellComponents = new string[level8SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellDuration = new string[level8SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellDescription = new string[level8SpellsList.Count];

        for (int i = 0; i < level8SpellsList.Count; i++)
        {
            SpellInfo spell = level8SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellNames[i] = spell.spellName.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[8].preparedSpells[i] = spell.prepared.isOn;
            CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellSchool[i] = spell.spellSchool.value;
            CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellRange[i] = spell.spellRange.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellComponents[i] = spell.spellComponents.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellDuration[i] = spell.spellDuration.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[8].spellDescription[i] = spell.spellDescription.text;
        }

        CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellCount = level9SpellsList.Count;
        CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellNames = new string[level9SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[9].preparedSpells = new bool[level9SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellSchool = new int[level9SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellCastingTime = new string[level9SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellRange = new string[level9SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellComponents = new string[level9SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellDuration = new string[level9SpellsList.Count];
        CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellDescription = new string[level9SpellsList.Count];

        for (int i = 0; i < level9SpellsList.Count; i++)
        {
            SpellInfo spell = level9SpellsList[i].GetComponent<SpellInfo>();

            CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellNames[i] = spell.spellName.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[9].preparedSpells[i] = spell.prepared.isOn;
            CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellSchool[i] = spell.spellSchool.value;
            CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellCastingTime[i] = spell.spellCastingTime.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellRange[i] = spell.spellRange.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellComponents[i] = spell.spellComponents.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellDuration[i] = spell.spellDuration.text;
            CSInfo.spellsPageCharacterInfo.spellLevelList[9].spellDescription[i] = spell.spellDescription.text;
        }
        #endregion

        #region actions
        CSInfo.actionsPageCharacterInfo.actionList.actionCount = actionList.Count;
        CSInfo.actionsPageCharacterInfo.actionList.actionName = new string[actionList.Count];
        CSInfo.actionsPageCharacterInfo.actionList.actionType = new int[actionList.Count];
        CSInfo.actionsPageCharacterInfo.actionList.actionWeapon = new int[actionList.Count];

        CSInfo.actionsPageCharacterInfo.actionList.actionAttackAbility = new int[actionList.Count];
        CSInfo.actionsPageCharacterInfo.actionList.actionAttackOtherBonus = new string[actionList.Count];
        CSInfo.actionsPageCharacterInfo.actionList.actionAttackProficency = new bool[actionList.Count];

        CSInfo.actionsPageCharacterInfo.actionList.actionD1NumDices = new string[actionList.Count];
        CSInfo.actionsPageCharacterInfo.actionList.actionD1DiceType = new int[actionList.Count];
        CSInfo.actionsPageCharacterInfo.actionList.actionD1Ability = new int[actionList.Count];
        CSInfo.actionsPageCharacterInfo.actionList.actionD1OtherBonus = new string[actionList.Count];
        CSInfo.actionsPageCharacterInfo.actionList.actionD1Type = new int[actionList.Count];

        CSInfo.actionsPageCharacterInfo.actionList.actionD2NumDices = new string[actionList.Count];
        CSInfo.actionsPageCharacterInfo.actionList.actionD2DiceType = new int[actionList.Count];
        CSInfo.actionsPageCharacterInfo.actionList.actionD2Ability = new int[actionList.Count];
        CSInfo.actionsPageCharacterInfo.actionList.actionD2OtherBonus = new string[actionList.Count];
        CSInfo.actionsPageCharacterInfo.actionList.actionD2Type = new int[actionList.Count];

        CSInfo.actionsPageCharacterInfo.actionList.actionDC = new int[actionList.Count];

        for (int i = 0; i < actionList.Count; i++)
        {
            PCActionInfo action = actionList[i].GetComponent<PCActionInfo>();

            CSInfo.actionsPageCharacterInfo.actionList.actionName[i] = action.actionName.text;
            CSInfo.actionsPageCharacterInfo.actionList.actionType[i] = action.actionType.value;
            CSInfo.actionsPageCharacterInfo.actionList.actionWeapon[i] = action.weaponTemplate.value;

            CSInfo.actionsPageCharacterInfo.actionList.actionAttackAbility[i] = action.wpnAttackAbility.value;
            CSInfo.actionsPageCharacterInfo.actionList.actionAttackOtherBonus[i] = action.wpnOtherAttackBonus.text;
            CSInfo.actionsPageCharacterInfo.actionList.actionAttackProficency[i] = action.wpnAttackProficency.isOn;

            CSInfo.actionsPageCharacterInfo.actionList.actionD1NumDices[i] = action.wpnDamage1NumberOfDices.text;
            CSInfo.actionsPageCharacterInfo.actionList.actionD1DiceType[i] = action.wpnDamage1DiceType.value;
            CSInfo.actionsPageCharacterInfo.actionList.actionD1Ability[i] = action.wpnDamage1Ability.value;
            CSInfo.actionsPageCharacterInfo.actionList.actionD1OtherBonus[i] = action.wpnDamage1OtherBonus.text;
            CSInfo.actionsPageCharacterInfo.actionList.actionD1Type[i] = action.wpnDamage1DamageType.value;

            CSInfo.actionsPageCharacterInfo.actionList.actionD2NumDices[i] = action.wpnDamage2NumberOfDices.text;
            CSInfo.actionsPageCharacterInfo.actionList.actionD2DiceType[i] = action.wpnDamage2DiceType.value;
            CSInfo.actionsPageCharacterInfo.actionList.actionD2Ability[i] = action.wpnDamage2Ability.value;
            CSInfo.actionsPageCharacterInfo.actionList.actionD2OtherBonus[i] = action.wpnDamage2OtherBonus.text;
            CSInfo.actionsPageCharacterInfo.actionList.actionD2Type[i] = action.wpnDamage2DamageType.value;

            CSInfo.actionsPageCharacterInfo.actionList.actionDC[i] = action.saveDC;
        }

        CSInfo.actionsPageCharacterInfo.bonusActionList.actionCount = bonusActionList.Count;
        CSInfo.actionsPageCharacterInfo.bonusActionList.actionName = new string[bonusActionList.Count];
        CSInfo.actionsPageCharacterInfo.bonusActionList.actionType = new int[bonusActionList.Count];
        CSInfo.actionsPageCharacterInfo.bonusActionList.actionWeapon = new int[bonusActionList.Count];

        CSInfo.actionsPageCharacterInfo.bonusActionList.actionAttackAbility = new int[bonusActionList.Count];
        CSInfo.actionsPageCharacterInfo.bonusActionList.actionAttackOtherBonus = new string[bonusActionList.Count];
        CSInfo.actionsPageCharacterInfo.bonusActionList.actionAttackProficency = new bool[bonusActionList.Count];

        CSInfo.actionsPageCharacterInfo.bonusActionList.actionD1NumDices = new string[bonusActionList.Count];
        CSInfo.actionsPageCharacterInfo.bonusActionList.actionD1DiceType = new int[bonusActionList.Count];
        CSInfo.actionsPageCharacterInfo.bonusActionList.actionD1Ability = new int[bonusActionList.Count];
        CSInfo.actionsPageCharacterInfo.bonusActionList.actionD1OtherBonus = new string[bonusActionList.Count];
        CSInfo.actionsPageCharacterInfo.bonusActionList.actionD1Type = new int[bonusActionList.Count];

        CSInfo.actionsPageCharacterInfo.bonusActionList.actionD2NumDices = new string[bonusActionList.Count];
        CSInfo.actionsPageCharacterInfo.bonusActionList.actionD2DiceType = new int[bonusActionList.Count];
        CSInfo.actionsPageCharacterInfo.bonusActionList.actionD2Ability = new int[bonusActionList.Count];
        CSInfo.actionsPageCharacterInfo.bonusActionList.actionD2OtherBonus = new string[bonusActionList.Count];
        CSInfo.actionsPageCharacterInfo.bonusActionList.actionD2Type = new int[bonusActionList.Count];

        CSInfo.actionsPageCharacterInfo.bonusActionList.actionDC = new int[bonusActionList.Count];

        for (int i = 0; i < bonusActionList.Count; i++)
        {
            PCActionInfo action = bonusActionList[i].GetComponent<PCActionInfo>();

            CSInfo.actionsPageCharacterInfo.bonusActionList.actionName[i] = action.actionName.text;
            CSInfo.actionsPageCharacterInfo.bonusActionList.actionType[i] = action.actionType.value;
            CSInfo.actionsPageCharacterInfo.bonusActionList.actionWeapon[i] = action.weaponTemplate.value;

            CSInfo.actionsPageCharacterInfo.bonusActionList.actionAttackAbility[i] = action.wpnAttackAbility.value;
            CSInfo.actionsPageCharacterInfo.bonusActionList.actionAttackOtherBonus[i] = action.wpnOtherAttackBonus.text;
            CSInfo.actionsPageCharacterInfo.bonusActionList.actionAttackProficency[i] = action.wpnAttackProficency.isOn;

            CSInfo.actionsPageCharacterInfo.bonusActionList.actionD1NumDices[i] = action.wpnDamage1NumberOfDices.text;
            CSInfo.actionsPageCharacterInfo.bonusActionList.actionD1DiceType[i] = action.wpnDamage1DiceType.value;
            CSInfo.actionsPageCharacterInfo.bonusActionList.actionD1Ability[i] = action.wpnDamage1Ability.value;
            CSInfo.actionsPageCharacterInfo.bonusActionList.actionD1OtherBonus[i] = action.wpnDamage1OtherBonus.text;
            CSInfo.actionsPageCharacterInfo.bonusActionList.actionD1Type[i] = action.wpnDamage1DamageType.value;

            CSInfo.actionsPageCharacterInfo.bonusActionList.actionD2NumDices[i] = action.wpnDamage2NumberOfDices.text;
            CSInfo.actionsPageCharacterInfo.bonusActionList.actionD2DiceType[i] = action.wpnDamage2DiceType.value;
            CSInfo.actionsPageCharacterInfo.bonusActionList.actionD2Ability[i] = action.wpnDamage2Ability.value;
            CSInfo.actionsPageCharacterInfo.bonusActionList.actionD2OtherBonus[i] = action.wpnDamage2OtherBonus.text;
            CSInfo.actionsPageCharacterInfo.bonusActionList.actionD2Type[i] = action.wpnDamage2DamageType.value;

            CSInfo.actionsPageCharacterInfo.bonusActionList.actionDC[i] = action.saveDC;
        }

        CSInfo.actionsPageCharacterInfo.reactionList.actionCount = reactionList.Count;
        CSInfo.actionsPageCharacterInfo.reactionList.actionName = new string[reactionList.Count];
        CSInfo.actionsPageCharacterInfo.reactionList.actionType = new int[reactionList.Count];
        CSInfo.actionsPageCharacterInfo.reactionList.actionWeapon = new int[reactionList.Count];

        CSInfo.actionsPageCharacterInfo.reactionList.actionAttackAbility = new int[reactionList.Count];
        CSInfo.actionsPageCharacterInfo.reactionList.actionAttackOtherBonus = new string[reactionList.Count];
        CSInfo.actionsPageCharacterInfo.reactionList.actionAttackProficency = new bool[reactionList.Count];

        CSInfo.actionsPageCharacterInfo.reactionList.actionD1NumDices = new string[reactionList.Count];
        CSInfo.actionsPageCharacterInfo.reactionList.actionD1DiceType = new int[reactionList.Count];
        CSInfo.actionsPageCharacterInfo.reactionList.actionD1Ability = new int[reactionList.Count];
        CSInfo.actionsPageCharacterInfo.reactionList.actionD1OtherBonus = new string[reactionList.Count];
        CSInfo.actionsPageCharacterInfo.reactionList.actionD1Type = new int[reactionList.Count];

        CSInfo.actionsPageCharacterInfo.reactionList.actionD2NumDices = new string[reactionList.Count];
        CSInfo.actionsPageCharacterInfo.reactionList.actionD2DiceType = new int[reactionList.Count];
        CSInfo.actionsPageCharacterInfo.reactionList.actionD2Ability = new int[reactionList.Count];
        CSInfo.actionsPageCharacterInfo.reactionList.actionD2OtherBonus = new string[reactionList.Count];
        CSInfo.actionsPageCharacterInfo.reactionList.actionD2Type = new int[reactionList.Count];

        CSInfo.actionsPageCharacterInfo.reactionList.actionDC = new int[reactionList.Count];

        for (int i = 0; i < reactionList.Count; i++)
        {
            PCActionInfo action = reactionList[i].GetComponent<PCActionInfo>();

            CSInfo.actionsPageCharacterInfo.reactionList.actionName[i] = action.actionName.text;
            CSInfo.actionsPageCharacterInfo.reactionList.actionType[i] = action.actionType.value;
            CSInfo.actionsPageCharacterInfo.reactionList.actionWeapon[i] = action.weaponTemplate.value;

            CSInfo.actionsPageCharacterInfo.reactionList.actionAttackAbility[i] = action.wpnAttackAbility.value;
            CSInfo.actionsPageCharacterInfo.reactionList.actionAttackOtherBonus[i] = action.wpnOtherAttackBonus.text;
            CSInfo.actionsPageCharacterInfo.reactionList.actionAttackProficency[i] = action.wpnAttackProficency.isOn;

            CSInfo.actionsPageCharacterInfo.reactionList.actionD1NumDices[i] = action.wpnDamage1NumberOfDices.text;
            CSInfo.actionsPageCharacterInfo.reactionList.actionD1DiceType[i] = action.wpnDamage1DiceType.value;
            CSInfo.actionsPageCharacterInfo.reactionList.actionD1Ability[i] = action.wpnDamage1Ability.value;
            CSInfo.actionsPageCharacterInfo.reactionList.actionD1OtherBonus[i] = action.wpnDamage1OtherBonus.text;
            CSInfo.actionsPageCharacterInfo.reactionList.actionD1Type[i] = action.wpnDamage1DamageType.value;

            CSInfo.actionsPageCharacterInfo.reactionList.actionD2NumDices[i] = action.wpnDamage2NumberOfDices.text;
            CSInfo.actionsPageCharacterInfo.reactionList.actionD2DiceType[i] = action.wpnDamage2DiceType.value;
            CSInfo.actionsPageCharacterInfo.reactionList.actionD2Ability[i] = action.wpnDamage2Ability.value;
            CSInfo.actionsPageCharacterInfo.reactionList.actionD2OtherBonus[i] = action.wpnDamage2OtherBonus.text;
            CSInfo.actionsPageCharacterInfo.reactionList.actionD2Type[i] = action.wpnDamage2DamageType.value;

            CSInfo.actionsPageCharacterInfo.reactionList.actionDC[i] = action.saveDC;
        }
        #endregion

        #region personality
        CSInfo.personalityPageCharacterInfo.traits = traits.text;
        CSInfo.personalityPageCharacterInfo.ideals = ideals.text;
        CSInfo.personalityPageCharacterInfo.bonds = bonds.text;
        CSInfo.personalityPageCharacterInfo.flaws = flaws.text;
        #endregion
    }

    // ability score methods

    private void CheckAbilityScore(InputField abilityScore, Text abilityModifier)
    {
        int score = int.Parse(abilityScore.text);
        if (score < 1)
        {
            abilityScore.text = "1";
        }
        else if (score > 30)
        {
            abilityScore.text = "30";
        }
        UpdateAbilityModifier(int.Parse(abilityScore.text), abilityModifier);
    }

    public int GetStrMod()
    {
        return int.Parse(strModifier.text);
    }

    public int GetDexMod()
    {
        return int.Parse(dexModifier.text);
    }

    public int GetConMod()
    {
        return int.Parse(conModifier.text);
    }

    public int GetIntMod()
    {
        return int.Parse(intModifier.text);
    }

    public int GetWisMod()
    {
        return int.Parse(wisModifier.text);
    }

    public int GetChaMod()
    {
        return int.Parse(chaModifier.text);
    }

    // dice roll methods
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
        int bonus = int.Parse(proficencyBonus.text);
        if (bonus < 2)
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
        int totalBonus;
        if (int.TryParse(bonus.text, out bonusMod))
        {
            totalBonus = profMod + charMod + bonusMod;
        }
        else
        {
            bonus.text = "0";
            totalBonus = profMod + charMod;
        }
        total.text = totalBonus.ToString();
    }

    public int GetProficencyBonus()
    {
        return int.Parse(proficencyBonus.text);
    }

    // features methods

    private void AddNewTrait(List<GameObject> listOfTraits, GameObject traitsParent)
    {
        GameObject trait = Instantiate(traitsPrefab);
        trait.GetComponent<RectTransform>().sizeDelta *= 0.83f;
        trait.GetComponent<CharacterTrait>().repositionListener += RepositionTraits;
        trait.GetComponent<CharacterTrait>().removeListener += RemoveTrait;

        traitsParent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 62);
        trait.GetComponent<RectTransform>().SetParent(traitsParent.transform);
        listOfTraits.Add(trait);
    }

    private void RepositionTraits()
    {
        for (int i = 0; i < traitList.Count; i++)
        {
            if (i == 0)
            {
                traitList[0].GetComponent<RectTransform>().localPosition = new Vector3(0, 2, 0);
                continue;
            }

            Vector3 newPosition = traitList[i - 1].GetComponent<RectTransform>().localPosition + new Vector3(0, traitList[i - 1].GetComponent<RectTransform>().sizeDelta.y + 2, 0);
            traitList[i].GetComponent<CharacterTrait>().Reposition(newPosition);
        }
    }

    private void RemoveTrait(GameObject trait)
    {
        foreach (GameObject traitOfList in traitList)
        {
            if (traitOfList == trait)
            {
                featuresParent.GetComponent<RectTransform>().sizeDelta -= new Vector2(0, trait.GetComponent<RectTransform>().sizeDelta.y);

                traitList.Remove(trait);
                Destroy(trait);

                break;
            }
        }

        RepositionTraits();
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

    // action methods

    private void AddNewAction(List<GameObject> listOfActions, GameObject actionsParent)
    {
        GameObject action = Instantiate(actionsPrefab);
        GameObject actionItem = action.GetComponent<PCActionInfo>().actionItem;
        action.GetComponent<PCActionInfo>().sheetManager = this;
        actionItem.GetComponent<RectTransform>().SetParent(actionsParent.transform);
        listOfActions.Add(action);
    }

    // token related methods

    void SpawnToken(ulong ownerID, string ownerName, int avatarID, CharacterSheetInfo characterSheetInfo)
    {
        tokenManager.DragPlayerToken(ownerName, avatarID, characterSheetInfo);

        //gameManager.SpawnPlayerToken(ownerID, ownerName, avatarID, Vector3.zero, characterSheetInfo);
    }

    public void UpdateTokenHealthPoints(string tempHP, string currHP)
    {
        TokenController[] tokens = FindObjectsOfType<TokenController>();

        foreach (TokenController token in tokens)
        {
            if (token.characterSheetInfo.sheetID == CSInfo.sheetID)
            {
                token.characterSheetInfo.basicPageCharacterInfo.tempHealthPoints = tempHP;
                token.characterSheetInfo.basicPageCharacterInfo.currHealthPoints = currHP;
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
        int profBonus = int.Parse(CSInfo.skillsPageCharacterInfo.proficencyBonus);
        int skillBonus;

        switch (profType.value)
        {
            case 1:
                skillBonus = profBonus;
                break;
            case 2:
                skillBonus = profBonus / 2;
                break;
            case 3:
                skillBonus = profBonus * 2;
                break;
            default:
                skillBonus = 0;
                break;
        }
        return skillBonus;
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
        SaveCharacterInfo();
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

    void RollCheck(string abilityOrSkillName, string bonus)
    {
        diceHandler.RollCheck(characterName.text, abilityOrSkillName, int.Parse(bonus));
    }

    void RollInitiative()
    {
        if (initiativeBonus.text == "")
        {
            initiativeBonus.text = "0";
        }

        diceHandler.RollInitiative(characterName.text, int.Parse(initiativeBonus.text));
    }

    void RollSavingThrow(string abilityName, string abilityModifier)
    {
        diceHandler.RollAbilitySave(characterName.text, abilityName, int.Parse(abilityModifier));
    }

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