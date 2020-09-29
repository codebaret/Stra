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

    public void SetFree()
    {
        _model.state = CellModel.State.Free;
    }

    public void SetSpawnPoint()
    {
        _model.isSpawnPoint = true;
        GetComponent<SpriteRenderer>().color = new Color32(13, 60, 122, 60);
    }
}
