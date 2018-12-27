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
            Hack,
            Work
        }
        public enum ActionState
        {
            Ready,
            Recharging,
            Disabled

        }
        public enum MaintenanceTypeEnum
        {
            OnUse,
            OnRecharge,
            Constant
        }

        
        public ActionType Effect;
        public float EnergyCost;
        public float EffectAmount;
        public float RechargeTime;


        public ActionState State { get; set; }
        private GameObject target;

        public GameObject GetTarget()
        { return target; }

        public void SetTarget(GameObject value)
        {
            if (ValidateTarget(value))
                target = value;
        }

        public abstract bool ValidateTarget(GameObject target);
        public abstract bool ValidateState();
        public abstract void Use();


        public virtual void Charge()
        {
            if(State == ActionState.Recharging)
            {
                State = ActionState.Ready;
            }
        }
        
        public virtual void Enable()
        {
            if(State == ActionState.Disabled)
            {
                State = ActionState.Recharging;
            }
        }
        public virtual void Disable()
        {
            State = ActionState.Disabled;
        }

    }
}
