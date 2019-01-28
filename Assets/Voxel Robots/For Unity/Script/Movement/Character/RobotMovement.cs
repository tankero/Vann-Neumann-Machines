using UnityEngine.AI;

namespace MoenenGames.VoxelRobot {
	using UnityEngine;
	using System.Collections;


	[DisallowMultipleComponent]
	public class RobotMovement : CharacterMovement {




		#region --- VAR ---




		// Cache
		private Leg[] Legs;
		private float LegSlipTime = float.MinValue;
		private bool PrevLegSliping = false;
        public Vector3? Destination;



		#endregion




		#region --- MSG ---



		protected override void Awake () {
			base.Awake();
			PrevLegSliping = true;
			Legs = GetComponentsInChildren<Leg>(true);
			SlipingForAllLegs(false);
            Destination = transform.position;

		}



		protected override void Update () {
			base.Update();
			LegSlipUpdate();
            MoveToDestination();

        }




		#endregion




		#region --- LGC ---



		private void SlipingForAllLegs (bool slip) {
			if (PrevLegSliping != slip) {
				PrevLegSliping = slip;
				for (int i = 0; i < Legs.Length; i++) {
					Legs[i].Sliping = slip;
				}
			}
		}



		private void LegSlipUpdate () {
			if (Time.time < LegSlipTime) {
				SlipingForAllLegs(AimVelocity.magnitude > 2f || Chr.velocity.magnitude > 2f);
			} else {
				MoveLerpRate = 1f;
				SlipingForAllLegs(false);
			}
		}

        private void MoveToDestination()
        {
            if (Destination == null)
            {
                Stop();
                return;
            }
            if(Vector3.Distance(transform.root.position, Destination.Value) <= 5f)
            {
                Stop();
                return;
            }


           



            if (!Agent.SetDestination(Destination.Value))
            {
                Stop();
                return;
            }

            Agent.isStopped = false;
            Agent.nextPosition = transform.position;
            var normalizedRelativeDirection = Agent.desiredVelocity.normalized;
            
            Move(normalizedRelativeDirection);
            Rotate(
                Quaternion.LookRotation(normalizedRelativeDirection, Vector3.up)
                );
        }

        public void Stop()
        {
            Destination = transform.root.position;
            Agent.SetDestination(Destination.Value);
            Agent.isStopped = true;
            Move(new Vector3(0f, 0f, 0f));
        }
		#endregion




	}
}