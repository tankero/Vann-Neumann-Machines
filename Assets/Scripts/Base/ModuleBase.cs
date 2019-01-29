using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



    
    public class ModuleBase : MonoBehaviour
    {

        
        public Image Icon;
        public ModuleStateEnum ModuleState;
        [Range(0f, 100f)]
        public float EnergyTotal;
        [Range(0f, 100f)]
        public float EnergyCurrent;
        [Range(0f, 50f)]
        public float EnergyCost;

    public enum MaintenanceTypeEnum
    {
        OnUse,
        OnRecharge,
        Constant
    }
    //Recharge time in milliseconds.
    public float EnergyRate;
        public MaintenanceTypeEnum MaintanceType;
        // Start is called before the first frame update
        void Start()
        {
            //ModuleEnable();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            //ModuleEnable();
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
                EnergyNeeded += EnergyRate * GameManager.TimeConstant;
            }

            if (MaintanceType == MaintenanceTypeEnum.Constant)
                EnergyNeeded += EnergyRate;

            return EnergyNeeded;
        }


        public void Charge(float energyCount)
        {

            EnergyCurrent += energyCount;
            if (ModuleState == ModuleStateEnum.Recharging)
            {
                if(EnergyCost <= EnergyCurrent)
                {
                    ModuleState = ModuleStateEnum.Ready;
                }
                if (EnergyCurrent >= EnergyTotal)
                {
                    EnergyCurrent = EnergyTotal;
                    return;
                }

            }
        }

        public virtual void ModuleEnable()
        {
            if (ModuleState == ModuleStateEnum.Disabled)
            {
                ModuleState = ModuleStateEnum.Recharging;
                EnergyCurrent = 0f;
            }
        }


        public virtual void Disable()
        {
            ModuleState = ModuleStateEnum.Disabled;
            EnergyCurrent = 0f;
        }
    }

