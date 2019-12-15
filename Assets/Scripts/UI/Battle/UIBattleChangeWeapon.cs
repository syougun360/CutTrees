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

        var paramList = GameParam.GetWeaponParamList();
		if (paramList != null)
		{
			scrollRectUI.SetItemData(paramList);
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
        var index = scrollRectUI.GetItemIndex(number);
        var paramList = GameParam.GetWeaponParamList();
        var id = (MasterData.WEAPONID)paramList[index].id;
        PlayerManager.GetPlayer().ChangeWeapon(id);

        scrollRectUI.RefreshItemData();
    }
}
