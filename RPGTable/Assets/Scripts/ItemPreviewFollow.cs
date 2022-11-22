using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPreviewFollow : MonoBehaviour
{
    void Update()
    {
        Vector3 worldPosition = GetTablePoint();
        transform.position = worldPosition;
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
