using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    GameManager gameManager;
    Canvas canvas;
    
    [SerializeField] GameObject characterSelector;
    [SerializeField] GameObject characterSheetPrefab;

    public int charID;

    [SerializeField] Button selectCharacter;
    public Image characterPortrait;
    public Text characterName;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        canvas = FindObjectOfType<Canvas>();

        selectCharacter.onClick.AddListener(() => OpenCharacterSheetServerRpc());
    }

    [ServerRpc(RequireOwnership = false)]
    void OpenCharacterSheetServerRpc()
    {
        CharacterSheetInfo CSInfo = gameManager.GetSheetInfo(charID);
        if (CSInfo == null) { return; }
        GameObject charSheet = Instantiate(characterSheetPrefab);
        charSheet.GetComponent<CharacterSheetManager>().CSInfo = CSInfo;
        //charSheet.GetComponent<NetworkObject>().SpawnWithOwnership(CSInfo.ownerID);
        charSheet.GetComponent<RectTransform>().SetParent(canvas.gameObject.transform, false);
    }

}
