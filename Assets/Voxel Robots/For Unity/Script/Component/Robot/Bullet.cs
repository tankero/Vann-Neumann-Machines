﻿using Assets.Scripts;

namespace MoenenGames.VoxelRobot
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;


    
    public class Bullet : AmmunitionBase
    {






        public override void Start()
        {


        }




        void OnCollisionEnter(Collision col)
        {
            if (col.collider.isTrigger)
            {
                return;
            }
            Colliding(col.transform);
        }




        void OnTriggerEnter(Collider c)
        {
            if (c.isTrigger)
            {
                return;
            }
            Colliding(c.transform);
        }







        #region --- LGC ---



        void Colliding(Transform tf)
        {

            if (!Alive)
            {
                return;
            }

            // Damage

            if (Shooter == tf)
            {
                return;
            }


            if (!DontDestroyOnHit)
            {

                // Logic
                Alive = false;

                // Stop
                if (StopOnHit)
                {
                    Rig.velocity = Vector3.zero;
                }
                else
                {
                    Rig.velocity = Vector3.ClampMagnitude(Rig.velocity, BULLET_MAX_REBOUND_SPEED);
                }

                // Particle
                if (Particle)
                {
                    Particle.Play();
                }

                // Col
                DisableCollider();

                // System
                CancelInvoke();
                DisableBullet();

            }


        }



        private void DisableCollider()
        {
            Alive = false;
            if (Col)
            {
                Col.enabled = false;
            }
        }



        private void DisableBullet()
        {
            Alive = false;
            TrailRenderer t = GetComponent<TrailRenderer>();
            if (t)
            {
                t.enabled = false;
            }
            if (Model)
            {
                Model.gameObject.SetActive(false);
            }

            // Disable
            gameObject.SetActive(false);
            
            
        }

        private void OnEnable()
        {
            // Self Kill
            Invoke("DisableCollider", LifeTime);
            Invoke("DisableBullet", LifeTime + 1f);
            // Size
            SetSize(transform.localScale.x);
        }



        private void SetSize(float size)
        {

            // Trail
            TrailRenderer t = transform.GetComponent<TrailRenderer>();
            if (t)
            {
                t.widthMultiplier = size;
            }

            // Rig
            Rig.mass *= size;

        }



        #endregion



    }
}