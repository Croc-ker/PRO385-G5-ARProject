using UnityEngine;
using UnityEngine.UI;

public class ARPickupInteraction : MonoBehaviour
{
    public GameObject pickupObject;      // The AR object to pick up
    public GameObject carObject;         // The car destination
    public GameObject[] beers;         // The beers you have to find and drink
    public Button pickUpButton;
    public Button enterCarButton;

    private bool hasItem = false;
    private float interactionDistance = 1.5f; // meters
    private Transform cameraTransform;

    void Start()
    {
      cameraTransform = Camera.main.transform;

      pickUpButton.gameObject.SetActive(false);
      enterCarButton.gameObject.SetActive(false);
      
      pickUpButton.onClick.AddListener(PickUpItem);
      enterCarButton.onClick.AddListener(enterCar);
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
            enterCarButton.gameObject.SetActive(true);
        }
        else
        {
            enterCarButton.gameObject.SetActive(false);
        }
    }

    void PickUpItem()
    {
        hasItem = true;
        pickupObject.SetActive(false); // "Pick up" by hiding the object
        pickUpButton.gameObject.SetActive(false);
    }

    void enterCar()
    {
        hasItem = false;
        // You could trigger an animation, sound, or spawn the object inside the car
        Debug.Log("Item delivered!");
        enterCarButton.gameObject.SetActive(false);
    }
}
