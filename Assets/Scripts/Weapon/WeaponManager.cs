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
		public MasterData.WEAPONID id;
		public GameObject loadObject = null;
		public AssetLoadHandle assetLoadHandle = null;
		public bool isLoaded = false;
        public MasterData.WeaponData info = null;
	}

	WeaponLoadData[] weaponDatas = new WeaponLoadData[(int)MasterData.WEAPONID.Max];
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
            var masterData = MasterDataManager.GetMasterData<MasterData.Weapon>(MasterDataManager.MASTER_DATE_ID.WEAPON);
            for (int i = 0; i < masterData.dataArray.Length; i++)
            {
                var masterDataRow = masterData.dataArray[i];
                var data = new WeaponLoadData();
                var path = "weapon/" + masterDataRow.Modelresname;
                data.id = masterDataRow.Weaponid;
                data.info = masterDataRow;

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
	public static WeaponObject CreateWeapon(MasterData.WEAPONID id, Transform weaponNode)
	{
        var data = Instance.weaponDatas;

        for (int i = 0; i < data.Length; i++)
		{
			if (!data[i].isLoaded)
			{
				continue;
			}

			if (data[i].id == id)
			{
				var obj = GameObject.Instantiate(data[i].loadObject, weaponNode);
                obj.layer = LayerDefine.Player;
				var weapon = obj.GetComponent<WeaponObject>();
                weapon.OnCreate(data[i].info);
                return weapon;
            }
		}

		return null;
	}

	public static MasterData.WeaponData GetWeaponData(MasterData.WEAPONID id)
	{
        var masterData = MasterDataManager.GetMasterData<MasterData.Weapon>(MasterDataManager.MASTER_DATE_ID.WEAPON);
        for (int i = 0; i < masterData.dataArray.Length; i++)
        {
            if (masterData.dataArray[i].ID == (int)id)
            {
                return masterData.dataArray[i];
            }
        }

        return null;
	}
}
