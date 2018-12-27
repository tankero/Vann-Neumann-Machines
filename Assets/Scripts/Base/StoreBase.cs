using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts
{

    public class StoreBase : MonoBehaviour
    {


        [Range(1, 20)]
        public int StorageSize;

        public List<StorageStruct> storageList;

        public struct StorageStruct
        {
            string ItemName;
            Image ItemIcon;
            GameObject ItemObject;
            
        }


        public void StoreItem(GameObject item)
        {
            item.SetActive(false);
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