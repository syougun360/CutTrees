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
    public enum MASTER_ID
    {
        WEAPON,
    }

    class ConvertData
    {
        public ConvertData(string file, string className)
        {
            fileName = file;
            masterClassName = className;
        }

        public string fileName = "";
        public string masterClassName = "";
    }

    MASTER_ID id;
    readonly ConvertData[] CONVERT_DATA = new ConvertData[] {
        new ConvertData("weapon","Weapon.WeaponInfoMasterData")
    };

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
        id = (MASTER_ID)EditorGUILayout.EnumPopup("マスタID", id);

		GUILayout.Space(10.0f);

		if (GUILayout.Button("コンバート"))
		{
            var fileName = CONVERT_DATA[(int)id].fileName;
            var masterClassName = CONVERT_DATA[(int)id].masterClassName;

            var text = File.ReadAllText(Application.dataPath + "/../MasterDataJson/" + fileName + ".json");
			Type type = Type.GetType(masterClassName);
			var jsonData = JsonUtility.FromJson(text, type);
			var msgPack = MasterDataUtility.Seialize(jsonData);
			File.WriteAllBytes(Application.dataPath + "/Resources/master_data/" + fileName + ".bytes", msgPack);

			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            Debug.Log("コンバート成功");
		}
	}
}
#endif