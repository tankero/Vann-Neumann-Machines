using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocationList : CollectionBase
{
    private List<Vector3> targets = new List<Vector3>();

    public Vector3 GetCurrentTargetLocation()
    {
        return targets[targets.Count];
    }

    public void RemoveCurrentTargetLocation()
    {
        if (targets.Count > 0)
        {
            targets.RemoveAt(targets.Count - 1);
        }
        
    }
    //Adds the given given GameObject to the targetlist
    public void AddCurrentTarget(Vector3 target)
    {
        targets.Add(target);
    }

    public void RemoveSpecificTargetLocation(Vector3 target)
    {
        if (targets.Contains(target))
        {
            targets.Remove(target);
        }
    }

    public void ClearTargetLocations()
    {
        targets.Clear();
    }
}
