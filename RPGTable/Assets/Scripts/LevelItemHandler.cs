using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeHandle;
using Unity.Netcode;
using System;

public class LevelItemHandler : NetworkBehaviour
{
    LevelEditorManager levelEditorManager;
    RuntimeTransformHandle transformHandle;
    Canvas canvas;

    [SerializeField] GameObject itemMenu;
    GameObject itemMenuInstance;

    public int id;

    private void Awake()
    {
        levelEditorManager = FindObjectOfType<LevelEditorManager>();
        canvas = FindObjectOfType<Canvas>();
        transformHandle = GetComponent<RuntimeTransformHandle>();
    }

    void Start()
    {
        if (transformHandle != null)
        {
            transformHandle.Clear();
            transformHandle.enabled = false;
        }     
    }

    private void Update()
    {
        if (!IsServer) { return; }

        if (Input.GetMouseButtonDown(1))
        {
            InteractWithSelection();
        }
    }

    public void SetPositionHandler()
    {
        levelEditorManager.SaveState();
        transformHandle.enabled = true;       
        transformHandle.type = HandleType.POSITION;
        transformHandle.CreateHandles();
        if (itemMenuInstance != null)
        {
            Destroy(itemMenuInstance);
        }
    }

    public void SetRotationHandler()
    {
        levelEditorManager.SaveState();
        transformHandle.enabled = true;
        transformHandle.type = HandleType.ROTATION;
        transformHandle.CreateHandles();      
        if (itemMenuInstance != null)
        {
            Destroy(itemMenuInstance);
        }
    }

    public void SetScaleHandler()
    {
        levelEditorManager.SaveState();
        transformHandle.enabled = true;
        transformHandle.type = HandleType.SCALE;
        transformHandle.CreateHandles();
        
        if (itemMenuInstance != null)
        {
            Destroy(itemMenuInstance);
        }
    }

    public void DestroyLevelItem()
    {
        levelEditorManager.SaveState();
        transformHandle.Clear();

        if (itemMenuInstance != null)
        {
            Destroy(itemMenuInstance);
        }

        gameObject.GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
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
                transformHandle.Clear();

                transformHandle.enabled = false;
                return;
            }

            OpenItemMenu();           
        }
    }
}
