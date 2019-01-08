using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class AmmunitionBase : MonoBehaviour
    {
        
        public enum TriggerEnum
        {
            OnImpact,
            OnExpiration,
            OnTrigger
        }
        public TriggerEnum Trigger;
        public float Radius;
        public float Duration;
        
        

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
      

    }
}
