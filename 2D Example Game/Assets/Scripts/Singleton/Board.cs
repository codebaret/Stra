using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static int initialDimension;
    public static float cellDimensionX;
    public static float cellDimensionY;

    private static Board _instance;
    public static Board Instance
    {
        get
        {
            return _instance;
        }
    }

    private Dictionary<string, CellController> _cells;
    public Dictionary<string, CellController> Cells
    {
        get { return _cells; }
    }

    public GameObject mCellPrefab;
    public Rect border;

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
        Publisher.MovementRequest += ObjectMoveRequest;
        Publisher.MakeSpaceRequest += MakeSpace;
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
        newCell.GetComponent<CellController>().Setup();
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

                Publisher.NotifyBadPlacement();
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

    public bool ObjectMoveRequest(GameObject obj, Vector2 pos)
    {
        var targetPosition = new Vector2(pos.x - cellDimensionX / 2, pos.y - cellDimensionY / 2);
        var targetCell = Cells[GenerateKey(targetPosition)];
        if (targetCell.IsOccupied())
        {
            return false;
        }
        var tx = Math.Round(obj.transform.position.x - cellDimensionX / 2, 2);
        var ty = Math.Round(obj.transform.position.y - cellDimensionY / 2, 2);
        var currentPosition = new Vector2((float)tx, (float)ty);
        var previousCell = Cells[GenerateKey(currentPosition)];
        previousCell?.SetFree();
        targetCell.SetOccupied();
        targetCell.SetOccupier(obj);
        obj.transform.position = new Vector3(pos.x, pos.y, obj.transform.position.z);
        return true;
    }

    public bool MakeSpace(Vector2 pos,ref HashSet<Vector2> hash)
    {
        var leftBottom = new Vector2(pos.x - cellDimensionX , pos.y - cellDimensionY);
        var mainCell = Cells[GenerateKey(pos)];
        bool ItIsPossible = false;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (x == 1 && y == 1)
                {
                    continue;
                }
                var side = leftBottom + new Vector2(x * cellDimensionX, y * cellDimensionY);
                var key = GenerateKey(side);
                if (!Cells.ContainsKey(key))
                    AddCell(0, 0, side);
                var cell = Cells[key];
                if ( !cell.IsOccupied() || cell.GetOccupier() != null )
                {
                    ItIsPossible = true;
                }
            }
        }
        if (ItIsPossible == false) return false;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (x == 1 && y == 1)
                {
                    continue;
                }
                var side = leftBottom + new Vector2(x * cellDimensionX, y * cellDimensionY);
                if (hash.Contains(side)) continue;
                var key = GenerateKey(side);
                if (!Cells.ContainsKey(key))
                    AddCell(0, 0, side);
                var cell = Cells[key];
                if (!cell.IsOccupied())
                {
                    GameObject occupier = mainCell.GetOccupier();
                    occupier.GetComponent<SoldierController>().SetupMovement(side);
                    return true;
                }
            }
        }
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (x == 1 && y == 1)
                {
                    continue;
                }
                var side = leftBottom + new Vector2(x * cellDimensionX, y * cellDimensionY);
                if (hash.Contains(side)) continue;
                var key = GenerateKey(side);
                if (!Cells.ContainsKey(key))
                    AddCell(0, 0, side);
                var cell = Cells[key];
                if (cell.IsOccupied() && cell.GetOccupier() != null)
                {
                    hash.Add(side);
                    bool res = MakeSpace(side,ref hash);
                    if (res)
                    {
                        GameObject occupier = mainCell.GetOccupier();
                        occupier.GetComponent<SoldierController>().SetupMovement(side);
                    }
                    return res;
                }
            }
        }
        return false;
    }

    private Rect MakeRectangleOutOfGameObject(GameObject gameObject)
    {
        var collider = gameObject.GetComponent<BoxCollider2D>();
        return new Rect(gameObject.transform.position - new Vector3(collider.size.x / 2f, collider.size.y / 2f, 0f), collider.size);
    }

}
