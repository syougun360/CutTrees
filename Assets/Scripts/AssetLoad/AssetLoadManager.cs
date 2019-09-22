using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoadManager : ManagerBehaviour<AssetLoadManager>
{
	List<AssetLoader> loadList = new List<AssetLoader>();
	Dictionary<int, AssetLoadHandle> assets = new Dictionary<int, AssetLoadHandle>();

	public override MANAGER_TYPE GetManagerType()
	{
		return MANAGER_TYPE.ASSET_LOAD;
	}

	private void Update()
	{
		for(int i = loadList.Count - 1; i >= 0 ;i--)
		{
			if (loadList[i].Update())
			{
				assets.Add(loadList[i].UniqueID, loadList[i]);
				loadList.RemoveAt(i);
			}
		}
	}

	/// <summary>
	/// 読込
	/// </summary>
	/// <param name="assetPath"></param>
	public static AssetLoadHandle Load<T>(ref string assetPath)
	{
		AssetLoader loader = GetAssetLoader(ref assetPath);
		if (loader == null)
		{
			// ロードされていない
			loader = new AssetLoader(ref assetPath, typeof(T));
			Instance.loadList.Add(loader);
			return loader;
		}
		else
		{
			// ロードされている
			AssetLoadHandle handle = GetAssetLoadHandle(ref assetPath);
			if (handle == null)
			{
				// まだハンドルがなければ、ローダーを渡す
				loader.AddRefCount();
				return loader;
			}
			else
			{
				// ハンドルがあれば、それを渡す
				loader = handle as AssetLoader;
				loader.AddRefCount();
				return handle;
			}
		}
	}

	/// <summary>
	/// 破棄
	/// </summary>
	public static void Unload(ref AssetLoadHandle handle)
	{
		AssetLoader loader = handle as AssetLoader;
		if (handle.RefCount > 0)
		{
			loader.RemoveRefCount();
			return;
		}

		loader.Unload();
		Instance.assets.Remove(handle.UniqueID);
		handle = null;
	}

	/// <summary>
	/// アセットを取得
	/// </summary>
	/// <param name="assetPath"></param>
	/// <returns></returns>
	public static AssetLoadHandle GetAssetLoadHandle(ref string assetPath)
	{
		int hash = assetPath.GetHashCode();
		if (Instance.assets.ContainsKey(hash))
		{
			AssetLoadHandle handle = Instance.assets[hash];
			return handle;
		}

		return null;
	}


	/// <summary>
	/// アセットローダーを取得
	/// </summary>
	/// <param name="assetPath"></param>
	/// <returns></returns>
	static AssetLoader GetAssetLoader(ref string assetPath)
	{
		int hash = assetPath.GetHashCode();
		for (int i = 0; i < Instance.loadList.Count; i++)
		{
			AssetLoader loader = Instance.loadList[i];
			if (loader.UniqueID == hash)
			{
				return loader;
			}
		}

		return null;
	}
}
