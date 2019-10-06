using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
	[SerializeField]
	int attackPower = 1;

	[SerializeField]
	int chargeAttackPower = 10;

	[SerializeField]
	int maxHp = 100;

    Weapon.WEAPON_ID id;
	int hp = 100;

	GameObject weaponObject = null;
	TrailRenderer trailRenderer = null;

	public void OnCreate(Weapon.WeaponInfo info)
	{
        id = (Weapon.WEAPON_ID)info.id;
        attackPower = info.attack_power;
        chargeAttackPower = info.charge_attack_power;
        maxHp = info.hp;
        hp = maxHp;

        weaponObject = gameObject;

		trailRenderer = weaponObject.transform.GetComponentInChildren<TrailRenderer>();
		if (trailRenderer != null)
		{
			trailRenderer.enabled = false;
		}
	}

	public void OnHit()
	{
		hp--;
		if (hp <= 0)
		{
			hp = 0;
		}
	}

	public void SetDrawTrailRendererEnable(bool enable)
	{
		if (trailRenderer != null)
		{
			trailRenderer.enabled = enable;
		}
	}

	public int GetAttackPower()
	{
		return attackPower;
	}

	public int GetChargeAttackPower()
	{
		return chargeAttackPower;
	}

	public int GetHp()
	{
		return hp;
	}

	public int GetHpMax()
	{
		return maxHp;
	}

    public Weapon.WEAPON_ID GetId()
    {
        return id;
    }
}
