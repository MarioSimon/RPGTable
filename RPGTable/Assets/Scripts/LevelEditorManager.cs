using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelEditorManager : NetworkBehaviour
{
    public LevelItemSpawner[] itemButtons;
    public GameObject[] itemPrefabs;
    public GameObject[] itemPreviews;
    public int currentButtonPressed;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && itemButtons[currentButtonPressed].clicked)
        {
            Vector3 worldPosition = GetTablePoint();
            if (worldPosition == new Vector3(-99, -99, -99)) { return; }

            itemButtons[currentButtonPressed].clicked = false;
            GameObject item = Instantiate(itemPrefabs[currentButtonPressed], worldPosition, Quaternion.identity);
            item.GetComponent<NetworkObject>().Spawn();
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
}
