using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

namespace MasterData
{
    public partial class GoogleDataAssetUtility
    {
        [MenuItem("Assets/Create/Google/Weapon")]
        public static void CreateWeaponAssetFile()
        {
            Weapon asset = CustomAssetUtility.CreateAsset<Weapon>();
            asset.SheetName = "CutTree_WeaponDB";
            asset.WorksheetName = "Weapon";
            EditorUtility.SetDirty(asset);        
        }
    
    }
}