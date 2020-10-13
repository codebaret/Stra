using UnityEngine;

public class PowerPlantController : BuildingController
{
    private PowerPlantView _powerPlantView { get { return (PowerPlantView)view; } set { view = value; } }
    private PowerPlantModel _powerPlantModel { get { return (PowerPlantModel)model; } set { model = value; } }

    public override void Setup(BuildingView bView, BuildingModel bModel)
    {
        _powerPlantView = (PowerPlantView)bView;
        _powerPlantModel = (PowerPlantModel)bModel;
    }

    public override GameObject InstantiateBuilding()
    {
        var gameObject = Instantiate(view.prefab);
        gameObject.AddComponent<PowerPlantController>();
        gameObject.GetComponent<PowerPlantController>().model = model;
        gameObject.GetComponent<PowerPlantController>().view = view;
        return gameObject;
    }

    protected override void GetInformation()
    {
        Publisher.PublishInformationPanelSetup(view.image, model.explanation, model.id);
    }
}
