using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{

    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update

        public static GameManager instance;
        private AllegianceManager allegiance;
        public Transform cratePrefab;
        public static float TimeConstant = 0.004f;

        public GameObject CurrentSpawner;

        void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);

            //Get a component reference to the attached BoardManager script
            allegiance = GetComponent<AllegianceManager>();

            //Call the InitGame function to initialize the first level 
            InitGame();
        }

        void InitGame()
        {


        }


        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }



        public static void DropItem(Vector3 position, ModuleBase item)
        {
            var crate = Instantiate(instance.cratePrefab, position, Quaternion.identity);
            var storage = crate.GetComponent<StoreBase>();
            storage.StoreItem(item);
        }

        public static AllegianceManager.AllegianceEnum CheckAlleigance(BrainBase sourceBrain, BrainBase targetBrain)
        {
            return instance.allegiance.CheckAllegiance(sourceBrain, targetBrain);
        }
    }


}