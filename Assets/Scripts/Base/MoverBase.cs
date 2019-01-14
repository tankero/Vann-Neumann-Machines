using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class MoverBase : ModuleBase
{

    public enum MovementTypeEnum
    {
        Ground,
        Hover,
        Jump,
        Teleport,
        Stationary
    }
    public MovementTypeEnum MovementType;
    public float Speed;
    public float Range;


    void Awake()
    {
        gameObject.tag = "Mover";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Move(Vector3 globalTargetPosition)
    {
        switch (MovementType)
        {
            case MovementTypeEnum.Ground:
                break;
            case MovementTypeEnum.Hover:
                break;
            case MovementTypeEnum.Jump:
                break;
            case MovementTypeEnum.Teleport:
                break;
            case MovementTypeEnum.Stationary:
                break;
            default:
                break;
        }
    }

    public virtual bool Pathfind(Vector3 globalTargetPosition)
    {
        switch (MovementType)
        {
            case MovementTypeEnum.Ground:
                break;
            case MovementTypeEnum.Hover:
                break;
            case MovementTypeEnum.Jump:
                break;
            case MovementTypeEnum.Teleport:
                break;
            case MovementTypeEnum.Stationary:
                if (Vector3.Distance(globalTargetPosition, gameObject.transform.position) > Range)
                {
                    return false;
                }
                break;
        }

        return false;
    }
}

