using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager instance;
    private AllegianceManager allegiance;
    public Transform cratePrefab;
    public static float TimeConstant = 0.004f;
    private GameObject PlayerBrain;
    public GameObject CurrentPlayerSpawner;
    public GameObject PlayerBrainTemplate;
    public GameObject CurrentPlayerTemplate;
    public HealthBar healthBar;
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

        //Get a component reference to the attached GameManager script
        allegiance = GetComponent<AllegianceManager>();

        //Call the InitGame function to initialize the first level 
        InitGame();


    }

    void InitGame()
    {
        PlayerBrain = GameObject.FindGameObjectWithTag("Player");
        if (!PlayerBrain)
        {
            PlayerBrain = Instantiate(PlayerBrainTemplate, null);
            PlayerBrain.SetActive(false);
        }
        healthBar = GameObject.Find("Canvas").GetComponentInChildren<HealthBar>();
        healthBar.gameObject.GetComponent<Slider>().maxValue = PlayerBrain.GetComponent<Health>().TotalHealth;
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.amount = PlayerBrain.GetComponent<Health>().CurrentHealth;
    }

    void OnPlayerDeath(GameObject deadPlayer)
    {

    }

    void OnNPCDeath(GameObject deadRobot)
    {

    }

    void RespawnPlayer()
    {
        PlayerBrain.transform.DetachChildren();
        CurrentPlayerSpawner.SendMessage("SpawnTemplate", PlayerBrain);
    }

    public static void DropItem(Vector3 position, ModuleBase item)
    {
        Vector3 heightOffset = new Vector3(0f, 0f, 3f);
        var crate = Instantiate(instance.cratePrefab, position + heightOffset, Quaternion.identity);
        var storage = crate.GetComponent<StoreBase>();
        storage.StoreItem(item);
        var randomX = Random.Range(0f, 3f);
        var randomZ = Random.Range(0f, 3f);
        crate.GetComponent<Rigidbody>().velocity = Vector3.up * 2.5f + new Vector3(randomX, 0f, randomZ);
    }

    public static AllegianceManager.AllegianceEnum CheckAlleigance(BrainBase sourceBrain, BrainBase targetBrain)
    {
        return instance.allegiance.CheckAllegiance(sourceBrain, targetBrain);
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }
    private void SetPlayerTemplate(GameObject template)
    {

    }


}


