using Scrmizu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleChangeWeapon : UIBase
{
	readonly int CLOSE_BUTTON_NAME_HASH = "Button_close".GetHashCode();

	[SerializeField]
	InfiniteScrollRect scrollRectUI = null;

	protected override void UpdateCloseUI(bool change)
	{
	}

	protected override void UpdateOpenUI(bool change)
	{
	}

	protected override void UpdateUI()
	{
	}

	protected override void Open()
	{
		base.Open();

		var masterData = MasterDataManager.GetMasterData<Weapon.WeaponInfoMasterData>(MasterDataManager.MASTER_DATE_ID.WEAPON);
		if (masterData != null)
		{
			scrollRectUI.SetItemData(masterData.datas);
			ResetButton();
		}
	}

	protected override void OnButtonCallback(Button button, int buttonNameHash)
	{
		if (CLOSE_BUTTON_NAME_HASH == buttonNameHash)
		{
			StartClose();
		}
	}

	protected override void OnButtonSelectCallback(Button button, int buttonNameHash, int number)
	{

	}
}
