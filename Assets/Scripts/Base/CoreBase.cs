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



        public List<GameObject> Modules;

        public int ModuleCapacity;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }



        public virtual void CalculateEnergyConsumption()
        {

        }

        public virtual void ConnectBrain(GameObject brainModule)
        {
            if (brainModule.CompareTag("Brain"))
            {
                var previousBrain = Modules.Find(g => g.CompareTag("Brain"));
                if (previousBrain)
                {
                    DisconnectModule(previousBrain);
                    Modules.Add(brainModule);
                }
            }
        }

        public virtual void ConnectModule(GameObject module)
        {

        }

        public virtual void DisconnectModule(GameObject module)
        {

        }

        public virtual void DestroyModule(GameObject module)
        {

        }

        public virtual bool ValidateConnection(GameObject module)
        {
            return false;
        }
        IEnumerator FindModuleType(string type)
        {
            foreach (GameObject gObject in Modules)
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