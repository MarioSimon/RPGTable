using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSheetManager : MonoBehaviour
{
    #region Variables

    [SerializeField] Button closeNPCSheet;
    [SerializeField] Button spawnNPCToken;

    public NPCSheetInfo NPCInfo;

    [Header("Stats")]
    [SerializeField] GameObject statsPage;
    [SerializeField] InputField NPCName;
    [SerializeField] Dropdown NPCSize;
    [SerializeField] Dropdown NPCType;
    [SerializeField] Dropdown NPCAlignement;

    [SerializeField] InputField NPCArmorClass;
    [SerializeField] InputField NPCHitPoints;
    [SerializeField] InputField NPCSpeed;

    [SerializeField] InputField NPCStrScore;
    [SerializeField] Text NPCStrModifier;
    [SerializeField] Button NPCStrCheck;
    [SerializeField] Button NPCStrSave;
    [SerializeField] InputField NPCDexScore;
    [SerializeField] Text NPCDexModifier;
    [SerializeField] Button NPCDexCheck;
    [SerializeField] Button NPCDexSave;
    [SerializeField] InputField NPCConScore;
    [SerializeField] Text NPCConModifier;
    [SerializeField] Button NPCConCheck;
    [SerializeField] Button NPCConSave;
    [SerializeField] InputField NPCIntScore;
    [SerializeField] Text NPCIntModifier;
    [SerializeField] Button NPCIntCheck;
    [SerializeField] Button NPCIntSave;
    [SerializeField] InputField NPCWisScore;
    [SerializeField] Text NPCWisModifier;
    [SerializeField] Button NPCWisCheck;
    [SerializeField] Button NPCWisSave;
    [SerializeField] InputField NPCChaScore;
    [SerializeField] Text NPCChaModifier;
    [SerializeField] Button NPCChaCheck;
    [SerializeField] Button NPCChaSave;

    [SerializeField] InputField NPCSkills;
    [SerializeField] InputField NPCDamageVulnerabilites;
    [SerializeField] InputField NPCDamageResistances;
    [SerializeField] InputField NPCDamageInmunities;
    [SerializeField] InputField NPCConditionInmunities;
    [SerializeField] InputField NPCSenses;
    [SerializeField] InputField NPCLanguages;
    [SerializeField] InputField NPCChallenge;

    [SerializeField] Button openCombatPage;

    [Header("Combat")]
    [SerializeField] GameObject combatPage;
    [SerializeField] GameObject traitsParent;
    [SerializeField] GameObject traitPrefab;
    [SerializeField] Button addTrait;
    [SerializeField] GameObject actionsParent;
    [SerializeField] GameObject actionPrefab;
    [SerializeField] Button addAction;

    [SerializeField] Button openStatsPage;
    #endregion
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
