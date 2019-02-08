using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

public class Orders : MonoBehaviour
{

    public enum TypeEnum
    {
        Attack,
        Move,
        Patrol,
        Work
    }

    


    public List<Order> OrderList;

    public int CurrentOrderIndex;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Reset()
    {
        
    }


    public Order GetCurrentOrder()
    {
        //Execute the order or get the next one.
    }
}
[Serializable]
public class Order
{
    public Orders.TypeEnum OrderType;
    public GameObject Target;
    public float Duration;

}
