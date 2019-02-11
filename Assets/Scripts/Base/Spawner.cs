using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject RobotTemplate;
    public Transform SpawnPosition;
    [Range(1, 8)]
    public int BrainAllegiance;
    public GameObject BrainTemplate;
    private GameObject[] brainList;



    // Start is called before the first frame update
    void Start()
    {
        //Instantiate some brains here for later use
        if (SpawnPosition == null)
        {
            SpawnPosition = transform.Find("_s");
        }
        brainList = new GameObject[10];
        for (var index = 0; index < brainList.Length; index++)
        {
            brainList[index] = Instantiate(BrainTemplate, transform.position, Quaternion.identity, null);
            brainList[index].SetActive(false);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnTemplate(GameObject brainObject)
    {
        Instantiate(RobotTemplate, SpawnPosition.position, Quaternion.identity, brainObject ? brainObject.transform : GetNextBrain().transform);

    }

    public GameObject GetNextBrain()
    {
        var first = brainList.FirstOrDefault(b => !b.gameObject.activeInHierarchy);
        if (first != null)
        {
            SetOrders(first);
        }

        var newBrainPool = new GameObject[brainList.Length + 1];
        newBrainPool[0] = Instantiate(BrainTemplate);
        SetOrders(newBrainPool[0]);
        newBrainPool[0].gameObject.SetActive(false);

        for (int i = 1; i < newBrainPool.Length; i++)
        {
            newBrainPool[i] = brainList[i - 1];
        }
        brainList = newBrainPool;
        return brainList[0];

    }
    private void SetOrders(GameObject instance)
    {
        var orders = instance.GetComponent<Orders>();
        if (orders)
        {
            orders.OrderList.Clear();
        }
        else
        {
            instance.AddComponent<Orders>();
        }
        foreach (var templateOrder in GetComponent<Orders>().OrderList)
        {
            orders.OrderList.Add(new Order
            {
                Target = templateOrder.Target,
                Duration = templateOrder.Duration,
                OrderType = templateOrder.OrderType

            });
        }
    }
}
