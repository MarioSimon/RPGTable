using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LibraryManager : MonoBehaviour
{
    [SerializeField] Button showRaces;
    [SerializeField] Dropdown baseRacesList;
    [SerializeField] Dropdown subraceList;
    [SerializeField] Button showClasses;
    [SerializeField] Dropdown baseClassesList;
    [SerializeField] Dropdown subclassesList;
    [SerializeField] Button showBackgrounds;
    [SerializeField] Dropdown backgroundsList;
    [SerializeField] Button showFeats;
    [SerializeField] Dropdown featList;
    [SerializeField] Button showSpells;
    [SerializeField] Dropdown spellLevelList;
    [SerializeField] Dropdown spellList;
    [SerializeField] Text information;

    public List<Race> races;
     
    public List<Class> classes;
    
    public List<Background> backgrounds;
     
    public List<Feat> feats;
     
    public List<List<Spell>> spells;

    public bool loadedData;

    void Start()
    {
        LoadData();

        showRaces.onClick.AddListener(() => OpenRaces());
        baseRacesList.onValueChanged.AddListener(delegate { ShowRace(); });
        subraceList.onValueChanged.AddListener(delegate { ShowSubrace(); });

        showClasses.onClick.AddListener(() => OpenClasses());
        baseClassesList.onValueChanged.AddListener(delegate { ShowClass(); });
        subclassesList.onValueChanged.AddListener(delegate { ShowSubclass(); });

        showBackgrounds.onClick.AddListener(() => OpenBackgrounds());
        backgroundsList.onValueChanged.AddListener(delegate { ShowBackground(); });

        showFeats.onClick.AddListener(() => OpenFeats());
        featList.onValueChanged.AddListener(delegate { ShowFeat(); });

        showSpells.onClick.AddListener(() => OpenSpells());
        spellLevelList.onValueChanged.AddListener(delegate { ShowSpellList(); });
        spellList.onValueChanged.AddListener(delegate { ShowSpell(); });
    }

    public void LoadData()
    {
        if (loadedData) { return; }

        LoadRacesFromJSON();
        LoadClassesFromJSON();
        LoadBackgroundsFromJSON();
        loadedData = true;
    }

    #region navigation methods

    void OpenRaces()
    {
        baseClassesList.transform.gameObject.SetActive(false);
        subclassesList.transform.gameObject.SetActive(false);
        backgroundsList.transform.gameObject.SetActive(false);
        featList.transform.gameObject.SetActive(false);
        spellLevelList.transform.gameObject.SetActive(false);
        spellList.transform.gameObject.SetActive(false);

        baseRacesList.transform.gameObject.SetActive(true);
        subraceList.transform.gameObject.SetActive(true);

        information.text = "";
    }

    void OpenClasses()
    {
        baseRacesList.transform.gameObject.SetActive(false);
        subraceList.transform.gameObject.SetActive(false);
        backgroundsList.transform.gameObject.SetActive(false);
        featList.transform.gameObject.SetActive(false);
        spellLevelList.transform.gameObject.SetActive(false);
        spellList.transform.gameObject.SetActive(false);

        baseClassesList.transform.gameObject.SetActive(true);
        subclassesList.transform.gameObject.SetActive(true);

        information.text = "";
    }

    void OpenBackgrounds()
    {
        baseClassesList.transform.gameObject.SetActive(false);
        subclassesList.transform.gameObject.SetActive(false);
        baseRacesList.transform.gameObject.SetActive(false);
        subraceList.transform.gameObject.SetActive(false);
        featList.transform.gameObject.SetActive(false);
        spellLevelList.transform.gameObject.SetActive(false);
        spellList.transform.gameObject.SetActive(false);

        backgroundsList.transform.gameObject.SetActive(true);

        information.text = "";
    }

    void OpenFeats()
    {
        baseClassesList.transform.gameObject.SetActive(false);
        subclassesList.transform.gameObject.SetActive(false);
        baseRacesList.transform.gameObject.SetActive(false);
        subraceList.transform.gameObject.SetActive(false);
        backgroundsList.transform.gameObject.SetActive(false);
        spellLevelList.transform.gameObject.SetActive(false);
        spellList.transform.gameObject.SetActive(false);

        featList.transform.gameObject.SetActive(true);

        information.text = "";
    }

    void OpenSpells()
    {
        baseRacesList.transform.gameObject.SetActive(false);
        subraceList.transform.gameObject.SetActive(false);
        baseClassesList.transform.gameObject.SetActive(false);
        subclassesList.transform.gameObject.SetActive(false);
        backgroundsList.transform.gameObject.SetActive(false);
        featList.transform.gameObject.SetActive(false);

        spellLevelList.transform.gameObject.SetActive(true);
        spellList.transform.gameObject.SetActive(true);

        information.text = "";
    }

    void ShowRace()
    {
        int raceID = baseRacesList.value;

        if (raceID == 0)
        {
            information.text = "";
            return;
        }
        string newInfo = races[raceID-1].raceName + "\n\n" + races[raceID-1].raceDescription;
        information.text = newInfo;

        List<string> subraceOptions = new List<string>();
        subraceOptions.Add("");
        foreach (Subrace subrace in races[raceID-1].subraces.list)
        {
            subraceOptions.Add(subrace.subraceName);
        }

        subraceList.ClearOptions();
        subraceList.AddOptions(subraceOptions);
    }

    void ShowSubrace()
    {
        int raceID = baseRacesList.value;
        int subraceID = subraceList.value;

        if (raceID == 0)
        {
            information.text = "";
            return;
        }
        string newInfo = races[raceID - 1].raceName + "\n\n" + races[raceID - 1].raceDescription;

        if (subraceID == 0)
        {
            information.text = newInfo;
            return;
        }
        newInfo += "\n\n" + races[raceID-1].subraces.list[subraceID-1].subraceName + "\n\n" + races[raceID - 1].subraces.list[subraceID - 1].subraceDescription;
        information.text = newInfo;
    }

    void ShowClass()
    {
        int classID = baseClassesList.value;

        if (classID == 0)
        {
            information.text = "";
            return;
        }
        string newInfo = classes[classID - 1].className + "\n\n" + classes[classID - 1].classDescription;
        information.text = newInfo;

        List<string> subclassOptions = new List<string>();
        subclassOptions.Add("Base");
        foreach (Subclass subrace in classes[classID - 1].subclasses.list)
        {
            subclassOptions.Add(subrace.subclassName);
        }

        subclassesList.ClearOptions();
        subclassesList.AddOptions(subclassOptions);
    }

    void ShowSubclass()
    {
        int classID = baseClassesList.value;
        int subclassID = subclassesList.value;

        if (classID == 0)
        {
            information.text = "";
            return;
        }
        string newInfo = classes[classID - 1].className + "\n\n" + classes[classID - 1].classDescription;

        if (subclassID == 0)
        {
            information.text = newInfo;
            return;
        }
        newInfo = classes[classID - 1].subclasses.list[subclassID - 1].subclassName + "\n\n" + classes[classID - 1].subclasses.list[subclassID - 1].subclassDescription;
        information.text = newInfo;
    }

    void ShowBackground()
    {
        int backgroundID = backgroundsList.value;

        if (backgroundID == 0)
        {
            information.text = "";
            return;
        }
        string newInfo = backgrounds[backgroundID - 1].backgroundName + "\n\n" + backgrounds[backgroundID - 1].backgroundDescription;
        information.text = newInfo;
    }

    void ShowFeat()
    {
        int featID = backgroundsList.value;

        if (featID == 0)
        {
            information.text = "";
            return;
        }
        string newInfo =feats[featID - 1].featName + "\n\n" + feats[featID - 1].featDescription;
        information.text = newInfo;
    }

    void ShowSpellList()
    {
        int spellLevel = spellLevelList.value;

        if (spellLevel == 0)
        {
            information.text = "";
            return;
        }

        List<string> spellOptions = new List<string>();
        spellOptions.Add("");
        foreach (Spell spell in spells[spellLevel - 1])
        {
            spellOptions.Add(spell.spellName);
        }

        spellList.ClearOptions();
        spellList.AddOptions(spellOptions);
    }

    void ShowSpell()
    {
        int spellLevel = spellLevelList.value;
        int spellID = subclassesList.value;

        if (spellID == 0)
        {
            information.text = "";
            return;
        }
        string newInfo = spells[spellLevel - 1][spellID - 1].spellName + "\n\n" + spells[spellLevel - 1][spellID - 1].spellDescription;
        information.text = newInfo;
    }

    #endregion

    #region serialization methods
    void SaveRacesToJSON()
    {
        if (races.Count < 1) { return; }

        SerializableList<Race> savedRacesData = new SerializableList<Race>();
        savedRacesData.list = races;

        string json = JsonUtility.ToJson(savedRacesData);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/races.json", json);

        Debug.Log("SAVED RACES AT " + Application.dataPath + "/StreamingAssets/races.json");
    }

    void LoadRacesFromJSON()
    {
        if (!File.Exists(Application.dataPath + "/StreamingAssets/races.json")) { return; }

        string jsonString = File.ReadAllText(Application.dataPath + "/StreamingAssets/races.json");
        SerializableList<Race> savedData = JsonUtility.FromJson<SerializableList<Race>>(jsonString);

        races = savedData.list;

        List<string> raceNames = new List<string>();
        foreach (Race race in races)
        {
            raceNames.Add(race.raceName);
        }
        baseRacesList.AddOptions(raceNames);

        //Debug.Log("LOADED RACES FROM " + Application.dataPath + "/races.json");
    }

    void LoadClassesFromJSON()
    {
        if (!File.Exists(Application.dataPath + "/StreamingAssets/classes.json")) { return; }

        string jsonString = File.ReadAllText(Application.dataPath + "/StreamingAssets/classes.json");
        SerializableList<Class> savedData = JsonUtility.FromJson<SerializableList<Class>>(jsonString);

        classes = savedData.list;

        List<string> classNames = new List<string>();
        foreach (Class @class in classes)
        {
            classNames.Add(@class.className);
        }
        baseClassesList.AddOptions(classNames);

        //Debug.Log("LOADED CLASSES FROM " + Application.dataPath + "/classes.json");
    }

    void LoadBackgroundsFromJSON()
    {
        if (!File.Exists(Application.dataPath + "/StreamingAssets/backgrounds.json")) { return; }

        string jsonString = File.ReadAllText(Application.dataPath + "/StreamingAssets/backgrounds.json");
        SerializableList<Background> savedData = JsonUtility.FromJson<SerializableList<Background>>(jsonString);

         backgrounds = savedData.list;

        List<string> backgroundNames = new List<string>();
        foreach (Background background in backgrounds)
        {
            backgroundNames.Add(background.backgroundName);
        }
        backgroundsList.AddOptions(backgroundNames);

        //Debug.Log("LOADED BACKGROUNDS FROM " + Application.dataPath + "/backgrounds.json");
    }

    void LoadFeatsFromJSON()
    {
        if (!File.Exists(Application.dataPath + "/StreamingAssets/feats.json")) { return; }

        string jsonString = File.ReadAllText(Application.dataPath + "/StreamingAssets/feats.json");
        SerializableList<Feat> savedData = JsonUtility.FromJson<SerializableList<Feat>>(jsonString);

        feats = savedData.list;

        List<string> featNames = new List<string>();
        foreach (Feat feat in feats)
        {
            featNames.Add(feat.featName);
        }
        backgroundsList.AddOptions(featNames);

        Debug.Log("LOADED FEATS FROM " + Application.dataPath + "/StreamingAssets/feats.json");
    }

    void LoadSpellsFromJSON()
    {
        if (!File.Exists(Application.dataPath + "/StreamingAssets/spells.json")) { return; }

        string jsonString = File.ReadAllText(Application.dataPath + "/StreamingAssets/spells.json");
        SerializableList<List<Spell>> savedData = JsonUtility.FromJson<SerializableList<List<Spell>>>(jsonString);

        spells = savedData.list;

        Debug.Log("LOADED SPELLS FROM " + Application.dataPath + "/StreamingAssets/spells.json");
    }

    #endregion
}

#region data structs

[System.Serializable]
public struct Race
{
    public string raceName;
    public string raceDescription;
    public int[] raceBonus;
    public int raceSpeed;
    public SerializableList<Subrace> subraces;
}

[System.Serializable]
public struct Subrace
{
    public string subraceName;
    public string subraceDescription;
    public int[] subraceBonus;
}

[System.Serializable]
public struct Class
{
    public string className;
    public string classDescription;
    public int hitDice;
    public SerializableList<string> startingClassTraits;
    public SerializableList<Subclass> subclasses;
}

[System.Serializable]
public struct Subclass
{
    public string subclassName;
    public string subclassDescription;
    public SerializableList<string> startingSubclassTraits;
}

[System.Serializable]
public struct Background
{
    public string backgroundName;
    public string backgroundDescription;
}

[System.Serializable]
public struct Feat
{
    public string featName;
    public string featDescription;
}

[System.Serializable]
public struct Spell
{
    public string spellName;
    public string spellDescription;
}

#endregion