using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class TargetLocationList : CollectionBase
{
    

    [SerializeField]
    private List<Vector3> targets = new List<Vector3>();

    public Vector3? GetCurrentTargetLocation()
    {
        if (targets.Count > 0)
        {
            return targets.Last(); 
        }
        return null;
    }

    public void RemoveCurrentTargetLocation()
    {
        if (targets.Count > 0)
        {
            targets.RemoveAt(targets.Count - 1);
        }

    }
    //Adds the given given GameObject to the targetlist
    public void AddTargetLocation(Vector3 target)
    {
        if (!targets.Contains(target))
        {
            targets.Add(target);
        }
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
