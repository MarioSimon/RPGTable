using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    UIManager uIManager;
    GameManager gameManager;
    Canvas canvas;
    
    [SerializeField] GameObject characterSheetPrefab;
    [SerializeField] RightClickButton rightClickButton;

    public int charID;

    [SerializeField] Button selectCharacter;
    public Image characterPortrait;
    public Text characterName;

    [SerializeField] GameObject contextMenu;

    void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
        canvas = FindObjectOfType<Canvas>();

        selectCharacter.onClick.AddListener(delegate { OpenCharacterSheet(); uIManager.ToggleCharacterSelector(); });
        rightClickButton.rightClick.AddListener(() => OpenContextMenu());

        ContextMenu contextMenuHandler = contextMenu.GetComponent<ContextMenu>();
        //contextMenuHandler.buttonList[0].onClick.AddListener(() => RemoveCharacterOwnership());
        contextMenuHandler.buttonList[1].onClick.AddListener(() => DeleteCharacter());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CloseContextMenu();
        }
    }

    private void CloseContextMenu()
    {
        contextMenu.SetActive(false);
    }

    private void OpenContextMenu()
    {
        contextMenu.SetActive(true);      
    }

    private void DeleteCharacter()
    {
        contextMenu.SetActive(false);

        gameManager.DeleteCharacter(charID);
    }

    void OpenCharacterSheet()
    {
        CharacterSheetInfo CSInfo = gameManager.GetSheetInfo(charID);
        if (CSInfo == null) { return; }
        GameObject charSheet = Instantiate(characterSheetPrefab);
        charSheet.GetComponent<CharacterSheetManager>().CSInfo = CSInfo;
        charSheet.GetComponent<RectTransform>().SetParent(canvas.gameObject.transform, false);
    }

}
