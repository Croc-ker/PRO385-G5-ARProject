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

   [HideInInspector] public bool driving = false;
   [HideInInspector] public bool hasKey = false;
   [HideInInspector] public bool drinking = true;
   private float interactionDistance = 2.5f;
   private Transform cameraTransform;
   private GameObject nearbyBeer;

   private int beerLimit = 5;

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
         if (beer != null && beer.activeSelf && Vector3.Distance(cameraTransform.position, beer.transform.position) <= interactionDistance)
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
      else if (!driving && carObject != null && Vector3.Distance(cameraTransform.position, carObject.transform.position) <= interactionDistance)
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
            SoundManager.PlaySound(SoundType.PICKUP, 0.75f);
            collectedBeers++;
            beerCount.text = "x" + collectedBeers;
            nearbyBeer.SetActive(false);
            if (collectedBeers == beerLimit) drinking = false;
         }
      }
      else if (!hasKey)
      {
         SoundManager.PlaySound(SoundType.PICKUP, 0.75f);
         hasKey = true;
         pickupObject.SetActive(false);
         interactButton.gameObject.SetActive(false);
      }
      else
      {
         driving = true;
         SoundManager.StopLoopingSound();
         SoundManager.PlaySound(SoundType.CAR, 0.75f);
         HUD.gameObject.SetActive(true);
         goal.text = "That bump is shaped like a deer, D.U.I how bout you die!";
         StartCoroutine(SoundManager.playAfterDelay(SoundType.ASGORE_DRUNK, 0.20f, 2));
         carObject.SetActive(false);
         spawnDess.enabled = true;
         interactButton.gameObject.SetActive(false);
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
      SoundManager.PlaySound(SoundType.DRINK, 10f);
      yield return new WaitForSeconds(1);
      drinkBeer.gameObject.SetActive(false);
      holdBeer.gameObject.SetActive(true);
   }

   public void restart()
   {
      StopAllCoroutines();
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
      driving = false;

      foreach (var obj in spawnedBeers)
      {
         Destroy(obj);
      }
      spawnedBeers.Clear();
   }
}
