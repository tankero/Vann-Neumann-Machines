using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryBase : MonoBehaviour
{
    // Start is called before the first frame update


    public ChargeType Type;

    [Range(1, 100)]
    public int ChargeAmount;

    public enum ChargeType
    {
        Energy,
        Ammunition,
        Health

    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Expend()
    {
        switch (Type)
        {
            case ChargeType.Energy:
                break;
            case ChargeType.Ammunition:
                var receiver = transform.parent.gameObject.GetComponent<ToolBase>();
                if (receiver == null) return;
                int ammoNeeded = receiver.AmmunitionCapacity - receiver.AmmunitionCount;
                receiver.AmmunitionCapacity += ammoNeeded <= ChargeAmount ? ammoNeeded : ChargeAmount;
                break;
            case ChargeType.Health:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }


    }
}
