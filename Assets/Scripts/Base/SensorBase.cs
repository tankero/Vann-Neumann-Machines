using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SensorBase : ModuleBase
{
    // Start is called before the first frame update

    public enum SensorTypeEnum
    {
        Optic,
        Radar,
    }

    public SensorTypeEnum SensorType;
    public CapsuleCollider SensorCollider;
    public float Range;



    public List<GameObject> DetectedObjects;


    private void OnTriggerEnter(Collider collision)
    {
        var intruderObject = collision.gameObject.transform.root.gameObject;
        if (intruderObject.GetComponentInParent<BrainBase>() && !DetectedObjects.Contains(intruderObject) && gameObject.transform.root.gameObject != intruderObject)
        {
            DetectedObjects.Add(intruderObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Debug.Log("Triggered!");
        var intruderObject = collision.gameObject;
        if (intruderObject.GetComponentInParent<BrainBase>())
        {
            DetectedObjects.Remove(intruderObject);
        }
    }

    public bool CheckLoS(GameObject target)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (target.transform.position - transform.position), out hit, Range))
        {
            if (hit.collider.gameObject == target)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator SensorPulse()
    {
        for (; ; )
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
            Debug.Log("Sensor Pulse");
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Start()
    {

        switch (SensorType)
        {
            case SensorTypeEnum.Optic:
                ;
                SensorCollider = gameObject.AddComponent<CapsuleCollider>();
                SensorCollider.direction = 1;
                SensorCollider.radius = 10f;
                SensorCollider.height = Range;
                SensorCollider.isTrigger = true;


                break;
            case SensorTypeEnum.Radar:
                SensorCollider = new CapsuleCollider
                {
                    center = transform.forward * Range / 2,
                    direction = 2,
                    radius = Range,
                    height = 10f,
                    isTrigger = true
                };
                StartCoroutine("SensorPulse");
                break;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

}

