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

	class WeaponLoadData
	{
		public Weapon.WEAPON_ID id;
		public GameObject loadObject = null;
		public AssetLoadHandle assetLoadHandle = null;
		public bool isLoaded = false;
	}

	WeaponLoadData[] weaponDatas = new WeaponLoadData[(int)Weapon.WEAPON_ID.MAX];
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
			var masterData = MasterDataManager.GetMasterData<Weapon.WeaponInfoMasterData>(MasterDataManager.MASTER_DATE_ID.WEAPON);
			for (int i = 0; i < (int)Weapon.WEAPON_ID.MAX; i++)
			{
				var masterDataRow = masterData.datas[i];
				var data = new WeaponLoadData();
				var path = "weapon/" + masterDataRow.res_name;
				data.id = (Weapon.WEAPON_ID)i;
				if (string.IsNullOrEmpty(path))
				{
					data.isLoaded = true;
				}
				else
				{
					data.assetLoadHandle = AssetLoadManager.Load<GameObject>(ref path);
				}

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
	public static WeaponObject CreateWeapon(Weapon.WEAPON_ID id, Transform weaponNode)
	{
		for (int i = 0; i < Instance.weaponDatas.Length; i++)
		{
			if (!Instance.weaponDatas[i].isLoaded)
			{
				continue;
			}

			if (Instance.weaponDatas[i].id == id)
			{
				var obj = GameObject.Instantiate(Instance.weaponDatas[i].loadObject, weaponNode);
				return obj.GetComponent<WeaponObject>();
			}
		}

		return null;
	}

	public static Weapon.WeaponInfo GetWeaponData(Weapon.WEAPON_ID id)
	{
		var masterData = MasterDataManager.GetMasterData<Weapon.WeaponInfoMasterData>(MasterDataManager.MASTER_DATE_ID.WEAPON);
		for (int i = 0; i < masterData.datas.Length; i++)
		{
			if (masterData.datas[i].id == (int)id)
			{
				return masterData.datas[i];
			}
		}

		return null;
	}
}
