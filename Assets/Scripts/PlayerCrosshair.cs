using UnityEngine;

public class PlayerCrosshair : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private GameObject _cube;

    private void Start()
    {
        InputManager.Instance.LeftMouseClick += OnLeftMouseClick;
        _camera = Camera.main;
    }

    private void OnLeftMouseClick()
    {
        Debug.Log("Click");
        Ray ray = _camera.ScreenPointToRay(new Vector2(Screen.height /2, Screen.width/2));


        var hit = Physics.Raycast(ray);

        var newCube = GameObject.Instantiate(_cube);

  
    }
}
