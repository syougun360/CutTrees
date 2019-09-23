using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : ManagerBehaviour<WeaponManager>
{
	enum SEQ_STATE
	{
		NONE,
		LOAD,
		UPDATE,
	}

	public enum WEAPON_ID
	{
		NONE,
		SWORD_01,
		SWORD_02,
		SWORD_03,
		STAFF_01,
		HAMMER_01,
		AXE_01,
		AXE_02,
		CLUB_01,
		MAX,
	}

	readonly string[] WEAPON_PATH_TABLE = new string[(int)WEAPON_ID.MAX] {
		"",
		"weapon/sword_01",
		"weapon/sword_02",
		"weapon/sword_03",
		"weapon/staff_01",
		"weapon/hammer_01",
		"weapon/axe_01",
		"weapon/axe_02",
		"weapon/club_01",
	};

	class WeaponLoadData
	{
		public WEAPON_ID id;
		public GameObject loadObject = null;
		public AssetLoadHandle assetLoadHandle = null;
		public bool isLoaded = false;
	}

	WeaponLoadData[] weaponDatas = new WeaponLoadData[(int)WEAPON_ID.MAX];
	SeqStateUtility<SEQ_STATE> seqState = new SeqStateUtility<SEQ_STATE>(SEQ_STATE.NONE);

	public override MANAGER_TYPE GetManagerType()
	{
		return MANAGER_TYPE.WEAPON;
	}

	private void Update()
	{
		switch (seqState.GetState())
		{
			case SEQ_STATE.LOAD:
				UpdateLoad();
				break;
			case SEQ_STATE.UPDATE:
				break;
		}
	}

	void UpdateLoad()
	{
		if (!seqState.IsInit())
		{
			for (int i = 0; i < WEAPON_PATH_TABLE.Length; i++)
			{
				var data = new WeaponLoadData();
				var path = WEAPON_PATH_TABLE[i];
				data.id = (WEAPON_ID)i;
				data.assetLoadHandle = AssetLoadManager.Load<GameObject>(ref path);
				weaponDatas[i] = data;
			}

			seqState.SetInit(true);
		}

		bool end = true;
		for (int i = 0; i < weaponDatas.Length; i++)
		{
			var data = weaponDatas[i];
			if (data.isLoaded)
			{
				continue;
			}

			if (data.assetLoadHandle.Result == ASSET_LOAD_RESULT_TYPE.SUCCESS)
			{
				data.isLoaded = true;
				data.loadObject = data.assetLoadHandle.LoadObject as GameObject;
			}
			else if (data.assetLoadHandle.Result == ASSET_LOAD_RESULT_TYPE.FAILURE)
			{
				data.isLoaded = true;
			}
			else
			{
				end = false;
			}
		}

		if (end)
		{
			seqState.ChangeState(SEQ_STATE.UPDATE);
		}
	}

	public static void StartLoad()
	{
		Instance.seqState.ChangeState(SEQ_STATE.LOAD);
	}

	public static bool IsLoaded()
	{
		return Instance.seqState.GetState() == SEQ_STATE.UPDATE;
	}

	/// <summary>
	/// 武器生成
	/// </summary>
	/// <param name="id"></param>
	/// <param name="weaponNode"></param>
	/// <returns></returns>
	public static Weapon CreateWeapon(WEAPON_ID id, Transform weaponNode)
	{
		for (int i = 0; i < Instance.weaponDatas.Length; i++)
		{
			if (!Instance.weaponDatas[i].loadObject)
			{
				continue;
			}

			if (Instance.weaponDatas[i].id == id)
			{
				var obj = GameObject.Instantiate(Instance.weaponDatas[i].loadObject, weaponNode);
				return obj.GetComponent<Weapon>();
			}
		}

		return null;
	}
}
