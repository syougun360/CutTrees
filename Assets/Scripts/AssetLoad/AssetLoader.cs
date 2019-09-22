using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ASSET_LOAD_RESULT_TYPE
{
	NONE = -1,
	
	SUCCESS,
	FAILURE,
}

/// <summary>
/// ロードハンドル
/// </summary>
public class AssetLoadHandle
{
	public int UniqueID { get;protected set; }
	public ASSET_LOAD_RESULT_TYPE Result { get; protected set; }
	public UnityEngine.Object LoadObject { get; protected set; }
	public string AssetPath { get; protected set; }
	public int RefCount { get; protected set; }
}

/// <summary>
/// ロードする実態
/// </summary>
public class AssetLoader : AssetLoadHandle
{
	enum STATE
	{
		NONE = -1,
		RESOURCES_LOAD,
	}

	STATE state = STATE.NONE;
	bool asyncLoad = false;
	ResourceRequest resourceRequest = null;
	Type loadObjectType = null;

	public AssetLoader(ref string path,Type loadObjectType)
	{
		this.loadObjectType = loadObjectType;

		AssetPath = path;
		RefCount = 1;

		Result = ASSET_LOAD_RESULT_TYPE.NONE;
		LoadObject = null;

		UniqueID = path.GetHashCode();
		state = STATE.RESOURCES_LOAD;
	}

	/// <summary>
	/// 更新処理
	/// </summary>
	/// <returns></returns>
	public bool Update()
	{
		switch (state)
		{
			case STATE.RESOURCES_LOAD:
				if (!asyncLoad)
				{
					resourceRequest = Resources.LoadAsync(AssetPath, loadObjectType);
					asyncLoad = true;
				}
				else
				{
					if (resourceRequest.isDone)
					{
						LoadObject = resourceRequest.asset;
						if (LoadObject != null)
						{
							Result = ASSET_LOAD_RESULT_TYPE.SUCCESS;
						}
						else
						{
							Result = ASSET_LOAD_RESULT_TYPE.FAILURE;
						}
					}
				}
				break;
		}

		return Result != ASSET_LOAD_RESULT_TYPE.NONE;
	}

	/// <summary>
	/// 参照カウントを加算させる
	/// </summary>
	public void AddRefCount()
	{
		RefCount++;
	}

	/// <summary>
	/// 参照カウントを減らす
	/// </summary>
	public void RemoveRefCount()
	{
		RefCount--;
	}

	/// <summary>
	/// 破棄する
	/// </summary>
	public void Unload()
	{

	}
}
