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


    public enum ToolTypeEnum
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
    public ToolTypeEnum TypeEnum;
    [Range(0f, 1f)]
    public float Accuracy;
    [Range(0f, 20f)]
    public float Range;
    [Range(0, 30)]
    public int AmmunitionCapacity;
    public bool UsesAmmunition;
    public int AmmunitionCount;
    public float ActivationSpeed = 0.2f;
    public bool SingleBarrel;
    private bool _triggerHeld = false;
    public GameObject AmmunitionTemplate;
    public GameObject[] AmmoPool;
    public Weapon[] BarrelSpawnPoints;

    // Start is called before the first frame update
    public virtual void Start()
    {


        BarrelSpawnPoints = transform.GetComponentsInChildren<Weapon>().ToArray();
        AmmoPool = new GameObject[AmmunitionCapacity > 10 ? AmmunitionCapacity : 10];
        for (int i = 0; i < AmmoPool.Length; i++)
        {
            AmmoPool[i] = Instantiate(AmmunitionTemplate, transform);
            AmmoPool[i].gameObject.SetActive(false);
        }
        switch (TypeEnum)
        {
            case ToolTypeEnum.Projectile:



                break;
            case ToolTypeEnum.Beam:
                break;
            case ToolTypeEnum.Meele:
                break;
            case ToolTypeEnum.Spawner:
                break;
            default:
                break;
        }

        foreach (var weapon in BarrelSpawnPoints)
        {
            weapon.Effect = Effect;
            weapon.EffectAmount = EffectAmount;
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
                switch (TypeEnum)
                {
                    case ToolTypeEnum.Projectile:
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
                        break;
                    case ToolTypeEnum.Beam:

                        break;
                    case ToolTypeEnum.Meele:

                        break;
                    case ToolTypeEnum.Spawner:

                        break;
                    default:
                        break;
                }

            }
            yield return null;

        }
    }

    public virtual void Activate()
    {
        _triggerHeld = true;
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

