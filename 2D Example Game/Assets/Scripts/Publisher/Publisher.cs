using System.Collections.Generic;
using UnityEngine;

public class Publisher : MonoBehaviour
{
    public delegate void PublishEventInformationPanelContent(Sprite image, string text, string id);
    public static event PublishEventInformationPanelContent InformationPanelContentSetup;

    public delegate void PublishEventPanelUnitMenu(GameObject gameObject, BuildingController setterObject);
    public static event PublishEventPanelUnitMenu InformationPanelActivateUnitMenu;

    public delegate void PublishEventWarningPanelContent(string text);
    public static event PublishEventWarningPanelContent WarningPanelContentSetup;

    public delegate bool PublishEventMovementRequest(GameObject obj, Vector2 pos);
    public static event PublishEventMovementRequest MovementRequest;

    public delegate bool PublishEventMakeSpaceRequest(Vector2 pos, ref HashSet<Vector2> hash);
    public static event PublishEventMakeSpaceRequest MakeSpaceRequest;

    public static void PublishInformationPanelSetup(Sprite image, string text, string setterId)
    {
        InformationPanelContentSetup(image, text, setterId);
    }

    public static void PublishInformationPanelUnitMenuSetup(GameObject gameObject, BuildingController setterObject)
    {
        InformationPanelActivateUnitMenu(gameObject, setterObject);
    }

    public static void PublishWarningPanelSetup(string text)
    {
        WarningPanelContentSetup(text);
    }

    public static void NotifyBadPlacement()
    {
        PublishWarningPanelSetup("Your placement of the building was incorrect. Buildings cannot be placed on top of other buildings, soldiers, or spawnpoints.");
    }

    public static bool PublishMovementRequest(GameObject obj,Vector2 pos)
    {
        return MovementRequest(obj, pos);
    }

    public static bool PublishMakeSpaceRequest(Vector2 pos)
    {
        HashSet<Vector2> hash = new HashSet<Vector2>();
        return MakeSpaceRequest(pos, ref hash);
    }
}
