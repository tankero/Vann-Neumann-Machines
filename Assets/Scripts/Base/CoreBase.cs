using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreBase : ModuleBase
{

    [Range(-1f, 10f)]
    public float EnergyGenerationRate;

    [SerializeField]
    public List<ModuleBase> Modules;

    public StoreBase Storage;

    public int ModuleCapacity;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var module in transform.parent.GetComponentsInChildren<ModuleBase>())
        {
            ConnectModule(module);
        }
        StartCoroutine("CoreGenerator");
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void ConnectModule(ModuleBase module)
    {
        Debug.Log("Connection Requested for: " + module.name);
        if (module.CompareTag("Brain") || module.CompareTag("Player"))
        {
            Debug.Log("Connecting Brain");
            {
                if (!transform.IsChildOf(module.transform))
                {
                    transform.parent = module.transform;
                }
                return;
            }
        }

        if (!module.transform.IsChildOf(transform))
        {
            module.transform.parent = transform;
        }

        if (module.CompareTag("Mover"))
        {
            var previousMover = Modules.Find(g => g.CompareTag("Mover"));
            if (previousMover)
            {
                DisconnectModule(previousMover);
            }
            Modules.Add(module);
            SendMessageUpwards("OnMoverConnection", module);
            return;

        }
        if (module.CompareTag("Sensor"))
        {
            var previousSensor = Modules.Find(g => g.CompareTag("Sensor"));
            if (previousSensor)
            {
                DisconnectModule(previousSensor);
            }
            Modules.Add(module);
            SendMessageUpwards("OnSensorConnection", module);
            return;

        }
        if (module.CompareTag("Tool"))
        {
            Modules.Add(module);
            SendMessageUpwards("OnToolConnection", module);
            return;
        }
    }

    public void DisconnectModule(ModuleBase module)
    {

        if (Modules.Contains(module))
        {
            module.Disable();
            Modules.Remove(module);

            if (Storage.StorageList.Count < Storage.StorageSize)
            {
                Storage.StoreItem(module);
            }
            else
            {
                GameManager.DropItem(transform.position, module);
            }
        }
    }



    IEnumerator CoreGenerator()
    {
        for (; ; )
        {
            if (EnergyCurrent < EnergyTotal)
            {
                EnergyCurrent += (EnergyGenerationRate * GameManager.TimeConstant) * 10;
                Debug.Log("Energy Generated: " + (EnergyGenerationRate * GameManager.TimeConstant) * 10);
            }
            if (EnergyCurrent > EnergyTotal || EnergyTotal - EnergyCurrent < 0.1f)
            {
                EnergyCurrent = EnergyTotal;
            }
            foreach (var module in Modules)
            {
                var energyRequested = module.EnergyNeeded();
                if (energyRequested <= 0)
                {
                    continue;
                }
                if (EnergyCurrent >= energyRequested)
                {
                    EnergyCurrent -= energyRequested;
                    module.Charge(energyRequested);
                }
                else
                {
                    module.Charge(EnergyCurrent);
                    EnergyCurrent = 0;
                }

            }
            yield return new WaitForSeconds(GameManager.TimeConstant);
        }
    }

    public void DestroyModule(ModuleBase module)
    {

    }



}
