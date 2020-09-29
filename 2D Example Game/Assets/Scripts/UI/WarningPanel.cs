using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WarningPanel : MonoBehaviour
{
    private Coroutine _coroutine;
    private bool _coroutineIsRunning;

    public GameObject text;
    
    private void Start()
    {
        Board.Instance.WarningPanelContentSetup += OnSetupPanel;
        gameObject.SetActive(false);
    }

    private void OnSetupPanel(string message)
    {
        gameObject.SetActive(true);
        if (_coroutineIsRunning)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(DisplayMessage(message));
    }

    private IEnumerator DisplayMessage(string message)
    {
        _coroutineIsRunning = true;
        text.GetComponent<Text>().text = message;
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
        _coroutineIsRunning = false;
    }


}
