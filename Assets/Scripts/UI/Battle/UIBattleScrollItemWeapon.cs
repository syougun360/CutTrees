using Scrmizu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleScrollItemWeapon : MonoBehaviour, IInfiniteScrollItem
{
    [SerializeField]
    GameObject equipIconObject = null;

    [SerializeField]
    Text levelValue = null;

    [SerializeField]
    Text nameValue = null;

    [SerializeField]
    Text attackPowerValue = null;

    [SerializeField]
    Text chargeAttackPowerValue = null;

    [SerializeField]
    Text usedLvValue = null;

    [SerializeField]
    Text hpValue = null;

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public void UpdateItemData(object data)
	{
		gameObject.SetActive(true);

        var param = (WeaponParam)data;
        var weaponData = WeaponManager.GetWeaponData((Weapon.WEAPON_ID)param.id);
        var player = PlayerManager.GetPlayer();
        var weapon = player.GetEquipWeaponObject();
        bool isEquip = (int)weapon.GetId() == param.id;
        equipIconObject.SetActive(isEquip);

        if(param.hp < 0)
        {
            hpValue.text = "無限";
        }
        else
        {
            hpValue.text = param.hp.ToString();
        }

        attackPowerValue.text = weaponData.attack_power.ToString();
        chargeAttackPowerValue.text = weaponData.charge_attack_power.ToString();
        nameValue.text = weaponData.name;
        levelValue.text = weaponData.level.ToString();
        usedLvValue.text = weaponData.equip_level.ToString();
    }
}
