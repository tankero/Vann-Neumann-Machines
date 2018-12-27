using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BrainBase : MonoBehaviour
{

    public string Name;
    /*States are managed by the "brain" component, but each module is supposed to react to the state of the entity on their own. 
    The brain basically broadcasts the state to the modules.*/
    public enum StateEnum
    {
        // Idle is intended to be the "off" state. Unresponsive, unpowered, etc..
        Idle,
        // Active is intended to be the state where things are, indeed, acting. This could be patrolling, attacking, fleeing, etc.
        Active,
        // Passive is the "Ready" state. Listening but not performing any actions yet.
        Passive
    }
    public enum AttitudeEnum
    {
        Aggressive,
        Protective,
        Fearful,
        Neutral
    }   


    public int Allegiance;
        
    public List<string> Memory;

    public TargetObjectList Targets;
    public TargetLocationList Locations;

    private StateEnum currentState;
    public StateEnum CurrentState
    {
        get { return currentState; }

        //Validate and set received state if appropriate.
        set { currentState = value; }
    }

    public virtual void Think()
    {
        
    }

    public abstract void Assist();
    public abstract void Attack();

    public abstract void Move();
    public abstract void Work();

}
