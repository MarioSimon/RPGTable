using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine.AI;


public class TokenController : NetworkBehaviour
{
    #region Variables

    bool selected = false;
    NavMeshAgent navMeshAgent;

    public NetworkVariable<FixedString64Bytes> tokenName;
    public NetworkVariable<FixedString64Bytes> ownerName;

    Color selectedColor = new Color(0, 0.75f, 0);
    Color standardColor = new Color(0.5f, 0.5f, 0.5f);

    #endregion

    #region Unity Event Functions

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    #endregion

    #region Selection

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

    #endregion

    #region Movement and Rotation

    public void MoveTo(Vector3 destination)
    {
        navMeshAgent.Warp(destination);
    }
    
    #endregion
    
    #region ServerRpc

    [ServerRpc(RequireOwnership = false)]
    public void MoveToServerRpc(Vector3 destination)
    {
        MoveTo(destination);
    }

    #endregion
}
