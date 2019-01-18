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

        var intruderObject = collision.gameObject;
        if (intruderObject.GetComponentInParent<BrainBase>())
        {
            DetectedObjects.Remove(intruderObject);
        }
    }

    public bool CheckLoS(GameObject target)
    {
        //Destroy(GetComponent<SphereCollider>());
        var direction = (target.transform.root.position - transform.position) + new Vector3(0f, 0.6f, 0f);
        var offset = transform.position + new Vector3(0f, 0.6f, 0f) + (direction * 0.2f);

        RaycastHit hit = new RaycastHit();
        
        Debug.DrawRay(offset, direction, Color.red, 5f);
        if (Physics.Raycast(offset, direction, out hit, Range))
        {

            if (hit.collider.transform.root.gameObject == target)
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

            var keepList = new List<GameObject>();
            var target = GameObject.FindGameObjectWithTag("Player");
            if (CheckLoS(target))
            {
                keepList.Add(target);
            }
            

            DetectedObjects = keepList;

            Debug.Log("Sensor Pulse");
            yield return new WaitForSeconds(0.1f);
        }


    }

    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {

    }


    public override void ModuleEnable()
    {
        base.ModuleEnable();
        switch (SensorType)
        {
            case SensorTypeEnum.Optic:

                var SensorSphere = gameObject.AddComponent<SphereCollider>();

                SensorSphere.radius = Range;

                SensorSphere.isTrigger = true;

                StartCoroutine("SensorPulse");
                break;
            case SensorTypeEnum.Radar:
                var SensorCollider = gameObject.AddComponent<CapsuleCollider>();


                SensorCollider.center = transform.forward * Range / 2;
                SensorCollider.direction = 2;
                SensorCollider.radius = Range;
                SensorCollider.height = 10f;
                SensorCollider.isTrigger = true;

                StartCoroutine("SensorPulse");
                break;
        }
    }
}

