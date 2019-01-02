﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts
{

    public class StoreBase : ModuleBase
    {


        [Range(1, 20)]
        public int StorageSize;

        public List<StorageStruct> storageList;

        public struct StorageStruct
        {
            public string ItemName;
            public Image ItemIcon;
            public ModuleBase ItemObject;

        }


        public void StoreItem(ModuleBase item)
        {
            item.gameObject.SetActive(false);
            storageList.Add(new StorageStruct()
            {
                ItemName = item.GetComponent<ModuleBase>().Name,
                ItemIcon = item.GetComponent<ModuleBase>().Icon,
                ItemObject = item

            });

        }

        public virtual void PickUp(GameObject item)
        {

        }

        public virtual void Drop(StorageStruct item)
        {

        }

        public virtual void Give(GameObject target, StorageStruct item)
        {

        }

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