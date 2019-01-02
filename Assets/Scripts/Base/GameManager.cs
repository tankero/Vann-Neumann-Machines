using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager instance;
    private Assets.Scripts.AllegianceManager allegiance;
    public Transform cratePrefab;


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
        allegiance = GetComponent<Assets.Scripts.AllegianceManager>();

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
}

