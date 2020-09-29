using UnityEngine;

public class NavigationController : NavigationElement
{
    public void UpdateNavigation(Vector3 move)
    {
        app.view.transform.position += move * app.model.speed * Time.deltaTime;

        var leftSide = app.view.transform.position - new Vector3(app.model.width / 2, 0f, 0f);
        var rightSide = app.view.transform.position + new Vector3(app.model.width / 2, 0f, 0f);
        var upSide = app.view.transform.position + new Vector3(0f, app.model.height / 2, 0f);
        var downSide = app.view.transform.position - new Vector3(0f, app.model.height / 2, 0f);
        if (!Board.Instance.border.Contains(leftSide))
        {
            app.model.onNavigateOutside.Invoke(leftSide);
        }
        if (!Board.Instance.border.Contains(rightSide))
        {
            app.model.onNavigateOutside.Invoke(rightSide);
        }
        if (!Board.Instance.border.Contains(upSide))
        {
            app.model.onNavigateOutside.Invoke(upSide);
        }
        if (!Board.Instance.border.Contains(downSide))
        {
            app.model.onNavigateOutside.Invoke(downSide);
        }
    }
}
