using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject Template;
    public Vector3 SpawnPosition;
    [Range(1, 8)]
    public int BrainAllegiance;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnRobot(bool player)
    {
        var instance = Instantiate(Template, SpawnPosition, Quaternion.identity, null);
        instance.tag = player ? "Player" : "NPC";
        instance.GetComponent<BrainBase>().Allegiance = player ? 1 : BrainAllegiance;
    }

}
