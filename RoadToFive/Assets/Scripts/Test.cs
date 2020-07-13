using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbodyPrefab;


    private void Start()
    {
        Debug.Log("Testing");

        if (rigidbodyPrefab) return;
        Debug.LogError("rigidbodyPrefab not initialized!");
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        var rigidbodyInstance = Instantiate(rigidbodyPrefab);
        rigidbodyInstance.AddForce(Random.insideUnitCircle * 10, ForceMode.Impulse);
    }
}