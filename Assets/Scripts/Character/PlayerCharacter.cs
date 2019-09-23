using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	readonly int IDLE_ANIM_NAME_HASH = Animator.StringToHash("Idle");

	[SerializeField]
	float chargeTime = 1.0f;

	[SerializeField]
	int attackPower = 1;

	[SerializeField]
	int chargeAttackPower = 10;

	[SerializeField]
	CharaMotionController motionController = null;

	[SerializeField]
	GameObject weaponObject = null;

	[SerializeField]
	TrailRenderer weaponTrailRenderer = null;

	GameObject cacheGameObject = null;
	Transform cacheTransform = null;

	int chargeEffectHandle = -1;
	int chargeStartEffectHandle = -1;
	float chargeElapsedTime = 0.0f;
	bool isChargeAttackSuccess = false;

	private void Awake()
	{
		cacheGameObject = gameObject;
		cacheTransform = transform;

		weaponTrailRenderer = weaponObject.GetComponentInChildren<TrailRenderer>();
		weaponTrailRenderer.enabled = false;
	}

	private void Start()
	{
		motionController.Setup(this);
	}

	private void Update()
	{
		if (IsCharge())
		{
			if (!isChargeAttackSuccess)
			{
				chargeElapsedTime += GlobalDefine.DeltaTime;
				if (chargeElapsedTime >= chargeTime)
				{
					SuccessCharge();
					isChargeAttackSuccess = true;
				}
			}
		}
	}

	public void Attack()
	{
		if (motionController.IsShortNameHash(IDLE_ANIM_NAME_HASH))
		{
			motionController.SetTriggerParam("attack");
			motionController.SetBoolParam("isChargeAttack", false);
			motionController.SetBoolParam("isCharge", false);
			isChargeAttackSuccess = false;

			EffectManager.StopEffect(EffectManager.EFFECT_ID.CHARGE, ref chargeEffectHandle);
			EffectManager.StopEffect(EffectManager.EFFECT_ID.CHARGE_START, ref chargeStartEffectHandle);
		}
	}

	public void StartCharge()
	{
		if (IsCharge())
		{
			return;
		}

		chargeElapsedTime = 0.0f;
		isChargeAttackSuccess = false;

		motionController.SetBoolParam("isCharge", true);
		motionController.SetBoolParam("isChargeAttack", false);

		var effectPos = cacheTransform.localPosition + GlobalDefine.Vec3Down;
		chargeStartEffectHandle = EffectManager.PlayEffect(EffectManager.EFFECT_ID.CHARGE_START, ref effectPos);
	}

	public void SuccessCharge()
	{
		EffectManager.StopEffect(EffectManager.EFFECT_ID.CHARGE, ref chargeEffectHandle);
		EffectManager.StopEffect(EffectManager.EFFECT_ID.CHARGE_START, ref chargeStartEffectHandle);

		var effectPos = cacheTransform.localPosition + GlobalDefine.Vec3Down;
		chargeEffectHandle = EffectManager.PlayEffect(EffectManager.EFFECT_ID.CHARGE, ref effectPos);
	}

	public void AttackCharge()
	{
		motionController.SetBoolParam("isCharge", false);
		motionController.SetBoolParam("isChargeAttack", isChargeAttackSuccess);

		EffectManager.StopEffect(EffectManager.EFFECT_ID.CHARGE, ref chargeEffectHandle);
		EffectManager.StopEffect(EffectManager.EFFECT_ID.CHARGE_START, ref chargeStartEffectHandle);
	}

	public bool IsCharge()
	{
		return motionController.GetBoolParam("isCharge");
	}

	public void Idle()
	{
		motionController.PlayIdle();
		isChargeAttackSuccess = false;

		EffectManager.StopEffect(EffectManager.EFFECT_ID.CHARGE, ref chargeEffectHandle);
		EffectManager.StopEffect(EffectManager.EFFECT_ID.CHARGE_START, ref chargeStartEffectHandle);
	}

	/// <summary>
	/// ツリーにあたった
	/// </summary>
	void onHit()
	{
		var effectPos = cacheTransform.localPosition;
		effectPos.y += 2.0f;
		effectPos.z += 3.0f;

		int damage = 0;
		if (isChargeAttackSuccess)
		{
			EffectManager.PlayEffect(EffectManager.EFFECT_ID.HIT_CHARGE, ref effectPos);
			isChargeAttackSuccess = false;
			damage = chargeAttackPower;
		}
		else
		{
			EffectManager.PlayEffect(EffectManager.EFFECT_ID.HIT_ATTACK, ref effectPos);
			damage = attackPower;
		}

		TreeObject.OnDamage(damage);

		effectPos.y += 1.0f;
		UIBattleController.GetDamageUI().DrawDamageNumber(damage, ref effectPos);
	}

	/// <summary>
	/// 武器軌跡の描画
	/// </summary>
	/// <param name="value"></param>
	void onDrawWeaponTrail(int value)
	{
		weaponTrailRenderer.enabled = value == 1;
	}

	public void PlayWinMotion()
	{
		Idle();
		motionController.SetTriggerParam("Win");
	}
}
