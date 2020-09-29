
public abstract class BuildingModel
{
    public string explanation;
    public int[] size;
    public string id;

    public BuildingModel(string exp,int[] s,string ID)
    {
        explanation = exp;
        size = s;
        id = ID;
    }
    
}
