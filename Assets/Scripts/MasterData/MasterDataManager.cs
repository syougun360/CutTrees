using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDataManager : ManagerBehaviour<MasterDataManager>
{
	public enum MASTER_DATE_ID
	{
		NONE,

		WEAPON,

		MAX,
	}

	class MasterData
	{
		public MASTER_DATE_ID id;
		public bool isLoaded = false;
		public string fileName = "";
		public AssetLoadHandle assetLoadHandle = null;
		public TextAsset textAsset = null;
	}

	readonly string[] MASTER_DATA_FILE_TABLE = new string[(int)MASTER_DATE_ID.MAX] {
		"",
		"weapon"
	};

	bool isLoaded = false;
	MasterData[] masterDatas = new MasterData[(int)MASTER_DATE_ID.MAX];

	public override MANAGER_TYPE GetManagerType()
	{
		return MANAGER_TYPE.MASTER_DATA;
	}

	void Start()
    {
		for (int i = 0; i < MASTER_DATA_FILE_TABLE.Length; i++)
		{
			MasterData data = new MasterData();
			string resPath = "master_data/" + MASTER_DATA_FILE_TABLE[i];
			data.fileName = MASTER_DATA_FILE_TABLE[i];
			data.id = (MASTER_DATE_ID)i;

			if (string.IsNullOrEmpty(data.fileName))
			{
				data.isLoaded = true;
			}
			else
			{
				data.assetLoadHandle = AssetLoadManager.Load<TextAsset>(ref resPath);
			}

			masterDatas[i] = data;
		}
	}

	private void Update()
	{
		if (isLoaded)
		{
			return;
		}

		isLoaded = true;
		for (int i = 0; i < masterDatas.Length; i++)
		{
			var data = masterDatas[i];
			if (data.isLoaded)
			{
				continue;
			}

			if (data.assetLoadHandle.Result == ASSET_LOAD_RESULT_TYPE.SUCCESS)
			{
				data.textAsset = data.assetLoadHandle.LoadObject as TextAsset;
				data.isLoaded = true;
			}
			else if (data.assetLoadHandle.Result == ASSET_LOAD_RESULT_TYPE.FAILURE)
			{
				data.isLoaded = true;
			}
			else
			{
				isLoaded = false;
			}
		}
	}

	public static bool IsLoaded()
	{
		if (IsInstance)
		{
			return Instance.isLoaded;
		}

		return false;
	}

	/// <summary>
	/// マスターデータ取得
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="id"></param>
	/// <returns></returns>
	public static T GetMasterData<T>(MASTER_DATE_ID id) where T : class
	{
		for (int i = 0; i < Instance.masterDatas.Length; i++)
		{
			var data = Instance.masterDatas[i];
			if (!data.isLoaded)
			{
				continue;
			}

			if (data.id == id)
			{
				byte[] bytes = data.textAsset.bytes;
				return MasterDataUtility.Deserialize<T>(ref bytes);
			}
		}

		return null;
	}

}
