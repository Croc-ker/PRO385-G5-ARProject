using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ARPickupInteraction : MonoBehaviour
{
   [HideInInspector] public GameObject pickupObject;      // The key
   [HideInInspector] public GameObject carObject;         // The car
   public Button interactButton;        // For key and car
   public Button drinkButton;           // For beer

   public List<GameObject> spawnedBeers = new List<GameObject>(); // dynamically tracked beers

   [HideInInspector] public bool hasKey = false;
   private float interactionDistance = 1.5f;
   private Transform cameraTransform;
   private GameObject nearbyBeer;

   void Start()
   {
      cameraTransform = Camera.main.transform;

      interactButton.gameObject.SetActive(false);
      drinkButton.gameObject.SetActive(false);

      interactButton.onClick.AddListener(HandleInteraction);
      drinkButton.onClick.AddListener(DrinkBeer);
   }

   void Update()
   {
      drinkButton.gameObject.SetActive(false);
      interactButton.gameObject.SetActive(false);

      // === Check for beer nearby ===
      nearbyBeer = null;
      foreach (GameObject beer in spawnedBeers)
      {
         if (beer != null && beer.activeSelf &&
             Vector3.Distance(cameraTransform.position, beer.transform.position) <= interactionDistance)
         {
            drinkButton.gameObject.SetActive(true);
            nearbyBeer = beer;
            break;
         }
      }

      // === Check for key ===
      if (!hasKey && pickupObject != null &&
          Vector3.Distance(cameraTransform.position, pickupObject.transform.position) <= interactionDistance)
      {
         //interactButton.GetComponentInChildren<Text>().text = "Pick Up Key";
         interactButton.gameObject.SetActive(true);
      }
      // === Check for car ===
      else if (hasKey && carObject != null &&
               Vector3.Distance(cameraTransform.position, carObject.transform.position) <= interactionDistance)
      {
         //interactButton.GetComponentInChildren<Text>().text = "Enter Car";
         interactButton.gameObject.SetActive(true);
      }
   }

   public void RegisterBeer(GameObject beer)
   {
      spawnedBeers.Add(beer);
   }

   void HandleInteraction()
   {
      if (!hasKey)
      {
         hasKey = true;
         pickupObject.SetActive(false);
         Debug.Log("Got the key!");
         interactButton.gameObject.SetActive(false);
      }
      else
      {
         Debug.Log("Entered the car with the key!");
         interactButton.gameObject.SetActive(false);
      }
   }

   void DrinkBeer()
   {
      if (nearbyBeer != null)
      {
         nearbyBeer.SetActive(false);
         drinkButton.gameObject.SetActive(false);
         Debug.Log("Beer drunk!");
      }
   }
}
