using UnityEngine;

public abstract class BuildingController : MonoBehaviour
{
    protected BuildingModel model;
    protected BuildingView view;
    
    public abstract void Setup(BuildingView buildingView, BuildingModel buildingModel);

    public abstract GameObject InstantiateBuilding();

    protected abstract void GetInformation();

    public virtual void ExecuteUniqueAction(params object[] numbers) { }

    public virtual void PlaceBuilding(GameObject gameObject)
    {
        Board.Instance.PlaceTheObjects(gameObject);
    }

    private void OnMouseDown()
    {
        GetInformation();
    }
}
