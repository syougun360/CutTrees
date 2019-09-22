using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObject : MonoBehaviour
{
	[SerializeField]
	int hp = 0;

	[SerializeField]
	int maxHp = 0;

	[SerializeField]
	int treeWave = 0;

	[SerializeField]
	float nextHpCoefficient = 1.5f;

	[SerializeField]
	float nextScaleCoefficient = 1.1f;

	[SerializeField]
	float scale = 1.0f;

	[SerializeField]
	Transform attackPoint = null;

	static TreeObject instance = null;
	static float playerDistance = 0.0f;
	static Vector3 cameraDistance = GlobalDefine.Vec3Zero;


	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		hp = maxHp;
		treeWave = 0;
		scale = 1.0f;

		var playerTrans = PlayerManager.GetPlayer().transform;
		playerDistance = (playerTrans.localPosition - attackPoint.position).magnitude;

		var cameraTrans = Camera.main.transform;
		cameraDistance = (cameraTrans.localPosition - playerTrans.position);

		var statusUI = UIBattleController.GetTreeStatusUI();
		statusUI.SetTreeHpValue(hp, GetHpRatio(), true);
		statusUI.SetTreeWave(treeWave);
	}
	
	public static void OnDamage(int damage)
	{
		instance.hp -= damage;

		var statusUI = UIBattleController.GetTreeStatusUI();
		statusUI.SetTreeHpValue(instance.hp, instance.GetHpRatio());

		if (instance.hp <= 0)
		{
			instance.treeWave++;
			statusUI.SetTreeWave(instance.treeWave);

			instance.scale = instance.scale * instance.nextScaleCoefficient;
			instance.transform.localScale = GlobalDefine.Vec3One * instance.scale;

			instance.maxHp = (int)(instance.maxHp * instance.nextHpCoefficient);
			instance.hp = instance.maxHp;
			statusUI.SetTreeHpValue(instance.hp, instance.GetHpRatio(), true);

			var playerTrans = PlayerManager.GetPlayer().transform;
			Vector3 playerPos = playerTrans.localPosition;
			playerPos.z = instance.attackPoint.position.z - playerDistance;
			playerTrans.localPosition = playerPos;

			var cameraTrans = Camera.main.transform;
			Vector3 cameraPos = cameraTrans.localPosition;
			cameraPos = playerTrans.position + cameraDistance;
			cameraTrans.localPosition = cameraPos;
		}
	}

	public int GetHp()
	{
		return hp;
	}

	public float GetHpRatio()
	{
		return (float)hp / (float)maxHp;
	}
}
