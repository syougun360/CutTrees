using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

namespace MasterData
{
    public partial class GoogleDataAssetUtility
    {
        [MenuItem("Assets/Create/Google/Tree")]
        public static void CreateTreeAssetFile()
        {
            Tree asset = CustomAssetUtility.CreateAsset<Tree>();
            asset.SheetName = "CutTree_TreeDB";
            asset.WorksheetName = "Tree";
            EditorUtility.SetDirty(asset);        
        }
    
    }
}