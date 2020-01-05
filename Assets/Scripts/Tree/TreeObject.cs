using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class TreeObject : MonoBehaviour
{
	public enum STATE
	{
		NONE,
		APPEAR,
		IDLE,
		DOWN,
	}

	[SerializeField]
	int hp = 0;

	[SerializeField]
	int maxHp = 0;

	[SerializeField]
	int treeWave = 0;

	[SerializeField]
	float scale = 1.0f;

	[SerializeField]
	Transform attackPoint = null;

	[SerializeField]
	Transform damageTrans = null;

	[SerializeField]
	MeshRenderer damageMeshRenderer = null;

	[SerializeField]
	Transform treeDownImpactEffectPoint = null;

	Animation treeAnimation = null;
	AnimationState treeAnimationState = null;

	static TreeObject instance = null;

	SeqStateUtility<STATE> state = new SeqStateUtility<STATE>(STATE.NONE);
	float playerDistance = 0.0f;
	Vector3 cameraDistance = GlobalDefine.Vec3Zero;


	private void Awake()
	{
		instance = this;
		treeAnimation = GetComponent<Animation>();
	}

    private void Start()
    {
        EventManager.AddEventListener(EVENT_ID.START_BATTLE, OnStartBattle);
    }

    void OnStartBattle(EventUserDara userData)
	{
        var masterData = MasterDataManager.GetMasterData<MasterData.Tree>(MasterDataManager.MASTER_DATE_ID.TREE);
        var data = masterData.dataArray[1];

        maxHp = data.Hp;
        hp = maxHp;
		treeWave = 0;
		scale = data.Scale;

		var playerTrans = PlayerManager.GetPlayer().transform;
		playerDistance = (playerTrans.localPosition - attackPoint.position).magnitude;

		var cameraTrans = Camera.main.transform;
		cameraDistance = (cameraTrans.localPosition - playerTrans.position);

		var statusUI = UIBattleController.GetTreeStatusUI();
		statusUI.SetTreeHpValue(hp, GetHpRatio(), true);
		statusUI.SetTreeWave(treeWave);

		damageTrans.localScale = new Vector3(1.0f, 0.0f, 1.0f);
		damageMeshRenderer.enabled = false;
	}

	private void Update()
	{
		if (state.GetState() == STATE.APPEAR)
		{
			if (!state.IsInit())
			{
				state.SetInit(true);
				return;
			}

			if (treeAnimationState.normalizedTime >= 1.0f)
			{
				state.ChangeState(STATE.IDLE);
			}
		}
		else if (state.GetState() == STATE.DOWN)
		{
			if (!state.IsInit())
			{
				treeAnimationState = treeAnimation.PlayQueued("objanim_tree_down", QueueMode.PlayNow);
				state.SetInit(true);
				return;
			}

			if (treeAnimationState.normalizedTime >= 1.0f)
			{
				NextWave();
			}
		}
	}

	void NextWave()
	{
		var statusUI = UIBattleController.GetTreeStatusUI();

		treeWave++;
		statusUI.SetTreeWave(treeWave);

        var masterData = MasterDataManager.GetMasterData<MasterData.Tree>(MasterDataManager.MASTER_DATE_ID.TREE);
        var data = masterData.dataArray[treeWave + 1];

        scale = data.Scale;
		transform.localScale = GlobalDefine.Vec3One * scale;

        maxHp = data.Hp;
        hp = maxHp;
		statusUI.SetTreeHpValue(hp, GetHpRatio(), true);

		var playerTrans = PlayerManager.GetPlayer().transform;
		Vector3 playerPos = playerTrans.localPosition;
		playerPos.z = attackPoint.position.z - playerDistance;
		playerTrans.localPosition = playerPos;

		var cameraTrans = Camera.main.transform;
		Vector3 cameraPos = cameraTrans.localPosition;
		cameraPos = playerTrans.position + cameraDistance;
		cameraTrans.localPosition = cameraPos;

		treeAnimationState = treeAnimation.PlayQueued("objanim_tree_appear", QueueMode.PlayNow);
		state.ChangeState(STATE.APPEAR);
	}

	public static void OnDamage(int damage)
	{
		instance.OnDamage_Inner(damage);
	}

	void OnDamage_Inner(int damage)
	{
		hp -= damage;

		var statusUI = UIBattleController.GetTreeStatusUI();
		statusUI.SetTreeHpValue(hp, GetHpRatio());

		if (hp <= 0)
		{
			damageTrans.localScale = new Vector3(1.0f, 0.0f, 1.0f);
			damageMeshRenderer.enabled = false;
			state.ChangeState(STATE.DOWN);
			return;
		}

		Vector3 damageScale = damageTrans.localScale;
		damageScale.y = 1.0f - GetHpRatio();
		damageTrans.localScale = damageScale;
		damageMeshRenderer.enabled = damageScale.y > 0.0f;
	}

	public int GetHp()
	{
		return hp;
	}

	public float GetHpRatio()
	{
		return (float)hp / (float)maxHp;
	}

	public static bool IsAnimationPlaying()
	{
		return instance.state.GetState() == STATE.APPEAR || instance.state.GetState() == STATE.DOWN;
	}

	void PlayDownImpactEffect()
	{
		var pos = treeDownImpactEffectPoint.position;
		EffectManager.PlayEffect(EffectManager.EFFECT_ID.TREE_DOWN_IMPACT, ref pos);

		PlayerManager.GetPlayer().PlayWinMotion();
	}

	void PlayAppearImpactEffect()
	{
		var pos = transform.position;
		EffectManager.PlayEffect(EffectManager.EFFECT_ID.TREE_APPEAR_IMPACT, ref pos);
	}
}
