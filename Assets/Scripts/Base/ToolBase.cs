using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoenenGames.VoxelRobot;
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

        [Header("Setting")]
        [SerializeField]
        private float AttackFrequency = 0.4f;
        
        


        [SerializeField]
        private AmmunitionBase AmmunitionTemplate;
        public AmmunitionBase[] AmmoPool;
        public Weapon[] BarrelSpawnPoints;

        // Start is called before the first frame update
        public virtual void Start()
        {
            gameObject.tag = "Tool";
            
            BarrelSpawnPoints = transform.GetComponentsInChildren<Weapon>().ToArray();
            if (AmmunitionCapacity > 0)
            {
                AmmoPool = new Bullet[AmmunitionCapacity];
                for (int i = 0; i < 6; i++)
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

        public virtual void Use()
        {
            if (MaintanceType == MaintenanceTypeEnum.OnUse)
            {
                EnergyCurrent -= EnergyCost;
            }
            int len = BarrelSpawnPoints.Length;
            foreach (var barrelSpawnPoint in BarrelSpawnPoints)
            {
                
                if (barrelSpawnPoint.PrevAttackTime + AttackFrequency * (len + 1) > Time.time)
                {
                    continue;
                }
                for (int i = 0; i < len; i++)
                {
                    if (barrelSpawnPoint == this)
                    {
                        continue;
                    }
                    if (barrelSpawnPoint.PrevAttackTime + AttackFrequency > Time.time)
                    {
                        continue;
                    }
                }

                barrelSpawnPoint.Fire(GetNextBullet());
            }

        }

        public AmmunitionBase GetNextBullet()
        {
            var first = AmmoPool.FirstOrDefault(b => !b.gameObject.activeInHierarchy);
            if (first == null)
            {
                var newAmmoPull = new AmmunitionBase[AmmoPool.Length + 1];
                newAmmoPull[0] = Instantiate(AmmunitionTemplate);
                newAmmoPull[0].gameObject.SetActive(false);

                for (int i = 1; i < newAmmoPull.Length; i++)
                {
                    newAmmoPull[i] = AmmoPool[i - 1];
                }
                AmmoPool = newAmmoPull;
                return AmmoPool[0];
            }

            return first;
        }
    }
}
