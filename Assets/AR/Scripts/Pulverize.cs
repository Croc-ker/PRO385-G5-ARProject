using UnityEngine;

public class Pulverize : MonoBehaviour
{
    [SerializeField] float force = 200f;
    [SerializeField] GameObject origin = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddExplosionForce(force, origin.transform.position, 5f);
        }
    }
}

