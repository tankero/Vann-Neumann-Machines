using System.Linq;
using Assets.Scripts;

namespace MoenenGames.VoxelRobot
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class Weapon : MonoBehaviour, Controllable
    {




        #region --- VAR ---


        // Shot Cut

        bool Controllable.Active
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        public Transform Model
        {
            get
            {
                return model ? model : transform;
            }
        }

        [HideInInspector] public float PrevAttackTime = float.MinValue;

        [HideInInspector] public ToolBase.ActionType Effect;

        [HideInInspector] public float EffectAmount;





        [SerializeField]
        private bool RotatableY = false;
        [SerializeField]
        private float RandomTimeGap = 0.05f;
        [Header("System")]
        [SerializeField]
        private AnimationCurve LerpCurveF = AnimationCurve.EaseInOut(0f, -0.4f, 0.4f, 0f);
        [SerializeField]
        private AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f) });
        [SerializeField]
        private Vector3 CurveRandom = Vector3.zero;


        [Header("Component")]
        [SerializeField]
        private ParticleSystem[] Particles = new ParticleSystem[0];


        [SerializeField]
        private Transform Shooter;
        [SerializeField]
        private LerpLight TheLight;
        [SerializeField]
        private Transform model;
        [SerializeField]
        private Transform bulletSpawnPivot;

        // Data

        private Vector3 InitLocalPos = Vector3.zero;
        private Vector3 CurrentCurveRandom = Vector3.zero;


        #endregion



        #region --- EDT ---

#if UNITY_EDITOR

        void Reset()
        {



            // Init _m
            Transform _m = transform.Find("_m");
            if (_m)
            {
                model = _m;
            }

            // Init _s
            Transform _s = transform.Find("_s");
            if (!_s)
            {
                bulletSpawnPivot = new GameObject("_s").transform;
                bulletSpawnPivot.SetParent(transform);
                bulletSpawnPivot.localPosition = Vector3.zero;
                bulletSpawnPivot.localRotation = Quaternion.identity;
                bulletSpawnPivot.localScale = Vector3.one;
            }
            else
            {
                bulletSpawnPivot = _s;
            }

            // Init Light
            LerpLight _l = transform.parent.GetComponentInChildren<LerpLight>(true);
            if (_l)
            {
                TheLight = _l;
            }

            // Init Particles
            ParticleSystem[] pss = GetComponentsInChildren<ParticleSystem>(true);
            Particles = pss;

        }

#endif

        void Awake()
        {
            InitLocalPos = Model.localPosition;
            StopAllParticles();
        }

        void Start()
        {


        }

        public void Update()
        {

            ModelUpdate();
            SpawnPivotUpdate();

        }



        public void Fire(GameObject bullet)
        {

            ShootBullet(bullet);
            TriggerLoghtOn();
            PlayAllParticles();
            CurrentCurveRandom = new Vector3(
                Random.Range(-CurveRandom.x, CurveRandom.x),
                Random.Range(-CurveRandom.y, CurveRandom.y),
                Random.Range(-CurveRandom.z, CurveRandom.z)
            );
            Debug.Log("Current Random Curve: " + CurrentCurveRandom);
            PrevAttackTime = Time.time + Random.Range(0f, RandomTimeGap);


        }



        void ModelUpdate()
        {
            float t = Time.time - PrevAttackTime;
            Model.localPosition = InitLocalPos + Vector3.forward * LerpCurveF.Evaluate(t) + CurrentCurveRandom;
            Model.localScale = Vector3.one * ScaleCurve.Evaluate(t);
        }



        void SpawnPivotUpdate()
        {
            if (RotatableY)
            {
                Vector3 mousePos = GetMouseWorldPosition(transform.position, Vector3.up);
                Vector3 dir = mousePos - bulletSpawnPivot.position + Vector3.up * 0.8f;
                Quaternion rot = Quaternion.LookRotation(dir);
                rot = Quaternion.Euler(
                    Mathf.Clamp(rot.eulerAngles.x, 0f, 80f),
                    bulletSpawnPivot.rotation.y,
                    bulletSpawnPivot.rotation.z
                );
                bulletSpawnPivot.localRotation = rot;
            }
        }



        private void PlayAllParticles()
        {
            for (int i = 0; i < Particles.Length; i++)
            {
                Particles[i].Play();
            }
        }



        private void StopAllParticles()
        {
            for (int i = 0; i < Particles.Length; i++)
            {
                Particles[i].Stop();
            }
        }



        private void ShootBullet(GameObject bullet)
        {

            AmmunitionBase b = bullet.GetComponent<AmmunitionBase>();
            Transform tf = b.transform;
            Vector3 pos = bulletSpawnPivot.position;
            if (b.LockBulletY)
            {
                pos.y = Shooter.transform.position.y + 1f;
            }


            
            b.Shooter = Shooter;
            b.Effect = Effect;
            b.EffectAmount = EffectAmount;
            b.Rig.velocity = Vector3.ClampMagnitude(bulletSpawnPivot.forward, 1f) * b.BulletSpeed;

            tf.position = pos;
            tf.rotation = bulletSpawnPivot.rotation;
            tf.parent = null;
            tf.localScale = Vector3.one * b.BulletSize;
            b.Col.enabled = true;

#if UNITY_2017_3
			var trail = tf.GetComponent<TrailRenderer>();
			if (trail) {
				trail.Clear();
			}
#endif
            b.Alive = true;
            tf.gameObject.SetActive(true);


        }



        private void TriggerLoghtOn()
        {
            if (TheLight)
            {
                TheLight.TriggerOn();
            }
        }



        private Vector3 GetMouseWorldPosition(Vector3 groundPosition, Vector3 groundNormal)
        {
            Plane plane = new Plane(groundNormal, groundPosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                return ray.origin + ray.direction * distance;
            }
            return groundPosition;
        }



        #endregion


    }
}
