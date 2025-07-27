using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ARPickupInteraction : MonoBehaviour
{
   [HideInInspector] public GameObject pickupObject;      // The key
   [HideInInspector] public GameObject carObject;         // The car
   public Button interactButton;        // For key and car
   public Button drinkButton;           // For beer

   public TMP_Text beerCount;
   public TMP_Text goal;

   public List<GameObject> spawnedBeers = new List<GameObject>(); // dynamically tracked beers
   public int collectedBeers = 0;

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

      goal.text = "Go find yourself some beer!";
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
         goal.text = "That bump is shaped like a deer, D.U.I LOL how bout you die!";
         interactButton.gameObject.SetActive(false);
      }
   }

   void DrinkBeer()
   {
      if (nearbyBeer != null)
      {
         collectedBeers++;
         beerCount.text = "x" + collectedBeers;
         nearbyBeer.SetActive(false);
         drinkButton.gameObject.SetActive(false);
         Debug.Log("Beer drunk!");
      }
   }

   public void restart()
   {
      collectedBeers = 0;
      beerCount.text = "x" + collectedBeers;
      goal.text = "Go find yourself some beer!";
      interactButton.gameObject.SetActive(false);
      drinkButton.gameObject.SetActive(false);
      if (pickupObject) Destroy(pickupObject);
      if (carObject) Destroy(carObject);
      hasKey = false;

      foreach (var obj in spawnedBeers)
      {
         Destroy(obj);
      }
      spawnedBeers.Clear();
   }
}
