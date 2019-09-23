using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityScene = UnityEngine.SceneManagement.Scene;

/// <summary>
/// シーン管理
/// </summary>
public class SceneManager : ManagerBehaviour<SceneManager>
{
	List<SceneLoader> sceneLoaderList = new List<SceneLoader>();
	List<SceneBase> sceneList = new List<SceneBase>();

	public override MANAGER_TYPE GetManagerType()
	{
		return MANAGER_TYPE.SCENE;
	}

	private void Start()
	{
		// 最初に再生したシーンを設定する。
		UnityScene unityScene = UnitySceneManager.GetActiveScene();
		string unitySceneName = unityScene.name;
		SceneBase scene = GameObject.Find(unitySceneName).GetComponent<SceneBase>();
		scene.Setup(GetSceneID(ref unitySceneName), unityScene);
		sceneList.Add(scene);

		UnitySceneManager.SetActiveScene(unityScene);
	}

	private void Update()
	{
		for (int i = 0; i < sceneLoaderList.Count; i++)
		{
			SceneLoader loader = sceneLoaderList[i];
			bool loaded = loader.Update();
			if (loaded)
			{
				SceneDefine.SCENE_ID id = loader.SceneID;
				UnityScene unityScene = UnitySceneManager.GetSceneByName(loader.Name);
				SceneBase scene = GameObject.Find(unityScene.name).GetComponent<SceneBase>();
				sceneList.Add(scene);

				sceneLoaderList.RemoveAt(i);

				UnitySceneManager.SetActiveScene(unityScene);
				scene.Setup(id, unityScene);
				break;
			}
		}

		for (int i = 0; i < sceneList.Count; i++)
		{
			// アンロード処理
			if (sceneList[i] == null)
			{
				sceneList.RemoveAt(i);
				break;
			}
		}
	}

	/// <summary>
	/// シーンをロードする。
	/// </summary>
	public static void LoadScene(SceneDefine.SCENE_ID id, bool async = true, bool addtive = false)
	{
		Instance.sceneLoaderList.Add(new SceneLoader(id, addtive, async));
	}

	/// <summary>
	/// シーンを破棄する
	/// </summary>
	public static void UnloadScene(SceneDefine.SCENE_ID id)
	{
		List<SceneBase> sceneList = Instance.sceneList;
		for (int i = 0; i < sceneList.Count; i++)
		{
			if (sceneList[i].SceneID == id)
			{
				sceneList[i].StartUnload();
				UnitySceneManager.UnloadSceneAsync(sceneList[i].UnityScene.name);
				break;
			}
		}
	}

	/// <summary>
	/// シーンをアクティブする。
	/// </summary>
	/// <param name="id"></param>
	public static void SetActiveScene(SceneDefine.SCENE_ID id)
	{
		List<SceneBase> sceneList = Instance.sceneList;
		for (int i = 0; i < sceneList.Count; i++)
		{
			if (sceneList[i].SceneID == id)
			{
				UnitySceneManager.SetActiveScene(sceneList[i].UnityScene);
				break;
			}
		}
	}

	/// <summary>
	/// シーンをオブジェクトを取得
	/// </summary>
	/// <param name="id"></param>
	public static SceneBase GetSceneObject(SceneDefine.SCENE_ID id)
	{
		List<SceneBase> sceneList = Instance.sceneList;
		for (int i = 0; i < sceneList.Count; i++)
		{
			if (sceneList[i].SceneID == id)
			{
				return sceneList[i];
			}
		}

		return null;
	}


	/// <summary>
	/// 読込終了したか？
	/// </summary>
	/// <returns></returns>
	public static bool IsLoaded()
	{
		return Instance.sceneLoaderList.Count == 0;
	}

	/// <summary>
	/// 読込終了したか？
	/// </summary>
	/// <returns></returns>
	public static bool IsLoaded(SceneDefine.SCENE_ID id)
	{
		List<SceneLoader> loaderList = Instance.sceneLoaderList;
		for (int i = 0; i < loaderList.Count; i++)
		{
			if (loaderList[i].SceneID == id)
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// 名前からシーンIDを取得
	/// </summary>
	/// <param name="sceneName"></param>
	/// <returns></returns>
	static SceneDefine.SCENE_ID GetSceneID(ref string sceneName)
	{
		int nHash = sceneName.GetHashCode();
		for (int i = 0; i < SceneDefine.SceneNames.Length; i++)
		{
			if (nHash == SceneDefine.SceneNames[i].GetHashCode())
			{
				return (SceneDefine.SCENE_ID)i;
			}
		}

		return SceneDefine.SCENE_ID.NONE;
	}
}
