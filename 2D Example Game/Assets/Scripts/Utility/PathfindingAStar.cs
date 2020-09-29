using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingAStar
{
    private enum State
    {
        Open,
        Closed
    }

    private class Node
    {
        public Vector2 location;
        public Node parent;
        public State state;
        public float G;
        public float H;
        public float F;
        public bool occupied;

        public Node(Node previousNode,Vector2 thisNode, Vector2 end)
        {
            location = thisNode;
            parent = previousNode;
            G = previousNode.G + 1f;
            H = Vector2.Distance(thisNode, end);
            F = G + H;
            state = State.Open;
            occupied = IsNotAvailable();
        }

        public Node(Vector2 thisNode, Vector2 end)
        {
            location = thisNode;
            parent = null;
            G = 0f;
            H = 0f;
            F = G + H;
            state = State.Open;
            occupied = IsNotAvailable();
        }
        
        public bool IsNotAvailable()
        {
            var key = Board.GenerateKey(location);
            if (Board.Instance.Cells.ContainsKey(key))
            {
                return Board.Instance.Cells[key].IsOccupied();
            }
            else
            {
                Board.Instance.AddCell(0, 0, location);
                return false;
            }
            
        }

        public List<Vector2> GetAdjacentPositions()
        {
            List<Vector2> positions = new List<Vector2>();
            var leftBottom = new Vector2(location.x - Board.Instance.cellDimensionX, location.y - Board.Instance.cellDimensionY);
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y ++)
                {
                    if (x == 1 && y == 1)
                    {
                        continue;
                    }
                    var pos = leftBottom + new Vector2(x * Board.Instance.cellDimensionX, y * Board.Instance.cellDimensionY);
                    positions.Add(pos);
                }
            }
            return positions;
        }
    }

    private List<Node> CalculateAdjacentNodes(Node parentNode,Vector2 end)
    {
        List<Node> adjacentNodes = new List<Node>();
        var positions = parentNode.GetAdjacentPositions();
        foreach (var position in positions)
        {
            var key = Board.GenerateKey(position);
            Node node;
            if (nodes.ContainsKey(key))
            {
                node = nodes[key];
                if (node.state == State.Closed || node.occupied)
                {
                    continue;
                }
                else if (node.state == State.Open)
                {
                    float traversalCost = Vector2.Distance(node.location, parentNode.location);
                    float gTemp = parentNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.parent = parentNode;
                        adjacentNodes.Add(node);
                        nodes[key] = node;
                    }

                }
            }
            else
            {
                node = new Node(parentNode, position, end);
                if (!node.occupied)
                {
                    adjacentNodes.Add(node);
                }
                nodes.Add(key, node);
            }
        }
        return adjacentNodes;
    }

    private Dictionary<string,Node> nodes;

    private bool Search(Node nodeToSearch, Vector2 end)
    {
        nodeToSearch.state = State.Closed;
        List<Node> adjacentNodes = CalculateAdjacentNodes(nodeToSearch, end).OrderBy(n => n.F).ToList();
        foreach (var node in adjacentNodes)
        {
            if(node.location == end)
            {
                return true;
            }
            else
            {
                if (Search(node, end))
                {
                    return true;
                }
            }
            
        }
        return false;
    }

    public List<Vector2> FindPath(Vector2 start,Vector2 end)
    {
        var endKey = Board.GenerateKey(end);
        if (Board.Instance.Cells.ContainsKey(endKey) && Board.Instance.Cells[endKey].IsOccupied())
        {
            return null;
        }
        if (start == end)
        {
            return null;
        }
        nodes = new Dictionary<string, Node>();
        List<Vector2> path = new List<Vector2>();
        Node startNode = new Node(start, end);
        var key = Board.GenerateKey(startNode.location);
        nodes[key] = startNode;
        bool success = Search(startNode,end);
        if (success)
        {
            Node node = nodes[endKey];
            while (node.parent != null)
            {
                path.Add(node.location + new Vector2(Board.Instance.cellDimensionX/2, Board.Instance.cellDimensionY / 2));
                node = node.parent;
            }
            path.Reverse();
            return path;
        }
        else
        {
            return null;
        }
    }
}

