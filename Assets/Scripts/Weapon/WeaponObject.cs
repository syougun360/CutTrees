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

	int hp = 100;

	GameObject weaponObject = null;
	TrailRenderer trailRenderer = null;

	public void OnCreate()
	{
		weaponObject = gameObject;

		trailRenderer = weaponObject.transform.GetComponentInChildren<TrailRenderer>();
		if (trailRenderer != null)
		{
			trailRenderer.enabled = false;
		}

		hp = maxHp;
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
}
