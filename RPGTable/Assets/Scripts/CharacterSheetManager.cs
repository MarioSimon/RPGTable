using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Unity.Collections;
using System;

public class CharacterSheetManager : NetworkBehaviour
{
    #region Variables
    public CharacterSheetInfo CSInfo;
    GameManager gameManager;

    [SerializeField] GameObject tokenPrefab;

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
    #endregion

    #region Basic Info
    [Header("Basic Info")]
    // Info
    [SerializeField] GameObject basicInfo;
    [SerializeField] InputField clasAndLevel;
    [SerializeField] InputField subclas;
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
    [SerializeField] InputField maxHealthPoints;
    [SerializeField] InputField currHealthPoints;
    [SerializeField] InputField tempHealthPoints;
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
    #endregion

    #region Skills

    [Serializable]
    struct Skill
    {
        public string skillName;
        public Button skillCheck;
        public Dropdown skillProficency;
        public Dropdown skillCharacteristic;
        public InputField skillExtraBonus;
        public Text skillTotalBonus;
    }

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

    #endregion

    #region Features

    [Header("Features")]
    [SerializeField] GameObject features;

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

    #endregion

    [Header("Spells")]
    [SerializeField] GameObject spells;

    [Header("Actions")]
    [SerializeField] GameObject actions;

    [Header("Personality")]
    [SerializeField] GameObject personailty;

    private GameObject currentPage;

    #endregion


    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        currentPage = publicInfo;

        // navigation related events
        buttonClose.onClick.AddListener(() => CloseSheet());
        buttonSpawnToken.onClick.AddListener(() => SpawnTokenServerRpc(NetworkManager.Singleton.LocalClientId, playerName.text));
        buttonPublicInfo.onClick.AddListener(() => OpenPublicInfoPage());
        buttonBasicInfo.onClick.AddListener(() => OpenBasicInfoPage());
        buttonSkills.onClick.AddListener(() => OpenSkillsPage());
        buttonFeatures.onClick.AddListener(() => OpenFeaturesPage());
        buttonInventory.onClick.AddListener(() => OpenInventoryPage());
        buttonSpells.onClick.AddListener(() => OpenSpellsPage());
        buttonActions.onClick.AddListener(() => OpenActionsPage());
        buttonPersonality.onClick.AddListener(() => OpenPersonalityPage());        

        //quiza sea mejor guardar textos al cambiar de pestaña o cerrar la ficha
        //charactername.onValueChanged.AddListener(delegate { SetCharName(); });

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

        strSave.onClick.AddListener(() => RollStrengthSaveServerRpc());
        dexSave.onClick.AddListener(() => RollDexteritySaveServerRpc());
        conSave.onClick.AddListener(() => RollConstitutionSaveServerRpc());
        intSave.onClick.AddListener(() => RollIntelligenceSaveServerRpc());
        wisSave.onClick.AddListener(() => RollWisdomSaveServerRpc());
        chaSave.onClick.AddListener(() => RollCharismaSaveServerRpc());

        // character stats related events
        maxHealthPoints.onValueChanged.AddListener(delegate { CheckInt(maxHealthPoints); });
        currHealthPoints.onValueChanged.AddListener(delegate { CheckInt(currHealthPoints); });
        tempHealthPoints.onValueChanged.AddListener(delegate { CheckInt(tempHealthPoints); });

        armorClass.onValueChanged.AddListener(delegate { CheckInt(armorClass); });
        initiativeBonus.onValueChanged.AddListener(delegate { CheckInt(initiativeBonus); });

        rollHitDice.onClick.AddListener(() => RollHitDiceServerRpc(hitDice.value));
        rollInitiative.onClick.AddListener(() => RollInitiativeServerRpc());
        rollDeathSave.onClick.AddListener(() => RollDeathSavingThrowServerRpc());
        resetDeathSaves.onClick.AddListener(() => ResetDeathSavingThrows());
       
        // skill related logic events
        proficencyBonus.onValueChanged.AddListener(delegate { CheckProficencyBonus(); });
        foreach (Skill skill in skillList)
        {
            proficencyBonus.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });
            skill.skillProficency.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });
            skill.skillCharacteristic.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });
            skill.skillExtraBonus.onValueChanged.AddListener(delegate { CalculateSkillTotalBonus(skill.skillProficency, skill.skillCharacteristic, skill.skillExtraBonus, skill.skillTotalBonus); });

            // skill related roll events
            skill.skillCheck.onClick.AddListener(() => RollSkillCheckServerRpc(skill.skillName, skill.skillTotalBonus.text));
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


        if (CSInfo != null)
        {
            GetCharacterInfo();
        }
        else
        {
            CSInfo = new CharacterSheetInfo();
        }

        proficencyBonus.text = "2";
    }

    #region Navigation Methods

    void OpenPublicInfoPage()
    {
        currentPage.SetActive(false);
        publicInfo.SetActive(true);
        currentPage = publicInfo;
    }

    void OpenBasicInfoPage()
    {
        currentPage.SetActive(false);
        basicInfo.SetActive(true);
        currentPage = basicInfo;
    }

    void OpenSkillsPage()
    {
        currentPage.SetActive(false);
        skills.SetActive(true);
        currentPage = skills;
    }

    void OpenFeaturesPage()
    {
        currentPage.SetActive(false);
        features.SetActive(true);
        currentPage = features;
    }

    void OpenInventoryPage()
    {
        currentPage.SetActive(false);
        inventory.SetActive(true);
        currentPage = inventory;
    }

    void OpenSpellsPage()
    {
        currentPage.SetActive(false);
        spells.SetActive(true);
        currentPage = spells;
    }

    void OpenActionsPage()
    {
        currentPage.SetActive(false);
        actions.SetActive(true);
        currentPage = actions;
    }
    void OpenPersonalityPage()
    {
        currentPage.SetActive(false);
        personailty.SetActive(true);
        currentPage = personailty;
    }
    void CloseSheet()
    {
        gameManager.SaveCharacterSheet(CSInfo);
        Destroy(characterSheet);
    }
    #endregion

    #region Logic Methods

    void GetCharacterInfo()
    {
        characterName.text = CSInfo.characterName;
        playerName.text = CSInfo.playerName;
        appearance.text = CSInfo.appearance;
    }

    // abilit score methods

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

    // falta un if para texto con modificador negativo y estos deben ser serverRpc
    void RollStrenghtCheck()
    {
        gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, int.Parse(strModifier.text), " [Strengh check (+" + strModifier.text +")]: ");
    }

    void RollDexterityCheck()
    {
        gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, int.Parse(dexModifier.text), " [Dexterity check (+" + dexModifier.text + ")]: ");
    }

    void RollConstitutionCheck()
    {
        gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, int.Parse(conModifier.text), " [Constitution check (+" + conModifier.text +")]: ");
    }

    void RollIntelligenceCheck()
    {
        gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, int.Parse(intModifier.text), " [Intelligence check (+" + intModifier.text + ")]: ");
    }

    void RollWisdomCheck()
    {
        gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, int.Parse(wisModifier.text), " [Wisdom check (+" + wisModifier.text + ")]: ");
    }

    void RollCharismaCheck()
    {
        gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, int.Parse(chaModifier.text), " [Charisma check (+" + chaModifier.text + ")]: ");
    }

    // Auxiliary methods

    void CheckInt(InputField inputField)
    {
        int value;
        if (!int.TryParse(inputField.text, out value))
        {
            inputField.text = "";
        }
    }

    diceType GetHitDice(int diceSelector)
    {
        switch (diceSelector)
        {
            case 0:
                return diceType.d6;
            case 1:
                return diceType.d8;
            case 2:
                return diceType.d10;
            case 3:
                return diceType.d12;
        }
        return diceType.pd; // error check
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

    #endregion

    #region ServerRpc

    // ability saving throws
    [ServerRpc]
    private void SpawnTokenServerRpc(ulong ownerID, string ownerName)
    {
        GameObject token = Instantiate(tokenPrefab, Vector3.zero, Quaternion.identity);
        token.GetComponent<TokenController>().ownerName.Value = new FixedString64Bytes(ownerName);
        token.GetComponent<NetworkObject>().SpawnWithOwnership(ownerID);
    }

    [ServerRpc]
    void RollStrengthSaveServerRpc()
    {
        if (strProficency.isOn)
        {
            int bonus = int.Parse(strModifier.text) + int.Parse(proficencyBonus.text);
            gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, bonus, " [Strengh saving throw (+" + bonus + ")]: ");
        }
        else
        {
            gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, int.Parse(strModifier.text), " [Strengh saving throw (+" + strModifier.text + ")]: ");
        }       
    }

    [ServerRpc]
    void RollDexteritySaveServerRpc()
    {
        if (dexProficency.isOn)
        {
            int bonus = int.Parse(dexModifier.text) + int.Parse(proficencyBonus.text);
            gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, bonus, " [Dexterity saving throw (+" + bonus + ")]: ");
        }
        else
        {
            gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, int.Parse(dexModifier.text), " [Dexterity saving throw (+" + dexModifier.text + ")]: ");
        }
    }
    
    [ServerRpc]
    void RollConstitutionSaveServerRpc()
    {
        if (conProficency.isOn)
        {
            int bonus = int.Parse(conModifier.text) + int.Parse(proficencyBonus.text);
            gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, bonus, " [Constitution saving throw (+" + bonus + ")]: ");
        }
        else
        {
            gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, int.Parse(conModifier.text), " [Constitution saving throw (+" + conModifier.text + ")]: ");
        }
    }

    [ServerRpc]
    void RollIntelligenceSaveServerRpc()
    {
        if (intProficency.isOn)
        {
            int bonus = int.Parse(intModifier.text) + int.Parse(proficencyBonus.text);
            gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, bonus, " [Intelligence saving throw (+" + bonus + ")]: ");
        }
        else
        {
            gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, int.Parse(conModifier.text), " [Intelligence saving throw (+" + conModifier.text + ")]: ");
        }
    }

    [ServerRpc]
    void RollWisdomSaveServerRpc()
    {
        if (wisProficency.isOn)
        {
            int bonus = int.Parse(wisModifier.text) + int.Parse(proficencyBonus.text);
            gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, bonus, " [Wisdom saving throw (+" + bonus + ")]: ");
        }
        else
        {
            gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, int.Parse(wisModifier.text), " [Wisdom saving throw (+" + wisModifier.text + ")]: ");
        }
    }

    [ServerRpc]
    void RollCharismaSaveServerRpc()
    {
        if (chaProficency.isOn)
        {
            int bonus = int.Parse(chaModifier.text) + int.Parse(proficencyBonus.text);
            gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, bonus, " [Charisma saving throw (+" + bonus + ")]: ");
        }
        else
        {
            gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, int.Parse(chaModifier.text), " [Charisma saving throw (+" + chaModifier.text + ")]: ");
        }
    }

    // other basic rolls
    [ServerRpc]
    void RollInitiativeServerRpc()
    {
        if (initiativeBonus.text == "")
        {
            initiativeBonus.text = "0";
        }

        gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, int.Parse(initiativeBonus.text), " [Initiative (+" + initiativeBonus.text + ")]: ");
    }

    [ServerRpc]
    void RollDeathSavingThrowServerRpc()
    {
        gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, 0, " [Death saving throw]: ");
    }

    [ServerRpc]
    void RollHitDiceServerRpc(int hitDiceType)
    {
        diceType hitDice = GetHitDice(hitDiceType);
        if (hitDice == diceType.pd) { return; }
        gameManager.RollDice(hitDice, Camera.main.transform.position, characterName.text, int.Parse(conModifier.text), " [Hit dice (+" + conModifier.text + "]: ");
    }

    // skill rolls
    [ServerRpc]
    void RollSkillCheckServerRpc(string skillName, string skillTotalBonus)
    {
        gameManager.RollDice(diceType.d20, Camera.main.transform.position, characterName.text, int.Parse(skillTotalBonus), " ["+ skillName + " check (+" + skillTotalBonus + ")]: ");       
    }

    #endregion
}
