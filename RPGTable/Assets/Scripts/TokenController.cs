using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class TokenController : NetworkBehaviour
{

    bool selected = false;

    void Update()
    {
        InteractWithMovement();
    }

    private void InteractWithMovement()
    {
        if ((!IsOwner) && (!IsHost)) { return; }
        RaycastHit hit;
        bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

        if (hasHit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MoveTokenServerRpc(hit.point);
            }
        }
    }

    private Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    [ServerRpc]
    private void MoveTokenServerRpc(Vector3 destination)
    {
        GetComponent<Mover>().MoveTo(destination);
    }
}
