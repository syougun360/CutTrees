using Scrmizu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleScrollItemWeapon : MonoBehaviour, IInfiniteScrollItem
{
	void Awake()
	{
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public void UpdateItemData(object data)
	{
		gameObject.SetActive(true);

	}
}
