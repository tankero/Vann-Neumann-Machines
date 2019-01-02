using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class ActionBase : MonoBehaviour
    {
        public string Name;
        public enum ActionType
        {
            Move,
            Heal,
            Damage,
            Scan,
            Work

        }

       
        public ActionType Effect;
        public float EffectAmount;


        private GameObject target;

        public GameObject GetTarget()
        { return target; }

        public void SetTarget(GameObject value)
        {
          
                target = value;
        }


        public virtual void Use()
        {

        }


        

    }
}
