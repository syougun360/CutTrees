using UnityEngine;
using System.Collections;

namespace MasterData
{
    [System.Serializable]
    public class WeaponData
    {
        [SerializeField]
        int id;
        public int ID { get {return id; } set { id = value;} }
        
        [SerializeField]
        WEAPONID weaponid;
        public WEAPONID Weaponid { get {return weaponid; } set { weaponid = value;} }
        
        [SerializeField]
        string name;
        public string Name { get {return name; } set { name = value;} }
        
        [SerializeField]
        string modelresname;
        public string Modelresname { get {return modelresname; } set { modelresname = value;} }
        
        [SerializeField]
        int iconid;
        public int Iconid { get {return iconid; } set { iconid = value;} }
        
        [SerializeField]
        int level;
        public int Level { get {return level; } set { level = value;} }
        
        [SerializeField]
        int equiplevel;
        public int Equiplevel { get {return equiplevel; } set { equiplevel = value;} }
        
        [SerializeField]
        int attackpower;
        public int Attackpower { get {return attackpower; } set { attackpower = value;} }
        
        [SerializeField]
        int chargeattackpower;
        public int Chargeattackpower { get {return chargeattackpower; } set { chargeattackpower = value;} }
        
        [SerializeField]
        int hp;
        public int HP { get {return hp; } set { hp = value;} }
        
    }
}