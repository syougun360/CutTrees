using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleButton : UIBase
{
	readonly int WEAPON_BUTTON_NAME_HASH = "Button_Weapon".GetHashCode();

	private void Start()
	{
		StartOpen();
	}

	protected override void UpdateCloseUI(bool change)
	{
	}

	protected override void UpdateOpenUI(bool change)
	{
	}

	protected override void UpdateUI()
	{
	}

	protected override void OnButtonCallback(Button button, int buttonNameHash)
	{
		if (WEAPON_BUTTON_NAME_HASH == buttonNameHash)
		{
			var ui = UIBattleController.GetChangeWeaponUI();
			ui.StartOpen();
		}
	}

}
