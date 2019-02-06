using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBase : MonoBehaviour
{

    public enum TypeEnum
    {
        Move,
        Attack,
        Patrol,
        Work
    }
    public TypeEnum OrderType;
    public GameObject Target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
