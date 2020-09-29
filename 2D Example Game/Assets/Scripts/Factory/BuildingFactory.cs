using UnityEngine;

public class BuildingFactory : MonoBehaviour
{
    private int _id;

    public GameObject barracksPrefab;
    public GameObject powerPlantPrefab;
    public GameObject soldierPrefab;
    public Sprite barracksImage;
    public Sprite powerPlantImage;
    public GameObject soldierImage;
    public GameObject soldierFactory;
    
    private void Start()
    {
        _id = -1;  
    }
   
    public BuildingView GetBuildingView(string name)
    {
        if (name.Equals("Barracks"))
        {
            var barracksView = new BarracksView(barracksPrefab, barracksImage,soldierImage,soldierPrefab);
            return barracksView;
        }
        else if (name.Equals("PowerPlant"))
        {
            var view = new PowerPlantView(powerPlantPrefab, powerPlantImage);
            return view;
        }
        return null;
    }

    public BuildingModel GetBuildingModel(string name)
    {
        _id++;
        if (name.Equals("Barracks"))
        {
            var barracksModel = new BarracksModel("This is a barracks building. It is capable of producing soldiers.", new int[] { 4, 4 }, "Barracks" + _id.ToString(),soldierFactory);
            return barracksModel;
        }
        else if (name.Equals("PowerPlant"))
        {
            var model = new PowerPlantModel("This is a powerplant building. It does not have a functionality for now, it just seems cool.", new int[] { 3, 2 }, "PowerPlant" + _id.ToString());
            return model;
        }
        return null;
    }

    public GameObject GetBuilding(string name)
    {
        var model = GetBuildingModel(name);
        var view = GetBuildingView(name);
        var gameObject = Instantiate(view?.prefab);
        if (name.Equals("Barracks"))
        {
            gameObject.AddComponent<BarracksController>();
            gameObject.GetComponent<BarracksController>().Setup(view, model);
            return gameObject;
        }
        else if (name.Equals("PowerPlant"))
        {
            gameObject.AddComponent<PowerPlantController>();
            gameObject.GetComponent<PowerPlantController>().Setup(view, model);
            return gameObject;
        }
        return null;
    }
    
}
