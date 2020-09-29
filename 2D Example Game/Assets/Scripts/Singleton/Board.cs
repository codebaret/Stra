using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private static Board _instance;
    public static Board Instance { get { return _instance; } }

    private Dictionary<string, CellController> _cells;
    public Dictionary<string, CellController> Cells
    {
        get { return _cells; }
    }

    public GameObject mCellPrefab;
    public Rect border;
    public int initialDimension;
    public string panelSetterId;
    public float cellDimensionX;
    public float cellDimensionY;

    public delegate void PublishEventInformationPanelContent(Sprite image, string text);
    public event PublishEventInformationPanelContent InformationPanelContentSetup;

    public delegate void PublishEventPanelUnitMenu(GameObject gameObject, BuildingController setterObject);
    public event PublishEventPanelUnitMenu InformationPanelActivateUnitMenu;

    public delegate void PublishEventWarningPanelContent( string text);
    public event PublishEventWarningPanelContent WarningPanelContentSetup;

    public static string GenerateKey(Vector2 vector2)
    {
        return Math.Round(vector2.x, 1).ToString() + "," + Math.Round(vector2.y, 1).ToString();
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        initialDimension = 30;
        Initialize();
    }

    private void Initialize()
    {
        cellDimensionX = 0.32f;
        cellDimensionY = 0.32f;
        int yCells = initialDimension;
        int xCells = initialDimension;
        _cells = new Dictionary<string, CellController>();
        border = new Rect(new Vector2(xCells * -cellDimensionX / 2, yCells * -cellDimensionY / 2), new Vector2(xCells * cellDimensionX, yCells * cellDimensionY));
        Vector2 minCorner = new Vector2(border.xMin, border.yMin);
        for (int y = 0; y < yCells; y++)
        {
            for (int x = 0; x < xCells; x++)
            {
                AddCell(x, y, minCorner);
            }
        }
    }

    public void AddCell(int x,int y,Vector2 minCorner)
    {
        GameObject newCell = Instantiate(mCellPrefab, transform);
        var pos = new Vector2((x * cellDimensionX), (y * cellDimensionY)) + minCorner;

        RectTransform rectTransform = newCell.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2((x * cellDimensionX) + cellDimensionX/2, (y * cellDimensionY) + cellDimensionY/2) + minCorner;

        var key = GenerateKey(pos);
        if (_cells.ContainsKey(key))
        {
            Destroy(newCell);
        }
        else
        {
            _cells.Add(key, newCell.GetComponent<CellController>());
        }
        
    }
    
    public void Expand(Vector3 navigationPosition)
    {
        int expandFactor = 4;
        var factor = new Vector2(expandFactor * cellDimensionX, expandFactor * cellDimensionY);
        float yIteration = 0f;
        float xIteration = 0f;
        Vector2 minCorner = new Vector2();
        if (navigationPosition.x >= border.xMax)//right
        {
            minCorner = new Vector2(border.xMax, border.yMin);
            border.xMax += factor.x;
            yIteration = (border.height / cellDimensionY);
            xIteration = expandFactor;
        }
        else if (navigationPosition.x <= border.xMin)//left
        {
            minCorner = new Vector2(border.xMin - factor.x, border.yMin);
            border.xMin -= factor.x;
            yIteration = (border.height / cellDimensionY);
            xIteration = expandFactor;
        }
        else if (navigationPosition.y >= border.yMax)//up
        {
            minCorner = new Vector2(border.xMin, border.yMax);
            border.yMax += factor.y;
            yIteration = expandFactor;
            xIteration = (border.width / cellDimensionX);
        }
        else if (navigationPosition.y <= border.yMin)//down
        {
            minCorner = new Vector2(border.xMin, border.yMin - factor.y);
            border.yMin -= factor.y;
            yIteration = expandFactor;
            xIteration = (border.width / cellDimensionX);
        }
        for (int y = 0; y < yIteration; y++)
        {
            for (int x = 0; x < xIteration; x++)
            {
                AddCell(x, y, minCorner);
            }
        }
    }
    
    public void PublishInformationPanelSetup(Sprite image, string text,string setterId)
    {
        panelSetterId = setterId;
        InformationPanelContentSetup(image, text);
    }

    public void PublishInformationPanelUnitMenuSetup(GameObject gameObject, BuildingController setterObject)
    {
        InformationPanelActivateUnitMenu( gameObject,  setterObject);
    }

    public void PublishWarningPanelSetup(string text)
    {
        WarningPanelContentSetup( text);
    }

    public bool PlaceTheObjects(params GameObject[] gameObjects)
    {
        Stack<string> keysToSetOccupied = new Stack<string>();
        foreach (var gameObject in gameObjects)
        {
            if(!ObjectPlacementIsOkay(MakeRectangleOutOfGameObject(gameObject), ref keysToSetOccupied))
            {
                foreach (var gameObj in gameObjects)
                {
                    Destroy(gameObj);
                }

                NotifyBadPlacement();
                return false;
            }
        }
        foreach (var key in keysToSetOccupied)
        {
            _cells[key].SetOccupied();
        }
        return true;
    }

    public bool ObjectPlacementIsOkay(Rect rect,ref Stack<string> keysToSetOccupied)
    {
        for (float i = 0; rect.xMin + i < rect.xMax - cellDimensionX / 2; i += cellDimensionX)
        {
            for (float k = 0; rect.yMin + k < rect.yMax - cellDimensionY / 2; k += cellDimensionY)
            {
                var key = GenerateKey(new Vector2(rect.xMin + i, rect.yMin + k));
                if (!_cells.ContainsKey(key))
                {
                    return false;
                }
                if (_cells[key].IsNotFree())
                {
                    return false;
                }
                else
                {
                    keysToSetOccupied.Push(key);
                }
            }
        }
        return true;
    }

    private Rect MakeRectangleOutOfGameObject(GameObject gameObject)
    {
        var collider = gameObject.GetComponent<BoxCollider2D>();
        return new Rect(gameObject.transform.position - new Vector3(collider.size.x / 2f, collider.size.y / 2f, 0f), collider.size);
    }

    public void NotifyBadPlacement()
    {
        PublishWarningPanelSetup("Your placement of the building was incorrect. Buildings cannot be placed on top of other buildings, soldiers, or spawnpoints.");
    }
    
}
