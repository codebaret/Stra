using UnityEngine;
using UnityEngine.EventSystems;

public class UnitMenuElement : MonoBehaviour,IPointerClickHandler
{
    private BuildingController _buildingController;

    public void SetBuildingController(BuildingController buildingController)
    {
        _buildingController = buildingController;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _buildingController.ExecuteUniqueAction();
    }
}