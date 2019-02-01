using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject Template;
    public Transform SpawnPosition;
    [Range(1, 8)]
    public int BrainAllegiance;

    

    // Start is called before the first frame update
    void Start()
    {
        if (SpawnPosition == null)
        {
            SpawnPosition = transform.Find("_s");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnRobot(bool player, GameObject brainObject)
    {
        var instance = Instantiate(Template, SpawnPosition.position, Quaternion.identity, null);
        instance.tag = player ? "Player" : "NPC";
        if (brainObject)
        {
            instance.transform.parent = brainObject.transform;
        }
        instance.transform.root.GetComponent<BrainBase>().Allegiance = player ? 1 : BrainAllegiance;

    }

}
