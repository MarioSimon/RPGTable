using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCActionInfo : MonoBehaviour
{
    #region variables

    Canvas canvas;
    DiceHandler diceHandler;

    public NPCSheetManager npcSheetManager;

    public GameObject actionItem;

    public InputField actionName;
    public Dropdown actionType;    
    public Button rollAction;
    public Button rollDamage;
    public Button openConfiguration;

    public int attackMod;
   
    public int damage1NumDices;
    public DiceType damage1Dice;
    public int damage1FlatDamage;
    public string damage1Type;

    public int damage2NumDices;
    public DiceType damage2Dice;
    public int damage2FlatDamage;
    public string damage2Type;
 
    public int saveDC;

    public GameObject weaponAttackConfiguration;
    public Button closeWeaponAttackPanel;
    public InputField wpnAttackModifier;
    public InputField wpnDamage1NumberOfDices;
    public Dropdown wpnDamage1DiceType;
    public InputField wpnDamage1OtherBonus;
    public Dropdown wpnDamage1DamageType;
    public InputField wpnDamage2NumberOfDices;
    public Dropdown wpnDamage2DiceType;
    public InputField wpnDamage2OtherBonus;
    public Dropdown wpnDamage2DamageType;

    #endregion

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        diceHandler = FindObjectOfType<DiceHandler>();

        rollAction.onClick.AddListener(() => RollAttack());
        rollDamage.onClick.AddListener(() => RollDamage());
        openConfiguration.onClick.AddListener(() => ShowConfigurationPanel());
        closeWeaponAttackPanel.onClick.AddListener(() => CloseWeaponConfigurationPanel());

        wpnAttackModifier.onValueChanged.AddListener(delegate { SetAttackModifier(wpnAttackModifier); });

        wpnDamage1NumberOfDices.onValueChanged.AddListener(delegate { SetDamage1NDices(wpnDamage1NumberOfDices); });
        wpnDamage1DiceType.onValueChanged.AddListener(delegate { SetDamage1Dice(wpnDamage1DiceType); });
        wpnDamage1OtherBonus.onValueChanged.AddListener(delegate { SetDamage1FlatDamage(wpnDamage1OtherBonus); });
        wpnDamage1DamageType.onValueChanged.AddListener(delegate { SetDamage1Type(wpnDamage1DamageType); });

        wpnDamage2NumberOfDices.onValueChanged.AddListener(delegate { SetDamage2NDices(wpnDamage2NumberOfDices); });
        wpnDamage2DiceType.onValueChanged.AddListener(delegate { SetDamage2Dice(wpnDamage2DiceType); });
        wpnDamage2OtherBonus.onValueChanged.AddListener(delegate { SetDamage2FlatDamage(wpnDamage2OtherBonus); });
        wpnDamage2DamageType.onValueChanged.AddListener(delegate { SetDamage2Type(wpnDamage2DamageType); });

        weaponAttackConfiguration.GetComponent<RectTransform>().SetParent(canvas.transform);
        weaponAttackConfiguration.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    public void SetActionConfig()
    {
        SetAttackModifier(wpnAttackModifier);

        SetDamage1NDices(wpnDamage1NumberOfDices);
        SetDamage1Dice(wpnDamage1DiceType);
        SetDamage1FlatDamage(wpnDamage1OtherBonus);
        SetDamage1Type(wpnDamage1DamageType);

        SetDamage2NDices(wpnDamage2NumberOfDices);
        SetDamage2Dice(wpnDamage2DiceType);
        SetDamage2FlatDamage(wpnDamage2OtherBonus);
        SetDamage2Type(wpnDamage2DamageType);
    }

    private void RollAttack()
    {
        if (actionType.value > 1) { return; }

        AttackRollInfo attackRollInfo = new AttackRollInfo();

        if (npcSheetManager != null)
        {
            attackRollInfo.sheetID = npcSheetManager.NPCInfo.sheetID;
            attackRollInfo.characterName = npcSheetManager.NPCInfo.NPCName;
        }

        attackRollInfo.actionName = actionName.text;

        attackRollInfo.toHitModifier = attackMod;

        attackRollInfo.damage1NumberOfDices = damage1NumDices;
        attackRollInfo.damage1Dice = damage1Dice;
        attackRollInfo.damage1Modifier = damage1FlatDamage;
        attackRollInfo.damage1Type = damage1Type;

        attackRollInfo.damage2NumberOfDices = damage2NumDices;
        attackRollInfo.damage2Dice = damage2Dice;
        attackRollInfo.damage2Modifier = damage2FlatDamage;
        attackRollInfo.damage2Type = damage2Type;

        diceHandler.RollAttackAction(attackRollInfo);
    }

    private void RollDamage()
    {
        AttackRollInfo attackRollInfo = new AttackRollInfo();

        if (npcSheetManager != null)
        {
            attackRollInfo.sheetID = npcSheetManager.NPCInfo.sheetID;
            attackRollInfo.characterName = npcSheetManager.NPCInfo.NPCName;
        }

        attackRollInfo.actionName = actionName.text;

        attackRollInfo.toHitModifier = attackMod;

        attackRollInfo.damage1NumberOfDices = damage1NumDices;
        attackRollInfo.damage1Dice = damage1Dice;
        attackRollInfo.damage1Modifier = damage1FlatDamage;
        attackRollInfo.damage1Type = damage1Type;

        attackRollInfo.damage2NumberOfDices = damage2NumDices;
        attackRollInfo.damage2Dice = damage2Dice;
        attackRollInfo.damage2Modifier = damage2FlatDamage;
        attackRollInfo.damage2Type = damage2Type;

        diceHandler.RollActionDamage(attackRollInfo);
    }

    private void ShowConfigurationPanel()
    {
        switch (actionType.value)
        {
            case 0:
                weaponAttackConfiguration.SetActive(true);
                break;
            case 1:
                weaponAttackConfiguration.SetActive(true);
                break;
            case 2:
                //weaponAttackConfiguration.SetActive(true);
                break;
            case 3:
                //weaponAttackConfiguration.SetActive(true);
                break;
        }
    }

    private void CloseWeaponConfigurationPanel()
    {
        weaponAttackConfiguration.SetActive(false);
    }

    private void SetAttackModifier(InputField attackBonus)
    {
        attackMod = int.Parse(attackBonus.text);
    }

    private void SetDamage1NDices(InputField diceNumber)
    {
        if (diceNumber.text == null || diceNumber.text == "")
        {
            diceNumber.text = "0";
            damage1NumDices = 0;
        }
        else
        {
            damage1NumDices = int.Parse(diceNumber.text);
        }
    }

    private void SetDamage1Dice(Dropdown diceType)
    {
        switch (diceType.value)
        {
            case 0:
                damage1Dice = DiceType.d4;
                break;
            case 1:
                damage1Dice = DiceType.d6;
                break;
            case 2:
                damage1Dice = DiceType.d8;
                break;
            case 3:
                damage1Dice = DiceType.d10;
                break;
            case 4:
                damage1Dice = DiceType.d12;
                break;
        }
    }

    private void SetDamage1FlatDamage(InputField damageBonus)
    {
        if (damageBonus.text == null || damageBonus.text == "")
        {
            damageBonus.text = "0";
            damage1FlatDamage = 0;
        }
        else
        {
            damage1FlatDamage = int.Parse(damageBonus.text);
        }
    }

    private void SetDamage1Type(Dropdown damageType)
    {
        switch (damageType.value)
        {
            case 0:
                damage1Type = "slashing";
                break;
            case 1:
                damage1Type = "piercing";
                break;
            case 2:
                damage1Type = "bludgeoning";
                break;
            case 3:
                damage1Type = "fire";
                break;
            case 4:
                damage1Type = "cold";
                break;
            case 5:
                damage1Type = "shock";
                break;
            case 6:
                damage1Type = "thunder";
                break;
            case 7:
                damage1Type = "psychic";
                break;
            case 8:
                damage1Type = "radiant";
                break;
            case 9:
                damage1Type = "necrotic";
                break;
            case 10:
                damage1Type = "poison";
                break;
            case 11:
                damage1Type = "force";
                break;
        }
    }

    private void SetDamage2NDices(InputField diceNumber)
    {
        if (diceNumber.text == null || diceNumber.text == "")
        {
            diceNumber.text = "0";
            damage2NumDices = 0;
        }
        else
        {
            damage2NumDices = int.Parse(diceNumber.text);
        }
    }

    private void SetDamage2Dice(Dropdown diceType)
    {
        switch (diceType.value)
        {
            case 0:
                damage2Dice = DiceType.d4;
                break;
            case 1:
                damage2Dice = DiceType.d6;
                break;
            case 2:
                damage2Dice = DiceType.d8;
                break;
            case 3:
                damage2Dice = DiceType.d10;
                break;
            case 4:
                damage2Dice = DiceType.d12;
                break;
        }
    }

    private void SetDamage2FlatDamage(InputField damageBonus)
    {
        if (damageBonus.text == null || damageBonus.text == "")
        {
            damageBonus.text = "0";
            damage2FlatDamage = 0;
        }
        else
        {
            damage2FlatDamage = int.Parse(damageBonus.text);
        }
    }

    private void SetDamage2Type(Dropdown damageType)
    {
        switch (damageType.value)
        {
            case 0:
                damage2Type = "slashing";
                break;
            case 1:
                damage2Type = "piercing";
                break;
            case 2:
                damage2Type = "bludgeoning";
                break;
            case 3:
                damage2Type = "fire";
                break;
            case 4:
                damage2Type = "cold";
                break;
            case 5:
                damage2Type = "shock";
                break;
            case 6:
                damage2Type = "thunder";
                break;
            case 7:
                damage2Type = "psychic";
                break;
            case 8:
                damage2Type = "radiant";
                break;
            case 9:
                damage2Type = "necrotic";
                break;
            case 10:
                damage2Type = "poison";
                break;
            case 11:
                damage2Type = "force";
                break;
        }
    }

}
