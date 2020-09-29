using UnityEngine;
using UnityEngine.UI;

public class InformationPanel : MonoBehaviour
{
    public GameObject panel;
    public GameObject content;

    private void Start()
    {
        Board.Instance.InformationPanelContentSetup += OnSetupPanel;
        Board.Instance.InformationPanelActivateUnitMenu += OnSetupUnitMenu;
        panel.SetActive(false);
    }

    private void OnSetupPanel(Sprite image,string text)
    {
        ClearMenu();
        content.transform.parent.transform.parent.gameObject.SetActive(false);
        panel.SetActive(true);
        panel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = image;
        panel.transform.GetChild(1).gameObject.GetComponent<Text>().text = text;
    }

    private void OnSetupUnitMenu(GameObject gameObject,BuildingController buildingController)
    {
        ClearMenu();
        content.transform.parent.transform.parent.gameObject.SetActive(true);
        gameObject = Instantiate(gameObject, content.transform);
        gameObject.GetComponent<UnitMenuElement>().SetBuildingController(buildingController);
    }

    private void ClearMenu()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Flush()
    {
        ClearMenu();
        panel.SetActive(false);
    }
    
}
