using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts
{
    public abstract class CoreBase : MonoBehaviour
    {
        [Range(0f, 100f)]
        public float EnergyCapacity;

        [Range(-10f, 10f)]
        public float EnergyGenerationRate;



        public List<ModuleBase> Modules;

        public int ModuleCapacity;

        // Start is called before the first frame update
        void Start()
        {
            foreach(var module in gameObject.GetComponents<ModuleBase>())
            {
                ConnectModule(module);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }



        public virtual void ConnectModule(ModuleBase module)
        {
            if (module.CompareTag("Brain"))
            {
                var previousBrain = Modules.Find(g => g.CompareTag("Brain"));
                if (previousBrain)
                {
                    DisconnectModule(previousBrain);
                    Modules.Add(module);
                }
            }
            if (module.CompareTag("Mover"))
            {
                var previousMover = Modules.Find(g => g.CompareTag("Mover"));
                if (previousMover)
                {
                    DisconnectModule(previousMover);
                    Modules.Add(module);
                }
            }
            if (module.CompareTag("Sensor"))
            {
                var previousSensor = Modules.Find(g => g.CompareTag("Sensor"));
                if (previousSensor)
                {
                    DisconnectModule(previousSensor);
                    Modules.Add(module);
                }
            }

        }

        public virtual void DisconnectModule(ModuleBase module)
        {

            if (Modules.Contains(module))
            {
                module.Disable();
                Modules.Remove(module);
                StoreBase storageModule = (StoreBase)Modules.Find(m => m.CompareTag("Storage"));
                if (storageModule && storageModule.State == ModuleBase.ModuleStateEnum.Ready && storageModule.storageList.Count < storageModule.StorageSize)
                {
                    storageModule.StoreItem(module);
                }
                else
                {

                }
            }
        }

        public virtual void DestroyModule(ModuleBase module)
        {

        }

        public virtual bool ValidateConnection(ModuleBase module)
        {
            return false;
        }
        IEnumerator FindModuleType(string type)
        {
            foreach (var gObject in Modules)
            {
                if (gObject.CompareTag(type))
                {
                    yield break;
                }
                yield return null;
            }
        }
    }
}