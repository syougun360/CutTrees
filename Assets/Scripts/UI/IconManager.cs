using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconManager : ManagerBehaviour<IconManager>
{
    class IconLoadData
    {
        public int id;
        public MasterData.ICONID labelId;
        public bool isLoaded = false;
        public AssetLoadHandle assetLoadHandle;
        public Sprite sprite;
    }

    List<IconLoadData> iconLoadData = new List<IconLoadData>();
    bool isLoaded = false;
    bool isLoading = false;

    public override MANAGER_TYPE GetManagerType()
    {
        return MANAGER_TYPE.ICON;
    }

    public static void StartLoad()
    {
        if (!IsInstance)
        {
            return;
        }

        var iconData = MasterDataManager.GetMasterData<MasterData.Icon>(MasterDataManager.MASTER_DATE_ID.ICON);
        foreach (var data in iconData.dataArray)
        {
            IconLoadData loadData = new IconLoadData();
            string path = data.Folder + data.Filename;
            loadData.assetLoadHandle = AssetLoadManager.Load<Sprite>(ref path);
            loadData.labelId = data.Iconid;
            loadData.id = data.ID;
            Instance.iconLoadData.Add(loadData);
        }

        Instance.isLoading = true;
    }

    private void Update()
    {
        if (isLoaded)
        {
            return;
        }

        if (!isLoading)
        {
            return;
        }

        int loadedCount = 0;
        foreach (var data in iconLoadData)
        {
            if (data.isLoaded)
            {
                continue;
            }

            if (data.assetLoadHandle.Result == ASSET_LOAD_RESULT_TYPE.SUCCESS)
            {
                var asset = data.assetLoadHandle.LoadObject as Sprite;
                data.sprite = asset;
                data.isLoaded = true;
                loadedCount++;
            }
            else if (data.assetLoadHandle.Result == ASSET_LOAD_RESULT_TYPE.FAILURE)
            {
                Debug.LogError("[Icon] 読み込み失敗：" + data.id);

                data.isLoaded = true;
                loadedCount++;
            }
        }

        if (loadedCount >= iconLoadData.Count)
        {
            isLoading = false;
            isLoaded = true;
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

    public static Sprite GetIconSprite(int id)
    {
        if (!IsInstance)
        {
            return null;
        }

        foreach (var data in Instance.iconLoadData)
        {
            if (data.id == id)
            {
                if (!data.isLoaded)
                {
                    return Instance.iconLoadData[0].sprite;
                }

                return data.sprite;
            }
        }

        return Instance.iconLoadData[0].sprite;
    }

    public static Sprite GetIconSprite(MasterData.ICONID label)
    {
        if (!IsInstance)
        {
            return null;
        }

        foreach (var data in Instance.iconLoadData)
        {
            if (data.labelId == label)
            {
                if (!data.isLoaded)
                {
                    return Instance.iconLoadData[0].sprite;
                }

                return data.sprite;
            }
        }

        return Instance.iconLoadData[0].sprite;
    }
}
