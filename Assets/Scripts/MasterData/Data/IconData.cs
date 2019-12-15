using UnityEngine;
using System.Collections;

namespace MasterData
{
    [System.Serializable]
    public class IconData
    {
        [SerializeField]
        int id;
        public int ID { get {return id; } set { id = value;} }
        
        [SerializeField]
        ICONID iconid;
        public ICONID Iconid { get {return iconid; } set { iconid = value;} }
        
        [SerializeField]
        string folder;
        public string Folder { get {return folder; } set { folder = value;} }
        
        [SerializeField]
        string filename;
        public string Filename { get {return filename; } set { filename = value;} }
        
    }
}