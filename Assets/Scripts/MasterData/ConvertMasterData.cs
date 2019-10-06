#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using MsgPack;
using System.IO;
using System;

public class ConvertMasterData : EditorWindow
{
	string fileName = "";
	string masterClassName = "";

	[MenuItem("Window/ConvertMasterData")]
	static void Open()
	{
		var window = GetWindow<ConvertMasterData>();
		window.titleContent = new GUIContent("マスターデータ変換");
		window.maximized = false;
		window.maxSize = new Vector2(300.0f, 200.0f);
		window.Show();
	}

	private void OnGUI()
	{
		fileName = EditorGUILayout.TextField("ファイル名：", fileName);
		masterClassName = EditorGUILayout.TextField("クラス名：", masterClassName);

		GUILayout.Space(10.0f);

		if (GUILayout.Button("コンバート"))
		{
			var text = File.ReadAllText(Application.dataPath + "/../MasterDataJson/" + fileName + ".json");
			Type type = Type.GetType(masterClassName);
			var jsonData = JsonUtility.FromJson(text, type);
			var msgPack = MasterDataUtility.Seialize(jsonData);
			File.WriteAllBytes(Application.dataPath + "/Resources/master_data/" + fileName + ".bytes", msgPack);

			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
		}
	}
}
#endif