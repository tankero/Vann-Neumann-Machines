using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObjectList : CollectionBase
{
    private List<GameObject> targets = new List<GameObject>();

    public GameObject GetCurrentTargetObject()
    {
        return targets[targets.Count];
    }
    
    public GameObject RemoveLastTargetObject()
    {
        if (targets.Count > 0)
        {
            targets.RemoveAt(targets.Count-1);
            return targets[targets.Count];
        }
        else
        {
            return null;
        }
    }
    //Adds the given given GameObject to the targetlist
    public void AddTargetObject(GameObject target)
    {
        targets.Add(target);
    }

    public void RemoveSpecificTargetObject(GameObject target)
    {
        if (targets.Contains(target))
        {
            targets.Remove(target);
        }
    }

    public void ClearTargetObjects()
    {
        targets.Clear();
    }
}
