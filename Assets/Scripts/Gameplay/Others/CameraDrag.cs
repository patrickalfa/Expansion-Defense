using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class CameraDrag : MonoBehaviour
{
    public float draggingThreshold;
    public float minZoom, maxZoom, zoomSpeed;

    private Vector3 origin;
    private Vector3 offset;
    private bool dragging = false;

    private Transform _transform;
    private Camera _camera;

    private void Start()
    {
        _transform = transform;
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (GameManager.instance.state == GAME_STATE.PAUSED)
            return;

        HandleDrag();
        HandleZoom();
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButton(0))
        {
            offset = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - _transform.position;

            if (!dragging)
            {
                dragging = true;
                origin = _camera.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                float change = Vector3.Distance(origin, _camera.ScreenToWorldPoint(Input.mousePosition));
                if (change > draggingThreshold)
                    GameManager.instance.state = GAME_STATE.DRAGGING;

                _transform.position = origin - offset;
            }
        }
        else
        {
            dragging = false;
            GameManager.instance.state = GAME_STATE.PLANNING;
        }
    }

    private void HandleZoom()
    {
        float zoom = Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + zoom, minZoom, maxZoom);
    }
}