using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LibraryManager : NetworkBehaviour
{
    #region variables

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

    #endregion

    void Start()
    {
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
       //
       //showSpells.onClick.AddListener(() => OpenSpells());
       //spellLevelList.onValueChanged.AddListener(delegate { ShowSpellList(); });
       //spellList.onValueChanged.AddListener(delegate { ShowSpell(); });
    }

    public void LoadData()
    {
        if (loadedData) { return; }

        LoadRacesFromJSON();
        LoadClassesFromJSON();
        LoadBackgroundsFromJSON();
        LoadFeatsFromJSON();
        loadedData = true;
    }

    #region races

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

        if (!IsHost)
        {
            LoadRaceListServerRpc();
        }
    }

    void ShowRace()
    {
        int raceID = baseRacesList.value;

        if (raceID == 0)
        {
            information.text = "";
            return;
        }

        if (IsHost)
        {
            string newInfo = races[raceID - 1].raceName + "\n\n" + races[raceID - 1].raceDescription;
            information.text = newInfo;

            List<string> subraceOptions = new List<string>();
            subraceOptions.Add("Base");
            foreach (Subrace subrace in races[raceID - 1].subraces.list)
            {
                subraceOptions.Add(subrace.subraceName);
            }

            subraceList.ClearOptions();
            subraceList.AddOptions(subraceOptions);
        }
        else
        {
            ShowRaceDescriptionServerRpc(raceID);
        }
        
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
        
        if (IsHost)
        {
            if (subraceID == 0)
            {
                information.text = races[raceID - 1].raceName + "\n\n" + races[raceID - 1].raceDescription;
            }
            else
            {
                information.text = races[raceID - 1].subraces.list[subraceID - 1].subraceName + "\n\n" + races[raceID - 1].subraces.list[subraceID - 1].subraceDescription;
            }            
        }
        else
        {
            ShowSubraceDescriptionServerRpc(raceID, subraceID);
        }
    }

    #endregion

    #region classes

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

        if (!IsHost)
        {
            LoadClassListServerRpc();
        }
    }

    void ShowClass()
    {
        int classID = baseClassesList.value;

        if (classID == 0)
        {
            information.text = "";
            return;
        }

        if (IsHost)
        {
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
        else
        {
            ShowClassDescriptionServerRpc(classID);
        }
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
        
        if (IsHost)
        {
            string newInfo = classes[classID - 1].className + "\n\n" + classes[classID - 1].classDescription;

            if (subclassID == 0)
            {
                information.text = newInfo;
                return;
            }
            newInfo = classes[classID - 1].subclasses.list[subclassID - 1].subclassName + "\n\n" + classes[classID - 1].subclasses.list[subclassID - 1].subclassDescription;
            information.text = newInfo;
        }
        else
        {
            ShowSubclassDescriptionServerRpc(classID, subclassID);
        }
    }

    #endregion

    #region backgrounds

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

        if (!IsHost)
        {
            LoadBackgroundListServerRpc();
        }
    }

    void ShowBackground()
    {
        int backgroundID = backgroundsList.value;

        if (backgroundID == 0)
        {
            information.text = "";
            return;
        }
        
        if (IsHost)
        {
            string newInfo = backgrounds[backgroundID - 1].backgroundName + "\n\n" + backgrounds[backgroundID - 1].backgroundDescription;
            information.text = newInfo;
        }
        else
        {
            ShowBackgroundDescriptionServerRpc(backgroundID);
        }
    }

    #endregion

    #region feats

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

        if (!IsHost)
        {
            LoadFeatListServerRpc();
        }
    }

    void ShowFeat()
    {
        int featID = featList.value;

        if (featID == 0)
        {
            information.text = "";
            return;
        }
        
        if (IsHost)
        {
            string newInfo = feats[featID - 1].featName + "\n\n" + feats[featID - 1].featDescription;
            information.text = newInfo;
        }
        else
        {
            ShowFeatDescriptionServerRpc(featID);
        }
    }

    #endregion

    #region spells

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

    void ShowSpellList()
    {
        int spellLevel = spellLevelList.value;

        if (spellLevel == 0)
        {
            information.text = "";
            return;
        }

        if (IsHost)
        {
            List<string> spellOptions = new List<string>();
            spellOptions.Add("");
            foreach (Spell spell in spells[spellLevel - 1])
            {
                spellOptions.Add(spell.spellName);
            }

            spellList.ClearOptions();
            spellList.AddOptions(spellOptions);
        }
        else
        {
            ShowSpellListServerRpc(spellLevel);
        }
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
        
        if (IsHost)
        {
            string newInfo = spells[spellLevel - 1][spellID - 1].spellName + "\n\n" + spells[spellLevel - 1][spellID - 1].spellDescription;
            information.text = newInfo;
        }
        else
        {
            ShowSpellDescriptionServerRpc(spellLevel, spellID);
        }
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
        if (!IsHost) { return; }

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
        if (!IsHost) { return; }

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
        if (!IsHost) { return; }

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
        if (!IsHost) { return; }

        if (!File.Exists(Application.dataPath + "/StreamingAssets/feats.json")) { return; }

        string jsonString = File.ReadAllText(Application.dataPath + "/StreamingAssets/feats.json");
        SerializableList<Feat> savedData = JsonUtility.FromJson<SerializableList<Feat>>(jsonString);

        feats = savedData.list;

        List<string> featNames = new List<string>();
        foreach (Feat feat in feats)
        {
            featNames.Add(feat.featName);
        }
        featList.AddOptions(featNames);

        //Debug.Log("LOADED FEATS FROM " + Application.dataPath + "/StreamingAssets/feats.json");
    }

    void LoadSpellsFromJSON()
    {
        if (!IsHost) { return; }

        if (!File.Exists(Application.dataPath + "/StreamingAssets/spells.json")) { return; }

        string jsonString = File.ReadAllText(Application.dataPath + "/StreamingAssets/spells.json");
        SerializableList<List<Spell>> savedData = JsonUtility.FromJson<SerializableList<List<Spell>>>(jsonString);

        spells = savedData.list;

        //Debug.Log("LOADED SPELLS FROM " + Application.dataPath + "/StreamingAssets/spells.json");
    }

    #endregion

    #region rpc

    #region serverRpc

    #region races serverRpc

    [ServerRpc(RequireOwnership = false)]
    private void LoadRaceListServerRpc(ServerRpcParams serverRpcParams = default)
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
    private void ShowRaceDescriptionServerRpc(int raceID, ServerRpcParams serverRpcParams = default)
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

            StringContainer newInfo = new StringContainer(races[raceID - 1].raceName + "\n\n" + races[raceID - 1].raceDescription);

            List<Subrace> subraces = races[raceID - 1].subraces.list;
            int subraceCount = subraces.Count;
            StringContainer[] subraceList = new StringContainer[subraceCount];

            for (int i = 0; i < subraceCount; i++)
            {
                subraceList[i] = new StringContainer(subraces[i].subraceName);
            }

            ShowRaceDescriptionClientRpc(newInfo, subraceList, clientRpcParams);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShowSubraceDescriptionServerRpc(int raceID, int subraceID, ServerRpcParams serverRpcParams = default)
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

            StringContainer newInfo = new StringContainer("");

            if (subraceID == 0)
            {
                newInfo.SomeText = races[raceID - 1].raceName + "\n\n" + races[raceID - 1].raceDescription;
            }
            else
            {
                newInfo.SomeText = races[raceID - 1].subraces.list[subraceID - 1].subraceName + "\n\n" + races[raceID - 1].subraces.list[subraceID - 1].subraceDescription;
            }

            ShowSubraceDescriptionClientRpc(newInfo, clientRpcParams);
        }
    }

    #endregion

    #region classes serverRpc

    [ServerRpc(RequireOwnership = false)]
    private void LoadClassListServerRpc(ServerRpcParams serverRpcParams = default)
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

            int classCount = classes.Count;

            StringContainer[] classList = new StringContainer[classCount];

            for (int i = 0; i < classCount; i++)
            {
                classList[i] = new StringContainer(classes[i].className);
            }

            LoadClassListClientRpc(classList, clientRpcParams);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShowClassDescriptionServerRpc(int classID, ServerRpcParams serverRpcParams = default)
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

            StringContainer newInfo = new StringContainer(classes[classID - 1].className + "\n\n" + classes[classID - 1].classDescription);

            List<Subclass> subclasses = classes[classID - 1].subclasses.list;
            int subclassCount = subclasses.Count;
            StringContainer[] subclassList = new StringContainer[subclassCount];

            for (int i = 0; i < subclassCount; i++)
            {
                subclassList[i] = new StringContainer(subclasses[i].subclassName);
            }

            ShowClassDescriptionClientRpc(newInfo, subclassList, clientRpcParams);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShowSubclassDescriptionServerRpc(int classID, int subclassID, ServerRpcParams serverRpcParams = default)
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

            StringContainer newInfo = new StringContainer("");

            if (subclassID == 0)
            {
                newInfo.SomeText = classes[classID - 1].className + "\n\n" + classes[classID - 1].classDescription;
            }
            else
            {
                newInfo.SomeText = classes[classID - 1].subclasses.list[subclassID - 1].subclassName + "\n\n" + classes[classID - 1].subclasses.list[subclassID - 1].subclassDescription;
            }

            ShowSubclassDescriptionClientRpc(newInfo, clientRpcParams);
        }
    }

    #endregion

    #region backgrounds serverRpc

    [ServerRpc(RequireOwnership = false)]
    private void LoadBackgroundListServerRpc(ServerRpcParams serverRpcParams = default)
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

            int backgroundCount = backgrounds.Count;

            StringContainer[] backgroundList = new StringContainer[backgroundCount];

            for (int i = 0; i < backgroundCount; i++)
            {
                backgroundList[i] = new StringContainer(backgrounds[i].backgroundName);
            }

            LoadBackgroundListClientRpc(backgroundList, clientRpcParams);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShowBackgroundDescriptionServerRpc(int backgroundID, ServerRpcParams serverRpcParams = default)
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

            StringContainer newInfo = new StringContainer(backgrounds[backgroundID - 1].backgroundName + "\n\n" + backgrounds[backgroundID - 1].backgroundDescription);

            ShowBackgroundDescriptionClientRpc(newInfo, clientRpcParams);
        }
    }

    #endregion

    #region feats serverRpc

    [ServerRpc(RequireOwnership = false)]
    private void LoadFeatListServerRpc(ServerRpcParams serverRpcParams = default)
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

            int featCount = backgrounds.Count;

            StringContainer[] featList = new StringContainer[featCount];

            for (int i = 0; i < featCount; i++)
            {
                featList[i] = new StringContainer(feats[i].featName);
            }

            LoadFeatListClientRpc(featList, clientRpcParams);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShowFeatDescriptionServerRpc(int featID, ServerRpcParams serverRpcParams = default)
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

            StringContainer newInfo = new StringContainer(feats[featID - 1].featName + "\n\n" + feats[featID - 1].featDescription);

            ShowFeatDescriptionClientRpc(newInfo, clientRpcParams);
        }
    }

    #endregion

    #region spells serverRpc


    [ServerRpc(RequireOwnership = false)]
    private void ShowSpellListServerRpc(int spellLevel, ServerRpcParams serverRpcParams = default)
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

            List<Spell> spellList = spells[spellLevel - 1];
            int spellCount = spellList.Count;
            StringContainer[] spellNames = new StringContainer[spellCount];

            for (int i = 0; i < spellCount; i++)
            {
                spellNames[i] = new StringContainer(spellList[i].spellName);
            }

            ShowSpellListClientRpc(spellNames, clientRpcParams);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShowSpellDescriptionServerRpc(int spellLevel, int spellID, ServerRpcParams serverRpcParams = default)
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

            StringContainer newInfo = new StringContainer(spells[spellLevel - 1][spellID - 1].spellName + "\n\n" + spells[spellLevel - 1][spellID - 1].spellDescription);   

            ShowSpellDescriptionClientRpc(newInfo, clientRpcParams);
        }
    }

    #endregion

    #endregion

    #region clientRpc

    #region races clientRpc

    [ClientRpc]
    private void LoadRaceListClientRpc(StringContainer[] raceList, ClientRpcParams clientRpcParams)
    {

        if (IsOwner) { return; }

        List<string> raceOptions = new List<string>();

        foreach (StringContainer raceName in raceList)
        {
            raceOptions.Add(raceName.SomeText);
        }

        baseRacesList.AddOptions(raceOptions);
    }

    [ClientRpc]
    private void ShowRaceDescriptionClientRpc(StringContainer newInfo, StringContainer[] subraces, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        information.text = newInfo.SomeText;

        List<string> subraceOptions = new List<string>();
        subraceOptions.Add("Base");
        foreach (StringContainer container in subraces)
        {
            subraceOptions.Add(container.SomeText);
        }

        subraceList.ClearOptions();
        subraceList.AddOptions(subraceOptions);
    }

    [ClientRpc]
    private void ShowSubraceDescriptionClientRpc(StringContainer newInfo, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        information.text = newInfo.SomeText;
    }

    #endregion

    #region classes clientRpc

    [ClientRpc]
    private void LoadClassListClientRpc(StringContainer[] classList, ClientRpcParams clientRpcParams)
    {

        if (IsOwner) { return; }

        List<string> classOptions = new List<string>();

        foreach (StringContainer className in classList)
        {
            classOptions.Add(className.SomeText);
        }

        baseClassesList.AddOptions(classOptions);
    }
   
    [ClientRpc]
    private void ShowClassDescriptionClientRpc(StringContainer newInfo, StringContainer[] subclasses, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        information.text = newInfo.SomeText;

        List<string> subclassOptions = new List<string>();
        subclassOptions.Add("Base");
        foreach (StringContainer container in subclasses)
        {
            subclassOptions.Add(container.SomeText);
        }

        subclassesList.ClearOptions();
        subclassesList.AddOptions(subclassOptions);
    }

    [ClientRpc]
    private void ShowSubclassDescriptionClientRpc(StringContainer newInfo, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        information.text = newInfo.SomeText;
    }

    #endregion

    #region backgrounds clientRpc

    [ClientRpc]
    private void LoadBackgroundListClientRpc(StringContainer[] backgroundList, ClientRpcParams clientRpcParams)
    {

        if (IsOwner) { return; }

        List<string> backgroundOptions = new List<string>();

        foreach (StringContainer backgroundName in backgroundList)
        {
            backgroundOptions.Add(backgroundName.SomeText);
        }

        backgroundsList.AddOptions(backgroundOptions);
    }

    [ClientRpc]
    private void ShowBackgroundDescriptionClientRpc(StringContainer newInfo, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        information.text = newInfo.SomeText;
    }

    #endregion

    #region feats clientRpc

    [ClientRpc]
    private void LoadFeatListClientRpc(StringContainer[] featNameList, ClientRpcParams clientRpcParams)
    {

        if (IsOwner) { return; }

        List<string> featOptions = new List<string>();

        foreach (StringContainer featName in featNameList)
        {
            featOptions.Add(featName.SomeText);
        }

        featList.AddOptions(featOptions);
    }

    [ClientRpc]
    private void ShowFeatDescriptionClientRpc(StringContainer newInfo, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        information.text = newInfo.SomeText;
    }

    #endregion

    #region spells clientRpc

    [ClientRpc]
    private void ShowSpellListClientRpc(StringContainer[] spells, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        List<string> spellOptions = new List<string>();
        spellOptions.Add("");
        foreach (StringContainer container in spells)
        {
            spellOptions.Add(container.SomeText);
        }

        spellList.ClearOptions();
        spellList.AddOptions(spellOptions);
    }

    [ClientRpc]
    private void ShowSpellDescriptionClientRpc(StringContainer newInfo, ClientRpcParams clientRpcParams)
    {
        if (IsOwner) { return; }

        information.text = newInfo.SomeText;
    }

    #endregion

    #endregion

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
    public int spellLevel;
}

#endregion