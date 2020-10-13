using System.Collections.Generic;
using UnityEngine;

public class BarracksController : BuildingController
{
    private BarracksView _barracksView { get { return (BarracksView)view; } set { view = value; } }
    private BarracksModel _barracksModel { get { return (BarracksModel)model; } set { model = value; } }
    
    public override void Setup(BuildingView bView, BuildingModel bModel)
    {
        _barracksView = (BarracksView)bView;
        _barracksModel = (BarracksModel)bModel;
    }

    public override GameObject InstantiateBuilding()
    {
        var gameObject = Instantiate(view.prefab);
        gameObject.AddComponent<BarracksController>();
        gameObject.GetComponent<BarracksController>().model = model;
        gameObject.GetComponent<BarracksController>().view = view;
        return gameObject;
    }

    public override void ExecuteUniqueAction(params object[] parameters)
    {
        MakeSoldier();
    }

    private void MakeSoldier()
    {
        if (_barracksView.spawnPoint.IsOccupied())
        {
            var pos = _barracksView.spawnPoint.gameObject.transform.position;
            var alteredPosition = new Vector2(pos.x - Board.cellDimensionX / 2, pos.y - Board.cellDimensionY / 2);
            if (!Publisher.PublishMakeSpaceRequest(alteredPosition))
            {
                Publisher.PublishWarningPanelSetup("Soldier spawn failed. There is no possible cells for the soldiers to make space.");
                return;
            }
        }
        var soldier = Instantiate(_barracksView.soldierPrefab,transform);
        _barracksModel.soldierFactory.GetComponent<SoldierFactory>().PrepareSoldier(model.id, soldier);
        soldier.transform.position = new Vector3(_barracksView.spawnPoint.gameObject.transform.position.x, _barracksView.spawnPoint.gameObject.transform.position.y, -5);
        Publisher.PublishMovementRequest(soldier, soldier.transform.position);
    }

    protected override void GetInformation()
    {
        Publisher.PublishInformationPanelSetup(_barracksView.image, _barracksModel.explanation, _barracksModel.id);
        Publisher.PublishInformationPanelUnitMenuSetup(_barracksView.soldierImage, this);
    }

    public override void PlaceBuilding(GameObject gameObject)
    {
        var collider = gameObject.GetComponent<BoxCollider2D>();
        Rect spawnPoint = new Rect(gameObject.transform.position - new Vector3(collider.size.x / 2f, (collider.size.y / 2f) + Board.cellDimensionY, 0f), new Vector2(Board.cellDimensionX, Board.cellDimensionY));
        Stack<string> spawnPointKey = new Stack<string>();
        if (!Board.Instance.ObjectPlacementIsOkay(spawnPoint,ref spawnPointKey))
        {
            Destroy(gameObject);
            Publisher.NotifyBadPlacement();
            return;
        }
        if (Board.Instance.PlaceTheObjects(gameObject))
        {
            var sp =  Board.Instance.Cells[spawnPointKey.Pop()];
            sp.SetSpawnPoint();
            _barracksView.spawnPoint = sp;
        }
    }
}
