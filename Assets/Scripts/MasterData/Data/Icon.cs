using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MasterData
{
    [System.Serializable]
    public class Icon : ScriptableObject 
    {
        [HideInInspector] [SerializeField] 
        public string SheetName = "";
    
        [HideInInspector] [SerializeField] 
        public string WorksheetName = "";
    
        // Note: initialize in OnEnable() not here.
        public IconData[] dataArray;
    
        void OnEnable()
        {
            if (dataArray == null)
                dataArray = new IconData[0];
        }
    }
}