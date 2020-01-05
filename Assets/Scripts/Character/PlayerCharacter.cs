using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	readonly int IDLE_ANIM_NAME_HASH = Animator.StringToHash("Idle");

	[SerializeField]
	float chargeTime = 1.0f;

	[SerializeField]
	CharaMotionController motionController = null;

	[SerializeField]
	Transform weaponNode = null;

    [SerializeField]
    float autoAttackTime = 2.0f;

    GameObject cacheGameObject = null;
	Transform cacheTransform = null;

	WeaponObject equipWeapon = null;

	int chargeEffectHandle = -1;
	int chargeStartEffectHandle = -1;
	float chargeElapsedTime = 0.0f;
	bool isChargeAttackSuccess = false;
    float elapsedAutoAttackTime = 0.0f;

    private void Awake()
	{
		cacheGameObject = gameObject;
		cacheTransform = transform;

		EventManager.AddEventListener(EVENT_ID.START_BATTLE, StartBattle);
	}

	private void Start()
	{
		motionController.Setup(this);
	}

	void StartBattle(EventUserDara userData)
	{
		var weaponObject = WeaponManager.CreateWeapon(MasterData.WEAPONID.SWORD_01, weaponNode);
		equipWeapon = weaponObject;
	}

	private void Update()
    {
        if (!BattleScene.IsPlayableState())
        {
            return;
        }

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
        else
        {
            elapsedAutoAttackTime += GlobalDefine.DeltaTime;
            if (elapsedAutoAttackTime >= autoAttackTime)
            {
                Attack();
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

        elapsedAutoAttackTime = 0.0f;
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

        elapsedAutoAttackTime = 0.0f;
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
			damage = equipWeapon.GetChargeAttackPower();
		}
		else
		{
			EffectManager.PlayEffect(EffectManager.EFFECT_ID.HIT_ATTACK, ref effectPos);
			damage = equipWeapon.GetAttackPower();
		}

		TreeObject.OnDamage(damage);

		effectPos.y += 1.0f;
		UIBattleController.GetDamageUI().DrawDamageNumber(damage, ref effectPos);

		equipWeapon.OnHit();
	}

	/// <summary>
	/// 武器軌跡の描画
	/// </summary>
	/// <param name="value"></param>
	void onDrawWeaponTrail(int value)
	{
		equipWeapon.SetDrawTrailRendererEnable(value == 1);
	}

	public void PlayWinMotion()
	{
		Idle();
		motionController.SetTriggerParam("Win");
	}

    public WeaponObject GetEquipWeaponObject()
    {
        return equipWeapon;
    }

    public void ChangeWeapon(MasterData.WEAPONID id)
    {
        Destroy(equipWeapon.gameObject);

        var weaponObject = WeaponManager.CreateWeapon(id, weaponNode);
        equipWeapon = weaponObject;

        GameParam.SetEquipWeaponID(id);
    }
}
