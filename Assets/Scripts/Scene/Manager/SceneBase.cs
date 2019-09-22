using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScene = UnityEngine.SceneManagement.Scene;

/// <summary>
/// シーンの基底クラス
/// シーンクラスは、このクラスを継承してください
/// </summary>
public abstract class SceneBase : MonoBehaviour
{
	public SceneDefine.SCENE_ID SceneID { get; private set; }
	public UnityScene UnityScene { get; private set;}

	/// <summary>
	/// セットアップ（読込完了）
	/// </summary>
	/// <param name="id"></param>
	public void Setup(SceneDefine.SCENE_ID id, UnityScene unityScene)
	{
		SceneID = id;
		UnityScene = unityScene;

		Loaded();
	}

	protected abstract void Loaded();

	/// <summary>
	/// 破棄開始
	/// </summary>
	public abstract void StartUnload();
}
