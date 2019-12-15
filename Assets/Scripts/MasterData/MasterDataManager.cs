using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDataManager : ManagerBehaviour<MasterDataManager>
{
    public enum MASTER_DATE_ID
    {
        NONE,

        WEAPON,
        ICON,

        MAX,
    }

    class MasterData
    {
        public MASTER_DATE_ID id;
        public bool isLoaded = false;
        public string fileName = "";
        public ScriptableObject dataObject = null;
		public AssetLoadHandle assetLoadHandle = null;
    }

    readonly string[] MASTER_DATA_FILE_TABLE = {
        "",
        "weapon",
        "icon",
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
				data.assetLoadHandle = AssetLoadManager.Load<ScriptableObject>(ref resPath);
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
                data.dataObject = data.assetLoadHandle.LoadObject as ScriptableObject;
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
    public static T GetMasterData<T>(MASTER_DATE_ID id) where T : ScriptableObject
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
                var masterData = data.dataObject as T;
                return masterData;
            }
        }

        return null;
    }



}
