using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class NavigationComponent : MonoBehaviour
{
    [SerializeField] private bool willGenerateMissingNavMesh = true;
    public Transform target;

    public NavMeshAgent navAgent;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        CreateAndBakeMissingNavMeshSurface();
    }
    
    public void CreateAndBakeMissingNavMeshSurface()
    {
        //NavMeshSurface surface = FindObjectOfType<NavMeshSurface>();
        if (FindObjectOfType<NavMeshSurface>() != null)
        {
            return;
        }

        GameObject newNavMeshSurface = new GameObject("NavMesh Surface", typeof(NavMeshSurface));
        newNavMeshSurface.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    public void EnableTracking()
    {
        navAgent.enabled = true;
        if (TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = true;
        }
    }

    public Vector3 TargetPosition()
    {
        return target.transform.position;
    }
}