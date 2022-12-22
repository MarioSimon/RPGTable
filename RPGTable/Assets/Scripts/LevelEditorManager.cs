using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelEditorManager : NetworkBehaviour
{
    [SerializeField] GameManager gameManager;

    public LevelItemSpawner[] itemButtons;
    public GameObject[] itemPrefabs;
    public GameObject[] itemPreviews;
    public int currentButtonPressed;

    [SerializeField] List<List<LevelItemInfo>> previousStates;
    const int MAX_SAVED_STATES = 20;

    private void Start()
    {
        previousStates = new List<List<LevelItemInfo>>();
    }

    void Update()
    {
        if (!IsServer) { return; }

        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Z))
        {
            LoadPreviousState();
        }

        if (Input.GetMouseButtonDown(0) && itemButtons[currentButtonPressed].clicked)
        {
            SaveState();

            Vector3 worldPosition = GetTablePoint();
            if (worldPosition == new Vector3(-99, -99, -99)) { return; }

            itemButtons[currentButtonPressed].clicked = false;
            GameObject item = Instantiate(itemPrefabs[currentButtonPressed], worldPosition, Quaternion.identity);
            item.GetComponent<LevelItemHandler>().id = currentButtonPressed;
            item.GetComponent<NetworkObject>().Spawn();
            gameManager.currentLevel.Add(item);
            Destroy(GameObject.FindGameObjectWithTag("ItemImage"));
        }
    }

    Vector3 GetTablePoint()
    {
        Ray worldPositionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPosition = new Vector3(-99, -99, -99);

        RaycastHit[] hits = Physics.RaycastAll(worldPositionRay);

        foreach (RaycastHit hit in hits)
        {
            WalkableZone target = hit.transform.GetComponent<WalkableZone>();
            if (target == null) continue;

            worldPosition = hit.point;
            break;
        }
        return worldPosition;
    }

    public void SpawnLevelItem(LevelItemInfo itemInfo)
    {
        GameObject item = Instantiate(itemPrefabs[itemInfo.itemID],itemInfo.itemPosition, Quaternion.Euler(itemInfo.itemRotation));
        item.transform.localScale = itemInfo.itemScale;
        item.GetComponent<LevelItemHandler>().id = itemInfo.itemID;
        item.GetComponent<NetworkObject>().Spawn();
        gameManager.currentLevel.Add(item);
    }

    public void SaveState()
    {
        List<LevelItemInfo> state = gameManager.GetLevelState();

        if (previousStates.Count >= MAX_SAVED_STATES)
        {
            previousStates.RemoveAt(0);
            previousStates.Add(state);
        }
        else
        {
            previousStates.Add(state);
        }
    }

    public void LoadPreviousState()
    {
        if (previousStates.Count < 1) { return; }
        List<LevelItemInfo> state = previousStates[previousStates.Count - 1];
        previousStates.RemoveAt(previousStates.Count - 1);

        gameManager.LoadLevelState(state);
    }
}

[Serializable]
public struct LevelItemInfo
{
    public int itemID;
    public Vector3 itemPosition;
    public Vector3 itemRotation;
    public Vector3 itemScale;
}
