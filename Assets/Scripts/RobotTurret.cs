using MoenenGames.VoxelRobot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTurret : Turret
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    [SerializeField]
    private float RotateSpeed = 720f;
    [HideInInspector]
    public GameObject TargetObject;


    protected override void Update()
    {
        if (!TargetObject)
        {
            return;
        }

        var targetPositon = TargetObject.transform.position;
        
        Quaternion aimRot = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.LookRotation(
                targetPositon - transform.position,
                Vector3.up
            ),
            Time.deltaTime * RotateSpeed
        );

        Rotate(aimRot.eulerAngles.y);

        base.Update();


    }
}
