﻿using System.Collections;
using System.Collections.Generic;
using MoenenGames.VoxelRobot;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public abstract class AmmunitionBase : MonoBehaviour
{

    public Transform Shooter
    {
        get; set;
    }

    public float Damage
    {
        get; set;
    }




    public float BulletSize = 0.3f;
    public float BulletSpeed = 60f;
    public bool LockBulletY = true;
    public ParticleSystem Particle;
    public Transform Model;


    // Data
    [HideInInspector]
    public bool Alive = false;


    // Setting
    public const float BULLET_MAX_REBOUND_SPEED = 20f;

    public enum TriggerEnum
    {
        OnImpact,
        OnExpiration,
        OnTrigger
    }
    [Header("Setting")]
    public TriggerEnum Trigger;
    public float Radius;
    public float Duration;
    public float Delay;
    [Header("Set by weapon")]
    public ToolBase.ActionType Effect;
    public float EffectAmount;
    // Serialize

    public DamageType DamegeType;

    public float LifeTime = 1f;

    public Rigidbody Rig
    {
        get
        {
            if (!rig)
            {
                rig = GetComponent<Rigidbody>();
            }
            return rig;
        }
    }

    public Collider Col
    {
        get
        {
            if (!col)
            {
                col = GetComponent<Collider>();
            }
            return col;
        }
    }

    [Header("On Hit")]
    public bool DontDestroyOnHit = false;
    public bool StopOnHit = false;

    private Rigidbody rig = null;
    private Collider col = null;

    

    // Start is called before the first frame update
    public virtual void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnHit(GameObject target)
    {
        Debug.Log("Bullet has hit " + target.transform.gameObject.name);
        if (target.transform == Shooter) return;

        if (Trigger == TriggerEnum.OnImpact)
        {
            if (Effect == ToolBase.ActionType.Damage)
            {
                var targetHealth = target.transform.root.gameObject.GetComponent<Health>();
                if(targetHealth)
                targetHealth.TakeDamage(EffectAmount, gameObject);
            }
        }
    }



}

