using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using GDataDB;
using GDataDB.Linq;

using UnityQuickSheet;

namespace MasterData
{
    [CustomEditor(typeof(Tree))]
    public class TreeEditor : BaseGoogleEditor<Tree>
    {	    
        public override bool Load()
        {        
            Tree targetData = target as Tree;
        
            var client = new DatabaseClient("", "");
            string error = string.Empty;
            var db = client.GetDatabase(targetData.SheetName, ref error);	
            var table = db.GetTable<TreeData>(targetData.WorksheetName) ?? db.CreateTable<TreeData>(targetData.WorksheetName);
        
            List<TreeData> myDataList = new List<TreeData>();
        
            var all = table.FindAll();
            foreach(var elem in all)
            {
                TreeData data = new TreeData();
            
                data = Cloner.DeepCopy<TreeData>(elem.Element);
                myDataList.Add(data);
            }
                
            targetData.dataArray = myDataList.ToArray();
        
            EditorUtility.SetDirty(targetData);
            AssetDatabase.SaveAssets();
        
            return true;
        }
    }
}