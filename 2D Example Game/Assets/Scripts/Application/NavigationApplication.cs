using UnityEngine;

public class NavigationElement : MonoBehaviour
{
    public NavigationApplication app { get { return GameObject.FindObjectOfType<NavigationApplication>(); } }
}

public class NavigationApplication : MonoBehaviour
{
    public NavigationModel model;
    public NavigationView view;
    public NavigationController controller;

    private void Start()
    {
        if (model.onNavigateOutside == null)
            model.onNavigateOutside = new OnNavigateOutside();

        model.onNavigateOutside.AddListener(Board.Instance.Expand);
        model.width = Board.initialDimension * Board.cellDimensionX;
        float ratio = (float)Screen.height / Screen.width;
        model.height = model.width * ratio;
    }

    private void Update()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        controller.UpdateNavigation(move);
    }
}
