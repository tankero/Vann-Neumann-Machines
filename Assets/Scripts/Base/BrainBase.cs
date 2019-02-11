using MoenenGames.VoxelRobot;
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


    public List<ToolBase> ToolList;

    private ModuleBase sensor;
    private RobotTurret turret;
    private RobotMovement mover;
    private Order currentOrder;

    public GameObject CurrentTarget;
    public AttitudeEnum BrainAttitude;

    [Range(1, 8)]
    public int Allegiance;

    public List<string> Memory;

    public TargetObjectList Targets;
    public TargetLocationList Locations;

    public BrainStateEnum BrainState;

    private ToolBase selectedTool;

    private Orders orders;


    void Awake()
    {
        IAmPlayer = CompareTag("Player");


        if (!IAmPlayer)
        {
            turret = GetComponentInChildren<RobotTurret>();
            mover = GetComponent<RobotMovement>();
            orders = GetComponent<Orders>();
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
                GameObject.FindGameObjectWithTag("GameManager").SendMessage("OnPlayerDeath", transform.position);
                return;
            }

            GameObject.FindGameObjectWithTag("GameManager").SendMessage("OnNPCDeath", gameObject);
            return;

        }
        if (!IAmPlayer) return;


        if (selectedTool)
        {
            if (Input.GetMouseButtonDown(0))

                selectedTool.Activate();


            if (Input.GetMouseButtonUp(0))
            {
                if (!selectedTool)
                {
                    selectedTool = ToolList.First();
                }

                selectedTool.Deactivate();

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

        

        if (BrainState == BrainStateEnum.Active)
        {
            switch (BrainAttitude)
            {
                // I want to attack things
                case AttitudeEnum.Aggressive:

                    //So lets figure out if I have a target to attack.
                    Targets.ListUpdate();

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
            // If our attitude doesn't give us an answer as to what we should be doing, let's review our orders

            if (!orders) return;

            currentOrder = orders.GetCurrentOrder();

            switch (currentOrder.OrderType)
            {
                case Orders.TypeEnum.Attack:
                    BrainAttitude = AttitudeEnum.Aggressive;
                    mover.Destination = currentOrder.Target.transform.position;
                    break;
                case Orders.TypeEnum.Move:
                    BrainAttitude = AttitudeEnum.Neutral;
                    mover.Destination = currentOrder.Target.transform.position;
                    break;
                case Orders.TypeEnum.Patrol:
                    BrainAttitude = AttitudeEnum.Aggressive;
                    
                    mover.Destination = currentOrder.Target.transform.position;
                    break;
                case Orders.TypeEnum.Work:
                    BrainAttitude = AttitudeEnum.Neutral;
                    break;
                default:
                    BrainAttitude = AttitudeEnum.Neutral;
                    break;
            }



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
        turret.TargetObject = CurrentTarget;

        if (selectedTool == null)
        {
            selectedTool = ToolList.First();
        }
        if (turret) turret.TargetObject = target;
        var tools = ToolList.Where(t => t.Effect == ToolBase.ActionType.Damage)
            .OrderByDescending(r => r.EffectAmount).ToArray();
        for (int i = 0; i < tools.Length; i++)
        {
            switch (ValidateTarget(target, tools[i]))
            {
                case TargetStateEnum.Ok:
                    SwitchToNewTool(tools[i]);
                    if (selectedTool.TriggerType == ToolBase.ToolTriggerType.Activate)
                    {
                        selectedTool.Activate();
                        mover.Stop();

                    }

                    return;
                case TargetStateEnum.OutOfRange:
                    if (Vector3.Distance(transform.position, target.transform.position) > 2f)
                    {
                        selectedTool.Deactivate();
                        mover.Destination = target.transform.position;
                    }
                    break;
            }

        }

        //if(currentOrder.OrderType == Orders.TypeEnum.Attack)
        //{
        //    currentOrder.Target = target;
        //}
        //If I can do something else, do that instead (heal)

        //Okay, so I can't do something else.

        //If I'm out of range or my target is out of sight, move to attack!

        //If I'm out of energy or ammo on all my weapons, move to get away :(
    }






    private void SwitchToNewTool(ToolBase newTool)
    {
        selectedTool.Deactivate();
        selectedTool = newTool;
    }

    public void RequestMove() { }
    public void Work() { }

    public TargetStateEnum ValidateTarget(GameObject target, ToolBase weapon)
    {
        if (weapon == null) return TargetStateEnum.Invalid;

        if (weapon.ModuleState == ModuleStateEnum.Recharging)
        {
            return TargetStateEnum.OutOfEnergy;
        }
        if (Vector3.Distance(transform.position, target.transform.position) > weapon.Range)
        {
            return TargetStateEnum.OutOfRange;
        }
        if (!Targets.CurrentlyTracking(target))
        {
            return TargetStateEnum.OutOfRange;
        }
        if (weapon.UsesAmmunition && weapon.AmmunitionCount <= 0)
        {
            return TargetStateEnum.OutOfAmmo;
        }
        var targetBrain = target.GetComponent<BrainBase>();
        if (targetBrain)
        {
            var allegiance = GameManager.CheckAlleigance(this, targetBrain);
            if (allegiance == AllegianceManager.AllegianceEnum.Ally && weapon.Effect == ToolBase.ActionType.Damage)
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
            ToolList = ToolList.OrderBy(t => t.EffectAmount).ToList();
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

    void OnSensorConnection(ModuleBase connectingSensor)
    {

        sensor = connectingSensor;
        sensor.ModuleEnable();
        if (!IAmPlayer)
        {
            StartCoroutine("ThinkPulse");

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

