using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreator : MonoBehaviour
{

    #region variables
    [SerializeField] GameObject characterCreatorWindow;
    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager uIManager;
    [SerializeField] LibraryManager libraryManager;

    [Header("Basic info")]
    [SerializeField] InputField characterName;
    [SerializeField] Image avatarPortrait;
    [SerializeField] Button selectNewAvatar;
    [SerializeField] GameObject avatarSelector;
    [SerializeField] Button closeAvatarSelector;
    [SerializeField] List<Button> avatarButtons;
    private int avatarID = 0;

    [Header("Race")]
    [SerializeField] GameObject raceSelector;   
    [SerializeField] Button raceMenuNext;
    [SerializeField] Dropdown race;
    [SerializeField] Dropdown subrace;

    [Header("Class")]
    [SerializeField] GameObject classSelector;
    [SerializeField] Button classMenuBack;
    [SerializeField] Button classMenuNext;
    [SerializeField] Dropdown classes;
    [SerializeField] Dropdown subclasses;

    [Header("Background")]
    [SerializeField] GameObject backgroundSelector;
    [SerializeField] Button backgroundMenuBack;
    [SerializeField] Button backgroundMenuNext;
    [SerializeField] Dropdown background;

    [Header("Scores")]
    [SerializeField] GameObject scoresSelector;
    [SerializeField] Button scoresMenuBack;
    [SerializeField] Button scoresMenuNext;
    [SerializeField] Dropdown scoreType;
    [SerializeField] GameObject pointBuy;
    [SerializeField] Text usedPoints;
    [SerializeField] GameObject rollScores;
    [SerializeField] InputField strScore;
    [SerializeField] Text strRacialBonus;
    [SerializeField] Text strTotalScore;
    [SerializeField] Text strModifier;
    [SerializeField] Button rollStrenght;
    [SerializeField] Button addStrenght;
    [SerializeField] Button takeStrenght;
    [SerializeField] InputField dexScore;
    [SerializeField] Text dexRacialBonus;
    [SerializeField] Text dexTotalScore;
    [SerializeField] Text dexModifier;
    [SerializeField] Button rollDexterity;
    [SerializeField] Button addDexterity;
    [SerializeField] Button takeDexterity;
    [SerializeField] InputField conScore;
    [SerializeField] Text conRacialBonus;
    [SerializeField] Text conTotalScore;
    [SerializeField] Text conModifier;
    [SerializeField] Button rollConstitution;
    [SerializeField] Button addConstitution;
    [SerializeField] Button takeConstitution;
    [SerializeField] InputField intScore;
    [SerializeField] Text intRacialBonus;
    [SerializeField] Text intTotalScore;
    [SerializeField] Text intModifier;
    [SerializeField] Button rollIntelligence;
    [SerializeField] Button addIntelligence;
    [SerializeField] Button takeIntelligence;
    [SerializeField] InputField wisScore;
    [SerializeField] Text wisRacialBonus;
    [SerializeField] Text wisTotalScore;
    [SerializeField] Text wisModifier;
    [SerializeField] Button rollWisdom;
    [SerializeField] Button addWisdom;
    [SerializeField] Button takeWisdom;
    [SerializeField] InputField chaScore;
    [SerializeField] Text chaRacialBonus;
    [SerializeField] Text chaTotalScore;
    [SerializeField] Text chaModifier;
    [SerializeField] Button rollCharisma;
    [SerializeField] Button addCharisma;
    [SerializeField] Button takeCharisma;

    [Header("Resume")]
    [SerializeField] GameObject resume;
    [SerializeField] Button resumeMenuBack;
    [SerializeField] Button resumeMenuFinish;
    [SerializeField] Text resumeRace;
    [SerializeField] Text resumeClass;
    [SerializeField] Text resumeBackground;
    [SerializeField] Text resumeStrScore;
    [SerializeField] Text resumeStrModifier;
    [SerializeField] Text resumeDexScore;
    [SerializeField] Text resumeDexModifier;
    [SerializeField] Text resumeConScore;
    [SerializeField] Text resumeConModifier;
    [SerializeField] Text resumeIntScore;
    [SerializeField] Text resumeIntModifier;
    [SerializeField] Text resumeWisScore;
    [SerializeField] Text resumeWisModifier;
    [SerializeField] Text resumeChaScore;
    [SerializeField] Text resumeChaModifier;

    [SerializeField] GameObject descriptionView;
    [SerializeField] Text description;  

    private CharacterSheetInfo newCharacterSheet;

    private int[] raceBonus;
    private int[] subraceBonus;

    private int pointBuyCount;
    private const int PointBuyMax = 27;
    private int pointBuyStrenght;
    private int pointBuyDexterity;
    private int pointBuyConstitution;
    private int pointBuyIntelligence;
    private int pointBuyWisdom;
    private int pointBuyCharisma;
    private const int PointBuyMaxScore = 15;
    private const int PointBuyMinScore = 8;

    private int passingStrenght;
    private int passingDexterity;
    private int passingConstitution;
    private int passingIntelligence;
    private int passingWisdom;
    private int passingCharisma;

    #endregion

    #region unity event functions

    private void Start()
    {
        LoadCharacterCreationOptions();

        selectNewAvatar.onClick.AddListener(() => ToggleAvatarSelector());
        closeAvatarSelector.onClick.AddListener(() => ToggleAvatarSelector());
        for (int i = 0; i < avatarButtons.Count; i++)
        {
            int avatarID = i;
            avatarButtons[avatarID].onClick.AddListener(delegate { SetNewAvatar(avatarID); ToggleAvatarSelector(); });
        }

        race.onValueChanged.AddListener(delegate { LoadSubraceOptions(); ShowRaceInfo(); });
        raceMenuNext.onClick.AddListener(delegate { WriteRaceTraits(); OpenClassSelector(); });

        classes.onValueChanged.AddListener(delegate { LoadSubclassOptions(); ShowClassInfo(); });
        classMenuNext.onClick.AddListener(delegate { WriteClassTraits(); OpenBackgroundSelector(); });
        classMenuBack.onClick.AddListener(delegate { OpenRaceSelector(); });

        background.onValueChanged.AddListener(delegate { ShowBackgroundInfo(); });
        backgroundMenuNext.onClick.AddListener(delegate { WriteBackgroundTraits(); OpenScoreSelector(); });
        backgroundMenuBack.onClick.AddListener(delegate { OpenClassSelector(); });

        scoresMenuBack.onClick.AddListener(delegate { OpenBackgroundSelector(); });
        scoresMenuNext.onClick.AddListener(delegate { WriteScores(); OpenResume(); });
        scoreType.onValueChanged.AddListener(delegate { SwitchScoringType(); });
        strScore.onValueChanged.AddListener(delegate { CheckInt(strScore); UpdateTotalAbilityScoreAndModifier(strScore, strRacialBonus, strTotalScore, strModifier, ref passingStrenght); });
        dexScore.onValueChanged.AddListener(delegate { CheckInt(dexScore); UpdateTotalAbilityScoreAndModifier(dexScore, dexRacialBonus, dexTotalScore, dexModifier, ref passingDexterity); });
        conScore.onValueChanged.AddListener(delegate { CheckInt(conScore); UpdateTotalAbilityScoreAndModifier(conScore, conRacialBonus, conTotalScore, conModifier, ref passingConstitution); });
        intScore.onValueChanged.AddListener(delegate { CheckInt(intScore); UpdateTotalAbilityScoreAndModifier(intScore, intRacialBonus, intTotalScore, intModifier, ref passingIntelligence); });
        wisScore.onValueChanged.AddListener(delegate { CheckInt(wisScore); UpdateTotalAbilityScoreAndModifier(wisScore, wisRacialBonus, wisTotalScore, wisModifier, ref passingWisdom); });
        chaScore.onValueChanged.AddListener(delegate { CheckInt(chaScore); UpdateTotalAbilityScoreAndModifier(chaScore, chaRacialBonus, chaTotalScore, chaModifier, ref passingCharisma); });
        addStrenght.onClick.AddListener(() => Add1(ref pointBuyStrenght, strScore));
        takeStrenght.onClick.AddListener(() => Take1(ref pointBuyStrenght, strScore));
        addDexterity.onClick.AddListener(() => Add1(ref pointBuyDexterity, dexScore));
        takeDexterity.onClick.AddListener(() => Take1(ref pointBuyDexterity, dexScore));
        addConstitution.onClick.AddListener(() => Add1(ref pointBuyConstitution, conScore));
        takeConstitution.onClick.AddListener(() => Take1(ref pointBuyConstitution, conScore));
        addIntelligence.onClick.AddListener(() => Add1(ref pointBuyIntelligence, intScore));
        takeIntelligence.onClick.AddListener(() => Take1(ref pointBuyIntelligence, intScore));
        addWisdom.onClick.AddListener(() => Add1(ref pointBuyWisdom, wisScore));
        takeWisdom.onClick.AddListener(() => Take1(ref pointBuyWisdom, wisScore));
        addCharisma.onClick.AddListener(() => Add1(ref pointBuyCharisma, chaScore));
        takeCharisma.onClick.AddListener(() => Take1(ref pointBuyCharisma, chaScore));

        resumeMenuBack.onClick.AddListener(delegate { OpenScoreSelector(); });
        resumeMenuFinish.onClick.AddListener(delegate { WriteFinalDetails(); gameManager.AddNewCharacterSheetInfo(newCharacterSheet); characterCreatorWindow.SetActive(false); });

        characterCreatorWindow.SetActive(false);
    } 

    private void OnEnable()
    {
        ResetCharacterCreator();
    }

    #endregion
   
    private void LoadCharacterCreationOptions()
    {
        libraryManager.LoadData();
        LoadRaceOptions();
        LoadClassOptions();
        LoadBackgroundOptions();
    }

    private void ResetCharacterCreator()
    {
        newCharacterSheet = new CharacterSheetInfo();

        characterName.text = "";
        SetNewAvatar(0);

        race.value = 0;
        subrace.value = 0;

        classes.value = 0;
        subclasses.value = 0;

        background.value = 0;

        scoreType.value = 0;
        strScore.text = "";
        dexScore.text = "";
        conScore.text = "";
        intScore.text = "";
        wisScore.text = "";
        chaScore.text = "";
        usedPoints.text = "0";

        description.text = "";

        OpenRaceSelector();
    }

    private void SetNewAvatar(int id)
    {
        avatarID = id;
        avatarPortrait.sprite = gameManager.avatarPortrait[id];
    }

    #region race

    private void LoadRaceOptions()
    {
        List<string> raceOptions = new List<string>();
        foreach (Race race in libraryManager.races)
        {
            raceOptions.Add(race.raceName);
        }
        race.AddOptions(raceOptions);
    }

    private void LoadSubraceOptions()
    {
        int raceID = race.value;

        if (raceID == 0)
        {
            subrace.gameObject.SetActive(false);
            return;           
        }

        List<Subrace> subraceList = libraryManager.races[raceID - 1].subraces.list;

        if (subraceList.Count > 0)
        {
            subrace.gameObject.SetActive(true);

            List<string> subraceOptions = new List<string>();
            subraceOptions.Add("");            

            foreach(Subrace subrace in subraceList)
            {
                subraceOptions.Add(subrace.subraceName);
            }

            subrace.ClearOptions();
            subrace.AddOptions(subraceOptions);
        }
        else
        {
            subrace.gameObject.SetActive(false);
        }
    }

    private void ShowRaceInfo()
    {
        int raceID = race.value;

        if (raceID == 0)
        {
            description.text = "";
            return;
        }

        string raceDescription = libraryManager.races[raceID - 1].raceDescription;

        description.text = raceDescription;
    }

    private void WriteRaceTraits()
    {
        int raceID = race.value;
        int subraceID = subrace.value;

        if (raceID == 0) { return; }

        newCharacterSheet.race = libraryManager.races[raceID - 1].raceName;

        resumeRace.text = "Race: " + libraryManager.races[raceID - 1].raceName;
        raceBonus = libraryManager.races[raceID - 1].raceBonus;

        newCharacterSheet.speed = libraryManager.races[raceID - 1].raceSpeed.ToString();


        if (subrace.isActiveAndEnabled && subraceID > 0)
        {
            subraceBonus = libraryManager.races[raceID - 1].subraces.list[subraceID - 1].subraceBonus;

            strRacialBonus.text = (raceBonus[0] + subraceBonus[0]).ToString();
            dexRacialBonus.text = (raceBonus[1] + subraceBonus[1]).ToString();
            conRacialBonus.text = (raceBonus[2] + subraceBonus[2]).ToString();
            intRacialBonus.text = (raceBonus[3] + subraceBonus[3]).ToString();
            wisRacialBonus.text = (raceBonus[4] + subraceBonus[4]).ToString();
            chaRacialBonus.text = (raceBonus[5] + subraceBonus[5]).ToString();
        }
        else
        {
            strRacialBonus.text = raceBonus[0].ToString();
            dexRacialBonus.text = raceBonus[1].ToString();
            conRacialBonus.text = raceBonus[2].ToString();
            intRacialBonus.text = raceBonus[3].ToString();
            wisRacialBonus.text = raceBonus[4].ToString();
            chaRacialBonus.text = raceBonus[5].ToString();
        }

        //have to separate race traits in the json to write them in the sheet
    }

    #endregion

    #region class

    private void LoadClassOptions()
    {
        List<string> classOptions = new List<string>();
        foreach (Class @class in libraryManager.classes)
        {
            classOptions.Add(@class.className);
        }
        classes.AddOptions(classOptions);
    }

    private void LoadSubclassOptions()
    {
        int classID = classes.value;

        if (classID == 0)
        {
            subclasses.gameObject.SetActive(false);
            return;
        }

        List<Subclass> subclassList = libraryManager.classes[classID - 1].subclasses.list;

        if (subclassList.Count == 0 || subclassList[0].startingSubclassTraits.list.Count == 0) 
        {
            subclasses.gameObject.SetActive(false);
            return;
        }  

        if (subclassList.Count > 0)
        {
            subclasses.gameObject.SetActive(true);

            List<string> subclassOptions = new List<string>();
            subclassOptions.Add("");
            subclasses.ClearOptions();

            foreach (Subclass subclass in subclassList)
            {
                subclassOptions.Add(subclass.subclassName);
            }

            subclasses.AddOptions(subclassOptions);
        }
        else
        {
            subclasses.gameObject.SetActive(false);
        }
    }

    private void ShowClassInfo()
    {
        int classID = classes.value;

        if (classID == 0)
        {
            description.text = "";
            return;
        }

        string classDescription = libraryManager.classes[classID - 1].classDescription;

        description.text = classDescription;
    }

    private void WriteClassTraits()
    {
        int classID = classes.value;
        int subclassID = subclasses.value;

        if (classID == 0) { return; }

        newCharacterSheet.clasAndLevel = libraryManager.classes[classID - 1].className + " 1";

        resumeClass.text = "Class: " + libraryManager.classes[classID - 1].className;

        newCharacterSheet.hitDiceType = libraryManager.classes[classID- 1].hitDice;

        List<string> classTraits = libraryManager.classes[classID - 1].startingClassTraits.list;

        

        foreach (string trait in classTraits)
        {
            newCharacterSheet.featuresAndTraits += trait + "\n";
        }
        
        if (subclasses.isActiveAndEnabled && subclassID > 0)
        {
            newCharacterSheet.subclass = libraryManager.classes[classID - 1].subclasses.list[subclassID - 1].subclassName; ;
        }
    }

    #endregion

    #region background

    private void LoadBackgroundOptions()
    {
        List<string> backgroundOptions = new List<string>();
        foreach (Background background in libraryManager.backgrounds)
        {
            backgroundOptions.Add(background.backgroundName);
        }
        background.AddOptions(backgroundOptions);
    }

    private void ShowBackgroundInfo()
    {
        int backgroundID = background.value;

        if (backgroundID == 0)
        {
            description.text = "";
            return;
        }

        string backgroundDescription = libraryManager.backgrounds[backgroundID - 1].backgroundDescription;

        description.text = backgroundDescription;
    }

    private void WriteBackgroundTraits()
    {
        int backgroundID = background.value;

        if (backgroundID == 0) { return; }

        newCharacterSheet.background = libraryManager.backgrounds[backgroundID - 1].backgroundName;

        resumeBackground.text = "Background: " + libraryManager.backgrounds[backgroundID - 1].backgroundName;
    }

    #endregion

    #region ability scores

    private void SwitchScoringType()
    {
        int type = scoreType.value;

        switch (type)
        {
            case 0:
                SwitchToCustomScoring();
                break;
            case 1:
                SwitchToPointBuy();
                break;
            case 2:
                SwitchToRollD20();
                break;
        }
    }

    private void SwitchToCustomScoring()
    {
        rollScores.SetActive(false);
        pointBuy.SetActive(false);

        strScore.interactable = true;
        dexScore.interactable = true;
        conScore.interactable = true;
        intScore.interactable = true;
        wisScore.interactable = true;
        chaScore.interactable = true;
    }

    private void SwitchToPointBuy()
    {
        rollScores.SetActive(false);
        pointBuy.SetActive(true);

        strScore.interactable = false;
        dexScore.interactable = false;
        conScore.interactable = false;
        intScore.interactable = false;
        wisScore.interactable = false;
        chaScore.interactable = false;

        usedPoints.text = "0";
        pointBuyCount = 0;
        strScore.text = "8";
        pointBuyStrenght = 8;
        dexScore.text = "8";
        pointBuyDexterity = 8;
        conScore.text = "8";
        pointBuyConstitution = 8;
        intScore.text = "8";
        pointBuyIntelligence = 8;
        wisScore.text = "8";
        pointBuyWisdom = 8;
        chaScore.text = "8";
        pointBuyCharisma = 8;
    }

    private void SwitchToRollD20()
    {
        rollScores.SetActive(true);
        pointBuy.SetActive(false);

        strScore.interactable = false;
        dexScore.interactable = false;
        conScore.interactable = false;
        intScore.interactable = false;
        wisScore.interactable = false;
        chaScore.interactable = false;
    }

    private void UpdateTotalAbilityScoreAndModifier(InputField baseScore, Text raceBonus, Text totalScore, Text modifier, ref int scoreNumber)
    {
        int score;
        if (!int.TryParse(baseScore.text, out score))
        {
            baseScore.text = "";
        }
        else if (score < 1)
        {
            baseScore.text = "1";
        }
        else if (score > 20)
        {
            baseScore.text = "20";
        }

        int bonus;
        if (!int.TryParse(raceBonus.text, out bonus))
        {
            bonus = 0;
        }

        int total = score + bonus;

        if (total > 20)
        {
            total = 20;
        }

        scoreNumber = total;
        totalScore.text = total.ToString();
        modifier.text = CalculateAbilityModifier(total).ToString();
    }

    private void CheckInt(InputField inputField)
    {
        int value;
        if (!int.TryParse(inputField.text, out value))
        {
            inputField.text = "0";
        }
    }

    private int CalculateAbilityModifier(int abilityScore)
    {
        int mod = -5; // modifier value for a score of 1

        mod += abilityScore / 2;

        return mod;
    }

    private void Add1(ref int scoreValue, InputField score)
    {
        if (scoreValue >= PointBuyMaxScore || pointBuyCount >= PointBuyMax) { return; }

        scoreValue += 1;
        score.text = scoreValue.ToString();

        if (scoreValue < 14)
        {
            pointBuyCount += 1;
        }
        else
        {
            pointBuyCount += 2;
        }

        usedPoints.text = pointBuyCount.ToString();
    }

    private void Take1(ref int scoreValue, InputField score)
    {
        if (scoreValue <= PointBuyMinScore) { return; }

        if (scoreValue < 14)
        {
            pointBuyCount -= 1;
        }
        else
        {
            pointBuyCount -= 2;
        }

        scoreValue--;
        score.text = scoreValue.ToString();

        usedPoints.text = pointBuyCount.ToString();
    }

    private void WriteScores()
    {
        newCharacterSheet.strScore = strTotalScore.text;
        newCharacterSheet.dexScore = dexTotalScore.text;
        newCharacterSheet.conScore = conTotalScore.text;
        newCharacterSheet.intScore = intTotalScore.text;
        newCharacterSheet.wisScore = wisTotalScore.text;
        newCharacterSheet.chaScore = chaTotalScore.text;

        resumeStrScore.text = strTotalScore.text;
        resumeDexScore.text = dexTotalScore.text;
        resumeConScore.text = conTotalScore.text;
        resumeIntScore.text = intTotalScore.text;
        resumeWisScore.text = wisTotalScore.text;
        resumeChaScore.text = chaTotalScore.text;

        resumeStrModifier.text = strModifier.text;
        resumeDexModifier.text = dexModifier.text;
        resumeConModifier.text = conModifier.text;
        resumeIntModifier.text = intModifier.text;
        resumeWisModifier.text = wisModifier.text;
        resumeChaModifier.text = chaModifier.text;
    }

    #endregion

    private void WriteFinalDetails()
    {
        newCharacterSheet.characterName = characterName.text;       
        newCharacterSheet.playerName = uIManager.localPlayer.givenName.Value.ToString();
        newCharacterSheet.sheetID = gameManager.GetNewSheetID();
        newCharacterSheet.avatarID = avatarID;

        newCharacterSheet.initiativeBonus = CalculateAbilityModifier(pointBuyDexterity).ToString();
        newCharacterSheet.armorClass = (10 + CalculateAbilityModifier(pointBuyDexterity)).ToString();
        

        switch (newCharacterSheet.hitDiceType)
        {
            case 0:
                newCharacterSheet.maxHealthPoints = (6 + CalculateAbilityModifier(pointBuyConstitution)).ToString();
                newCharacterSheet.currHealthPoints = (6 + CalculateAbilityModifier(pointBuyConstitution)).ToString();
                break;
            case 1:
                newCharacterSheet.maxHealthPoints = (8 + CalculateAbilityModifier(pointBuyConstitution)).ToString();
                newCharacterSheet.currHealthPoints = (8 + CalculateAbilityModifier(pointBuyConstitution)).ToString();
                break;
            case 2:
                newCharacterSheet.maxHealthPoints = (10 + CalculateAbilityModifier(pointBuyConstitution)).ToString();
                newCharacterSheet.currHealthPoints = (10 + CalculateAbilityModifier(pointBuyConstitution)).ToString();
                break;
            case 3:
                newCharacterSheet.maxHealthPoints = (12 + CalculateAbilityModifier(pointBuyConstitution)).ToString();
                newCharacterSheet.currHealthPoints = (12 + CalculateAbilityModifier(pointBuyConstitution)).ToString();
                break;
        }
    }

    #region navigation methods

    private void ToggleAvatarSelector()
    {
        bool toggle = !avatarSelector.activeInHierarchy;
        avatarSelector.SetActive(toggle);
    }

    private void OpenRaceSelector()
    {
        classSelector.SetActive(false);
        backgroundSelector.SetActive(false);
        resume.SetActive(false);

        raceSelector.SetActive(true);
        descriptionView.SetActive(true);

        ShowRaceInfo();
    }

    private void OpenClassSelector()
    {
        raceSelector.SetActive(false);
        backgroundSelector.SetActive(false);

        classSelector.SetActive(true);

        ShowClassInfo();
    }

    private void OpenBackgroundSelector()
    {
        classSelector.SetActive(false);
        scoresSelector.SetActive(false);

        backgroundSelector.SetActive(true);
        descriptionView.SetActive(true);

        ShowBackgroundInfo();
    }

    private void OpenScoreSelector()
    {
        backgroundSelector.SetActive(false);
        resume.SetActive(false);
        descriptionView.SetActive(false);

        scoresSelector.SetActive(true);
    }

    private void OpenResume()
    {
        scoresSelector.SetActive(false);

        resume.SetActive(true);
    }
    #endregion

}
