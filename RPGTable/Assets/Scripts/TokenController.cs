using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;


public class TokenController : NetworkBehaviour
{
    [SerializeField] string tokenName;
    [SerializeField] bool selected = false;

    public NetworkVariable<FixedString64Bytes> ownerName;

    Color selectedColor = new Color(0, 0.75f, 0);
    Color standardColor = new Color(0.5f, 0.5f, 0.5f);


    public void ChangeSelection()
    {
        selected = !selected;
        ChangeColor(selected);
    }

    public void ChangeColor(bool selected)
    {
        if (selected)
        {
            GetComponent<Renderer>().material.color = selectedColor;
        }
        else
        {
            GetComponent<Renderer>().material.color = standardColor;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void MoveToServerRpc(Vector3 destination)
    {
        GetComponent<Mover>().MoveTo(destination);
    }
}
