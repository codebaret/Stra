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

    public bool WalkingIsPossible(Vector2 end)
    {
        var tx = Math.Round(transform.position.x - Board.cellDimensionX / 2, 2);
        var ty = Math.Round(transform.position.y - Board.cellDimensionY / 2, 2);
        var startPoint = new Vector2((float)tx, (float)ty);
        return _pathfindingAStar.FindPath(startPoint, end) != null;
    }

    public void SetupMovement(Vector2 end)
    {
        var tx = Math.Round(transform.position.x - Board.cellDimensionX / 2, 2);
        var ty = Math.Round(transform.position.y - Board.cellDimensionY / 2, 2);
        var startPoint = new Vector2((float)tx, (float)ty);
        var commandList = _pathfindingAStar.FindPath(startPoint, end);
        if (commandList != null)
        {
            StartCoroutine(Move(commandList, end));
        }
        else
        {
                NotifyBadCross();
        }
    }

    private void Update()
    {
        if (_model !=null && InformationPanel.IsSetter(_model.id) && Input.GetMouseButtonDown(1))
        {
            var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var destinationX = (float)Math.Floor(position.x / Board.cellDimensionX) * Board.cellDimensionX;
            var destinationY = (float)Math.Floor(position.y / Board.cellDimensionY) * Board.cellDimensionY;
            var destination = new Vector2(destinationX, destinationY);
            SetupMovement(destination);
        }
    }

    private void OnMouseDown()
    {
        Publisher.PublishInformationPanelSetup(_view.image, _model.explanation,_model.id);
    }

    private void NotifyBadCross()
    {
        Publisher.PublishWarningPanelSetup("It is not possible for this soldier to move to the selected spot.");
    }
    
    private IEnumerator Move(List<Vector2> dirs, Vector2 destination)
    {
        foreach (var dir in dirs)
        {
            if ( !Publisher.PublishMovementRequest(gameObject, dir))
            {
                SetupMovement(destination);
                yield break;
            }
            yield return _delay;
        }
    }
}
