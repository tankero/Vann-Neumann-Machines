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
        public AmmunitionBase AmmunitionTemplate;
        [Range(0, 30)]
        public int AmmunitionCapacity;
        public AmmunitionBase[] AmmoPool;
        public int AmmunitionCount;
        public Vector3 MuzzleOffset;
        [Range(0f, 1f)]
        public float Accuracy;
        [Range(0f, 20f)]
        public float Range;


        // Start is called before the first frame update
        void Start()
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
        void Update()
        {

        }

        public void Reload()
        {

        }

        public override void Use()
        {

            if (MaintanceType == MaintenanceTypeEnum.OnUse)
            {
                EnergyCurrent -= EnergyCost;
            }
            if (AmmunitionTemplate != null)
            {
                
                
            }
        }

        public override void Use(GameObject targetObject)
        {

            if (MaintanceType == MaintenanceTypeEnum.OnUse)
            {
                EnergyCurrent -= EnergyCost;
            }

        }
    }
}
