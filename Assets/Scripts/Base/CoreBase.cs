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

        public StoreBase Storage;

        public int ModuleCapacity;

        // Start is called before the first frame update
        void Start()
        {
            foreach (var module in gameObject.GetComponents<ModuleBase>())
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
                {
                    if (!transform.IsChildOf(module.transform))
                    {
                        transform.parent = module.transform;
                    }
                    return;
                }
            }

            if (!module.transform.IsChildOf(transform))
            {
                module.transform.parent = transform;
            }

            if (module.CompareTag("Mover"))
            {
                var previousMover = Modules.Find(g => g.CompareTag("Mover"));
                if (previousMover)
                {
                    DisconnectModule(previousMover);
                    Modules.Add(module);
                    SendMessageUpwards("OnMoverConnection", module);
                    return;
                }
            }
            if (module.CompareTag("Sensor"))
            {
                var previousSensor = Modules.Find(g => g.CompareTag("Sensor"));
                if (previousSensor)
                {
                    DisconnectModule(previousSensor);
                    Modules.Add(module);
                    SendMessageUpwards("OnSensorConnection", module);
                    return;
                }
            }
            if (module.CompareTag("Tool"))
            {

                Modules.Add(module);
                SendMessageUpwards("OnToolConnection", module);
                return;
            }
        }

        public virtual void DisconnectModule(ModuleBase module)
        {

            if (Modules.Contains(module))
            {
                module.Disable();
                Modules.Remove(module);

                if (Storage.StorageList.Count < Storage.StorageSize)
                {
                    Storage.StoreItem(module);
                }
                else
                {
                    GameManager.DropItem(transform.position, module);
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

    }
}