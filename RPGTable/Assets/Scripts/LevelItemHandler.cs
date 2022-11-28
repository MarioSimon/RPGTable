using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeHandle;

public class LevelItemHandler : MonoBehaviour
{
    RuntimeTransformHandle transformHandle;
    void Start()
    {
        transformHandle = GetComponent<RuntimeTransformHandle>();
    }


}
