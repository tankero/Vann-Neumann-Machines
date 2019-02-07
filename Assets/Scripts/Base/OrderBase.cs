using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

public class OrderBase : MonoBehaviour
{

    public enum TypeEnum
    {
        Move,
        Attack,
        Patrol,
        Wait,
        Work
    }

    [Serializable]
    public class Order
    {
        public TypeEnum OrderType;
        public GameObject Target;
        public float Duration;

    }

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

    void ExecuteNextOrder()
    {

        var order = OrderList.First();
        switch (order.OrderType)
        {
            case TypeEnum.Move:
                break;
            case TypeEnum.Attack:
                break;
            case TypeEnum.Patrol:
                break;
            case TypeEnum.Wait:
                StartCoroutine("OrderWait", order.Duration);
                break;
            case TypeEnum.Work:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        OrderList.Remove(order);
    }

    IEnumerator OrderWait(float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    IEnumerator OrderMove(GameObject target)
    {
        // Think about this some more. Is this really the structure you want?

    }
    void NextObjective()
    {

    }



}
