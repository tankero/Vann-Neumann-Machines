using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AllegianceManager : MonoBehaviour
{

    public enum AllegianceEnum
    {
        Ally,
        Enemy,
        Neutral
    }
    private Dictionary<int, AllegianceEnum[]> AllegianceDictionary;

    public struct AllegianceLogEntry
    {

        KeyValuePair<string, AllegianceEnum[]> StartingAllegiance;
        KeyValuePair<string, AllegianceEnum[]> EndingAllegiance;

    }


    // Start is called before the first frame update
    void Start()
    {
        AllegianceDictionary = new Dictionary<int, AllegianceEnum[]>();
        for (int i = 0; i < 8; i++)
        {
            AllegianceDictionary[i] = new AllegianceEnum[8];
            for (int j = 0; j < AllegianceDictionary[i].Length; j++)
            {
                AllegianceDictionary[i][j] = i == j ? AllegianceEnum.Ally : AllegianceEnum.Enemy;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public AllegianceEnum CheckAllegiance(BrainBase brain1, BrainBase brain2)
    {
        return AllegianceDictionary[brain1.Allegiance][brain2.Allegiance];
    }

    public void AllyTargetToMe(BrainBase sourceBrain, BrainBase targetBrain)
    {
        AllegianceDictionary[sourceBrain.Allegiance][targetBrain.Allegiance] = AllegianceEnum.Ally;
        AllegianceDictionary[targetBrain.Allegiance][sourceBrain.Allegiance] = AllegianceEnum.Ally;
    }

    public void MakeTargetEnemy(BrainBase sourceBrain, BrainBase targetBrain)
    {
        AllegianceDictionary[sourceBrain.Allegiance][targetBrain.Allegiance] = AllegianceEnum.Enemy;
        AllegianceDictionary[targetBrain.Allegiance][sourceBrain.Allegiance] = AllegianceEnum.Enemy;
    }

    public void TakeOverTargetAllegiance(BrainBase sourceBrain, BrainBase targetBrain)
    {
        targetBrain.Allegiance = sourceBrain.Allegiance;
    }

    public void JoinTargetAllegiance(BrainBase sourceBrain, BrainBase targetBrain)
    {
        sourceBrain.Allegiance = targetBrain.Allegiance;
    }

    public void BetrayTarget(BrainBase sourceBrain, BrainBase targetBrain)
    {
        AllegianceDictionary[sourceBrain.Allegiance][targetBrain.Allegiance] = AllegianceEnum.Enemy;
    }

    public void BecomeNeutralWithTarget(BrainBase sourceBrain, BrainBase targetBrain)
    {
        AllegianceDictionary[sourceBrain.Allegiance][targetBrain.Allegiance] = AllegianceEnum.Neutral;
        AllegianceDictionary[targetBrain.Allegiance][sourceBrain.Allegiance] = AllegianceEnum.Neutral;
    }
}
