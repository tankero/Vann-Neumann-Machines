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
    private ModuleBase mover;



    public int Allegiance;

    public List<string> Memory;

    public TargetObjectList Targets;
    public TargetLocationList Locations;

    public BrainStateEnum BrainState;

    [HideInInspector]
    public Health BrainHealth;


    void Awake()
    {
        IAmPlayer = CompareTag("Player");
        selectedToolIndex = 0;
        BrainHealth = GetComponent<Health>();
        if (!IAmPlayer)
        {
            StartCoroutine("ThinkPulse");
            sensor = GetComponent<SensorBase>();
            if (!sensor)
            {
                sensor = new SensorBase();
            }
        }

    }

    void Start()
    {
        var rootObject = transform.root;
        foreach (var child in rootObject.GetComponentsInChildren<Collider>())
        {
            Debug.Log("Collider found on:" + child.gameObject.name);
        }


    }

    private void Update()
    {
        //Check if we're dead
        if (BrainHealth.CurrentHealth <= 0)
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
        Debug.Log("Currently selected tool: " + ToolList[selectedToolIndex].name);
        if (Input.GetMouseButtonDown(0))
            if (ToolList[selectedToolIndex].TriggerType == ToolBase.ToolTriggerType.Activate)
            {
                Debug.Log("Activating: " + ToolList[selectedToolIndex].name);
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


        if (!sensor || !sensor.isActiveAndEnabled) return;
        var targets = sensor.GetComponent<SensorBase>().DetectedObjects;
        foreach (var item in targets)
        {
            Debug.Log("I see a: " + gameObject.name + "!");
        }
        // Check State & target priority

        // Set Attitude

        // Set Action
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
        if (toolAction.UsesAmmunition && toolAction.AmmunitionCount <= 0)
        {
            return TargetStateEnum.OutOfAmmo;
        }
        var targetBrain = target.GetComponent<BrainBase>();
        if (targetBrain)
        {
            var allegiance = GameManager.CheckAlleigance(this, targetBrain);
            if (allegiance == AllegianceManager.AllegianceEnum.Ally && toolAction.Effect == ActionType.Damage)
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

        while (BrainState == BrainStateEnum.Active)
        {
            Think();
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }

    void OnToolConnection(ToolBase connectingTool)
    {
        Debug.Log("Brain has received Tool connection: " + connectingTool.gameObject.name);
        if (!ToolList.Contains(connectingTool))
        {
            ToolList.Add(connectingTool);
            connectingTool.ModuleEnable();
        }
        if (ToolList.Contains(connectingTool))
        {
            Debug.Log("Tool connection successful");
        }
    }

    void OnMoverConnection(ModuleBase connectingMover)
    {
        if (connectingMover.CompareTag("Mover"))
        {
            mover = connectingMover;
            mover.ModuleEnable();
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

