using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class NavigationComponent : MonoBehaviour
{
    [SerializeField] private bool willGenerateMissingNavMesh = true;
    public Transform target;

    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

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

    public void EnableTracking()
    {
        navMeshAgent.enabled = true;
        if (TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = true;
        }
    }
}