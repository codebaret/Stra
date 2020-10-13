using UnityEngine;

public class CellController : MonoBehaviour
{
    private CellModel _model;
    private CellView _view;

    private void Start()
    {
        _model = new CellModel();
        _view = new CellView();
    }

    public void Setup()
    {
        _model = new CellModel();
        _view = new CellView();
    }

    public bool IsNotFree()
    {
        return _model.state != CellModel.State.Free || _model.isSpawnPoint;
    }

    public bool IsSpawnPoint()
    {
        return _model.isSpawnPoint;
    }

    public bool IsOccupied()
    {
        return _model.state == CellModel.State.Occupied;
    }

    public void SetOccupied()
    {
        _model.state = CellModel.State.Occupied;
    }

    public void SetOccupier(GameObject obj)
    {
        _model.occupyingObject = obj;
    }

    public GameObject GetOccupier()
    {
        return _model.occupyingObject;
    }

    public void SetFree()
    {
        _model.state = CellModel.State.Free;
        _model.occupyingObject = null;
    }

    public void SetSpawnPoint()
    {
        _model.isSpawnPoint = true;
        GetComponent<SpriteRenderer>().color = new Color32(13, 60, 122, 60);
        _model.occupyingObject = null;
    }
}
