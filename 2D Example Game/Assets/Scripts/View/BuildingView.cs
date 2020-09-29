using UnityEngine;

public abstract class BuildingView
{
    public GameObject prefab;
    public Sprite image;

    public BuildingView(GameObject pf,Sprite img)
    {
        prefab = pf;
        image = img;
    }
}
