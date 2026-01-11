using UnityEngine;

public class WallController : MonoBehaviour, IActionable
{
    [Header("Wall Settings")]
    [SerializeField] private GameObject _wall;
    [SerializeField] private float _moveDistance = 5f;
    [SerializeField] private float _moveDuration = 2f;
    [SerializeField] private MoveDirection _direction = MoveDirection.Left;

    [Header("UI Settings")]
    [SerializeField] private GameObject _diceButton;

    private bool _hasBeenActivated = false;
    private Vector3 _startPosition;
    private Vector3 _targetPosition;

    public enum MoveDirection
    {
        Left,
        Right,
        Up,
        Down,
        Forward,
        Back
    }

    private void Start()
    {
        if (_wall != null)
        {
            _startPosition = _wall.transform.position;

            Vector3 moveDirection = GetDirectionVector();
            _targetPosition = _startPosition + (moveDirection * _moveDistance);
        }
    }

    private Vector3 GetDirectionVector()
    {
        switch (_direction)
        {
            case MoveDirection.Left:
                return Vector3.left;
            case MoveDirection.Right:
                return Vector3.right;
            case MoveDirection.Up:
                return Vector3.up;
            case MoveDirection.Down:
                return Vector3.down;
            case MoveDirection.Forward:
                return Vector3.forward;
            case MoveDirection.Back:
                return Vector3.back;
            default:
                return Vector3.left;
        }
    }

    public void Action(Player CurrentPawn)
    {
        if (!_hasBeenActivated && _wall != null)
        {
            _hasBeenActivated = true;
            StartCoroutine(MoveWall());
        }
    }

    private System.Collections.IEnumerator MoveWall()
    {
        if (_diceButton != null)
        {
            _diceButton.SetActive(false);
        }

        float elapsedTime = 0f;

        while (elapsedTime < _moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _moveDuration;

            _wall.transform.position = Vector3.Lerp(_startPosition, _targetPosition, t);

            yield return null;
        }

        _wall.transform.position = _targetPosition;

        if (_diceButton != null)
        {
            _diceButton.SetActive(true);
        }
    }
}
