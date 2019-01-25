﻿using MoenenGames.VoxelRobot;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using UnityEngine;


[RequireComponent(typeof(Health))]
public class BrainBase : ModuleBase
{


    /*States are managed by the "brain" component, but each module is supposed to react to the state of the entity on their own. 
    The brain basically broadcasts the state to the modules.*/
    public enum BrainStateEnum
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

    public enum TargetStateEnum
    {
        Ok,
        OutOfRange,
        OutOfEnergy,
        OutOfAmmo,
        Invalid
    }


    public bool IAmPlayer;

    [SerializeField]
    public List<ToolBase> ToolList;
    private int selectedToolIndex;
    private ModuleBase sensor;
    private RobotTurret turret;
    private RobotMovement mover;

    public GameObject CurrentTarget;
    public AttitudeEnum BrainAttitude;

    [Range(1, 8)]
    public int Allegiance;

    public List<string> Memory;

    public TargetObjectList Targets;
    public TargetLocationList Locations;

    public BrainStateEnum BrainState;

    private ToolBase selectedTool;


    void Awake()
    {
        IAmPlayer = CompareTag("Player");
        selectedToolIndex = 0;

        if (!IAmPlayer)
        {
            turret = GetComponentInChildren<RobotTurret>();
            mover = GetComponent<RobotMovement>();
        }
        Targets = new TargetObjectList();
        Locations = new TargetLocationList();
    }

    void Start()
    {



    }

    private void Update()
    {
        //Check if we're dead
        if (GetComponent<Health>().CurrentHealth <= 0)
        {
            if (IAmPlayer)
            {
                GameObject.FindGameObjectWithTag("Manager").SendMessage("OnPlayerDeath", transform.position);
                return;
            }

            GameObject.FindGameObjectWithTag("Manager").SendMessage("OnNPCDeath", gameObject);
            return;

        }
        if (!IAmPlayer) return;
        if (!selectedTool)
        {
            selectedTool = ToolList.First();
        }

        if (selectedTool)
        {
            if (Input.GetMouseButtonDown(0))

                if (selectedTool.TriggerType == ToolBase.ToolTriggerType.Activate)
                {
                    selectedTool.Activate();
                }
                else
                {
                    selectedTool.Use();
                }

            if (Input.GetMouseButtonUp(0))
            {
                if (!selectedTool)
                {
                    selectedTool = ToolList.First();
                }

                if (selectedTool && selectedTool.TriggerType == ToolBase.ToolTriggerType.Activate)
                {
                    selectedTool.Deactivate();
                }
            }
        }


    }


    public void StopAttacking()
    {
        turret.TargetObject = null;
        selectedTool.Deactivate();
    }

    public void Think()
    {

        Targets.ListUpdate();



        if (BrainState == BrainStateEnum.Active)
        {
            switch (BrainAttitude)
            {
                // I want to attack things
                case AttitudeEnum.Aggressive:


                    //So lets figure out if I have a target to attack.

                    foreach (var trackedTarget in Targets.GetTrackedList())
                    {
                        CurrentTarget =
                            GameManager.CheckAlleigance(this, trackedTarget.GetComponent<BrainBase>()) ==
                            AllegianceManager.AllegianceEnum.Enemy
                                ? trackedTarget
                                : CurrentTarget;
                        //Now let's see if I can attack my target with any of my weapons
                        if (CurrentTarget)
                        {
                            Attack(CurrentTarget);
                            return;
                        }

                    }
                    



                    //If I can do something else, do that instead (heal)

                    //Okay, so I can't do something else.

                    //If I'm out of range or my target is out of sight, move to attack!

                    //If I'm out of energy or ammo on all my weapons, move to get away :(

                    break;
                case AttitudeEnum.Protective:
                    break;
                case AttitudeEnum.Fearful:
                    break;
                case AttitudeEnum.Neutral:
                    break;
                default:
                    break;
            }



            //var destination = CurrentTarget? Targets.GetLastKnownPosition(CurrentTarget) : null;
            //mover.Destination = destination == null ? transform.position : destination.Value;
            turret.TargetObject = CurrentTarget;


            // Check State & target priority

            // Set Attitude

            // Set Action
        }
    }

    void OnTargetDetected(GameObject newTarget)
    {
        Targets.TrackObject(newTarget);
    }

    void OnTargetLost(GameObject lostTarget)
    {
        if (lostTarget == CurrentTarget)
        {
            CurrentTarget = null;
            selectedTool.Deactivate();
            turret.TargetObject = null;
            mover.Destination = Targets.GetLastKnownPosition(lostTarget);
        }
        Targets.StopTrackingObject(lostTarget);

    }

    public void Assist() { }

    //Convert this to a coroutine that basically checks that the target is visible/in-range/alive in order to attack it. Use the tool's use or Activate method accordingly.
    public void Attack(GameObject target)
    {
        if (turret) turret.TargetObject = target;
        var weapon = ToolList.Where(t => t.Effect == ToolBase.ActionType.Damage).OrderBy(r => r.Range).First();

        selectedTool = weapon;
        //If I can, attack!
        switch (ValidateTarget(target))
        {
            case TargetStateEnum.Ok:
                Debug.Log(gameObject.name + " reports attacking.");
                if (selectedTool.TriggerType == ToolBase.ToolTriggerType.Activate)
                {
                    selectedTool.Activate();
                    mover.Stop();

                }
                return;
            case TargetStateEnum.OutOfRange:
                Debug.Log(gameObject.name + " reports out of range.");

                if (Vector3.Distance(transform.position, target.transform.position) > 2f)
                {
                    selectedTool.Deactivate();
                    mover.Destination = target.transform.position;
                }

                return;
                ;


        }


    }

    public void RequestMove() { }
    public void Work() { }

    public TargetStateEnum ValidateTarget(GameObject target)
    {
        Debug.Log("Validating target: " + target.name + " for " + name);
        Debug.Log("Range to target: " + Vector3.Distance(transform.position, target.transform.position) + " for " + name);
        if (selectedTool.ModuleState == ModuleStateEnum.Recharging)
        {
            return TargetStateEnum.OutOfEnergy;
        }

        var toolAction = selectedTool;


        if (Vector3.Distance(transform.position, target.transform.position) > toolAction.Range)
        {
            return TargetStateEnum.OutOfRange;
        }
        if (!Targets.CurrentlyTracking(target))
        {
            return TargetStateEnum.OutOfRange;
        }
        if (toolAction.UsesAmmunition && toolAction.AmmunitionCount <= 0)
        {
            return TargetStateEnum.OutOfAmmo;
        }
        var targetBrain = target.GetComponent<BrainBase>();
        if (targetBrain)
        {
            var allegiance = GameManager.CheckAlleigance(this, targetBrain);
            if (allegiance == AllegianceManager.AllegianceEnum.Ally && toolAction.Effect == ToolBase.ActionType.Damage)
            {
                return TargetStateEnum.Invalid;
            }
        }
        return TargetStateEnum.Ok;
    }

    public void Use([CanBeNull] GameObject target)
    {
        if (target)
        {
            selectedTool.SendMessage("Use", target);
        }

    }

    IEnumerator ThinkPulse()
    {
        for (; ; )
        {
            if (BrainState == BrainStateEnum.Active)
            {
                Think();
            }
            yield return new WaitForSeconds(0.5f);
        }


    }

    void OnToolConnection(ToolBase connectingTool)
    {
        if (!ToolList.Contains(connectingTool))
        {
            ToolList.Add(connectingTool);
            connectingTool.ModuleEnable();
        }
    }

    public ModuleBase GetSelectedTool()
    {
        if (selectedTool == null)
        {
            selectedTool = ToolList.First();
        }
        return selectedTool;
    }

    public void SetSelectedTool(ToolBase tool)
    {
        selectedToolIndex = ToolList.IndexOf(tool);
    }

    void OnSensorConnection(ModuleBase connectingSensor)
    {
        if (connectingSensor.CompareTag("Sensor"))
        {
            sensor = connectingSensor;
            sensor.ModuleEnable();
            if (!IAmPlayer)
            {
                StartCoroutine("ThinkPulse");

            }
        }
    }

    void ConnectToCore(CoreBase coreParam)
    {
        coreParam.ConnectModule(this);
        ToolList = coreParam.Modules.OfType<ToolBase>().ToList();
    }


    void DisconnectFromCore()
    {
        Disable();
    }

    public override void Disable()
    {
        base.Disable();
        ToolList.Clear();
        Targets.ClearTargetObjects();

    }





}

