using UnityEngine;

public class SoldierFactory : MonoBehaviour
{
    private int _soldierId;

    public Sprite soldierImage;
    public GameObject soldierPrefab;

    private void Awake()
    {
        _soldierId = 0;
    }

    public void PrepareSoldier(string preId,GameObject soldierPrefab)
    {
        SoldierView view = new SoldierView(soldierImage);
        var id = preId + "," + _soldierId.ToString();
        SoldierModel model = new SoldierModel("This is a soldier. It can walk around.", id);
        _soldierId++;
        soldierPrefab.GetComponent<SoldierController>().Setup(model, view);
    }
    
}
