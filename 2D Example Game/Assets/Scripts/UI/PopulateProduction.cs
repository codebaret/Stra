using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateProduction : MonoBehaviour
{
    private int _numberOfRows;
    private LinkedList<GameObject> _pool;
    private LinkedListNode<GameObject> _activatePoint;

    public GameObject BarracksImage;
    public GameObject PowerPlantImage;

    void Start()
    {
        _numberOfRows = 20;
        _pool = new LinkedList<GameObject>();
        Populate();
    }
    
    void Update()
    {
        var rectTransform = GetComponent<RectTransform>();
        var cellSizeY = GetComponent<GridLayoutGroup>().cellSize.y;
        var spacingY = GetComponent<GridLayoutGroup>().spacing.y;
        if (rectTransform.transform.localPosition.y > cellSizeY + spacingY)
        {
            rectTransform.transform.localPosition -= new Vector3(0f, cellSizeY + spacingY, 0f);

            var first = _pool.First;
            var second = first.Next;
            var third = second.Next;
            var fourth = third.Next;
            first.Value.SetActive(false);
            second.Value.SetActive(false);
            third.Value.SetActive(false);
            fourth.Value.SetActive(false);

            _activatePoint.Value.SetActive(true);
            _activatePoint.Next.Value.SetActive(true);
            _activatePoint.Next.Next.Value.SetActive(true);
            _activatePoint.Next.Next.Next.Value.SetActive(true);

            _pool.RemoveFirst();
            _pool.RemoveFirst();
            _pool.RemoveFirst();
            _pool.RemoveFirst();
            _pool.AddLast(first);
            _pool.AddLast(second);
            _pool.AddLast(third);
            _pool.AddLast(fourth);
            _activatePoint = first;
        }
        else if (rectTransform.transform.localPosition.y <= 0f)
        {
            rectTransform.transform.localPosition += new Vector3(0f, 3*cellSizeY + 3*spacingY, 0f);
            
        }
    }
    
    private void Populate()
    {
        for (int i = 0; i < _numberOfRows -2; i++)
        {
            InsertRow();
        }
        GameObject newObject1 = Instantiate(BarracksImage, transform);
        _pool.AddLast(newObject1);
        _activatePoint = _pool.Last;
        GameObject newObject2 = Instantiate(PowerPlantImage, transform);
        _pool.AddLast(newObject2);
        GameObject newObject3 = Instantiate(BarracksImage, transform);
        _pool.AddLast(newObject3);
        GameObject newObject4 = Instantiate(PowerPlantImage, transform);
        _pool.AddLast(newObject4);
    }

    private void InsertRow()
    {
        GameObject newObject1 = Instantiate(BarracksImage, transform);
        _pool.AddLast(newObject1);
        GameObject newObject2 = Instantiate(PowerPlantImage, transform);
        _pool.AddLast(newObject2);
    }

}
