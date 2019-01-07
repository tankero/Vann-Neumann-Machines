using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(CharacterController))]
    public class BrainBase : ModuleBase
    {


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
        public List<ModuleBase> ToolList;
        private int selectedToolIndex;
        private ModuleBase sensor;
        private ModuleBase mover;



        public int Allegiance;

        public List<string> Memory;

        private CharacterController controller;

        public TargetObjectList Targets;
        public TargetLocationList Locations;

        public StateEnum CurrentState;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            ToolList = new List<ModuleBase>();
            IAmPlayer = CompareTag("Player");
        }

        private void Update()
        {
            if (IAmPlayer)
            {
                controller.Move(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")));
                return;
            }
        }


        public void Think()
        {

        }

        public void Assist() { }
        public void Attack() { }

        public void RequestMove() { }
        public void Work() { }

        public TargetStateEnum ValidateTarget(GameObject target)
        {

            if (ToolList[selectedToolIndex].State == ModuleStateEnum.Recharging)
            {
                return TargetStateEnum.OutOfEnergy;
            }

            var toolAction = (ToolBase)ToolList[selectedToolIndex].Action;

            if (Vector3.Distance(transform.position, target.transform.position) > toolAction.Range)
            {
                return TargetStateEnum.OutOfRange;
            }
            if (toolAction.AmmunitionCapacity > 0 && toolAction.AmmunitionCount <= 0)
            {
                return TargetStateEnum.OutOfAmmo;
            }
            var targetBrain = target.GetComponent<BrainBase>();
            if (targetBrain)
            {
                var allegiance = GameManager.CheckAlleigance(this, targetBrain);
                if (allegiance == AllegianceManager.AllegianceEnum.Ally && toolAction.Effect == ActionBase.ActionType.Damage)
                {
                    return TargetStateEnum.Invalid;
                }
            }
            return TargetStateEnum.Ok;
        }

        public void Use(GameObject target)
        {
            ToolList[selectedToolIndex].SendMessage("Use", target);
        }

        void OnToolConnection(ModuleBase connectingTool)
        {
            if (!ToolList.Contains(connectingTool))
            {
                ToolList.Add(connectingTool);
                connectingTool.ModuleEnable();
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

        public void SetSelectedTool(ModuleBase tool)
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
        }

    }

}