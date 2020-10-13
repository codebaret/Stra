using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProductionMenuElement : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    private string _buildingName;
    private GameObject _buildingObject;

    public BuildingFactory buildingFactory;

    private void Start()
    {
        if (Regex.IsMatch(name, Regex.Escape("barracks"), RegexOptions.IgnoreCase))
        {
            _buildingName = "Barracks";
        }
        else if (Regex.IsMatch(name, Regex.Escape("powerplant"), RegexOptions.IgnoreCase))
        {
            _buildingName = "PowerPlant";
        }
        else
        {
            _buildingName = "";
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _buildingObject.GetComponent<BuildingController>().PlaceBuilding(_buildingObject);
        _buildingObject = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var collider = _buildingObject.GetComponent<BoxCollider2D>();
        var offsetX = (collider.size.x / Board.cellDimensionX) % 2;
        var offsetY = (collider.size.y / Board.cellDimensionY) % 2;
        _buildingObject.transform.position = new Vector3(position.x - (position.x % Board.cellDimensionX) + offsetX * Board.cellDimensionX / 2, position.y - (position.y % Board.cellDimensionY) + offsetY * Board.cellDimensionY / 2, -1); ;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _buildingObject = buildingFactory.GetBuilding(_buildingName);
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var collider = _buildingObject.GetComponent<BoxCollider2D>();
        var offsetX = (collider.size.x / Board.cellDimensionX) % 2;
        var offsetY = (collider.size.y / Board.cellDimensionY) % 2;
        _buildingObject.transform.position = new Vector3(position.x - (position.x % Board.cellDimensionX) + offsetX * Board.cellDimensionX / 2, position.y - (position.y % Board.cellDimensionY) + offsetY * Board.cellDimensionY / 2, -1); ;

    }
}