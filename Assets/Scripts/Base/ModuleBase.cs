using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public abstract class ModuleBase : MonoBehaviour
    {

        public string Name;
        public Image Icon;
        public ModuleStateEnum State;
        public float EnergyTotal;
        [HideInInspector]
        public float EnergyCurrent;
        public float Health;
        public ActionBase Action;
        public enum MaintenanceTypeEnum
        {
            OnUse,
            OnRecharge,
            Constant
        }

        //Recharge time in milliseconds.
        public float RechargeTime;
        public MaintenanceTypeEnum MaintanceType;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public enum ModuleStateEnum
        {
            Ready,
            Recharging,
            Disabled

        }

        public float EnergyNeeded()
        {
            float EnergyNeeded = 0f;
            if (EnergyCurrent < EnergyTotal)
            {
                EnergyNeeded += EnergyTotal / RechargeTime;
            }

            if (MaintanceType == MaintenanceTypeEnum.Constant)
                EnergyNeeded += EnergyTotal;
            return EnergyNeeded;
        }

        public virtual void Charge()
        {
            if (State == ModuleStateEnum.Recharging)
            {
                if (EnergyCurrent >= EnergyTotal)
                {
                    State = ModuleStateEnum.Ready;
                    EnergyCurrent = EnergyTotal;
                    return;
                }
                EnergyCurrent += RechargeTime * Time.deltaTime;
            }
        }

        public virtual void Enable()
        {
            if (State == ModuleStateEnum.Disabled)
            {
                State = ModuleStateEnum.Recharging;
                EnergyCurrent = 0f;
            }
        }


        public virtual void Disable()
        {
            State = ModuleStateEnum.Disabled;
            EnergyCurrent = 0f;
        }
    }

}