using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DiceScoreCheck : MonoBehaviour
{
    [SerializeField] UIManager UIManager;

    private void OnTriggerStay(Collider col)
    {
        StartCoroutine(ResetDice(col));
    }

    private IEnumerator ResetDice(Collider col)
    {
        NetworkObject dice = col.transform.parent.parent.GetComponent<NetworkObject>();

        yield return new WaitForSeconds(1.5f);

        dice.Despawn();
    }
}
