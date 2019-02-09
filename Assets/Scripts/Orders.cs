using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using Time = UnityEngine.Time;

public class Orders : MonoBehaviour
{

    public enum TypeEnum
    {
        Attack,
        Move,
        Patrol,
        Work
    }

    private float start = 0f;
    private bool inRegion = false;
    private float threshold = 0.5f;
    public List<Order> OrderList;

    

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

    //Execute the order or get the next one.
    public Order GetCurrentOrder()
    {
        var oldOrder = OrderList.First();
        switch (oldOrder.OrderType)
        {
            case TypeEnum.Attack:
                if (!oldOrder.Target)
                {
                    OrderList.Remove(oldOrder);
                    return OrderList.Count > 0 ? OrderList.First() : null;
                }

                return oldOrder;
                
            case TypeEnum.Move:
                if (inRegion & Time.time - start < threshold)
                {
                    OrderList.Remove(oldOrder);
                    return OrderList.Count > 0 ? OrderList.First() : null;
                }
                break;
            case TypeEnum.Patrol:
                if (inRegion & Time.time - start < threshold)
                {
                    OrderList.Remove(oldOrder);
                    OrderList.Add(oldOrder);
                    return OrderList.Count > 0 ? OrderList.First() : null;
                }
                break;
            case TypeEnum.Work:
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject == OrderList.First().Target)
        {
            inRegion = true;
            start = Time.time;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject == OrderList.First().Target)
        {
            inRegion = false;
        }
    }


}
[Serializable]
public class Order
{
    public Orders.TypeEnum OrderType;
    public GameObject Target;
    public float Duration;

}


