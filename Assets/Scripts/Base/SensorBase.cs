using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class SensorBase : ModuleBase
    {
        // Start is called before the first frame update

        public enum SensorTypeEnum
        {
            Optic,
            Radar,
        }

        public SensorTypeEnum SensorType;
        public Collider SensorCollider;
        public float Range;



        public List<GameObject> DetectedObjects;


        private void OnTriggerEnter(Collider collision)
        {
            var intruderObject = collision.gameObject;
            if (intruderObject.CompareTag("Enitity"))
            {
                DetectedObjects.Add(intruderObject);
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            var intruderObject = collision.gameObject;
            if (intruderObject.CompareTag("Enitity"))
            {
                DetectedObjects.Remove(intruderObject);
            }
        }

        public bool CheckLoS(GameObject target)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (target.transform.position - transform.position), out hit,  Range))
            {
                if(hit.collider.gameObject == target)
                {
                    return true;
                }
            }
            return false;
        }

        IEnumerator Pulse()
        {
            if (DetectedObjects.Count > 0)
            {
                var keepList = new List<GameObject>();
                foreach (var target in DetectedObjects)
                {
                    if (CheckLoS(target))
                    {
                        keepList.Add(target);
                    }
                    yield return new WaitForSeconds(0.1f);

                }
                DetectedObjects = keepList;
            }
        }

        void Start()
        {

            switch (SensorType)
            {
                case SensorTypeEnum.Optic:
                    SensorCollider = new CapsuleCollider
                    {
                        center = transform.position,
                        direction = 1,
                        radius = 10f,
                        height = Range,
                        isTrigger = true
                    };
                    break;
                case SensorTypeEnum.Radar:
                    SensorCollider = new CapsuleCollider
                    {
                        center = transform.forward * Range/2,
                        direction = 2,
                        radius = Range,
                        height = 10f,
                        isTrigger = true
                    };
                    StartCoroutine("Pulse");
                    break;
            }
           

        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}
