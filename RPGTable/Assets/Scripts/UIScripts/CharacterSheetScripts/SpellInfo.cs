using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellInfo : MonoBehaviour
{
    Canvas canvas;

    [SerializeField] GameObject spell;
    public GameObject spellItem;
    [SerializeField] GameObject spellInfo;

    public Toggle prepared;
    public InputField spellName;
    [SerializeField] Button showSpellInfo;

    [SerializeField] Text spellInfoName;
    [SerializeField] Button closeSpellInfo;

    public Dropdown spellLevel;
    public Dropdown spellSchool;
    public InputField spellCastingTime;
    public InputField spellRange;
    public InputField spellComponents;
    public InputField spellDuration;
    public InputField spellDescription;

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();

        showSpellInfo.onClick.AddListener(() => ShowSpellInfo());
        closeSpellInfo.onClick.AddListener(() => CloseSpellInfo());

        spellName.onValueChanged.AddListener(delegate { SetSpellName(); });

        spellInfo.GetComponent<RectTransform>().SetParent(canvas.transform);
        spellInfo.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    void ShowSpellInfo()
    {
        spellInfo.SetActive(true);
    }

    void CloseSpellInfo()
    {
        spellInfo.SetActive(false);
    }

    void SetSpellName()
    {
        spellInfoName.text = spellName.text.ToUpper();
    }

    public void DestroySpell()
    {
        Destroy(spell);
    }
}
