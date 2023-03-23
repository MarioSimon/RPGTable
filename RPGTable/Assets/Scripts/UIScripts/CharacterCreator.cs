using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreator : NetworkBehaviour
{

    #region variables
    [SerializeField] GameObject characterCreatorWindow;
    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager uIManager;
    [SerializeField] LibraryManager libraryManager;
    [SerializeField] DiceHandler diceHandler;

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
        ResetCharacterCreator();

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
        resumeMenuFinish.onClick.AddListener(delegate { WriteFinalDetails(); CreateCharacterServerRpc(newCharacterSheet); ResetCharacterCreator(); characterCreatorWindow.SetActive(false); });
    }

    #endregion

    public IEnumerator LoadCharacterCreationOptions()
    {
        yield return new WaitForSeconds(1);

        if (IsHost)
        {
            libraryManager.LoadData();
        }     
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
        
        if (IsHost)
        {
            foreach (Race race in libraryManager.races)
            {
                raceOptions.Add(race.raceName);
            }
        }
        else
        {         
            GetRaceListServerRpc();
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

        if (IsHost)
        {
            List<Subrace> subraceList = libraryManager.races[raceID - 1].subraces.list;

            if (subraceList.Count > 0)
            {
                subrace.gameObject.SetActive(true);

                List<string> subraceOptions = new List<string>();
                subraceOptions.Add("");

                foreach (Subrace subrace in subraceList)
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
        else
        {
            GetSubraceListServerRpc(raceID);
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

        if (IsHost)
        {
            string raceDescription = libraryManager.races[raceID - 1].raceDescription;

            description.text = raceDescription;
        }
        else
        {
            GetRaceDescripionServerRpc(raceID);
        }

    } 

    private void WriteRaceTraits()
    {
        int raceID = race.value;
        int subraceID = subrace.value;

        if (raceID == 0) { return; }

        if (IsHost)
        {
            newCharacterSheet.race = libraryManager.races[raceID - 1].raceName;
            resumeRace.text = "Race: " + newCharacterSheet.race;

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
        }
        else
        {
            GetRaceTraitsServerRpc(raceID, subraceID);
        }
    }

    #endregion

    #region class

    private void LoadClassOptions()
    {
        if (IsHost)
        {
            List<string> classOptions = new List<string>();

            foreach (Class @class in libraryManager.classes)
            {
                classOptions.Add(@class.className);
            }
            classes.AddOptions(classOptions);

        }
        else
        {
            GetClassListServerRpc();
        }
        
    }

    private void LoadSubclassOptions()
    {
        int classID = classes.value;

        if (classID == 0)
        {
            subclasses.gameObject.SetActive(false);
            return;
        }

        if (IsHost)
        {
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
        else
        {
            GetSubclassListServerRpc(classID);
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

        if (IsHost)
        {
            string classDescription = libraryManager.classes[classID - 1].classDescription;

            description.text = classDescription;
        }
        else
        {
            GetClassDescripionServerRpc(classID);
        }
    }

    private void WriteClassTraits()
    {
        int classID = classes.value;
        int subclassID = subclasses.value;

        if (classID == 0) { return; }

        if (IsHost)
        {
            newCharacterSheet.clasAndLevel = libraryManager.classes[classID - 1].className + " 1";

            resumeClass.text = "Class: " + libraryManager.classes[classID - 1].className;

            newCharacterSheet.hitDiceType = libraryManager.classes[classID - 1].hitDice;

            List<string> classTraits = libraryManager.classes[classID - 1].startingClassTraits.list;

            foreach (string trait in classTraits)
            {
                newCharacterSheet.featuresAndTraits += trait + "\n";
            }

            if (subclasses.isActiveAndEnabled && subclassID > 0)
            {
                newCharacterSheet.subclass = libraryManager.classes[classID - 1].subclasses.list[subclassID - 1].subclassName;

                List<string> subclassTraits = libraryManager.classes[classID - 1].subclasses.list[subclassID - 1].startingSubclassTraits.list;
                foreach (string trait in subclassTraits)
                {
                    newCharacterSheet.featuresAndTraits += trait + "\n";
                }
            }
        }
        else
        {
            GetClassTraitsServerRpc(classID, subclassID);
        }
    }  

    #endregion

    #region background

    private void LoadBackgroundOptions()
    {
        if (IsHost)
        {
            List<string> backgroundOptions = new List<string>();
            foreach (Background background in libraryManager.backgrounds)
            {
                backgroundOptions.Add(background.backgroundName);
            }
            background.AddOptions(backgroundOptions);
        }
        else
        {
            GetBackgroundListServerRpc();
        }
    }
 
    private void ShowBackgroundInfo()
    {
        int backgroundID = background.value;

        if (backgroundID == 0)
        {
            description.text = "";
            return;
        }

       if (IsHost)
        {
            string backgroundDescription = libraryManager.backgrounds[backgroundID - 1].backgroundDescription;

            description.text = backgroundDescription;
        }
        else
        {
            GetBackgroundDescriptionServerRpc(backgroundID);
        }
    }

    private void WriteBackgroundTraits()
    {
        int backgroundID = background.value;

        if (backgroundID == 0) { return; }

        if (IsHost)
        {
            newCharacterSheet.background = libraryManager.backgrounds[backgroundID - 1].backgroundName;
            resumeBackground.text = "Background: " + libraryManager.backgrounds[backgroundID - 1].backgroundName;
        }
        else
        {
            GetBackgroundTraitsServerRpc(backgroundID);
        }
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
            case 3:
                SwitchToRoll3D6();
                break;
            case 4:
                SwitchToRoll4D6();
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

        rollStrenght.onClick.RemoveAllListeners();
        rollStrenght.onClick.AddListener(() => RollScoreD20(uIManager.localPlayer.name, SetStrengthD20));
        rollDexterity.onClick.RemoveAllListeners();
        rollDexterity.onClick.AddListener(() => RollScoreD20(uIManager.localPlayer.name, SetDexterityD20));
        rollConstitution.onClick.RemoveAllListeners();
        rollConstitution.onClick.AddListener(() => RollScoreD20(uIManager.localPlayer.name, SetConstitutionD20));
        rollIntelligence.onClick.RemoveAllListeners();
        rollIntelligence.onClick.AddListener(() => RollScoreD20(uIManager.localPlayer.name, SetIntelligenceD20));
        rollWisdom.onClick.RemoveAllListeners();
        rollWisdom.onClick.AddListener(() => RollScoreD20(uIManager.localPlayer.name, SetWisdomD20));
        rollCharisma.onClick.RemoveAllListeners();
        rollCharisma.onClick.AddListener(() => RollScoreD20(uIManager.localPlayer.name, SetCharismaD20));

        strScore.interactable = false;
        dexScore.interactable = false;
        conScore.interactable = false;
        intScore.interactable = false;
        wisScore.interactable = false;
        chaScore.interactable = false;
    }

    private void SwitchToRoll3D6()
    {
        rollScores.SetActive(true);
        pointBuy.SetActive(false);

        rollStrenght.onClick.RemoveAllListeners();
        rollStrenght.onClick.AddListener(() => RollScore3D6(uIManager.localPlayer.name, SetStrength3D6));
        rollDexterity.onClick.RemoveAllListeners();
        rollDexterity.onClick.AddListener(() => RollScore3D6(uIManager.localPlayer.name, SetDexterity3D6));
        rollConstitution.onClick.RemoveAllListeners();
        rollConstitution.onClick.AddListener(() => RollScore3D6(uIManager.localPlayer.name, SetConstitution3D6));
        rollIntelligence.onClick.RemoveAllListeners();
        rollIntelligence.onClick.AddListener(() => RollScore3D6(uIManager.localPlayer.name, SetIntelligence3D6));
        rollWisdom.onClick.RemoveAllListeners();
        rollWisdom.onClick.AddListener(() => RollScore3D6(uIManager.localPlayer.name, SetWisdom3D6));
        rollCharisma.onClick.RemoveAllListeners();
        rollCharisma.onClick.AddListener(() => RollScore3D6(uIManager.localPlayer.name, SetCharisma3D6));

        strScore.interactable = false;
        dexScore.interactable = false;
        conScore.interactable = false;
        intScore.interactable = false;
        wisScore.interactable = false;
        chaScore.interactable = false;
    }

    private void SwitchToRoll4D6()
    {
        rollScores.SetActive(true);
        pointBuy.SetActive(false);

        rollStrenght.onClick.RemoveAllListeners();
        rollStrenght.onClick.AddListener(() => RollScore4D6(uIManager.localPlayer.name, SetStrength4D6));
        rollDexterity.onClick.RemoveAllListeners();
        rollDexterity.onClick.AddListener(() => RollScore4D6(uIManager.localPlayer.name, SetDexterity4D6));
        rollConstitution.onClick.RemoveAllListeners();
        rollConstitution.onClick.AddListener(() => RollScore4D6(uIManager.localPlayer.name, SetConstitution4D6));
        rollIntelligence.onClick.RemoveAllListeners();
        rollIntelligence.onClick.AddListener(() => RollScore4D6(uIManager.localPlayer.name, SetIntelligence4D6));
        rollWisdom.onClick.RemoveAllListeners();
        rollWisdom.onClick.AddListener(() => RollScore4D6(uIManager.localPlayer.name, SetWisdom4D6));
        rollCharisma.onClick.RemoveAllListeners();
        rollCharisma.onClick.AddListener(() => RollScore4D6(uIManager.localPlayer.name, SetCharisma4D6));

        strScore.interactable = false;
        dexScore.interactable = false;
        conScore.interactable = false;
        intScore.interactable = false;
        wisScore.interactable = false;
        chaScore.interactable = false;
    }

    #region d20 rolls

    private void RollScoreD20(string thrownBy, Action<string, int> resultFunction)
    {
        string rollKey = diceHandler.GetNewRollKey(thrownBy + "-");
        string message = "";

        diceHandler.AddRoll(rollKey, thrownBy, 1, message);

        StartCoroutine(diceHandler.RollDice(rollKey, diceType.d20, 0, resultFunction));
    }

    private void SetStrengthD20(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        foreach (int diceScore in roll.diceScores)
        {
            result += diceScore;
        }

        strScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    private void SetDexterityD20(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        foreach (int diceScore in roll.diceScores)
        {
            result += diceScore;
        }

        dexScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    private void SetConstitutionD20(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        foreach (int diceScore in roll.diceScores)
        {
            result += diceScore;
        }

        conScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    private void SetIntelligenceD20(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        foreach (int diceScore in roll.diceScores)
        {
            result += diceScore;
        }

        intScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    private void SetWisdomD20(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        foreach (int diceScore in roll.diceScores)
        {
            result += diceScore;
        }

        wisScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    private void SetCharismaD20(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        foreach (int diceScore in roll.diceScores)
        {
            result += diceScore;
        }

        chaScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    #endregion

    #region 3d6 rolls

    private void RollScore3D6(string thrownBy, Action<string, int> resultFunction)
    {
        string rollKey = diceHandler.GetNewRollKey(thrownBy + "-");
        string message = "";

        diceHandler.AddRoll(rollKey, thrownBy, 3, message);

        StartCoroutine(diceHandler.RollDice(rollKey, diceType.d6, 0, resultFunction));
        StartCoroutine(diceHandler.RollDice(rollKey, diceType.d6, 0, resultFunction));
        StartCoroutine(diceHandler.RollDice(rollKey, diceType.d6, 0, resultFunction));
    }

    private void SetStrength3D6(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        foreach (int diceScore in roll.diceScores)
        {
            result += diceScore;
        }

        strScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    private void SetDexterity3D6(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        foreach (int diceScore in roll.diceScores)
        {
            result += diceScore;
        }

        dexScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    private void SetConstitution3D6(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        foreach (int diceScore in roll.diceScores)
        {
            result += diceScore;
        }

        conScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    private void SetIntelligence3D6(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        foreach (int diceScore in roll.diceScores)
        {
            result += diceScore;
        }

        intScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    private void SetWisdom3D6(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        foreach (int diceScore in roll.diceScores)
        {
            result += diceScore;
        }

        wisScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    private void SetCharisma3D6(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        foreach (int diceScore in roll.diceScores)
        {
            result += diceScore;
        }

        chaScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    #endregion

    #region 4d6 rolls

    private void RollScore4D6(string thrownBy, Action<string, int> resultFunction)
    {
        string rollKey = diceHandler.GetNewRollKey(thrownBy + "-");
        string message = "";

        diceHandler.AddRoll(rollKey, thrownBy, 4, message);

        StartCoroutine(diceHandler.RollDice(rollKey, diceType.d6, 0, resultFunction));
        StartCoroutine(diceHandler.RollDice(rollKey, diceType.d6, 0, resultFunction));
        StartCoroutine(diceHandler.RollDice(rollKey, diceType.d6, 0, resultFunction));
        StartCoroutine(diceHandler.RollDice(rollKey, diceType.d6, 0, resultFunction));
    }

    private void SetStrength4D6(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        List<int> diceResults = new List<int>(roll.diceScores);
        int minValueIndex = 0;

        for (int i = 1; i < diceResults.Count; i++)
        {
            if (diceResults[i] < diceResults[minValueIndex])
            {
                minValueIndex = i;
            }
        }

        diceResults.RemoveAt(minValueIndex);

        foreach (int diceScore in diceResults)
        {
            result += diceScore;
        }

        strScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    private void SetDexterity4D6(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        List<int> diceResults = new List<int>(roll.diceScores);
        int minValueIndex = 0;

        for (int i = 1; i < diceResults.Count; i++)
        {
            if (diceResults[i] < diceResults[minValueIndex])
            {
                minValueIndex = i;
            }
        }

        diceResults.RemoveAt(minValueIndex);

        foreach (int diceScore in diceResults)
        {
            result += diceScore;
        }

        dexScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    private void SetConstitution4D6(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        List<int> diceResults = new List<int>(roll.diceScores);
        int minValueIndex = 0;

        for (int i = 1; i < diceResults.Count; i++)
        {
            if (diceResults[i] < diceResults[minValueIndex])
            {
                minValueIndex = i;
            }
        }

        diceResults.RemoveAt(minValueIndex);

        foreach (int diceScore in diceResults)
        {
            result += diceScore;
        }

        conScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    private void SetIntelligence4D6(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        List<int> diceResults = new List<int>(roll.diceScores);
        int minValueIndex = 0;

        for (int i = 1; i < diceResults.Count; i++)
        {
            if (diceResults[i] < diceResults[minValueIndex])
            {
                minValueIndex = i;
            }
        }

        diceResults.RemoveAt(minValueIndex);

        foreach (int diceScore in diceResults)
        {
            result += diceScore;
        }

        intScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    private void SetWisdom4D6(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        List<int> diceResults = new List<int>(roll.diceScores);
        int minValueIndex = 0;

        for (int i = 1; i < diceResults.Count; i++)
        {
            if (diceResults[i] < diceResults[minValueIndex])
            {
                minValueIndex = i;
            }
        }

        diceResults.RemoveAt(minValueIndex);

        foreach (int diceScore in diceResults)
        {
            result += diceScore;
        }

        wisScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    private void SetCharisma4D6(string rollKey, int modifier)
    {
        DiceRollInfo roll = diceHandler.GetRollInfo(rollKey);
        int result = 0;

        List<int> diceResults = new List<int>(roll.diceScores);
        int minValueIndex = 0;

        for (int i = 1; i < diceResults.Count; i++)
        {
            if (diceResults[i] < diceResults[minValueIndex])
            {
                minValueIndex = i;
            }
        }

        diceResults.RemoveAt(minValueIndex);

        foreach (int diceScore in diceResults)
        {
            result += diceScore;
        }

        chaScore.text = result.ToString();

        diceHandler.DeleteRoll(rollKey);
    }

    #endregion

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
        newCharacterSheet.ownerID = NetworkManager.Singleton.LocalClientId;

        newCharacterSheet.characterName = characterName.text;       
        newCharacterSheet.playerName = uIManager.localPlayer.playerName;
        newCharacterSheet.sheetID = gameManager.GetNewSheetID();
        newCharacterSheet.avatarID = avatarID;

        newCharacterSheet.initiativeBonus = CalculateAbilityModifier(passingDexterity).ToString();
        newCharacterSheet.armorClass = (10 + CalculateAbilityModifier(passingDexterity)).ToString();
        

        switch (newCharacterSheet.hitDiceType)
        {
            case 0:
                newCharacterSheet.maxHealthPoints = (6 + CalculateAbilityModifier(passingConstitution)).ToString();
                newCharacterSheet.currHealthPoints = (6 + CalculateAbilityModifier(passingConstitution)).ToString();
                break;
            case 1:
                newCharacterSheet.maxHealthPoints = (8 + CalculateAbilityModifier(passingConstitution)).ToString();
                newCharacterSheet.currHealthPoints = (8 + CalculateAbilityModifier(passingConstitution)).ToString();
                break;
            case 2:
                newCharacterSheet.maxHealthPoints = (10 + CalculateAbilityModifier(passingConstitution)).ToString();
                newCharacterSheet.currHealthPoints = (10 + CalculateAbilityModifier(passingConstitution)).ToString();
                break;
            case 3:
                newCharacterSheet.maxHealthPoints = (12 + CalculateAbilityModifier(passingConstitution)).ToString();
                newCharacterSheet.currHealthPoints = (12 + CalculateAbilityModifier(passingConstitution)).ToString();
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

    #region serverRpc

    [ServerRpc(RequireOwnership = false)]
    private void CreateCharacterServerRpc(CharacterSheetInfo newCSInfo)
    {
        gameManager.AddNewCharacterSheetInfo(newCSInfo);
    }

    #region race serverRpc

    [ServerRpc(RequireOwnership = false)]
    private void GetRaceListServerRpc(ServerRpcParams serverRpcParams = default)
    {
        if (!IsServer) { return; }

        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            List<Race> races = libraryManager.races;
            int raceCount = races.Count;

            StringContainer[] raceList = new StringContainer[raceCount];


            for (int i = 0; i < raceCount; i++)
            {
                raceList[i] = new StringContainer(races[i].raceName);
            }

            LoadRaceListClientRpc(raceList, clientRpcParams);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void GetSubraceListServerRpc(int raceID, ServerRpcParams serverRpcParams = default)
    {
        if (!IsServer) { return; }

        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            List<Subrace> subraces = libraryManager.races[raceID - 1].subraces.list;
            int subraceCount = subraces.Count;

            StringContainer[] subraceList = new StringContainer[subraceCount];

            for (int i = 0; i < subraceCount; i++)
            {
                subraceList[i] = new StringContainer(subraces[i].subraceName);
            }

            LoadSubraceListClientRpc(subraceList, clientRpcParams);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void GetRaceDescripionServerRpc(int raceID, ServerRpcParams serverRpcParams = default)
    {
        if (!IsServer) { return; }

        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            StringContainer raceDescription = new StringContainer(libraryManager.races[raceID - 1].raceDescription);

            LoadRaceDescriptionClientRpc(raceDescription, clientRpcParams);
        }

    }

    [ServerRpc(RequireOwnership = false)]
    private void GetRaceTraitsServerRpc(int raceID, int subraceID, ServerRpcParams serverRpcParams = default)
    {
        if (!IsServer) { return; }

        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            StringContainer raceName = new StringContainer(libraryManager.races[raceID - 1].raceName);

            int raceSpeed = libraryManager.races[raceID - 1].raceSpeed;

            int[] scoreBonus = libraryManager.races[raceID - 1].raceBonus;

            if (subrace.isActiveAndEnabled && subraceID > 0)
            {
                int[] subraceScoreBonus = libraryManager.races[raceID - 1].subraces.list[subraceID - 1].subraceBonus;

                scoreBonus[0] += subraceScoreBonus[0];
                scoreBonus[1] += subraceScoreBonus[1];
                scoreBonus[2] += subraceScoreBonus[2];
                scoreBonus[3] += subraceScoreBonus[3];
                scoreBonus[4] += subraceScoreBonus[4];
                scoreBonus[5] += subraceScoreBonus[5];
            }

            WriteRaceTraitsClientRpc(raceName, raceSpeed, scoreBonus, clientRpcParams);
        }
    }

    #endregion

    #region class serverRpc

    [ServerRpc(RequireOwnership = false)]
    private void GetClassListServerRpc(ServerRpcParams serverRpcParams = default)
    {
        if (!IsServer) { return; }

        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            List<Class> classes = libraryManager.classes;
            int raceCount = classes.Count;

            StringContainer[] classList = new StringContainer[raceCount];


            for (int i = 0; i < raceCount; i++)
            {
                classList[i] = new StringContainer(classes[i].className);
            }

            LoadClassListClientRpc(classList, clientRpcParams);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void GetSubclassListServerRpc(int classID, ServerRpcParams serverRpcParams = default)
    {
        if (!IsServer) { return; }

        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            List<Subclass> subclasses = libraryManager.classes[classID - 1].subclasses.list;
            int subclassCount = subclasses.Count;

            StringContainer[] subclassList;

            if (subclassCount == 0 || subclasses[0].startingSubclassTraits.list.Count == 0)
            {
                subclassList = new StringContainer[0];
            }
            else
            {
                subclassList = new StringContainer[subclassCount];

                for (int i = 0; i < subclassCount; i++)
                {
                    subclassList[i] = new StringContainer(subclasses[i].subclassName);
                }
            }

            LoadSubclassListClientRpc(subclassList, clientRpcParams);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void GetClassDescripionServerRpc(int classID, ServerRpcParams serverRpcParams = default)
    {
        if (!IsServer) { return; }

        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            StringContainer classDescription = new StringContainer(libraryManager.classes[classID - 1].classDescription);

            LoadClassDescriptionClientRpc(classDescription, clientRpcParams);
        }

    }

    [ServerRpc(RequireOwnership = false)]
    private void GetClassTraitsServerRpc(int classID, int subclassID, ServerRpcParams serverRpcParams = default)
    {
        if (!IsServer) { return; }

        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };


            StringContainer className = new StringContainer(libraryManager.classes[classID - 1].className);

            int hitDice = libraryManager.classes[classID - 1].hitDice;

            List<string> classTraits = libraryManager.classes[classID - 1].startingClassTraits.list;
            int classTraitCount = classTraits.Count;

            List<string> subclassTraits = new List<string>();
            int subclassTraitCount = 0;

            StringContainer subclassName = new StringContainer("");

            if (subclassID > 0)
            {
                subclassName.SomeText = libraryManager.classes[classID - 1].subclasses.list[subclassID - 1].subclassName;

                subclassTraits = libraryManager.classes[classID - 1].subclasses.list[subclassID - 1].startingSubclassTraits.list;
                subclassTraitCount = libraryManager.classes[classID - 1].subclasses.list[subclassID - 1].startingSubclassTraits.list.Count;
            }
            else
            {
                subclassName.SomeText = "";
            }
            StringContainer traits = new StringContainer("");

            for (int i = 0; i < classTraitCount; i++)
            {
                traits.SomeText += classTraits[i] + "\n";
            }

            for (int i = 0; i < subclassTraitCount; i++)
            {
                traits.SomeText += subclassTraits[i] + "\n";
            }

            WriteClassTraitsClientRpc(className, subclassName, hitDice, traits, clientRpcParams);
        }
    }

    #endregion

    #region background serverRpc

    [ServerRpc(RequireOwnership = false)]
    private void GetBackgroundListServerRpc(ServerRpcParams serverRpcParams = default)
    {
        if (!IsServer) { return; }

        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            List<Background> backgrounds = libraryManager.backgrounds;
            int raceCount = backgrounds.Count;

            StringContainer[] backgroundList = new StringContainer[raceCount];


            for (int i = 0; i < raceCount; i++)
            {
                backgroundList[i] = new StringContainer(backgrounds[i].backgroundName);
            }

            LoadBackgroundListClientRpc(backgroundList, clientRpcParams);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void GetBackgroundDescriptionServerRpc(int backgroundID, ServerRpcParams serverRpcParams = default)
    {
        if (!IsServer) { return; }

        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            StringContainer backgroundDescription = new StringContainer(libraryManager.backgrounds[backgroundID - 1].backgroundDescription);

            LoadBackgroundDescriptionClientRpc(backgroundDescription, clientRpcParams);
        }

    }

    [ServerRpc(RequireOwnership = false)]
    private void GetBackgroundTraitsServerRpc(int backgroundID, ServerRpcParams serverRpcParams = default)
    {
        if (!IsServer) { return; }

        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            StringContainer backgroundName = new StringContainer(libraryManager.backgrounds[backgroundID - 1].backgroundName);


            WriteBackgroundTraitsClientRpc(backgroundName, clientRpcParams);
        }
    }

    #endregion

    #endregion

    #region clientRpc

    #region race clientRpc

    [ClientRpc]
    private void LoadRaceListClientRpc(StringContainer[] raceList, ClientRpcParams clientRpcParams)
    {

        if (IsOwner) { return; }

        List<string> raceOptions = new List<string>();

        foreach (StringContainer raceName in raceList)
        {
            raceOptions.Add(raceName.SomeText);
        }

        race.AddOptions(raceOptions);
    }

    [ClientRpc]
    private void LoadSubraceListClientRpc(StringContainer[] subraceList, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        if (subraceList.Length > 0)
        {
            subrace.gameObject.SetActive(true);

            List<string> subraceOptions = new List<string>();
            subraceOptions.Add("");

            foreach (StringContainer container in subraceList)
            {
                subraceOptions.Add(container.SomeText);
            }

            subrace.ClearOptions();
            subrace.AddOptions(subraceOptions);
        }
        else
        {
            subrace.gameObject.SetActive(false);
        }
    }

    [ClientRpc]
    private void LoadRaceDescriptionClientRpc(StringContainer raceDescription, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        description.text = raceDescription.SomeText;
    }

    [ClientRpc]
    private void WriteRaceTraitsClientRpc(StringContainer raceName, int raceSpeed, int[] scoreBonus, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        newCharacterSheet.race = raceName.SomeText;
        resumeRace.text = "Race: " + raceName.SomeText;

        newCharacterSheet.speed = raceSpeed.ToString();

        strRacialBonus.text = scoreBonus[0].ToString();
        dexRacialBonus.text = scoreBonus[1].ToString();
        conRacialBonus.text = scoreBonus[2].ToString();
        intRacialBonus.text = scoreBonus[3].ToString();
        wisRacialBonus.text = scoreBonus[4].ToString();
        chaRacialBonus.text = scoreBonus[5].ToString();
    }

    #endregion

    #region class clientRpc

    [ClientRpc]
    private void LoadClassListClientRpc(StringContainer[] classList, ClientRpcParams clientRpcParams)
    {

        if (IsOwner) { return; }

        List<string> classOptions = new List<string>();

        foreach (StringContainer raceName in classList)
        {
            classOptions.Add(raceName.SomeText);
        }

        classes.AddOptions(classOptions);
    }

    [ClientRpc]
    private void LoadSubclassListClientRpc(StringContainer[] subclassList, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        if (subclassList.Length > 0)
        {
            subclasses.gameObject.SetActive(true);

            List<string> subclassOptions = new List<string>();
            subclassOptions.Add("");

            foreach (StringContainer container in subclassList)
            {
                subclassOptions.Add(container.SomeText);
            }

            subclasses.ClearOptions();
            subclasses.AddOptions(subclassOptions);
        }
        else
        {
            subclasses.gameObject.SetActive(false);
        }
    }

    [ClientRpc]
    private void LoadClassDescriptionClientRpc(StringContainer classDescription, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        description.text = classDescription.SomeText;
    }
    
    [ClientRpc]
    private void WriteClassTraitsClientRpc(StringContainer className, StringContainer subclassName, int hitDice, StringContainer traits, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        newCharacterSheet.clasAndLevel = className.SomeText + " 1";
        resumeClass.text = "Class: " + className.SomeText;

        if (subclasses.isActiveAndEnabled)
        {
            newCharacterSheet.subclass = subclassName.SomeText;
        }

        newCharacterSheet.hitDiceType = hitDice;

        newCharacterSheet.featuresAndTraits += traits.SomeText;
    }

    #endregion

    #region background clientRpc

    [ClientRpc]
    private void LoadBackgroundListClientRpc(StringContainer[] backgroundList, ClientRpcParams clientRpcParams)
    {

        if (IsOwner) { return; }

        List<string> backgroundOptions = new List<string>();

        foreach (StringContainer backgroundName in backgroundList)
        {
            backgroundOptions.Add(backgroundName.SomeText);
        }

        background.AddOptions(backgroundOptions);
    }

    [ClientRpc]
    private void LoadBackgroundDescriptionClientRpc(StringContainer backgroundDescription, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        description.text = backgroundDescription.SomeText;
    }

    [ClientRpc]
    private void WriteBackgroundTraitsClientRpc(StringContainer backgroundName, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        newCharacterSheet.background = backgroundName.SomeText;
        resumeBackground.text = "Background: " + backgroundName.SomeText;
    }

    #endregion

    #endregion
}
