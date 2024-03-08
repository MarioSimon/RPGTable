using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterTrait : MonoBehaviour
{
    public Button removeTrait;
    public InputField traitName;
    [SerializeField] Text traitDescription;
    public InputField traitDescriptionInput;

    private float lastSize;
    public Action repositionListener;
    public Action<GameObject> removeListener;

    void Start()
    {
        lastSize = traitDescription.GetComponent<LayoutElement>().minHeight;

        traitDescriptionInput.onValueChanged.AddListener(delegate { UpdateDescriptionAndSize(); });
        removeTrait.onClick.AddListener(RemoveTrait);
    }

    public void UpdateDescriptionAndSize()
    {
        traitDescription.text = traitDescriptionInput.text;

        if (lastSize != traitDescription.GetComponent<RectTransform>().sizeDelta.y)
        {
            lastSize = traitDescription.GetComponent<RectTransform>().sizeDelta.y;
            GetComponent<RectTransform>().sizeDelta += new Vector2(0, 16);
            GetComponent<RectTransform>().parent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 16);

            repositionListener();
        }
    }

    public void Reposition(Vector3 positionDelta)
    {
        GetComponent<RectTransform>().localPosition += positionDelta;
    }

    private void RemoveTrait()
    {
        removeListener(this.gameObject);
    }
}
