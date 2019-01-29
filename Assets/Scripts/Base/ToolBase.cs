using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoenenGames.VoxelRobot;
using UnityEngine;


public class ToolBase : ModuleBase
{




    public enum ActionType
    {
        Heal,
        Damage,
        Scan,
        Work,
        Communicate
    }


    public enum ProjectileType
    {
        Projectile,
        Beam,
        Meele,
        Spawner
    }

    public enum ToolTriggerType
    {
        Activate,
        Use
    }

    public ActionType Effect;
    public float EffectAmount;
    public ToolTriggerType TriggerType;
    public ProjectileType ToolType;
    [Range(0f, 1f)]
    public float Accuracy;
    [Range(0f, 20f)]
    public float Range;
    [Range(0, 30)]
    public int AmmunitionCapacity;
    public bool UsesAmmunition;
    public int AmmunitionCount;
    public float ActivationSpeed = 0.2f;

    [SerializeField]
    private bool SingleBarrel;

    private bool _triggerHeld = false;

    [SerializeField]
    private GameObject AmmunitionTemplate;
    public GameObject[] AmmoPool;
    public Weapon[] BarrelSpawnPoints;

    // Start is called before the first frame update
    public virtual void Start()
    {
        gameObject.tag = "Tool";

        BarrelSpawnPoints = transform.GetComponentsInChildren<Weapon>().ToArray();
        AmmoPool = new GameObject[AmmunitionCapacity > 10 ? AmmunitionCapacity : 10];
        for (int i = 0; i < AmmoPool.Length; i++)
        {
            AmmoPool[i] = Instantiate(AmmunitionTemplate, transform);
            AmmoPool[i].gameObject.SetActive(false);
        }
        switch (ToolType)
        {
            case ProjectileType.Projectile:

                

                break;
            case ProjectileType.Beam:
                break;
            case ProjectileType.Meele:
                break;
            case ProjectileType.Spawner:
                break;
            default:
                break;
        }




        StartCoroutine("TriggerTool");
    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    public virtual void Reload(GameObject ammo)
    {
        if (ammo.transform.parent != transform) ammo.transform.parent = transform;
        var component = ammo.GetComponent<BatteryBase>();
        if (!component | component.Type != BatteryBase.ChargeType.Ammunition) return;
        component.Expend();
    }

    IEnumerator TriggerTool()
    {
        for (; ; )
        {
            if (_triggerHeld)
            {
                if (MaintanceType == MaintenanceTypeEnum.OnUse)
                {
                    EnergyCurrent -= EnergyCost;
                }
                if (SingleBarrel)
                {
                    foreach (var barrel in BarrelSpawnPoints)
                    {
                        barrel.Fire(GetNextBullet());
                        yield return new WaitForSeconds(ActivationSpeed);
                    }
                }
                else
                {
                    foreach (var barrel in BarrelSpawnPoints)
                    {
                        barrel.Fire(GetNextBullet());

                    }
                    yield return new WaitForSeconds(ActivationSpeed);
                }
            }
            yield return null;

        }
    }

    public virtual void Activate()
    {
        _triggerHeld = true;
    }

    public virtual void Use()
    {



    }

    public virtual void Deactivate()
    {
        _triggerHeld = false;
    }

    public GameObject GetNextBullet()
    {
        var first = AmmoPool.FirstOrDefault(b => !b.gameObject.activeInHierarchy);
        if (first != null) return first;

        var newAmmoPull = new GameObject[AmmoPool.Length + 1];
        newAmmoPull[0] = Instantiate(AmmunitionTemplate);
        newAmmoPull[0].gameObject.SetActive(false);

        for (int i = 1; i < newAmmoPull.Length; i++)
        {
            newAmmoPull[i] = AmmoPool[i - 1];
        }
        AmmoPool = newAmmoPull;
        return AmmoPool[0];

    }
}

