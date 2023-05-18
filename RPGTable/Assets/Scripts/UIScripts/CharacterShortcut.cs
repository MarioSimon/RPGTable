using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterShortcut : MonoBehaviour
{
    UIManager uIManager;
    GameManager gameManager;
    Canvas canvas;

    [SerializeField] GameObject characterSheetPrefab;
    public int charID;

    [SerializeField] Button selectCharacter;
    public Image characterPortrait;
    public TMP_Text characterName;

    void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
        canvas = FindObjectOfType<Canvas>();

        selectCharacter.onClick.AddListener(delegate { OpenCharacterSheet(); });
    }

    void OpenCharacterSheet()
    {
        CharacterSheetInfo CSInfo = gameManager.GetSheetInfo(charID);
        if (CSInfo == null) { return; }
        GameObject charSheet = Instantiate(characterSheetPrefab);
        charSheet.GetComponent<CharacterSheetManager>().CSInfo = CSInfo;
        charSheet.GetComponent<RectTransform>().SetParent(canvas.transform, false);
    }
}
