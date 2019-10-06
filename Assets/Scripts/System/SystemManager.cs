using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マネージャー種類
/// </summary>
public enum MANAGER_TYPE
{
	NONE = -1,

	SCENE,
	ASSET_LOAD,
	CHARA,
	INPUT,
	CAMERA,
	EFFECT,
	WEAPON,
	MASTER_DATA,

	MAX,
}

/// <summary>
/// システム的なものを管理します。
/// 例えば、各ゲームで使うマネージャーだったり。
/// </summary>
public class SystemManager : MonoBehaviour
{

	public static void StartBattle()
	{

	}

}
