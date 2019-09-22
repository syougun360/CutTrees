using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBattleInputAttack : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField]
	float tapTime = 0.1f;

	bool isPress = false;
	float pressElapsedTime = 0.0f;

	public void OnPointerDown(PointerEventData eventData)
	{
		pressElapsedTime = 0.0f;
		isPress = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		var player = PlayerManager.GetPlayer();
		if (player.IsCharge())
		{
			PlayerManager.GetPlayer().AttackCharge();
		}
		else
		{
			PlayerManager.GetPlayer().Attack();
		}

		isPress = false;
		pressElapsedTime = 0.0f;
	}

	void Update()
	{
		if (!isPress)
		{
			return;
		}

		pressElapsedTime += GlobalDefine.DeltaTime;
		if (pressElapsedTime > tapTime)
		{
			var player = PlayerManager.GetPlayer();
			if (!player.IsCharge())
			{
				player.StartCharge();
			}
		}
	}
}
