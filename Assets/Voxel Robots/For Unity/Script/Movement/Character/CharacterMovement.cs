namespace MoenenGames.VoxelRobot {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.AI;

    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class CharacterMovement : MonoBehaviour, Controllable {




		#region --- VAR ---



		// Shot Cut

		bool Controllable.Active {
			get {
				return enabled;
			}
			set {
				enabled = value;
			}
		}

		public CharacterController Chr {
			get {
				if (!chr) {
					chr = GetComponent<CharacterController>();
				}
				return chr;
			}
		}

		private CapsuleCollider Col {
			get {
				if (!col) {
					col = GetComponent<CapsuleCollider>();
				}
				return col;
			}
		}

        public NavMeshAgent Agent
        {
            get
            {
                if (!agent)
                {
                    agent = GetComponent<NavMeshAgent>();
                }
                return agent;
            }

        }

		private float FixedSpeedMuti {
			get {
				return MoveSpeed * buffSpeedMuti;
			}
		}

		private float FixedRotateMuti {
			get {
				return RotateSpeed * buffSpeedMuti;
			}
		}


		// Setting
		[SerializeField]
		private float Mass = 100f;
		[SerializeField]
		protected float MoveSpeed = 6f;
		[SerializeField]
		protected float RotateSpeed = 360f;

		private const float MAX_DROP_SPEED = 30f;


		// Cache
		protected Vector3 AimVelocity = Vector3.zero;
		protected Quaternion AimRotation = Quaternion.identity;
		protected float buffSpeedMuti = 1f;
		protected float MoveLerpRate = 1f;
        private NavMeshAgent agent;
		private CharacterController chr = null;
		private CapsuleCollider col = null;
		private Vector3 CurrentVelocity = Vector3.zero;



		#endregion




		#region --- EDT ---

#if UNITY_EDITOR

		protected virtual void Reset () {
			Chr.height = 2f;
			Chr.radius = 0.7f;
			Chr.center = Vector3.up * 1f;
			Col.height = 2f;
			Col.radius = 0.7f;
			Col.center = Vector3.up * 1f;
		}

#endif

		#endregion




		#region --- MSG ---




		protected virtual void Awake () {
			AimRotation = transform.rotation;
			ClearBuff();
		    Agent.updatePosition = false;
		    Agent.updateRotation = false;
		}




		protected virtual void Update () {
            //This has to be edited to include a navmeshagent in order to do proper pathfinding using the builtin system. Already added the agent dependency.

			// Gravity
			AimVelocity.y = Mathf.Clamp(AimVelocity.y + Physics.gravity.y, -MAX_DROP_SPEED, MAX_DROP_SPEED);

			// OnGround
			if (!Chr.isGrounded) {
				AimVelocity.x = 0f;
				AimVelocity.z = 0f;
			}

			// Move
		    Agent.nextPosition = transform.position;
			CurrentVelocity = Vector3.Lerp(CurrentVelocity, AimVelocity, MoveLerpRate);
			Chr.Move(CurrentVelocity * Time.deltaTime);


			// Rotate
			transform.rotation = AimRotation;

		}




		#endregion




		#region --- API ---




		public void Move (Vector3 speed) {
			float oldY = AimVelocity.y;
			AimVelocity = speed.normalized * FixedSpeedMuti;
			AimVelocity.y = oldY;
		}



		public void Rotate (Quaternion rot) {
			AimRotation = Quaternion.RotateTowards(AimRotation, rot, Time.deltaTime * FixedRotateMuti);
		}



		public void SetSpeedBuff (float newSpeedMuti) {
			buffSpeedMuti = newSpeedMuti;
		}




		public void ClearBuff () {
			buffSpeedMuti = 1f;
			MoveLerpRate = 1f;
		}






		#endregion




		#region --- LGC ---





		private void Bump (Rigidbody rig, Vector3 point, Vector3 dirction) {
			if (rig) {
				rig.AddForceAtPosition(dirction * Mass, point);
			}
		}



		#endregion


	}
}