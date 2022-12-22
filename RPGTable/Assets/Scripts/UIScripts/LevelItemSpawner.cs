using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelItemSpawner : MonoBehaviour
{
    LevelEditorManager levelEditorManager;
    [SerializeField] Button spawnItem;

    public int itemID;
    public bool clicked = false;

    void Start()
    {
        levelEditorManager = FindObjectOfType<LevelEditorManager>();

        spawnItem.onClick.AddListener(() => ButtonClicked());
    }

    void ButtonClicked()
    {
        Instantiate(levelEditorManager.itemPreviews[itemID], new Vector3(-99, -99, -99), Quaternion.identity);

        clicked = true;
        levelEditorManager.currentButtonPressed = itemID;
    }

}
