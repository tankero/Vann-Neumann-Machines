using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ToolBase : ModuleBase
    {


        public enum ToolTypeEnum
        {
            Projectile,
            Beam,
            Meele
        }

        public ToolTypeEnum ToolType;
        [Range(0f, 1f)]
        public float Accuracy;
        [Range(0f, 20f)]
        public float Range;
        [Range(0, 30)]
        public int AmmunitionCapacity;
        public bool UsesAmmunition;
        public int AmmunitionCount;

        // Start is called before the first frame update
        public virtual void Start()
        {
            gameObject.tag = "Tool";

        }

        // Update is called once per frame
        public virtual void Update()
        {

        }

        public virtual void Reload()
        {

        }

        public virtual void Use()
        {
            if (MaintanceType == MaintenanceTypeEnum.OnUse)
            {
                EnergyCurrent -= EnergyCost;
            }

        }
    }
}
