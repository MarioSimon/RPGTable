using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeHandle;
using Unity.Netcode;

public class LevelItemHandler : NetworkBehaviour
{
    RuntimeTransformHandle transformHandle;
    Canvas canvas;
    [SerializeField] GameObject itemMenu;
    GameObject itemMenuInstance;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        transformHandle = GetComponent<RuntimeTransformHandle>();
    }

    private void Update()
    {
        if (!IsServer) { return; }

        if (Input.GetMouseButtonDown(1))
        {
            InteractWithSelection();
        }

        if (Input.GetMouseButtonDown(2))
        {
            //transformHandle.
            transformHandle.enabled = false;
        }
    }

    public void SetPositionHandler()
    {
        transformHandle.enabled = true;
        transformHandle.type = HandleType.POSITION;
        if (itemMenuInstance != null)
        {
            Destroy(itemMenuInstance);
        }
    }

    public void SetRotationHandler()
    {
        transformHandle.enabled = true;
        transformHandle.type = HandleType.ROTATION;
        if (itemMenuInstance != null)
        {
            Destroy(itemMenuInstance);
        }
    }

    public void SetScaleHandler()
    {
        transformHandle.enabled = true;
        transformHandle.type = HandleType.SCALE;
        if (itemMenuInstance != null)
        {
            Destroy(itemMenuInstance);
        }
    }

    public void DestroyLevelItem()
    {
        this.gameObject.GetComponent<NetworkObject>().Despawn();
        if (itemMenuInstance != null)
        {
            Destroy(itemMenuInstance);
        }
    }

    public void OpenItemMenu()
    {
        if (itemMenuInstance != null)
        {
            Destroy(itemMenuInstance);
        }

        GameObject contextMenu = Instantiate(itemMenu, Input.mousePosition, Quaternion.identity);
        contextMenu.GetComponent<RectTransform>().SetParent(canvas.transform);

        ContextMenu contextMenuHandler = contextMenu.GetComponent<ContextMenu>();
        contextMenuHandler.buttonList[0].onClick.AddListener(() => SetPositionHandler());
        contextMenuHandler.buttonList[1].onClick.AddListener(() => SetRotationHandler());
        contextMenuHandler.buttonList[2].onClick.AddListener(() => SetScaleHandler());
        contextMenuHandler.buttonList[3].onClick.AddListener(() => DestroyLevelItem());

        itemMenuInstance = contextMenu;
    }


    void InteractWithSelection()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);



        if (hasHit)
        {
            GameObject target = hit.transform.gameObject;
            
            if (target == null || target != this.transform.GetChild(0).gameObject)
            {
                if (itemMenuInstance != null)
                {
                    Destroy(itemMenuInstance);
                }
            
                return;
            }

            OpenItemMenu();

            
        }
    }
}
