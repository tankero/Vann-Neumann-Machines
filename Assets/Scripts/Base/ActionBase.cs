using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class ActionBase : MonoBehaviour
    {

        public enum ActionType
        {
            Heal,
            Damage,
            Scan,
            Work,
            Communicate
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


        public void Use()
        {

        }




    }
}
