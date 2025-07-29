using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Pulverize : MonoBehaviour
{
    [SerializeField] float force = 50f;
    [SerializeField] GameObject origin = null;
    [SerializeField] GameObject xrorig = null;
    private Vector3 prevPosition;
    private Vector3 currentVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        prevPosition = xrorig.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentVelocity = (xrorig.transform.position - prevPosition) / Time.deltaTime;
        prevPosition = xrorig.transform.position;

        Debug.Log("Device Velocity: " + currentVelocity);
    }
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            other.gameObject.GetComponent<Collider>().enabled = false;
            Vector3 forceDirection = (other.transform.position - origin.transform.position).normalized;
            float scaledForce = force * currentVelocity.magnitude;
            rb.AddForce(forceDirection * 2f * scaledForce, ForceMode.Impulse);
            rb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * force, ForceMode.Impulse);
            if(Random.Range(0,2) == 0)
            {
                SoundManager.PlaySound(SoundType.HIT, 2f);
            }
            else
            {
                SoundManager.PlaySound(SoundType.HIT2, 2f);
            }
            
        }
    }
}

