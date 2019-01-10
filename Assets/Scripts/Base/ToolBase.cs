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
        public Vector3 MuzzleOffset;
        [Range(0f, 1f)]
        public float Accuracy;
        [Range(0f, 20f)]
        public float Range;


        // Start is called before the first frame update
        public virtual void Start()
        {
            
            if(AmmunitionCapacity > 0)
            {
                AmmoPool = new AmmunitionBase[AmmunitionCapacity];
                for (int i = 0; i < AmmunitionCapacity; i++)
                {
                    AmmoPool[i] = Instantiate(AmmunitionTemplate, transform);
                    AmmoPool[i].gameObject.SetActive(false);
                }
            }
        }

        // Update is called once per frame
        public virtual void Update()
        {

        }

        public virtual void Reload()
        {

        }

        public void Use()
        {
            if (MaintanceType == MaintenanceTypeEnum.OnUse)
            {
                EnergyCurrent -= EnergyCost;
            }

        }
    }
}
