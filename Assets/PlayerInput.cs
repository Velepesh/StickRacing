using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _timeBetweenThrow;

    private Player _player;
    private float _elapsedTime = 0f;

    private void Start()
    {
        _elapsedTime = _timeBetweenThrow;
        _player = GetComponent<Player>();
    }
    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            if (_elapsedTime >= _timeBetweenThrow)
            {
                _elapsedTime = 0f;

                RaycastHit hit;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                    _player.ThrowStick(hit.point);
            }
        }
    }
}
