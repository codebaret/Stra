using UnityEngine;
using UnityEngine.Events;

public class OnNavigateOutside : UnityEvent<Vector3> { };

public class NavigationModel : NavigationElement
{
    public int speed;
    public OnNavigateOutside onNavigateOutside;
    public float width;
    public float height;
}
