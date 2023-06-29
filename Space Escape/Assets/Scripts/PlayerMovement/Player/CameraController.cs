using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerController _player;

    [Space]

    [SerializeField] float _sensitivity;

    [Space]

    [SerializeField] Camera _cam;

    [Space]

    [SerializeField] Transform _orientation;
    [SerializeField] Transform _camHolder;

    float _xRotation;
    float _yRotation;

    void Start()
    {
        GameManager.ToggleCursor(false);
    }

    void Update()
    {
        Vector2 delta =  _sensitivity * _cam.fieldOfView * Time.deltaTime * GetMouseDelta();

        _yRotation += delta.x;

        _xRotation -= delta.y;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _camHolder.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        _orientation.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }

    Vector2 GetMouseDelta() => _player.Actions.Camera.Look.ReadValue<Vector2>();
}