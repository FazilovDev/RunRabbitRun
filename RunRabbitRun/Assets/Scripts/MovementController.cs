using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public enum Direction
    {
        None, Right, Left, Up, Down
    }

    public float cellSize = 1f;
    public Direction MoveDirection { get; set; }
    public Vector3 Destination { get; set; }

    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private bool _isMoving { get { return MoveDirection != Direction.None; } }
    [SerializeField] private bool _canMove = true;

    private Animator _animator;
    private Rigidbody _rb;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!GWorld.Instance.IsPlay) return;
        MovementLogic();
        AnimateUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            GWorld.Instance.RabbitIsLife = false;

    }

    private Vector3 DirectionToVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.None:
                return Vector3.zero;
            case Direction.Right:
                return Vector3.right;
            case Direction.Left:
                return Vector3.left;
            case Direction.Up:
                return Vector3.forward;
            case Direction.Down:
                return Vector3.back;
            default:
                return Vector3.zero;
        }
    }

    private void MovementLogic()
    {
        Vector3 movement = Vector3.zero;
        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");

        if (Mathf.Abs(movement.x) > Mathf.Epsilon)
            MoveDirection = movement.x > 0 ? Direction.Right : Direction.Left;
        else if (Mathf.Abs(movement.z) > Mathf.Epsilon)
            MoveDirection = movement.z > 0 ? Direction.Up : Direction.Down;
        else
            MoveDirection = Direction.None;



        Vector3 direct = Vector3.RotateTowards(transform.forward, DirectionToVector(MoveDirection), _moveSpeed, 0.0f);
        transform.rotation = Quaternion.LookRotation(direct);
        var destination = transform.position + DirectionToVector(MoveDirection);
        if (Physics.OverlapSphere(destination, 0.1f, LayerMask.GetMask("Wall")).Length == 0)
            transform.position = Vector3.MoveTowards(transform.position, destination, _moveSpeed * Time.deltaTime);
    }

    private void AnimateUpdate()
    {
        _animator.SetBool("Run", _isMoving);
        _animator.SetBool("Idle", !_isMoving);
    }
}
