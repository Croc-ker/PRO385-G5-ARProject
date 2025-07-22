using UnityEngine;
using UnityEngine.UI;

public class ARPickupInteraction : MonoBehaviour
{
    public GameObject pickupObject;      // The AR object to pick up
    public GameObject carObject;         // The car destination
    public Button pickUpButton;
    public Button dropOffButton;

    private bool hasItem = false;
    private float interactionDistance = 1.5f; // meters
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;

        pickUpButton.gameObject.SetActive(false);
        dropOffButton.gameObject.SetActive(false);

        pickUpButton.onClick.AddListener(PickUpItem);
        dropOffButton.onClick.AddListener(DropOffItem);
    }

    void Update()
    {
        float distToPickup = Vector3.Distance(cameraTransform.position, pickupObject.transform.position);
        float distToCar = Vector3.Distance(cameraTransform.position, carObject.transform.position);

        if (!hasItem && distToPickup <= interactionDistance)
        {
            pickUpButton.gameObject.SetActive(true);
        }
        else
        {
            pickUpButton.gameObject.SetActive(false);
        }

        if (hasItem && distToCar <= interactionDistance)
        {
            dropOffButton.gameObject.SetActive(true);
        }
        else
        {
            dropOffButton.gameObject.SetActive(false);
        }
    }

    void PickUpItem()
    {
        hasItem = true;
        pickupObject.SetActive(false); // "Pick up" by hiding the object
        pickUpButton.gameObject.SetActive(false);
    }

    void DropOffItem()
    {
        hasItem = false;
        // You could trigger an animation, sound, or spawn the object inside the car
        Debug.Log("Item delivered!");
        dropOffButton.gameObject.SetActive(false);
    }
}
