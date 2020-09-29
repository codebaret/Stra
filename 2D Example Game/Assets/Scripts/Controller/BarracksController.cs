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
            Board.Instance.PublishWarningPanelSetup("You need to move the soldier on the spawnpoint before producing more.");
            return;
        }
        var soldier = Instantiate(_barracksView.soldierPrefab,transform);
        soldier.transform.position = new Vector3(_barracksView.spawnPoint.gameObject.transform.position.x, _barracksView.spawnPoint.gameObject.transform.position.y, -5);
        _barracksModel.soldierFactory.GetComponent<SoldierFactory>().PrepareSoldier(model.id, soldier);
        _barracksView.spawnPoint.SetOccupied();
    }

    protected override void GetInformation()
    {
        Board.Instance.PublishInformationPanelSetup(_barracksView.image, _barracksModel.explanation, _barracksModel.id);
        Board.Instance.PublishInformationPanelUnitMenuSetup(_barracksView.soldierImage, this);
    }

    public override void PlaceBuilding(GameObject gameObject)
    {
        var collider = gameObject.GetComponent<BoxCollider2D>();
        Rect spawnPoint = new Rect(gameObject.transform.position - new Vector3(collider.size.x / 2f, (collider.size.y / 2f) + Board.Instance.cellDimensionY, 0f), new Vector2(Board.Instance.cellDimensionX, Board.Instance.cellDimensionY));
        Stack<string> spawnPointKey = new Stack<string>();
        if (!Board.Instance.ObjectPlacementIsOkay(spawnPoint,ref spawnPointKey))
        {
            Destroy(gameObject);
            Board.Instance.NotifyBadPlacement();
            return;
        }
        if (Board.Instance.PlaceTheObjects(gameObject))
        {
            SetupSpawnPoint(Board.Instance.Cells[spawnPointKey.Pop()]);
        }
    }

    private void SetupSpawnPoint(CellController spawnPoint)
    {
        spawnPoint.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 0);
        spawnPoint.SetSpawnPoint();
        _barracksView.spawnPoint = spawnPoint;
    }
}
