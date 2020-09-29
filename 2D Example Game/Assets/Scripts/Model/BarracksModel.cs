using UnityEngine;

public class BarracksModel : BuildingModel
{
    public GameObject soldierFactory;

    public BarracksModel(string exp, int[] s,string id,GameObject soldierFact) : base(exp,s,id)
    {
        soldierFactory = soldierFact;
    }
}
