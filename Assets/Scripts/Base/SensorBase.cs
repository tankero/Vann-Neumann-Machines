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






    private void OnTriggerEnter(Collider collision)
    {
        var intruderObject = collision.gameObject.transform.root.gameObject;
        
        if (intruderObject.GetComponent<BrainBase>() &&  gameObject.transform.root.gameObject != intruderObject && CheckLoS(intruderObject))
        {
            SendMessageUpwards("OnTargetDetected", intruderObject);
        }
        
    }

    private void OnTriggerExit(Collider collision)
    {

        var intruderObject = collision.transform.root.gameObject;
        if (intruderObject.GetComponentInParent<BrainBase>())
        {
            SendMessageUpwards("OnTargetLost", intruderObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if(other.transform.root.gameObject == gameObject.transform.root.gameObject)
        {
            return;
        }

        if (!CheckLoS(other.gameObject))
        { 
            SendMessageUpwards("OnTargetLost", other.transform.root.gameObject);
        }
        else
        {
            SendMessageUpwards("OnTargetDetected", other.transform.root.gameObject);
        }




    }

    public bool CheckLoS(GameObject target)
    {
        //Destroy(GetComponent<SphereCollider>());
        var direction = (target.transform.root.position - transform.root.position) + new Vector3(0f, 0.6f, 0f);
        var offset = transform.root.position + new Vector3(0f, 0.6f, 0f) + (direction * 0.2f);

        RaycastHit hit = new RaycastHit();

        
        if (Physics.Raycast(transform.root.position, direction, out hit, Range))
        {

            if (hit.collider.transform.root.gameObject == target.transform.root.gameObject)
            {
                return true;
            }
        }



        return false;
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
        
        switch (SensorType)
        {
            case SensorTypeEnum.Optic:

                var SensorSphere = gameObject.AddComponent<SphereCollider>();

                SensorSphere.radius = Range;

                SensorSphere.isTrigger = true;


                break;
            case SensorTypeEnum.Radar:
                var SensorCollider = gameObject.AddComponent<CapsuleCollider>();


                SensorCollider.center = transform.forward * Range / 2;
                SensorCollider.direction = 2;
                SensorCollider.radius = Range;
                SensorCollider.height = 10f;
                SensorCollider.isTrigger = true;


                break;
        }
        base.ModuleEnable();
    }
}

