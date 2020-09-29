using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour
{
    private SoldierModel _model;
    private SoldierView _view;
    private WaitForSeconds _delay = new WaitForSeconds(0.5f);
    private PathfindingAStar _pathfindingAStar = new PathfindingAStar();

    public void Setup(SoldierModel sModel, SoldierView sView)
    {
        _model = sModel;
        _view = sView;
    }

    private void Update()
    {
        if (_model !=null && Board.Instance.panelSetterId == _model.id && Input.GetMouseButtonDown(1))
        {
            var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var destinationX = (float)Math.Floor(position.x / Board.Instance.cellDimensionX) * Board.Instance.cellDimensionX;
            var destinationY = (float)Math.Floor(position.y / Board.Instance.cellDimensionY) * Board.Instance.cellDimensionY;
            var destination = new Vector2(destinationX, destinationY);
            SetupMovement(destination);
        }
    }

    private void SetupMovement(Vector2 end)
    {
        var tx = Math.Round(transform.position.x - Board.Instance.cellDimensionX / 2, 2);
        var ty = Math.Round(transform.position.y - Board.Instance.cellDimensionY / 2, 2);
        var startPoint = new Vector2((float)tx, (float)ty);
        var commandList = _pathfindingAStar.FindPath(startPoint, end);
        if (commandList != null)
        {
            StartCoroutine(Move(commandList,startPoint,end));
        }
        else
        {
            NotifyBadCross();
        }
    }

    private void OnMouseDown()
    {
        Board.Instance.PublishInformationPanelSetup(_view.image, _model.explanation,_model.id);
    }

    private void NotifyBadCross()
    {
        Board.Instance.PublishWarningPanelSetup("It is not possible for this soldier to move to the selected spot.");
    }

    private IEnumerator Move(List<Vector2> dirs, Vector2 start,Vector2 destination)
    {

        CellController previousCell = Board.Instance.Cells[Board.GenerateKey(start)];
        foreach (var dir in dirs)
        {
            var cellPosition = new Vector2(dir.x - Board.Instance.cellDimensionX / 2, dir.y - Board.Instance.cellDimensionY / 2);
            var cell = Board.Instance.Cells[Board.GenerateKey(cellPosition)];
            if (cell.IsOccupied())
            {
                SetupMovement(destination);
                yield break;
            }
            previousCell?.SetFree();
            cell.SetOccupied();
            previousCell = cell;
            gameObject.transform.position = new Vector3(dir.x, dir.y, gameObject.transform.position.z);
            
            yield return _delay;
        }
    }
}
