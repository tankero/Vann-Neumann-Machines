using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{

    [RequireComponent(typeof(Health))]
    public class ModuleBase : MonoBehaviour
    {

        
        public Image Icon;
        public ModuleStateEnum State;
        [Range(0f, 100f)]
        public float EnergyTotal;
        [Range(0f, 100f)]
        public float EnergyCurrent;
        [Range(0f, 50f)]
        public float EnergyCost;
        [HideInInspector]
        public Health ModuleHealth;
        
        public enum MaintenanceTypeEnum
        {
            OnUse,
            OnRecharge,
            Constant
        }

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
        
        //Recharge time in milliseconds.
        public float EnergyRate;
        public MaintenanceTypeEnum MaintanceType;
        // Start is called before the first frame update
        void Start()
        {
            
            ModuleHealth = GetComponent<Health>();
            ModuleEnable();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            ModuleEnable();
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
            Debug.Log("Energy deficit requested from: " + gameObject.name + " -- Requested: " + EnergyNeeded);
            return EnergyNeeded;
        }


        public void Charge(float energyCount)
        {
            Debug.Log("Energy charge received by: " + gameObject.name + " -- Received: " + energyCount);
            EnergyCurrent += energyCount;
            if (State == ModuleStateEnum.Recharging)
            {
                if(EnergyCost <= EnergyCurrent)
                {
                    State = ModuleStateEnum.Ready;
                }
                if (EnergyCurrent >= EnergyTotal)
                {
                    EnergyCurrent = EnergyTotal;
                    return;
                }

            }
        }

        public void ModuleEnable()
        {
            if (State == ModuleStateEnum.Disabled)
            {
                State = ModuleStateEnum.Recharging;
                EnergyCurrent = 0f;
            }
        }


        public void Disable()
        {
            State = ModuleStateEnum.Disabled;
            EnergyCurrent = 0f;
        }
    }

}