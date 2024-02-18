using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private Camera _camera;

    [SerializeField] private Vector3 _cameraNarrowPosition, _cameraNarrowRotation;
    [SerializeField] private Vector3 _cameraWidePosition, _cameraWideRotation;

    protected override void Awake()
    {
        base.Awake();

        _camera = GetComponent<Camera>();
    }

    public void SetCamera(bool isWide)
    {
        if (isWide)
        {
            _camera.transform.position = _cameraWidePosition;
            _camera.transform.eulerAngles = _cameraWideRotation;
        }
        else
        {
            _camera.transform.position = _cameraNarrowPosition;
            _camera.transform.eulerAngles = _cameraNarrowRotation;
        }
    }
}
