using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoenenGames.VoxelRobot;
using UnityEngine;


public class ToolBase : ModuleBase
{


    public enum ProjectileType
    {
        Projectile,
        Beam,
        Meele
    }

    public enum ToolTriggerType
    {
        Activate,
        Use
    }


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


    [SerializeField]
    private bool SingleBarrel;

    private bool _triggerHeld = false;

    [SerializeField]
    private AmmunitionBase AmmunitionTemplate;
    public AmmunitionBase[] AmmoPool;
    public Weapon[] BarrelSpawnPoints;

    // Start is called before the first frame update
    public virtual void Start()
    {
        gameObject.tag = "Tool";

        BarrelSpawnPoints = transform.GetComponentsInChildren<Weapon>().ToArray();
        if (AmmunitionCapacity > 0)
        {
            AmmoPool = new Bullet[10];
            for (int i = 0; i < AmmoPool.Length; i++)
            {
                AmmoPool[i] = Instantiate(AmmunitionTemplate, transform);
                AmmoPool[i].gameObject.SetActive(false);
                AmmoPool[i].CancelInvoke();
            }
        }
        StartCoroutine("TriggerTool");
    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    public virtual void Reload()
    {

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

    public AmmunitionBase GetNextBullet()
    {
        var first = AmmoPool.FirstOrDefault(b => !b.gameObject.activeInHierarchy);
        if (first == null)
        {
            var newAmmoPull = new AmmunitionBase[AmmoPool.Length + 1];
            newAmmoPull[0] = Instantiate(AmmunitionTemplate);
            newAmmoPull[0].gameObject.SetActive(false);

            for (int i = 1; i < newAmmoPull.Length; i++)
            {
                newAmmoPull[i] = AmmoPool[i - 1];
            }
            AmmoPool = newAmmoPull;
            return AmmoPool[0];
        }

        return first;
    }
}

