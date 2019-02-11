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

    /*So basically, this is the way that "orders" are executed:
     * 1. The brain checks to see what the current order is. 
     * 2. The order itself pretty much determines if the order is complete with the method below.
     * 3. The brain acts according to what the order dictates.
     * 4. Looping orders don't get removed, they get resorted.
     * Also, attack/move orders are basically the same, except that the brain sets itself to "aggressive" while it's performing attack, and "neutral" while it's performing a move.
    */ 
    public Order GetCurrentOrder()
    {
        
        var oldOrder = OrderList.First();
        switch (oldOrder.OrderType)
        {
            case TypeEnum.Attack:
                if (!oldOrder.Target.activeInHierarchy || (inRegion & Mathf.Abs(Time.time - start) > oldOrder.Duration & !oldOrder.Target.GetComponent<BrainBase>()))
                {
                    OrderList.Remove(oldOrder);
                    inRegion = false;
                    return OrderList.Count > 0 ? OrderList.First() : null;
                }
                break;
              
                
            case TypeEnum.Move:
                if (inRegion & Mathf.Abs(Time.time - start) > oldOrder.Duration)
                {
                    OrderList.Remove(oldOrder);
                    inRegion = false;
                    return OrderList.Count > 0 ? OrderList.First() : null;
                }
                break;
            case TypeEnum.Patrol:
                if (inRegion & Mathf.Abs(Time.time - start) > oldOrder.Duration)
                {
                    OrderList.Remove(oldOrder);
                    OrderList.Add(oldOrder);
                    inRegion = false;
                    return OrderList.Count > 0 ? OrderList.First() : null;
                }
                break;
            case TypeEnum.Work:
                if (inRegion & Mathf.Abs(Time.time - start) > oldOrder.Duration)
                {
                    OrderList.Remove(oldOrder);
                    return OrderList.Count > 0 ? OrderList.First() : null;
                }
                break;
            default:
                break;
        }
        Debug.Log("Returning order type: " + oldOrder.OrderType);
        return oldOrder;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entering region: " +  other.transform.gameObject);
        if (other.transform.gameObject == OrderList.First().Target)
        {
            Debug.Log("Entered target region");
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


