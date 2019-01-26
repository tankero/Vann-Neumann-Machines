using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject Blueprint;
    public bool PlayerSpawn;
    public Vector3 SpawnOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBlueprint(GameObject blueprint)
    {
        Blueprint = blueprint;
    }

    void Spawn()
    {
        Instantiate(Blueprint, SpawnOffset, Quaternion.identity, null);
    }
}
