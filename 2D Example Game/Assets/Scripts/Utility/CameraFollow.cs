using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _cameraPos;
    private Vector3 _velocity = Vector3.zero;

    public Transform Cursor;
    public float dampTime = 0.4f;
    
    private void Start()
    {
        float orthoSize = Board.initialDimension * Board.cellDimensionX * Screen.height / Screen.width * 0.5f;
        Camera.main.orthographicSize = orthoSize;
    }

    void Update()
    {
        _cameraPos = new Vector3(Cursor.position.x, Cursor.position.y, -10f);
        transform.position = Vector3.SmoothDamp(gameObject.transform.position, _cameraPos, ref _velocity, dampTime);
    }
}
