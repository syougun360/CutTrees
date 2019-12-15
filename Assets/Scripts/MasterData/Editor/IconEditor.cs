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
    [CustomEditor(typeof(Icon))]
    public class IconEditor : BaseGoogleEditor<Icon>
    {	    
        public override bool Load()
        {        
            Icon targetData = target as Icon;
        
            var client = new DatabaseClient("", "");
            string error = string.Empty;
            var db = client.GetDatabase(targetData.SheetName, ref error);	
            var table = db.GetTable<IconData>(targetData.WorksheetName) ?? db.CreateTable<IconData>(targetData.WorksheetName);
        
            List<IconData> myDataList = new List<IconData>();
        
            var all = table.FindAll();
            foreach(var elem in all)
            {
                IconData data = new IconData();
            
                data = Cloner.DeepCopy<IconData>(elem.Element);
                myDataList.Add(data);
            }
                
            targetData.dataArray = myDataList.ToArray();
        
            EditorUtility.SetDirty(targetData);
            AssetDatabase.SaveAssets();
        
            return true;
        }
    }
}