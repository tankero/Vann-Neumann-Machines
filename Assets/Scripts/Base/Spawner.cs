using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject RobotTemplate;
    public Transform SpawnPosition;
    [Range(1, 8)]
    public int BrainAllegiance;
    public GameObject BrainTemplate;
    public



    // Start is called before the first frame update
    void Start()
    {
        //Instantiate some brains here for later use
        if (SpawnPosition == null)
        {
            SpawnPosition = transform.Find("_s");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnTemplate(GameObject brainObject)
    {
        var instance = Instantiate(RobotTemplate, SpawnPosition.position, Quaternion.identity, null);
        if (brainObject)
        {
            instance.transform.Find("Neck").parent = brainObject.transform;
        }
        else if(BrainTemplate)
        {
            //Pull the brain instances here that were created at the start.
        }


    }

}
