using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaterTrait : MonoBehaviour
{
    [SerializeField] Button removeTrait;
    [SerializeField] InputField traitName;
    [SerializeField] Text traitDescription;
    [SerializeField] InputField traitDescriptionInput;

    private float lastSize;
    public Action repositionListener;

    // Start is called before the first frame update
    void Start()
    {
        lastSize = traitDescription.GetComponent<LayoutElement>().minHeight;

        traitDescriptionInput.onValueChanged.AddListener(delegate { UpdateDescriptionAndSize(); });

    }

    private void UpdateDescriptionAndSize()
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
}
