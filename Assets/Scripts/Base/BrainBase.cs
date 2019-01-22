using MoenenGames.VoxelRobot;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    private bool IAmPlayer;

    [SerializeField]
    public List<ToolBase> ToolList;
    private int selectedToolIndex;
    private ModuleBase sensor;
    private RobotTurret turret;
    private RobotMovement mover;

    public AttitudeEnum BrainAttitude;

    [Range(1, 8)]
    public int Allegiance;

    public List<string> Memory;

    public TargetObjectList Targets;
    public TargetLocationList Locations;

    public BrainStateEnum BrainState;


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

        if (Input.GetMouseButtonDown(0))
            if (ToolList[selectedToolIndex].TriggerType == ToolBase.ToolTriggerType.Activate)
            {
                ToolList[selectedToolIndex].Activate();
            }
            else
            {
                ToolList[selectedToolIndex].Use();
            }

        if (Input.GetMouseButtonUp(0))
        {
            if (ToolList[selectedToolIndex].TriggerType == ToolBase.ToolTriggerType.Activate)
            {
                ToolList[selectedToolIndex].Deactivate();
            }
        }



    }


    public void Think()
    {

        Targets.ListUpdate();
        var possibleTarget = Targets.GetCurrentTargetObject();
        ToolBase selectedTool = ToolList[selectedToolIndex];

        switch (BrainAttitude)
        {
            case AttitudeEnum.Aggressive:


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

        if (possibleTarget)
        {
            foreach (var tool in ToolList)
            {


                switch (ValidateTarget(possibleTarget))
                {
                    case TargetStateEnum.Ok:
                        mover.Stop();

                        break;

                    case TargetStateEnum.OutOfRange:


                        return;

                    case TargetStateEnum.OutOfEnergy:
                        break;
                    case TargetStateEnum.OutOfAmmo:
                        break;
                    case TargetStateEnum.Invalid:
                        break;
                    default:
                        break;
                }

            }
            var destination = Targets.GetLastKnownPosition(possibleTarget);
            mover.Destination = destination == null ? transform.position : destination.Value;
            turret.TargetObject = possibleTarget;
        }

        // Check State & target priority

        // Set Attitude

        // Set Action
    }
    void OnTargetDetected(GameObject newTarget)
    {
        Targets.TrackObject(newTarget);
    }

    void OnTargetLost(GameObject lostTarget)
    {
        Targets.StopTrackingObject(lostTarget);
        if (turret.TargetObject == lostTarget)
        {
            turret.TargetObject = null;
        }
    }

    public void Assist() { }
    public void Attack() { }
    public void RequestMove() { }
    public void Work() { }

    public TargetStateEnum ValidateTarget(GameObject target)
    {

        if (ToolList[selectedToolIndex].ModuleState == ModuleStateEnum.Recharging)
        {
            return TargetStateEnum.OutOfEnergy;
        }

        var toolAction = ToolList[selectedToolIndex];

        
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
            ToolList[selectedToolIndex].SendMessage("Use", target);
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
        return ToolList[selectedToolIndex];
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
        ToolList.Clear();
    }





}

