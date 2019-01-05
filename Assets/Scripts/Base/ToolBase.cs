using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ToolBase : ActionBase
    {


        public enum ToolTypeEnum
        {
            Projectile,
            Beam,
            Meele
        }



        
        public ToolTypeEnum ToolType;
        public GameObject AmmunitionTemplate;
        public int AmmunitionCapacity;
        public int AmmunitionCount;
        public Vector3 MuzzleOffset;
        [Range(0f, 1f)]
        public float Accuracy;
        [Range(0f, 20f)]
        public float Range;

        
        // Start is called before the first frame update
        void Start()
        {
            
        }
        
        // Update is called once per frame
        void Update()
        {

        }

    }
}
