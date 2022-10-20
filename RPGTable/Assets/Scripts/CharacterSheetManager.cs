using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class CharacterSheetManager : NetworkBehaviour
{
    #region Variables
    Canvas canvas;

    [Header("NavBar")]
    [SerializeField] Button buttonPublicInfo;
    [SerializeField] Button buttonBasicInfo;
    [SerializeField] Button buttonSkills;
    [SerializeField] Button buttonFeatures;
    [SerializeField] Button buttonInventory;
    [SerializeField] Button buttonSpells;
    [SerializeField] Button buttonActions;
    [SerializeField] Button buttonPersonality;

    [Header("Public Info")]
    [SerializeField] GameObject publicInfo;

    [Header("Basic Info")]
    [SerializeField] GameObject basicInfo;

    [Header("Skills")]
    [SerializeField] GameObject skills;

    [Header("Features")]
    [SerializeField] GameObject features;

    [Header("Inventory")]
    [SerializeField] GameObject inventory;

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
        currentPage = publicInfo;

        buttonPublicInfo.onClick.AddListener(() => openPublicInfoPage());
        buttonBasicInfo.onClick.AddListener(() => openBasicInfoPage());
        buttonSkills.onClick.AddListener(() => openSkillsPage());
        buttonFeatures.onClick.AddListener(() => openFeaturesPage());
        buttonInventory.onClick.AddListener(() => openInventoryPage());
        buttonSpells.onClick.AddListener(() => openSpellsPage());
        buttonActions.onClick.AddListener(() => openActionsPage());
        buttonPersonality.onClick.AddListener(() => openPersonalityPage());
    }

    #region Navigation Methods

    void openPublicInfoPage()
    {
        currentPage.SetActive(false);
        publicInfo.SetActive(true);
        currentPage = publicInfo;
    }

    void openBasicInfoPage()
    {
        currentPage.SetActive(false);
        basicInfo.SetActive(true);
        currentPage = basicInfo;
    }

    void openSkillsPage()
    {
        currentPage.SetActive(false);
        skills.SetActive(true);
        currentPage = skills;
    }

    void openFeaturesPage()
    {
        currentPage.SetActive(false);
        features.SetActive(true);
        currentPage = features;
    }

    void openInventoryPage()
    {
        currentPage.SetActive(false);
        inventory.SetActive(true);
        currentPage = inventory;
    }

    void openSpellsPage()
    {
        currentPage.SetActive(false);
        spells.SetActive(true);
        currentPage = spells;
    }

    void openActionsPage()
    {
        currentPage.SetActive(false);
        actions.SetActive(true);
        currentPage = actions;
    }
    void openPersonalityPage()
    {
        currentPage.SetActive(false);
        personailty.SetActive(true);
        currentPage = personailty;
    }

    #endregion

    #region Logic Methods



    #endregion
}
