using UnityEngine;

public class BarracksView : BuildingView
{
    public CellController spawnPoint;
    public GameObject soldierImage;
    public GameObject soldierPrefab;

    public BarracksView(GameObject prefab,Sprite image,GameObject sImage,GameObject sPrefab) : base(prefab,image)
    {
        soldierImage = sImage;
        soldierPrefab = sPrefab;
    }
}
