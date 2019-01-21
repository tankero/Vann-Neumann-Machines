using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;


[Serializable]
public class TargetInfo
{

    public Vector3 LastKnownLocation;
    public bool CurrentlyTracking;
    [Range(1, 10)]
    public int Priority;

}

[Serializable]
public class TargetObjectDictionary : SerializableDictionary<GameObject, TargetInfo>
{

}



[Serializable]
public class TargetObjectList : CollectionBase
{
    [SerializeField]
    private TargetObjectDictionary targets = new TargetObjectDictionary();



    public GameObject GetCurrentTargetObject()
    {
        if (targets.Count > 0)
        {
            return targets.Last().Key;
        }
        else
        {
            return null;
        }
    }

    public Vector3? GetLastKnownPosition(GameObject target)
    {
        if (targets.ContainsKey(target))
        {
            return targets[target].LastKnownLocation;
        }
        return null;

    }

    public bool CurrentlyTracking(GameObject target)
    {
        if (targets.ContainsKey(target))
        {
            return targets[target].CurrentlyTracking;
        }
        return false;
    }

    public void TrackObject(GameObject target)
    {
        if (targets.ContainsKey(target))
        {
            targets[target].CurrentlyTracking = true;
            return;
        }

        targets.Add(target, new TargetInfo
        {
            CurrentlyTracking = true,
            LastKnownLocation = target.transform.position,
            Priority = 1
        });
    }

    public void StopTrackingObject(GameObject target)
    {
        if (targets.ContainsKey(target) && targets[target].CurrentlyTracking)
        {
            targets[target].CurrentlyTracking = false;
            targets[target].LastKnownLocation = target.transform.position;
        }
    }

    public void SetPriority(GameObject target, int priority)
    {
        if (targets.ContainsKey(target))
        {
            targets[target].Priority = priority;
        }
        else
        {
            TrackObject(target);
            SetPriority(target, priority);
        }
    }


    public void IncreasePriority(GameObject target)
    {
        if (targets.ContainsKey(target))
        {
            targets[target].Priority += 1;
        }
        else
        {
            TrackObject(target);
            IncreasePriority(target);
        }
    }

    public void DecreasePriority(GameObject target)
    {
        if (targets.ContainsKey(target))
        {
            targets[target].Priority -= 1;
        }
        else
        {
            TrackObject(target);
            DecreasePriority(target);
        }
    }


    public void ListUpdate()
    {
        foreach (var item in targets)
        {
            if (targets[item.Key].CurrentlyTracking)
            {
                targets[item.Key].LastKnownLocation = item.Key.transform.position;
            }
        }
    }

    public void ClearTargetObjects()
    {
        targets.Clear();
    }
}
