using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionInfo : MonoBehaviour
{
    Canvas canvas;
    

    public CharacterSheetManager sheetManager;

    public GameObject actionItem;

    public InputField actionName;
    public Dropdown actionType;    
    public Button rollAction;
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
    public Dropdown weaponTemplate;
    public Dropdown wpnAttackAbility;
    public InputField wpnOtherAttackBonus;
    public Toggle wpnAttackProficency;
    public Text wpnTotalAttackModifier;
    public InputField wpnDamage1NumberOfDices;
    public Dropdown wpnDamage1DiceType;
    public Dropdown wpnDamage1Ability;
    public InputField wpnDamage1OtherBonus;
    public Dropdown wpnDamage1DamageType;
    public InputField wpnDamage2NumberOfDices;
    public Dropdown wpnDamage2DiceType;
    public Dropdown wpnDamage2Ability;
    public InputField wpnDamage2OtherBonus;
    public Dropdown wpnDamage2DamageType;


    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();

        openConfiguration.onClick.AddListener(() => ShowConfigurationPanel());
        closeWeaponAttackPanel.onClick.AddListener(() => CloseWeaponConfigurationPanel());

        wpnAttackAbility.onValueChanged.AddListener(delegate { SetAttackModifier(wpnAttackAbility, wpnOtherAttackBonus, wpnAttackProficency, wpnTotalAttackModifier); });
        wpnOtherAttackBonus.onValueChanged.AddListener(delegate { SetAttackModifier(wpnAttackAbility, wpnOtherAttackBonus, wpnAttackProficency, wpnTotalAttackModifier); });
        wpnAttackProficency.onValueChanged.AddListener(delegate { SetAttackModifier(wpnAttackAbility, wpnOtherAttackBonus, wpnAttackProficency, wpnTotalAttackModifier); });

        wpnDamage1NumberOfDices.onValueChanged.AddListener(delegate { SetDamage1NDices(wpnDamage1NumberOfDices); });
        wpnDamage1DiceType.onValueChanged.AddListener(delegate { SetDamage1Dice(wpnDamage1DiceType); });
        wpnDamage1Ability.onValueChanged.AddListener(delegate { SetDamage1FlatDamage(wpnDamage1Ability, wpnDamage1OtherBonus); });
        wpnDamage1OtherBonus.onValueChanged.AddListener(delegate { SetDamage1FlatDamage(wpnDamage1Ability, wpnDamage1OtherBonus); });
        wpnDamage1DamageType.onValueChanged.AddListener(delegate { SetDamage1Type(wpnDamage1DamageType); });

        wpnDamage2NumberOfDices.onValueChanged.AddListener(delegate { SetDamage1NDices(wpnDamage2NumberOfDices); });
        wpnDamage2DiceType.onValueChanged.AddListener(delegate { SetDamage1Dice(wpnDamage2DiceType); });
        wpnDamage2Ability.onValueChanged.AddListener(delegate { SetDamage1FlatDamage(wpnDamage2Ability, wpnDamage2OtherBonus); });
        wpnDamage2OtherBonus.onValueChanged.AddListener(delegate { SetDamage1FlatDamage(wpnDamage2Ability, wpnDamage2OtherBonus); });
        wpnDamage2DamageType.onValueChanged.AddListener(delegate { SetDamage1Type(wpnDamage2DamageType); });

        weaponAttackConfiguration.GetComponent<RectTransform>().SetParent(canvas.transform);
        weaponAttackConfiguration.GetComponent<RectTransform>().localPosition = Vector3.zero;
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

    private void SetAttackModifier(Dropdown attackAbility, InputField attackBonus, Toggle proficency, Text totalModifier)
    {
        int abilityBonus = 0;

        switch (attackAbility.value)
        {
            case 0:
                abilityBonus = sheetManager.GetStrMod();
                break;
            case 1:
                abilityBonus = sheetManager.GetDexMod();
                break;
            case 2:
                abilityBonus = sheetManager.GetConMod();
                break;
            case 3:
                abilityBonus = sheetManager.GetIntMod();
                break;
            case 4:
                abilityBonus = sheetManager.GetWisMod();
                break;
            case 5:
                abilityBonus = sheetManager.GetChaMod();
                break;
        }

        int otherBonus = 0;

        if (!int.TryParse(attackBonus.text, out otherBonus))
        {
            attackBonus.text = "0";
            otherBonus = 0;
        }

        int proficencyBonus = 0;

        if (proficency.isOn)
        {
            proficencyBonus = sheetManager.GetProficencyBonus();
        }

        attackMod = abilityBonus + otherBonus + proficencyBonus;
        totalModifier.text = attackMod.ToString();
    }

    private void SetDamage1NDices(InputField diceNumber)
    {
        int nDices;

        if (int.TryParse(diceNumber.text, out nDices))
        {
            damage1NumDices = nDices;
        }
        else
        {
            diceNumber.text = "0";
            damage1NumDices = 0;
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

    private void SetDamage1FlatDamage(Dropdown damageAbility, InputField damageBonus)
    {
        int abilityDamage = 0;

        switch (damageAbility.value)
        {
            case 0:
                abilityDamage = sheetManager.GetStrMod();
                break;
            case 1:
                abilityDamage = sheetManager.GetDexMod();
                break;
            case 2:
                abilityDamage = sheetManager.GetConMod();
                break;
            case 3:
                abilityDamage = sheetManager.GetIntMod();
                break;
            case 4:
                abilityDamage = sheetManager.GetWisMod();
                break;
            case 5:
                abilityDamage = sheetManager.GetChaMod();
                break;
        }

        int bonusDamage;

        if (int.TryParse(damageBonus.text, out bonusDamage))
        {
            damage1FlatDamage = abilityDamage + bonusDamage;
        }
        else
        {
            damageBonus.text = "0";
            damage1FlatDamage = abilityDamage;
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
        int nDices;

        if (int.TryParse(diceNumber.text, out nDices))
        {
            damage2NumDices = nDices;
        }
        else
        {
            diceNumber.text = "0";
            damage2NumDices = 0;
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

    private void SetDamage2FlatDamage(Dropdown damageAbility, InputField damageBonus)
    {
        int abilityDamage = 0;

        switch (damageAbility.value)
        {
            case 0:
                abilityDamage = sheetManager.GetStrMod();
                break;
            case 1:
                abilityDamage = sheetManager.GetDexMod();
                break;
            case 2:
                abilityDamage = sheetManager.GetConMod();
                break;
            case 3:
                abilityDamage = sheetManager.GetIntMod();
                break;
            case 4:
                abilityDamage = sheetManager.GetWisMod();
                break;
            case 5:
                abilityDamage = sheetManager.GetChaMod();
                break;
        }

        int bonusDamage;

        if (int.TryParse(damageBonus.text, out bonusDamage))
        {
            damage2FlatDamage = abilityDamage + bonusDamage;
        }
        else
        {
            damageBonus.text = "0";
            damage2FlatDamage = abilityDamage;
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
