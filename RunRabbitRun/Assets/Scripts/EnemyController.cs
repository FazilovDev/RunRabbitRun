using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private List<Vector3> PathToObject = new List<Vector3>();
    private List<Vector3> RandomPath = new List<Vector3>();
    private List<Vector3> CurrentPath = new List<Vector3>();
    private Rigidbody _rb;
    private Animator _animator;
    private PathFinder pathFinder;
    private bool isMoving;
    private bool seeObject;

    public GameObject target;
    [SerializeField] private float _moveSpeed;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        if (target != null)
        {
            pathFinder = GetComponent<PathFinder>();
            ReCalculatePath();
            isMoving = true;
        }

    }
    private bool IsEqualVector(Vector3 obj1, Vector3 obj2)
    {
        return Mathf.Abs(obj1.x - obj2.x) < Mathf.Epsilon && Mathf.Abs(obj1.y - obj2.y) < Mathf.Epsilon && Mathf.Abs(obj1.z - obj2.z) < Mathf.Epsilon;
    }
    public void ReCalculatePath()
    {
        PathToObject = pathFinder.GetPath(target.transform.position);

        if (PathToObject.Count == 0)
        {
            seeObject = false;
            var r = Random.Range(0, pathFinder.FreeNodes.Count);
            RandomPath = pathFinder.GetPath(pathFinder.FreeNodes[r].Position);
            CurrentPath = RandomPath;
        }
        else
        {
            CurrentPath = PathToObject;
            seeObject = true;
        }
    }
    Vector3 GetDirection(Vector3 vec)
    {
        if (Mathf.Abs(vec.x) < Mathf.Epsilon && Mathf.Abs(vec.z) < Mathf.Epsilon)
            return Vector3.zero;
        if (Mathf.Abs(vec.x) > Mathf.Abs(vec.z))
            return vec.x > 0 ? Vector3.right : Vector3.left;
        return vec.z > 0 ? Vector3.forward : Vector3.back;
    }

    void Update()
    {
        if (!GWorld.Instance.IsPlay) return;
        if (target == null) return;


        if (CurrentPath.Count == 0 && IsEqualVector(transform.position, target.transform.position))
        {
            ReCalculatePath();
            isMoving = true;
        }
        if (CurrentPath.Count == 0) return;


        if (isMoving)
        {
            if (!IsEqualVector(transform.position, CurrentPath[CurrentPath.Count - 1]))
            {
                var dir = (CurrentPath[CurrentPath.Count - 1] - transform.position);
                Vector3 direct = Vector3.RotateTowards(transform.forward, GetDirection(dir), _moveSpeed, 0.0f);
                transform.rotation = Quaternion.LookRotation(direct);
                transform.position = Vector3.MoveTowards(transform.position, CurrentPath[CurrentPath.Count - 1], _moveSpeed * Time.deltaTime);
            }
            else
            {
                isMoving = false;
            }
        }
        else
        {

            ReCalculatePath();
            isMoving = true;
        }
        //AnimateUpdate();
    }

    private void AnimateUpdate()
    {
        _animator.SetBool("Run", isMoving);
        _animator.SetBool("Idle", !isMoving);
    }
}
