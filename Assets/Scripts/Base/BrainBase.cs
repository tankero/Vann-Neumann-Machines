using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{

    public abstract class BrainBase : ModuleBase
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

        public enum TargetStateEnum
        {
            Ok,
            OutOfRange,
            OutOfEnergy,
            OutOfAmmo,
            Invalid

        }


        private List<ModuleBase> toolList;
        private int selectedToolIndex;
        private ModuleBase sensor;
        private ModuleBase mover;
        


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

        public TargetStateEnum ValidateTarget(GameObject target)
        {
            
            if (toolList[selectedToolIndex].State == ModuleBase.ModuleStateEnum.Recharging)
            {
                return TargetStateEnum.OutOfEnergy;
            }

            var toolAction = (ToolBase)toolList[selectedToolIndex].Action;

            if (Vector3.Distance(transform.position, target.transform.position) > toolAction.Range)
            {
                return TargetStateEnum.OutOfRange;
            }
            if(toolAction.AmmunitionCapacity > 0 && toolAction.AmmunitionCount <=0)
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

        void OnToolConnection(ModuleBase connectingTool)
        {
            if (!toolList.Contains(connectingTool))
            {
                toolList.Add(connectingTool);
            }
        }

        void OnMoverConnection(ModuleBase connectingMover)
        {
            if (connectingMover.CompareTag("Mover"))
            {
                mover = connectingMover;
            }
        }

        void OnSensorConnection(ModuleBase connectingSensor)
        {
            if (connectingSensor.CompareTag("Sensor"))
            {
                sensor = connectingSensor;
            }
        }

        void ConnectToCore(CoreBase coreParam)
        {
            coreParam.ConnectModule(this);
        }
    }

}