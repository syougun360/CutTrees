using UnityEngine;
using System.Collections;

namespace MasterData
{
    [System.Serializable]
    public class TreeData
    {
        [SerializeField]
        int id;
        public int ID { get {return id; } set { id = value;} }
        
        [SerializeField]
        int hp;
        public int Hp { get {return hp; } set { hp = value;} }
        
    }
}