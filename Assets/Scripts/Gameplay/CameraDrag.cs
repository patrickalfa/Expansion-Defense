using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class CameraDrag : MonoBehaviour
{
    public float draggingThreshold;

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

        if (Input.GetMouseButtonUp(1))
        {
            _transform.DOMove(new Vector3(0f, 0f, -10f), 1f);
            _camera.DOOrthoSize(10f, 1f);
        }
    }
}