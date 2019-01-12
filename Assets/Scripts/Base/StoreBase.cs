using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts
{

    public class StoreBase : ModuleBase
    {


        [Range(1, 20)]
        public int StorageSize;

        public List<StorageStruct> StorageList;

        public struct StorageStruct
        {
            public string ItemName;
            public Image ItemIcon;
            public ModuleBase ItemObject;

        }


        public void StoreItem(ModuleBase item)
        {
            item.transform.parent = transform;
            item.gameObject.SetActive(false);
            StorageList.Add(new StorageStruct()
            {
                ItemName = item.GetComponent<ModuleBase>().gameObject.name,
                ItemIcon = item.GetComponent<ModuleBase>().Icon,
                ItemObject = item

            });

        }

        public virtual void PickUp(GameObject item)
        {

        }

        public virtual void Drop(StorageStruct item)
        {
            StorageList.Remove(item);
            GameManager.DropItem(transform.position, item.ItemObject);
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