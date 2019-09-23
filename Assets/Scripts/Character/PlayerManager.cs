using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ManagerBehaviour<PlayerManager>
{
	[SerializeField]
	PlayerCharacter playerCharacter = null;

	public override MANAGER_TYPE GetManagerType()
	{
		return MANAGER_TYPE.CHARA;
	}

	public static PlayerCharacter GetPlayer()
	{
		if (!IsInstance)
		{
			return null;
		}

		return Instance.playerCharacter;
	}

}
