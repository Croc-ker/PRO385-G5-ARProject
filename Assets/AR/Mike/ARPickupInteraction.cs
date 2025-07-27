using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class ARPickupInteraction : MonoBehaviour
{
   [HideInInspector] public GameObject pickupObject;      // The key
   [HideInInspector] public GameObject carObject;         // The car
   public Button interactButton;        // For key and car
   public Button drinkButton;           // For beer
   public Image HUD;           // Truck
   public Image holdBeer;           // HoldBeer
   public Image drinkBeer;           // HoldBeer
   public PlaceObjects spawnDess;

   public TMP_Text beerCount;
   public TMP_Text goal;

   public List<GameObject> spawnedBeers = new List<GameObject>(); // dynamically tracked beers
   public int collectedBeers = 1;

   [HideInInspector] public bool hasKey = false;
   [HideInInspector] public bool drinking = true;
   private float interactionDistance = 1.5f;
   private Transform cameraTransform;
   private GameObject nearbyBeer;

   void Start()
   {
      cameraTransform = Camera.main.transform;

      interactButton.gameObject.SetActive(false);
      drinkButton.gameObject.SetActive(true);

      interactButton.onClick.AddListener(HandleInteraction);
      drinkButton.onClick.AddListener(DrinkBeer);
      spawnDess.enabled = false;

      goal.text = "Go find yourself some beer!";
      holdBeer.gameObject.SetActive(true);
   }

   void Update()
   {
      interactButton.gameObject.SetActive(false);

      // === Check for beer nearby ===
      nearbyBeer = null;
      foreach (GameObject beer in spawnedBeers)
      {
         if (beer != null && beer.activeSelf &&
             Vector3.Distance(cameraTransform.position, beer.transform.position) <= interactionDistance)
         {
            interactButton.gameObject.SetActive(true);
            nearbyBeer = beer;
            break;
         }
      }

      // === Check for key ===
      if (!hasKey && pickupObject != null && Vector3.Distance(cameraTransform.position, pickupObject.transform.position) <= interactionDistance)
      {
         interactButton.gameObject.SetActive(true);
      }
      // === Check for car ===
      else if (hasKey && carObject != null && Vector3.Distance(cameraTransform.position, carObject.transform.position) <= interactionDistance)
      {
         interactButton.gameObject.SetActive(true);
      }
   }

   public void RegisterBeer(GameObject beer)
   {
      spawnedBeers.Add(beer);
   }

   void HandleInteraction()
   {
      if (drinking)
      {
         if(nearbyBeer != null)
         {
            collectedBeers++;
            beerCount.text = "x" + collectedBeers;
            nearbyBeer.SetActive(false);
            Debug.Log("Got Beer!");
            //play pickup beer sound
            if (collectedBeers == spawnedBeers.Count + 1) drinking = false;
         }
      }
      else if (!hasKey)
      {
         hasKey = true;
         pickupObject.SetActive(false);
         Debug.Log("Got the key!");
         interactButton.gameObject.SetActive(false);
         //play key sound
      }
      else
      {
         SoundManager.StopLoopingSound();
         Debug.Log("Entered the car with the key!");
         HUD.gameObject.SetActive(true);
         goal.text = "That bump is shaped like a deer, D.U.I LOL how bout you die!";
         SoundManager.PlayLoopingSound(SoundType.ASGORE_DRUNK, 0.75f);
         carObject.SetActive(false);
         spawnDess.enabled = true;
         interactButton.gameObject.SetActive(false);
         drinkButton.gameObject.SetActive(false);
         holdBeer.gameObject.SetActive(false);
         drinkBeer.gameObject.SetActive(false);
         //play enter car sound
      }
   }

   void DrinkBeer()
   {
      StartCoroutine(Drink());
   }

   IEnumerator Drink()
   {
      holdBeer.gameObject.SetActive(false);
      drinkBeer.gameObject.SetActive(true);
      //play drinking sound
      yield return new WaitForSeconds(1);
      drinkBeer.gameObject.SetActive(false);
      holdBeer.gameObject.SetActive(true);
   }

   public void restart()
   {
      collectedBeers = 1;
      beerCount.text = "x" + collectedBeers;
      goal.text = "Go find yourself some beer!";
      interactButton.gameObject.SetActive(false);
      drinkButton.gameObject.SetActive(true);
      HUD.gameObject.SetActive(false);
      holdBeer.gameObject.SetActive(true);
      drinkBeer.gameObject.SetActive(false);
      if (pickupObject) Destroy(pickupObject);
      if (carObject) Destroy(carObject);
      spawnDess.enabled = false;
      hasKey = false;
      drinking = true;

      foreach (var obj in spawnedBeers)
      {
         Destroy(obj);
      }
      spawnedBeers.Clear();
   }
}
