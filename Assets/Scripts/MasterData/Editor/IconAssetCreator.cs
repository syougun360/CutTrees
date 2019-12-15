using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

namespace MasterData
{
    public partial class GoogleDataAssetUtility
    {
        [MenuItem("Assets/Create/Google/Icon")]
        public static void CreateIconAssetFile()
        {
            Icon asset = CustomAssetUtility.CreateAsset<Icon>();
            asset.SheetName = "CutTree_IconDB";
            asset.WorksheetName = "Icon";
            EditorUtility.SetDirty(asset);        
        }
    
    }
}