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
    [CustomEditor(typeof(Weapon))]
    public class WeaponEditor : BaseGoogleEditor<Weapon>
    {	    
        public override bool Load()
        {        
            Weapon targetData = target as Weapon;
        
            var client = new DatabaseClient("", "");
            string error = string.Empty;
            var db = client.GetDatabase(targetData.SheetName, ref error);	
            var table = db.GetTable<WeaponData>(targetData.WorksheetName) ?? db.CreateTable<WeaponData>(targetData.WorksheetName);
        
            List<WeaponData> myDataList = new List<WeaponData>();
        
            var all = table.FindAll();
            foreach(var elem in all)
            {
                WeaponData data = new WeaponData();
            
                data = Cloner.DeepCopy<WeaponData>(elem.Element);
                myDataList.Add(data);
            }
                
            targetData.dataArray = myDataList.ToArray();
        
            EditorUtility.SetDirty(targetData);
            AssetDatabase.SaveAssets();
        
            return true;
        }
    }
}