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

	static TreeObject instance = null;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		hp = maxHp;
		treeWave = 0;

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

			instance.maxHp = (int)(instance.maxHp * 1.5f);
			instance.hp = instance.maxHp;
			statusUI.SetTreeHpValue(instance.hp, instance.GetHpRatio(), true);
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
